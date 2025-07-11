using BLL.Interfaces;
using DAL.Enums;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[Authorize]
public class NotificationController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    private readonly IUserService _userService;
    private readonly INotificationService _notify;
    public NotificationController(UserManager<IdentityUser> userManager, IUserService userService, INotificationService notify)
    {
        _userManager = userManager;
        _userService = userService;
        _notify = notify;
    }
    private string? GetUserIdentityId() => _userManager.GetUserId(User);
    public IActionResult GetNotificationCount()
    {
        string? userId = GetUserIdentityId();
        int count = _notify.GetNotificationCount(userId);
        return Json(new { count });
    }
    public IActionResult GetNotification()
    {
        string? userId = GetUserIdentityId();
        List<Notification> notifications = _notify.GetNotifications(userId);
        return PartialView("_notification", notifications);
    }
    public IActionResult MarkAsRead(int notificationId)
    {
        bool notifications = _notify.MarkAsRead(notificationId);
        if (notifications)  return Ok(new { status = AjaxError.Success.ToString() });
        return Ok(new { status = AjaxError.Error.ToString() });    
    }
    public IActionResult MarkAllAsRead()
    {
        string? userId = GetUserIdentityId();
        bool notifications = _notify.MarkAllAsRead(userId);
        if (notifications) return Ok(new { status = AjaxError.Success.ToString() });
        return Ok(new { status = AjaxError.NotFound.ToString() });   
    }
}
