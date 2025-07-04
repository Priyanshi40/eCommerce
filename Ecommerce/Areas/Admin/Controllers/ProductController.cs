using BLL.Interfaces;
using BLL.Utility;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserService _userService;
    private readonly ICategoryService _catService;
    private readonly IProductService _proService;
    public ProductController(UserManager<IdentityUser> userManager,IUserService userService, ICategoryService catService, IProductService proService)
    {
        _userManager = userManager;
        _userService = userService;
        _catService = catService;
        _proService = proService;
    }
    public IActionResult Index()
    {
        ViewBag.Category = new SelectList(_catService.GetCategoriesService(), "Id", "Name");
        return View();
    }
    public IActionResult ProductList(string searchString,int category,string statusFilter, int pageNumber = 1, int pageSize = 5)
    {
        ProductViewModel productsView = _proService.GetProductsService(searchString,category,statusFilter, pageNumber, pageSize);
        return PartialView("_productList", productsView);
    }
    public IActionResult ProductDetails(int productId)
    {
        ProductViewModel productDetails = _proService.GetProductDetailsService(productId);
        return View("ProductDetails", productDetails);
    }
    public IActionResult ApproveProduct(int productId)
    {
        if (productId <= 0)
        {
            return Ok(new { status = AjaxError.NotFound.ToString() });
        }
        Product product = new()
        {
            Id = productId,
            ModifiedBy = _userService.GetUserById(_userManager.GetUserId(User)).UserId
        };
        var result = _proService.ApproveProduct(product);
        if (!result)
        {
            return Ok(new { status = AjaxError.NotFound.ToString() });
        }
        return RedirectToAction("ProductList");
    }
}
