using BLL.Interfaces;
using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.User.Controllers;


[Area("User")]
[Authorize(Roles = "User")]
public class HomeController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserService _userService;
    private readonly ICategoryService _catService;
    private readonly IProductService _proService;
    private readonly INotificationService _notify;
    public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ICategoryService catService, IProductService proService, IUserService userService, INotificationService notify)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _catService = catService;
        _proService = proService;
        _userService = userService;
        _notify = notify;
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
        ViewBag.CurrentUser = _userService.GetUserById(_userManager.GetUserId(User)).UserId;
        ProductViewModel productsView = _proService.GetProductsService(searchString,SortOrder.Name, category, ProductStatus.Approved.ToString(), pageNumber, pageSize);
        return View("ProductList", productsView);
    }
    public IActionResult ProductDetails(int productId)
    {
        ProductViewModel productDetails = _proService.GetProductDetailsService(productId);
        return View(productDetails);
    }

    public IActionResult GetNotificationCount()
    {
        var userId = _userManager.GetUserId(User);
        var count = _notify.GetNotificationCount(userId);
        return Json(new { count });
    }
    public IActionResult GetNotification()
    {
        var userId = _userManager.GetUserId(User);
        List<Notification> notifications = _notify.GetNotifications(userId);
        return PartialView("_notification", notifications);
        // return Json(notifications);
    }
    public IActionResult MarkAsRead(int notificationId)
    {
        bool notifications = _notify.MarkAsRead(notificationId);
        if (notifications)
            return Ok(new { status = AjaxError.Success.ToString() });
        else
            return Ok(new { status = AjaxError.Error.ToString() });    
    }
    public IActionResult MarkAllAsRead()
    {
        var userId = _userManager.GetUserId(User);
        bool notifications = _notify.MarkAllAsRead(userId);
        if (notifications)
            return Ok(new { status = AjaxError.Success.ToString() });
        else
            return Ok(new { status = AjaxError.NotFound.ToString() });
            
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account", new { area = "" });
    }
}
