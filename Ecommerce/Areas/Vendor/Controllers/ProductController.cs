using BLL.Interfaces;
using BLL.Utility;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ecommerce.Areas.Vendor.Controllers;

[Area("Vendor")]
[Authorize(Roles = "Vendor")]
public class ProductController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserService _userService;
    private readonly ICategoryService _catService;
    public readonly ImageService _imgService;
    private readonly IProductService _proService;

    public ProductController(UserManager<IdentityUser> userManager, ICategoryService catService, IUserService userService, ImageService imgService, IProductService proService)
    {
        _userManager = userManager;
        _catService = catService;
        _userService = userService;
        _imgService = imgService;
        _proService = proService;
    }
    public IActionResult Index()
    {
        ViewBag.Category = new SelectList(_catService.GetCategoriesService(), "Id", "Name");
        return View();
    }
    public IActionResult ProductList(string searchString,int category,string statusFilter, int pageNumber = 1, int pageSize = 5)
    {
        var user = _userService.GetUserById(_userManager.GetUserId(User));
        ProductViewModel productsView = _proService.GetProductsService(searchString,category,statusFilter, pageNumber, pageSize,user.UserId);
        return PartialView("_productList", productsView);
    }
    public IActionResult ProductModal(int productId)
    {
        ViewBag.Category = new SelectList(_catService.GetCategoriesService(), "Id", "Name");
        ProductViewModel productDetails = _proService.GetProductDetailsService(productId);
        return PartialView("_productModal", productDetails);
    }
    public IActionResult ProductDetails(int productId)
    {
        ProductViewModel productDetails = _proService.GetProductDetailsService(productId);
        return View("ProductDetails", productDetails);
    }
    public IActionResult AddProduct(ProductViewModel productToAdd, IFormFile? ProductImage, string? RemovedImages)
    {
        if (!ModelState.IsValid)
        {
            return Ok(new { status = AjaxError.ValidationError.ToString() });
        }
        else
        {
            if (ProductImage != null)
            {
                productToAdd.CoverImage = _imgService.SaveImageService(ProductImage);
            }

            productToAdd.RemovedImages = RemovedImages?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            var user = _userService.GetUserById(_userManager.GetUserId(User));
            if (productToAdd.Id == 0)
            {
                productToAdd.CreatedBy = user.UserId;
                productToAdd.ModifiedBy = user.UserId;
            }
            else
            {
                productToAdd.ModifiedBy = user.UserId;
            }
            _proService.UpSertProduct(productToAdd);
            return RedirectToAction("ProductList", "Product");
        }
    }
    public IActionResult DeleteProduct(int productId)
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
        var isDeleted = _proService.DeleteProduct(product);
        if (!isDeleted)
        {
            return Ok(new { status = AjaxError.NotFound.ToString() });
        }
        return RedirectToAction("ProductList", "Product");
    }
}
