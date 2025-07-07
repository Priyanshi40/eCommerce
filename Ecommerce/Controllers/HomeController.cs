using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ecommerce.Controllers;

public class HomeController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ICategoryService _catService;
    private readonly IProductService _proService;
    public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ICategoryService catService, IProductService proService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _catService = catService;
        _proService = proService;
    }
    public IActionResult Index(string statusFilter, string searchString)
    {
        List<Category> Categories = _catService.GetQueryableCategories(searchString, statusFilter).ToList();
        var CategoryView = new CategoryViewModel
        {
            Categories = Categories,
        };
        return View(CategoryView);
    }
    public IActionResult ProductByCategory(string searchString, int category, int pageNumber = 1, int pageSize = 5)
    {
        ProductViewModel productsView = _proService.GetProductsService(searchString, category, ProductStatus.Approved.ToString(), pageNumber, pageSize);
        return View("ProductList", productsView);
    }
    public IActionResult ProductDetails(int productId)
    {
        ProductViewModel productDetails = _proService.GetProductDetailsService(productId);
        return View("ProductDetails", productDetails);
    }
    public IActionResult AddToCart([FromBody] CartViewModel addToCart)
    {
        var cart = HttpContext.Session.GetString("Cart") ?? "";
        if (!string.IsNullOrEmpty(cart))
        {
            var cartItems = JsonConvert.DeserializeObject<List<CartViewModel>>(cart);
            if (cartItems.Any(c => c.ProductId == addToCart.ProductId))
            {
                var existingItem = cartItems.FirstOrDefault(c => c.ProductId == addToCart.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
            }
            else
            {
                cartItems.Add(new CartViewModel
                {
                    ProductId = addToCart.ProductId,
                    ProductName = addToCart.ProductName,
                    Price = addToCart.Price,
                    CoverImage = addToCart.CoverImage,
                    Quantity = 1
                });
            }
            cart = JsonConvert.SerializeObject(cartItems);
        }
        else
        {
            var cartItems = new List<CartViewModel>
            {
                new CartViewModel
                {
                    ProductId = addToCart.ProductId,
                    ProductName = addToCart.ProductName,
                    Price = addToCart.Price,
                    CoverImage = addToCart.CoverImage,
                    Quantity = 1
                }
            };
            cart = JsonConvert.SerializeObject(cartItems);
        }
        HttpContext.Session.SetString("Cart", cart);
        return RedirectToAction("Index", "Cart");
    }

    


}
