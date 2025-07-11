using BLL.Interfaces;
using BLL.Utility;
using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;
using Ecommerce.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace Ecommerce.Areas.Vendor.Controllers;

[Area("Vendor")]
[Authorize(Roles = "Vendor")]
public class ProductController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUserService _userService;
    private readonly ICategoryService _catService;
    public readonly ImageService _imgService;
    private readonly IProductService _proService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly INotificationService _noficationService;

    public ProductController(UserManager<IdentityUser> userManager,
                            ICategoryService catService,
                            IUserService userService,
                            ImageService imgService,
                            IProductService proService,
                            RoleManager<IdentityRole> roleManager,
                            IHubContext<NotificationHub> hubContext,
                            INotificationService noficationService)
    {
        _userManager = userManager;
        _catService = catService;
        _userService = userService;
        _imgService = imgService;
        _proService = proService;
        _roleManager = roleManager;
        _hubContext = hubContext;
        _noficationService = noficationService;
    }
    private int GetCurrentUserId() => _userService.GetUserById(_userManager.GetUserId(User)!).UserId;
    public IActionResult Index()
    {
        ViewBag.Category = new SelectList(_catService.GetCategoriesService(), "Id", "Name");
        return View();
    }
    public IActionResult ProductList(string searchString, SortOrder sortOrder, int category, string statusFilter, int pageNumber = 1, int pageSize = 5)
    {
        int userId = GetCurrentUserId();
        ProductViewModel productsView = _proService.GetProductsService(searchString, sortOrder, category, statusFilter, pageNumber, pageSize, userId);
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

    public async Task<IActionResult> AddProduct(ProductViewModel productToAdd, IFormFile? ProductImage, string? RemovedImages)
    {
        if (!ModelState.IsValid)
            return Ok(new { status = AjaxError.ValidationError.ToString() });

        else
        {
            if (ProductImage != null)
                productToAdd.CoverImage = _imgService.SaveImageService(ProductImage);

            productToAdd.RemovedImages = RemovedImages?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList();

            int userId = GetCurrentUserId();
            if (productToAdd.Id == 0)
            {
                productToAdd.CreatedBy = userId;
                productToAdd.ModifiedBy = userId;
            }
            else
                productToAdd.ModifiedBy = userId;

            _proService.UpSertProduct(productToAdd);

            Notification notification = new()

            {
                Message = productToAdd.Id == 0 ? "New Product has been added!!" : "Product has been Updated",
                IsRead = false,
                UserId = "ba76242f-8d36-4bf7-ab67-b8bdcb0552d3",
            };
            _noficationService.AddNotification(notification);
            if (_hubContext?.Clients != null && !string.IsNullOrEmpty(notification.UserId))
            {
                await _hubContext.Clients.User(notification.UserId).SendAsync("ReceiveNotification", notification.Message);
            }
            return RedirectToAction("ProductList", "Product");
        }
    }
    public IActionResult DeleteProduct(int productId)
    {
        if (productId <= 0)
            return Ok(new { status = AjaxError.NotFound.ToString() });

        Product product = new()
        {
            Id = productId,
            ModifiedBy = GetCurrentUserId(),
        };

        bool isDeleted = _proService.DeleteProduct(product);

        if (!isDeleted)
            return Ok(new { status = AjaxError.NotFound.ToString() });

        return RedirectToAction("ProductList", "Product");
    }
}
