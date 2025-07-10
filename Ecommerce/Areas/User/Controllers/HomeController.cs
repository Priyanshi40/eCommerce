using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles = "User")]
public class HomeController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserService _userService;
    private readonly ICartService _cartService;
    public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserService userService, ICartService cartService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userService = userService;
        _cartService = cartService;
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Account", new { area = "" });
    }
}
