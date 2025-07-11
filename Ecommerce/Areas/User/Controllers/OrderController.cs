using BLL.Interfaces;
using DAL.Enums;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Ecommerce.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles = "User")]
public class OrderController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserService _userService;
    private readonly IAddressService _addService;
    private readonly ICartService _cartService;
    private readonly IProductService _proService;
    public OrderController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ICartService cartService, IProductService proService, IUserService userService, IAddressService addService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _cartService = cartService;
        _proService = proService;
        _userService = userService;
        _addService = addService;
    }
    private string? GetUserIdentityId() => _userManager.GetUserId(User);
    private int GetAppUserId(string identityId) => _userService.GetUserById(identityId).UserId;
    private bool IsAuthenticated() => !string.IsNullOrEmpty(GetUserIdentityId());
    public IActionResult Index()
    {
        string cart = HttpContext.Session.GetString("Cart") ?? "";
        if (IsAuthenticated())
        {
            if (!string.IsNullOrEmpty(cart))
            {
                int userId = GetAppUserId(GetUserIdentityId()!);
                List<CartViewModel>? cartItems = JsonConvert.DeserializeObject<List<CartViewModel>>(cart);

                if (cartItems != null)
                    _cartService.AddToCart(cartItems, userId);

                HttpContext.Session.Remove("Cart");
            }
            return RedirectToAction("OrderCard");
        }
        TempData["Error"] = "Please log in to Place Order";
        return RedirectToAction("Login", "Account", new { area = "" });
    }
    // public IActionResult BuyNow([FromBody] CartViewModel addToCart)
    // {
    //     if (IsAuthenticated())
    //     {
    //         int userId = GetAppUserId(GetUserIdentityId()!);
    //         _cartService.AddToCart(new List<CartViewModel> {addToCart}, userId);
    //         return RedirectToAction("OrderCard");
    //     }
    //     TempData["Error"] = "Please log in to Place Order";
    //     return RedirectToAction("Login", "Account", new { area = "" });
    // }
    public IActionResult OrderCard()
    {
        return View();
    }
    public IActionResult Address()
    {
        int userId = GetAppUserId(GetUserIdentityId()!);
        AddressViewModel addresses = _addService.GetUserAddresses(userId);
        return PartialView("_selectAddress", addresses);
    }
    public IActionResult AddressModal(int AddressId)
    {
        ViewBag.Countries = new SelectList(_addService.GetCountryService(), "Id", "Name");
        int userId = GetAppUserId(GetUserIdentityId()!);
        AddressViewModel addresses = _addService.GetAddressById(AddressId, userId);
        return PartialView("_addressModal", addresses);
    }
    public IActionResult EditAddress(AddressViewModel address)
    {
        if (!ModelState.IsValid)
            return Ok(new { status = AjaxError.ValidationError.ToString() });

        int userId = GetAppUserId(GetUserIdentityId()!);
        address.ModifiedBy = userId;
        _addService.AddUserAddress(address);
        return Ok(new { status = AjaxError.Success.ToString() });
    }
    public IActionResult ConfirmOrder(int AddressId)
    {
        if (!IsAuthenticated())
            return Unauthorized();

        int userId = GetAppUserId(GetUserIdentityId()!);
        List<CartViewModel> cartItems = _cartService.GetCart(userId);
        CartViewModel cartData = new()
        {
            CartItems = cartItems,
            AddressId = AddressId,
        };
        return PartialView("_confirmOrder", cartData);
    }

}