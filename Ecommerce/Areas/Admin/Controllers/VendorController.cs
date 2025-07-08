using BLL.Interfaces;
using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;
using Ecommerce.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Ecommerce.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize(Roles = "Admin")]
public class VendorController : Controller
{
    private readonly IVendorService _vendorService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly INotificationService _noficationService;
    public VendorController(IVendorService vendorService, IHubContext<NotificationHub> hubContext, INotificationService noficationService)
    {
        _vendorService = vendorService;
        _hubContext = hubContext;
        _noficationService = noficationService;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult VendorList(string searchString,SortOrder sortOrder, string statusFilter, int pageNumber = 1, int pageSize = 5)
    {
        VendorDetailsViewModel vendorsView = _vendorService.GetVendorsService(searchString,sortOrder,statusFilter, pageNumber, pageSize);
        return PartialView("_vendorList", vendorsView);
    }
    public IActionResult VendorDetails(int vendorId)
    {
        VendorDetailsViewModel vendorsView = _vendorService.GetVendorDetailsService(vendorId);
        return PartialView("_vendorDetails", vendorsView);
    }

    [HttpPost]
    public async Task<IActionResult> VendorAction([FromBody] UserDetails vendor)
    {
        if (vendor.Id <= 0)
            return Ok(new { status = AjaxError.NotFound.ToString() });

        var userId = _vendorService.ApproveVendor(vendor);
        if (!string.IsNullOrEmpty(userId))
        {
             var notification = new Notification
            {
                Message = "Your Profile has been " + vendor.Status + " By Admin !!",
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
            
        return RedirectToAction("VendorList");
    }
    
}
