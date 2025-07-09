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
    public HomeController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IUserService userService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _userService = userService;
    }
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account", new { area = "" });
    }
}
