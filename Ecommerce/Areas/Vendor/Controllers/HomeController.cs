using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Vendor.Controllers;

[Area("Vendor")]
[Authorize(Roles = "Vendor")]
public class HomeController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    public HomeController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }
    public IActionResult Index()
    {
        return View();
    }
}
