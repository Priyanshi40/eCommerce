using BLL.Interfaces;
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
    private readonly IAddressService _addService;
    private readonly IVendorService _vendorService;
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly INotificationService _noficationService;
    public CustomerController(UserManager<IdentityUser> userManager,
                                IUserService userService,
                                IAddressService addService,
                                IVendorService vendorService,
                                IHubContext<NotificationHub> hubContext,
                                INotificationService noficationService)
    {
        _userManager = userManager;
        _userService = userService;
        _addService = addService;
        _vendorService = vendorService;
        _hubContext = hubContext;
        _noficationService = noficationService;
    }
    private string? GetUserIdentityId() => _userManager.GetUserId(User);
    private int GetAppUserId(string identityId) => _userService.GetUserById(identityId).UserId;

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
        AddressViewModel addresses = _addService.GetUserAddresses(userId);
        return PartialView("_addresses", addresses);
    }

    [AllowAnonymous]
    public IActionResult GetStateByCountry(int countryId)
    {
        IEnumerable<State> stateList = _addService.GetStateService(countryId);
        return Json(stateList);
    }

    [AllowAnonymous]
    public IActionResult GetCityByState(int stateId)
    {
        IEnumerable<City> cityList = _addService.GetCityService(stateId);
        return Json(cityList);
    }

    [HttpPost]
    public IActionResult DetailsViaModel([FromBody] AddressViewModel model)
    {
        ViewBag.Countries = new SelectList(_addService.GetCountryService(), "Id", "Name");
        return View("AddressDetail", model);
    }
    public IActionResult EditDetails(RegisterViewModel userDetails)
    {
        userDetails.ModifiedBy = GetAppUserId(GetUserIdentityId()!);
        _userService.AddUser(userDetails);
        return RedirectToAction("CustomerList", "Customer");
    }
    public IActionResult EditAddress(AddressViewModel address)
    {
        if (!ModelState.IsValid)
            return Ok(new { status = AjaxError.ValidationError.ToString() });

        int userId = GetAppUserId(GetUserIdentityId()!);
        address.ModifiedBy = userId;
        _addService.AddUserAddress(address);
        return Ok(new { status = AjaxError.Success.ToString() });
    }



}
