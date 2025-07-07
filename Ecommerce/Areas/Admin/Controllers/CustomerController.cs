using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CustomerController : Controller
{
    private readonly IUserService _userService;
    private readonly IVendorService _vendorService;
    public CustomerController(IUserService userService, IVendorService vendorService)
    {
        _userService = userService;
        _vendorService = vendorService;
    }
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> CustomerList(string searchString, string statusFilter, int pageNumber = 1, int pageSize = 5)
    {
        UserViewModel users = await _userService.GetUsersService(searchString, statusFilter, pageNumber, pageSize);
        return PartialView("_customerList", users);
    }
    [HttpPost]
    public IActionResult UserAction([FromBody] UserDetails vendor)
    {
        if (vendor.Id <= 0)
            return Ok(new { status = AjaxError.NotFound.ToString() });

        var result = _vendorService.ApproveVendor(vendor);

        if (!result)
            return Ok(new { status = AjaxError.NotFound.ToString() });
            
        return RedirectToAction("CustomerList");
    }
    
    
}
