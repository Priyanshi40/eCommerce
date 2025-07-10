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
    public IActionResult GetCartCount()
    {
        int count = 0;

        string? userIdentityId = _userManager.GetUserId(User);
        if (!string.IsNullOrEmpty(userIdentityId))
        {
            int userId = _userService.GetUserById(userIdentityId).UserId;
            count = _cartService.GetCartItemsCount(userId);
        }

        string cart = HttpContext.Session.GetString("Cart") ?? "";
        if (!string.IsNullOrEmpty(cart))
            count = JsonConvert.DeserializeObject<List<CartViewModel>>(cart).Count;

        return Json(new { count });
    }

    [HttpPost]
    public IActionResult AddToCart([FromBody] CartViewModel addToCart)
    {
        string? userIdentityId = _userManager.GetUserId(User);
        if (!string.IsNullOrEmpty(userIdentityId))
        {
            int userId = _userService.GetUserById(userIdentityId).UserId;
            int cartId = _cartService.AddToCart(addToCart, userId);
            if (cartId > 0)
                return Ok(new { success = true });
            return BadRequest();
        }

        string cart = HttpContext.Session.GetString("Cart") ?? "";
        if (!string.IsNullOrEmpty(cart))
        {
            List<CartViewModel>? cartItems = JsonConvert.DeserializeObject<List<CartViewModel>>(cart);
            if (cartItems.Any(c => c.ProductId == addToCart.Id))
            {
                CartViewModel? existingItem = cartItems.FirstOrDefault(c => c.ProductId == addToCart.Id);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
            }
            else
            {
                CartViewModel newItem = new()
                {
                    Id = addToCart.Id,
                    ProductId = addToCart.Id,
                    Name = addToCart.Name,
                    Price = addToCart.Price,
                    CoverImage = addToCart.CoverImage,
                    Quantity = 1,
                    StockQuantity = addToCart.StockQuantity,

                };
                cartItems.Add(newItem);
            }
            cart = JsonConvert.SerializeObject(cartItems);
        }
        else
        {
            List<CartViewModel> newCart = new()
            {
                new() {
                    Id = addToCart.Id,
                    ProductId = addToCart.Id,
                    Name = addToCart.Name,
                    Price = addToCart.Price,
                    CoverImage = addToCart.CoverImage,
                    Quantity = 1,
                    StockQuantity = addToCart.StockQuantity,
                }
            };
            cart = JsonConvert.SerializeObject(newCart);
        }
        HttpContext.Session.SetString("Cart", cart);
        return Ok(new { success = true });
    }
    public IActionResult UpdateCart(int productId, int quantity)
    {
        string? userIdentityId = _userManager.GetUserId(User);
        if (!string.IsNullOrEmpty(userIdentityId))
        {
            int userId = _userService.GetUserById(userIdentityId).UserId;
            int cartId = _cartService.UpdateCart(productId, userId, quantity);
            if (cartId > 0)
                return Ok(new { redirect = Url.Action("Cart", "Cart") });
                // return RedirectToAction("Cart", "Home");
        }

        string cart = HttpContext.Session.GetString("Cart") ?? "";
        if (!string.IsNullOrEmpty(cart))
        {
            List<CartViewModel>? cartItems = JsonConvert.DeserializeObject<List<CartViewModel>>(cart);
            if (cartItems.Any(c => c.ProductId == productId))
            {
                CartViewModel? existingItem = cartItems.FirstOrDefault(c => c.ProductId == productId);
                if (existingItem != null)
                {
                    existingItem.Quantity = quantity;
                    cart = JsonConvert.SerializeObject(cartItems);
                    HttpContext.Session.SetString("Cart", cart);
                    return Ok(new { redirect = Url.Action("Cart", "Cart") });
                }
            }
        }
        return BadRequest();
    }
    public IActionResult RemoveFromCart(int productId)
    {
        string? userIdentityId = _userManager.GetUserId(User);
        if (!string.IsNullOrEmpty(userIdentityId))
        {
            int userId = _userService.GetUserById(userIdentityId).UserId;
            int cartId = _cartService.UpdateCart(productId, userId);
            if (cartId > 0)
                return Ok(new { redirect = Url.Action("Cart", "Cart") });
            return BadRequest();
        }

        string cart = HttpContext.Session.GetString("Cart") ?? "";
        if (!string.IsNullOrEmpty(cart))
        {
            List<CartViewModel>? cartItems = JsonConvert.DeserializeObject<List<CartViewModel>>(cart);
            if (cartItems.Any(c => c.ProductId == productId))
            {
                CartViewModel? existingItem = cartItems.FirstOrDefault(c => c.ProductId == productId);
                if (existingItem != null)
                {
                    cartItems.Remove(existingItem);
                    cart = JsonConvert.SerializeObject(cartItems);
                    HttpContext.Session.SetString("Cart", cart);
                    return Ok(new { redirect = Url.Action("Cart", "Cart") });
                }
            }
        }
        return Ok(new { status = AjaxError.NotFound.ToString() });
    }
    public IActionResult Cart(int pageNumber = 1, int pageSize = 8)
    {
        List<CartViewModel> cartItems = new();

        string? userIdentityId = _userManager.GetUserId(User);
        if (!string.IsNullOrEmpty(userIdentityId))
        {
            ViewBag.CurrentUser = _userService.GetUserById(userIdentityId).UserId;
            int userId = _userService.GetUserById(userIdentityId).UserId;
            cartItems = _cartService.GetCart(userId);
        }
        else
        {
            string cart = HttpContext.Session.GetString("Cart") ?? "";
            if (!string.IsNullOrEmpty(cart))
                cartItems = JsonConvert.DeserializeObject<List<CartViewModel>>(cart);
        }
        
        int totalRecords = cartItems.Count;
        CartViewModel cartView = new()
        {
            CartItems = cartItems,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
        };
        return View(cartView);
    }
}
