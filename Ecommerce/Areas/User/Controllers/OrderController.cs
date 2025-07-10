using BLL.Interfaces;
using DAL.Models;
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
    public IActionResult Index()
    {
        string cart = HttpContext.Session.GetString("Cart") ?? "";
        string? userIdentityId = _userManager.GetUserId(User);
        if (!string.IsNullOrEmpty(userIdentityId))
        {
            if (!string.IsNullOrEmpty(cart))
            {
                int userId = _userService.GetUserById(userIdentityId).UserId;
                List<CartViewModel>? cartItems = JsonConvert.DeserializeObject<List<CartViewModel>>(cart);
                if (cartItems != null)
                {
                    foreach (CartViewModel item in cartItems)
                        _cartService.AddToCart(item, userId);
                }
                HttpContext.Session.Remove("Cart");
            }
            return RedirectToAction("OrderCard");
        }
        TempData["Error"] = "Please log in to Place Order";
        return RedirectToAction("Login", "Account", new { area = "" });
    }
    public IActionResult OrderCard()
    {
        return View();
    }
    public IActionResult Address()
    {
        int userId = _userService.GetUserById(_userManager.GetUserId(User)).UserId;
        AddressViewModel addresses = _addService.GetUserAddresses(userId);
        return PartialView("_selectAddress", addresses);
    }
    public IActionResult AddressModal(int AddressId)
    {
        ViewBag.Countries = new SelectList(_addService.GetCountryService(), "Id", "Name");
        int userId = _userService.GetUserById(_userManager.GetUserId(User)).UserId;
        AddressViewModel addresses = _addService.GetAddressById(AddressId, userId);
        return PartialView("_addressModal", addresses);
    }
    public IActionResult ConfirmOrder(int AddressId)
    {
        string? userIdentityId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userIdentityId))
        {
            return Unauthorized();
        }
        int userId = _userService.GetUserById(userIdentityId).UserId;
        List<CartViewModel> cartItems = _cartService.GetCart(userId);
        CartViewModel cartData = new()
        {
            CartItems = cartItems,
            AddressId = AddressId,
        };
        return PartialView("_confirmOrder", cartData);
    }

}
