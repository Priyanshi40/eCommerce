using BLL.Interfaces;
using DAL.Enums;
using DAL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ecommerce.Controllers;

public class CartController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserService _userService;
    private readonly ICategoryService _catService;
    private readonly IProductService _proService;
    private readonly ICartService _cartService;
    public CartController(UserManager<IdentityUser> userManager, IUserService userService, ICategoryService catService, IProductService proService, ICartService cartService)
    {
        _userManager = userManager;
        _userService = userService;
        _catService = catService;
        _proService = proService;
        _cartService = cartService;
    }
    private string? GetUserIdentityId() => _userManager.GetUserId(User);
    private int GetAppUserId(string identityId) => _userService.GetUserById(identityId).UserId;

    private bool IsAuthenticated() => !string.IsNullOrEmpty(GetUserIdentityId());

    private List<CartViewModel> GetSessionCart()
    {
        string cart = HttpContext.Session.GetString("Cart") ?? "";
        return !string.IsNullOrEmpty(cart)
            ? JsonConvert.DeserializeObject<List<CartViewModel>>(cart)
            : new List<CartViewModel>();
    }
    private void SaveSessionCart(List<CartViewModel> cartItems)
    {
        string updatedCart = JsonConvert.SerializeObject(cartItems);
        HttpContext.Session.SetString("Cart", updatedCart);
    }
    public IActionResult GetCartCount()
    {
        int count = 0;

        if (IsAuthenticated())
        {
            int userId = GetAppUserId(GetUserIdentityId()!);
            count = _cartService.GetCartItemsCount(userId);
        }
        else
        {
            count = GetSessionCart().Count;
        }

        return Json(new { count });
    }

    [HttpPost]
    public IActionResult AddToCart([FromBody] CartViewModel addToCart)
    {
        if (IsAuthenticated())
        {
            int userId = GetAppUserId(GetUserIdentityId()!);
            bool cartId = _cartService.AddToCart(new List<CartViewModel> { addToCart }, userId);
            return cartId ? Ok(new { success = true }) : BadRequest();
        }

        List<CartViewModel> cartItems = GetSessionCart();
        CartViewModel? existing = cartItems.FirstOrDefault(c => c.ProductId == addToCart.Id);
        if (existing != null)
            existing.Quantity++;

        else
        {
            cartItems.Add(new CartViewModel
            {
                Id = addToCart.Id,
                ProductId = addToCart.Id,
                Name = addToCart.Name,
                Price = addToCart.Price,
                CoverImage = addToCart.CoverImage,
                Quantity = 1,
                StockQuantity = addToCart.StockQuantity,
            });
        }
        SaveSessionCart(cartItems);
        return Ok(new { success = true });
    }
    public IActionResult UpdateCart(int productId, int quantity, bool isConfirmPage = false)
    {
        if (IsAuthenticated())
        {
            int userId = GetAppUserId(GetUserIdentityId()!);
            int cartId = _cartService.UpdateCart(productId, userId, quantity);
            if (cartId > 0)
            {
                return Ok(new
                {
                    redirect = isConfirmPage
                        ? Url.Action("ConfirmOrder", "Order", new {area = "User"})
                        : Url.Action("Cart", "Cart")
                });
            }
        }

        List<CartViewModel> cartItems = GetSessionCart();
        CartViewModel? existing = cartItems.FirstOrDefault(c => c.ProductId == productId);

        if (existing != null)
        {
            existing.Quantity = quantity;
            SaveSessionCart(cartItems);
            return Ok(new { redirect = Url.Action("Cart", "Cart") });
        }
        return BadRequest();
    }
    public IActionResult RemoveFromCart(int productId,bool isConfirmPage = false)
    {
        if (IsAuthenticated())
        {
            int userId = GetAppUserId(GetUserIdentityId()!);
            bool cart = _cartService.DeleteCart(userId,productId);
            if (cart)
            {
                return Ok(new
                {
                    redirect = isConfirmPage
                        ? Url.Action("ConfirmOrder", "Order", new {area = "User"})
                        : Url.Action("Cart", "Cart")
                });
            }
        }

        List<CartViewModel> cartItems = GetSessionCart();
        CartViewModel? item = cartItems.FirstOrDefault(c => c.ProductId == productId);

        if (item != null)
        {
            cartItems.Remove(item);
            SaveSessionCart(cartItems);
            return Ok(new { redirect = Url.Action("Cart", "Cart") });
        }
        return Ok(new { status = AjaxError.NotFound.ToString() });
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Cart(int pageNumber = 1, int pageSize = 8)
    {
        List<CartViewModel> cartItems = new();

        if (IsAuthenticated())
        {
            string identityId = GetUserIdentityId()!;
            int userId = GetAppUserId(identityId);
            ViewBag.CurrentUser = userId;
            cartItems = _cartService.GetCart(userId);
        }
        else
        {
            cartItems = GetSessionCart();
        }

        CartViewModel cartView = new ()
        {
            CartItems = cartItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = cartItems.Count,
            TotalPages = (int)Math.Ceiling((double)cartItems.Count / pageSize),
        };
        return PartialView("_Cart",cartView);
    }
}