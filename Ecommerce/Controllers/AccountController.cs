using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace Ecommerce.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Admin");

            if (User.IsInRole("User"))
                return RedirectToAction("Index", "User");
        }
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var role = await _userManager.GetRolesAsync(user);

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (_signInManager.IsSignedIn(User) && (User.IsInRole("Admin") || role.Contains("Admin")))
                    {
                        TempData["Message"] = "Welcome" + model.Email;
                        return RedirectToAction("Index", "Admin");
                    }
                    if (_signInManager.IsSignedIn(User) && User.IsInRole("User"))
                    {
                        TempData["Message"] = "Welcome" + model.Email;
                        return RedirectToAction("Index", "User");
                    }
                }
            }
            TempData["Error"] = "Invalid login attempt.";
        }
        return View(model);
    }
    
    public async Task<IActionResult> CreateRole()
    {
        IdentityRole identityRole = new()
        {
            Name = "User"
        };
        IdentityResult result = await _roleManager.CreateAsync(identityRole);
        if (result.Succeeded)
        {
            TempData["Message"] = "Role Created";
        }
        else
        {
            TempData["Error"] = "Error Creating Role";
        }
        return RedirectToAction("Login", "Account");
    }
}