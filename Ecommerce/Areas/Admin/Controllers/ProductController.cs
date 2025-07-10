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

namespace Ecommerce.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserService _userService;
    private readonly ICategoryService _catService;
    private readonly IProductService _proService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly INotificationService _noficationService;
    public ProductController(UserManager<IdentityUser> userManager, IUserService userService, ICategoryService catService, IProductService proService, IHubContext<NotificationHub> hubContext, INotificationService noficationService)
    {
        _userManager = userManager;
        _userService = userService;
        _catService = catService;
        _proService = proService;
        _hubContext = hubContext;
        _noficationService = noficationService;
    }
    public IActionResult Index()
    {
        ViewBag.Category = new SelectList(_catService.GetCategoriesService(), "Id", "Name");
        return View();
    }
    public IActionResult ProductList(string searchString,SortOrder sortOrder, int category, string statusFilter, int pageNumber = 1, int pageSize = 5)
    {
        ProductViewModel productsView = _proService.GetProductsService(searchString,sortOrder,category, statusFilter, pageNumber, pageSize);
        return PartialView("_productList", productsView);
    }
    public IActionResult ProductDetails(int productId)
    {
        ProductViewModel productDetails = _proService.GetProductDetailsService(productId);
        return View("ProductDetails", productDetails);
    }
    public async Task<IActionResult> ProductAction([FromBody] Product productToModify)
    {
        if (productToModify.Id <= 0)
            return Ok(new { status = AjaxError.NotFound.ToString() });

        productToModify.ModifiedBy = _userService.GetUserById(_userManager.GetUserId(User)).UserId;

        string? userId = _proService.ApproveProduct(productToModify);

        if (!string.IsNullOrEmpty(userId))
        {
            Notification notification = new Notification
            {
                Message = "Your Product has been " + productToModify.Status + " By Admin !!",
                IsRead = false,
                UserId = userId,
            };
            _noficationService.AddNotification(notification);
            if (_hubContext?.Clients != null && !string.IsNullOrEmpty(notification.UserId))
            {
                await _hubContext.Clients.User(notification.UserId).SendAsync("ReceiveNotification", notification.Message);
            }
        }
        else
            return Ok(new { status = AjaxError.NotFound.ToString() });

        return RedirectToAction("ProductList");
    }
}
