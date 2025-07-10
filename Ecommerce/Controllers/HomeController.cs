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
    public HomeController(UserManager<IdentityUser> userManager, IUserService userService, ICategoryService catService, IProductService proService)
    {
        _userManager = userManager;
        _userService = userService;
        _catService = catService;
        _proService = proService;
    }
    public IActionResult Index(string statusFilter, string searchString)
    {
        List<Category> Categories = _catService.GetQueryableCategories(searchString, SortOrder.Name, statusFilter).ToList();
        CategoryViewModel CategoryView = new CategoryViewModel
        {
            Categories = Categories,
        };
        return View(CategoryView);
    }
    public IActionResult ProductByCategory(string searchString, int category, int pageNumber = 1, int pageSize = 15)
    {
        string? userIdentityId = _userManager.GetUserId(User);
        if (userIdentityId != null)
            ViewBag.CurrentUser = _userService.GetUserById(userIdentityId).UserId;

        ProductViewModel productsView = _proService.GetProductsService(searchString, SortOrder.Name, category, ProductStatus.Approved.ToString(), pageNumber, pageSize);
        productsView.CategoryId = category;
        return View("ProductList", productsView);
    }
    public IActionResult ProductDetails(int productId)
    {
        string? userIdentityId = _userManager.GetUserId(User);
        if (userIdentityId != null)
            ViewBag.CurrentUser = _userService.GetUserById(userIdentityId).UserId;

        ProductViewModel productDetails = _proService.GetProductDetailsService(productId);
        return View("ProductDetails", productDetails);
    }

    
}
