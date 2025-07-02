using BLL.Interfaces;
using DAL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Vendor.Controllers;

[Area("Vendor")]
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserService _userService;
    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserService userService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userService = userService;
    }
    public IActionResult Index()
    {
        return View();
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
        return View("Register",model);
    }

}
