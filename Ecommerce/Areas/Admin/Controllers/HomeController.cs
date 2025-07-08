using BLL.Interfaces;
using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize(Roles = "Admin")]
public class HomeController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    private readonly IUserService _userService;
    private readonly INotificationService _notify;
    public HomeController(SignInManager<IdentityUser> signInManager,UserManager<IdentityUser> userManager, IUserService userService, INotificationService notify)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _userService = userService;
        _notify = notify;
    }
    public IActionResult Index()
    {
        return View();
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
