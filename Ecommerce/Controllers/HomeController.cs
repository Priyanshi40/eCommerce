using BLL.Interfaces;
using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ecommerce.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserService _userService;
    private readonly ICategoryService _catService;
    private readonly IProductService _proService;
    public HomeController(UserManager<IdentityUser> userManager,IUserService userService,ICategoryService catService, IProductService proService)
    {
        _userManager = userManager;
        _userService = userService;
        _catService = catService;
        _proService = proService;
    }
    public IActionResult Index(string statusFilter, string searchString)
    {
        List<Category> Categories = _catService.GetQueryableCategories(searchString, SortOrder.Name, statusFilter).ToList();
        var CategoryView = new CategoryViewModel
        {
            Categories = Categories,
        };
        return View(CategoryView);
    }
    public IActionResult ProductByCategory(string searchString, int category, int pageNumber = 1, int pageSize = 15)
    {
        var userIdentityId = _userManager.GetUserId(User);
        if(userIdentityId != null)
            ViewBag.CurrentUser = _userService.GetUserById(userIdentityId).UserId;

        ProductViewModel productsView = _proService.GetProductsService(searchString, SortOrder.Name, category, ProductStatus.Approved.ToString(), pageNumber, pageSize);
        return View("ProductList", productsView);
    }
    public IActionResult ProductDetails(int productId)
    {
        var userIdentityId = _userManager.GetUserId(User);
        if(userIdentityId != null)
            ViewBag.CurrentUser = _userService.GetUserById(userIdentityId).UserId;  

        ProductViewModel productDetails = _proService.GetProductDetailsService(productId);
        return View("ProductDetails", productDetails);
    }

    public IActionResult GetCartCount()
    {
        var count = 0;
        var cart = HttpContext.Session.GetString("Cart") ?? "";
        if (!string.IsNullOrEmpty(cart))
            count = JsonConvert.DeserializeObject<List<CartViewModel>>(cart).Count;

        return Json(new { count });
    }

    [HttpPost]
    public IActionResult AddToCart([FromBody] CartViewModel addToCart)
    {
        var cart = HttpContext.Session.GetString("Cart") ?? "";
        if (!string.IsNullOrEmpty(cart))
        {
            var cartItems = JsonConvert.DeserializeObject<List<CartViewModel>>(cart);
            if (cartItems.Any(c => c.ProductId == addToCart.Id))
            {
                var existingItem = cartItems.FirstOrDefault(c => c.ProductId == addToCart.Id);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
            }
            else
            {
                cartItems.Add(new CartViewModel
                {
                    ProductId = addToCart.Id,
                    Name = addToCart.Name,
                    Price = addToCart.Price,
                    CoverImage = addToCart.CoverImage,
                    Quantity = 1,
                    StockQuantity = addToCart.StockQuantity,
                });
            }
            cart = JsonConvert.SerializeObject(cartItems);
        }
        else
        {
            var newCart = new List<CartViewModel>
            {
                new CartViewModel
                {
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
        return Ok(new { status = AjaxError.Success.ToString() });
    }
    public IActionResult UpdateCart(int productId,int quantity)
    {
        var cart = HttpContext.Session.GetString("Cart") ?? "";
        if (!string.IsNullOrEmpty(cart))
        {
            var cartItems = JsonConvert.DeserializeObject<List<CartViewModel>>(cart);
            if (cartItems.Any(c => c.ProductId == productId))
            {
                var existingItem = cartItems.FirstOrDefault(c => c.ProductId == productId);
                if (existingItem != null)
                {
                    existingItem.Quantity = quantity;
                    cart = JsonConvert.SerializeObject(cartItems);
                    HttpContext.Session.SetString("Cart", cart);
                    return RedirectToAction("Cart", "Home");
                }
            }
        }
        return Ok(new { status = AjaxError.NotFound.ToString() });
    }
    public IActionResult RemoveFromCart(int productId)
    {
        var cart = HttpContext.Session.GetString("Cart") ?? "";
        if (!string.IsNullOrEmpty(cart))
        {
            var cartItems = JsonConvert.DeserializeObject<List<CartViewModel>>(cart);
            if (cartItems.Any(c => c.ProductId == productId))
            {
                var existingItem = cartItems.FirstOrDefault(c => c.ProductId == productId);
                if (existingItem != null)
                {
                    cartItems.Remove(existingItem);
                    cart = JsonConvert.SerializeObject(cartItems);
                    HttpContext.Session.SetString("Cart", cart);
                    return RedirectToAction("Cart", "Home");
                }
            }
        }
        return Ok(new { status = AjaxError.NotFound.ToString() });
    }

    public IActionResult Cart(int pageNumber = 1, int pageSize = 8)
    {
        var userIdentityId = _userManager.GetUserId(User);
        if(userIdentityId != null)
            ViewBag.CurrentUser = _userService.GetUserById(userIdentityId).UserId;

            
        var cart = HttpContext.Session.GetString("Cart") ?? "";
        List<CartViewModel> cartItems = new();
        if (!string.IsNullOrEmpty(cart))
            cartItems = JsonConvert.DeserializeObject<List<CartViewModel>>(cart);

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
