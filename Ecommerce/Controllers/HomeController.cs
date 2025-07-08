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
    private readonly ICategoryService _catService;
    private readonly IProductService _proService;
    public HomeController(ICategoryService catService, IProductService proService)
    {
        _catService = catService;
        _proService = proService;
    }
    public IActionResult Index(string statusFilter, string searchString)
    {
        List<Category> Categories = _catService.GetQueryableCategories(searchString,SortOrder.Name, statusFilter).ToList();
        var CategoryView = new CategoryViewModel
        {
            Categories = Categories,
        };
        return View(CategoryView);
    }
    public IActionResult ProductByCategory(string searchString, int category, int pageNumber = 1, int pageSize = 5)
    {
        ProductViewModel productsView = _proService.GetProductsService(searchString,SortOrder.Name, category, ProductStatus.Approved.ToString(), pageNumber, pageSize);
        return View("ProductList", productsView);
    }
    public IActionResult ProductDetails(int productId)
    {
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
                    Quantity = 1
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
                    Quantity = 1
                }
            };
            cart = JsonConvert.SerializeObject(newCart);
        }
        HttpContext.Session.SetString("Cart", cart);
        return Ok(new { status = AjaxError.Success.ToString() });
    }

    public IActionResult Cart(int pageNumber = 1, int pageSize = 8)
    {
        var cart = HttpContext.Session.GetString("Cart") ?? "";
        var cartItems = JsonConvert.DeserializeObject<List<CartViewModel>>(cart);
        int totalRecords = cartItems != null ? cartItems.Count : 0;
        CartViewModel cartView = new ()
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
