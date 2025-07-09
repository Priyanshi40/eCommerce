using System.Threading.Tasks;
using BLL.Interfaces;
using BLL.Services;
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
public class CustomerController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserService _userService;
    private readonly IVendorService _vendorService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly INotificationService _noficationService;
    public CustomerController(UserManager<IdentityUser> userManager, IUserService userService, IVendorService vendorService, IHubContext<NotificationHub> hubContext, INotificationService noficationService)
    {
        _userManager = userManager;
        _userService = userService;
        _vendorService = vendorService;
        _hubContext = hubContext;
        _noficationService = noficationService;
    }
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> CustomerList(string searchString, SortOrder sortOrder,int pageNumber = 1, int pageSize = 5)
    {
        UserViewModel users = await _userService.GetUsersService(searchString, sortOrder,pageNumber, pageSize);
        return PartialView("_customerList", users);
    }
    public IActionResult CustomerModal(int userId)
    {
        UserViewModel userDetails = _userService.GetUserById(userId);
        return PartialView("_customerModal", userDetails);
    }
    public IActionResult GetUserAddresses(int userId)
    {
        var addresses = _userService.GetUserAddresses(userId);
        return PartialView("_addresses", addresses);
    }
    public IActionResult GetStateByCountry(int countryId)
    {
        IEnumerable<State> stateList = _userService.GetStateService(countryId);
        return Json(stateList);
    }
    public IActionResult GetCityByState(int stateId)
    {
        IEnumerable<City> cityList = _userService.GetCityService(stateId);
        return Json(cityList);
    }

    [HttpPost]
    public IActionResult DetailsViaModel([FromBody] AddressViewModel model)
    {
        ViewBag.Countries = new SelectList(_userService.GetCountryService(), "Id", "Name");
        return View("AddressDetail", model);
    }
    public IActionResult EditDetails(RegisterViewModel userDetails)
    {
        var user = _userService.GetUserById(_userManager.GetUserId(User));
        userDetails.ModifiedBy = user.UserId;
        _userService.AddUser(userDetails);
        return RedirectToAction("CustomerList", "Customer");
    }
    public IActionResult EditAddress(AddressViewModel address)
    {
        if (!ModelState.IsValid)
            return Ok(new { status = AjaxError.ValidationError.ToString() });
        
        var user = _userService.GetUserById(_userManager.GetUserId(User));
        address.ModifiedBy = user.UserId;
        _userService.AddUserAddress(address);
        return Ok(new { status = AjaxError.Success.ToString() });
    }



}
