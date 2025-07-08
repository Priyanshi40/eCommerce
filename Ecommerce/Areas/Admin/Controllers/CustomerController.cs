using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Services;
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
public class CustomerController : Controller
{
    private readonly IUserService _userService;
    private readonly IVendorService _vendorService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly INotificationService _noficationService;
    public CustomerController(IUserService userService, IVendorService vendorService, IHubContext<NotificationHub> hubContext, INotificationService noficationService)
    {
        _userService = userService;
        _vendorService = vendorService;
        _hubContext = hubContext;
        _noficationService = noficationService;
    }
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> CustomerList(string searchString, SortOrder sortOrder, string statusFilter, int pageNumber = 1, int pageSize = 5)
    {
        UserViewModel users = await _userService.GetUsersService(searchString, sortOrder, statusFilter, pageNumber, pageSize);
        return PartialView("_customerList", users);
    }
    public IActionResult CustomerModal(int userId)
    {
        UserViewModel userDetails = _userService.GetUserById(userId);
        return PartialView("_customerModal", userDetails);
    }

    public IActionResult EditDetails(UserViewModel userDetails)
    {
        if (!ModelState.IsValid)
        {
            return Ok(new { status = AjaxError.Error.ToString() });
        }
        else
        {
            // var user = _userService.GetUserById(_userManager.GetUserId(User));

            //     userDetails.UpdatedBy = user.UserId;

            // _userService.UpSertCategory(userDetails);
            return RedirectToAction("CustomerList", "Customer");
        }
    }

    
    
}
