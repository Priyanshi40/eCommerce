using BLL.Interfaces;
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
    public IActionResult VendorList(string searchString, int pageNumber = 1, int pageSize = 5)
    {
        VendorDetailsViewModel vendorsView = _vendorService.GetVendorsService(searchString,pageNumber, pageSize);
        return PartialView("_vendorList", vendorsView);
    }
    public IActionResult VendorDetails(int vendorId)
    {
        VendorDetailsViewModel vendorsView = _vendorService.GetVendorDetailsService(vendorId);
        return PartialView("_vendorDetails", vendorsView);
    }
    
}
