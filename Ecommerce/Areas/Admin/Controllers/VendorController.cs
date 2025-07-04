using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Admin.Controllers;


[Area("Admin")]
[Authorize(Roles = "Admin")]
public class VendorController : Controller
{
    private readonly IVendorService _vendorService;
    public VendorController(IVendorService vendorService)
    {
        _vendorService = vendorService;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult VendorList(string searchString, string statusFilter, int pageNumber = 1, int pageSize = 5)
    {
        VendorDetailsViewModel vendorsView = _vendorService.GetVendorsService(searchString,statusFilter, pageNumber, pageSize);
        return PartialView("_vendorList", vendorsView);
    }
    public IActionResult VendorDetails(int vendorId)
    {
        VendorDetailsViewModel vendorsView = _vendorService.GetVendorDetailsService(vendorId);
        return PartialView("_vendorDetails", vendorsView);
    }

    [HttpPost]
    public IActionResult VendorAction([FromBody] UserDetails vendor)
    {
        if (vendor.Id <= 0)
            return Ok(new { status = AjaxError.NotFound.ToString() });

        var result = _vendorService.ApproveVendor(vendor);

        if (!result)
            return Ok(new { status = AjaxError.NotFound.ToString() });
            
        return RedirectToAction("VendorList");
    }
    
}
