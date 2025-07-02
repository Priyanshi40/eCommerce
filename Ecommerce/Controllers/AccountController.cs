using BLL.Interfaces;
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
    private readonly IUserService _userService;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IUserService userService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Account", new { area = "Admin" });

            if (User.IsInRole("User"))
                return RedirectToAction("Index", "Account", new { area = "User" });

            if (User.IsInRole("Vendor"))
                return RedirectToAction("Index", "Account", new { area = "Vendor" });
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
                if (result.Succeeded && User.Identity.IsAuthenticated)
                {
                    TempData["Message"] = "Welcome" + model.Email;

                    if (User.IsInRole("Admin") || role.Contains("Admin"))
                        return RedirectToAction("Index", "Account", new { area = "Admin" });

                    if (User.IsInRole("User"))
                        return RedirectToAction("Index", "Account", new { area = "User" });

                    if (User.IsInRole("Vendor"))
                        return RedirectToAction("Index", "Account", new { area = "Vendor" });
                }
            }
            TempData["Error"] = "Invalid login attempt.";
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterCustomer(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            IdentityUser user = new()
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone,
            };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                model.IdentityUserId = user.Id;
                _userService.AddUser(model);

                await _userManager.AddToRoleAsync(user, "User");

                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["Message"] = "User Registered Successfully";
                return RedirectToAction("Login", "Account");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View("Register", model);
    }
    // public async Task<IActionResult> CreateRole()
    // {
    //     IdentityRole identityRole = new()
    //     {
    //         Name = "Vendor"
    //     };
    //     IdentityResult result = await _roleManager.CreateAsync(identityRole);
    //     if (result.Succeeded)
    //     {
    //         TempData["Message"] = "Role Created";
    //     }
    //     else
    //     {
    //         TempData["Error"] = "Error Creating Role";
    //     }
    //     return RedirectToAction("Login", "Account");
    // }
}