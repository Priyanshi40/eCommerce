using BLL.Interfaces;
using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    private string? GetUserIdentityId() => _userManager.GetUserId(User);
    private int GetAppUserId(string identityId) => _userService.GetUserById(identityId).UserId;
    private bool IsAuthenticated() => !string.IsNullOrEmpty(GetUserIdentityId());

    public IActionResult Index(string statusFilter, string searchString)
    {
        List<Category> Categories = _catService.GetCategoriesService();
        CategoryViewModel CategoryView = new ()
        {
            Categories = Categories,
        };
        return View(CategoryView);
    }
    public IActionResult ProductByCategory(string searchString, int category, int pageNumber = 1, int pageSize = 15)
    {
        string? userIdentityId = GetUserIdentityId();
        if (userIdentityId != null)
            ViewBag.CurrentUser = _userService.GetUserById(userIdentityId).UserId;

        ProductViewModel productsView = _proService.GetProductsService(searchString, SortOrder.Name, category, ProductStatus.Approved.ToString(), pageNumber, pageSize);
        productsView.CategoryId = category;
        return View("ProductList", productsView);
    }
    public IActionResult ProductDetails(int productId)
    {
        if (IsAuthenticated())
        {
            int userId = GetAppUserId(GetUserIdentityId()!);
            ViewBag.CurrentUser = userId;
        }

        ProductViewModel productDetails = _proService.GetProductDetailsService(productId);
        if (productDetails.Id < 1)
        {
            TempData["Error"] = "Product doesn't exist!!";
            return RedirectToAction("ProductByCategory");
        }
        return View("ProductDetails", productDetails);
    }


}
