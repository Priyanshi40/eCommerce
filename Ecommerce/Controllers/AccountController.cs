using BLL.Interfaces;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            var userRole = _userManager.GetRolesAsync(_userManager.GetUserAsync(User).Result).Result;
            return RedirectToAction("Index", "Home", new { area = userRole[0].ToString() });
        }
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
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded && User.Identity.IsAuthenticated)
                {
                    TempData["Message"] = "Welcome" + model.Email;
                    var userRole = _userManager.GetRolesAsync(_userManager.GetUserAsync(User).Result).Result;
                    return RedirectToAction("Index", "Home", new { area = userRole[0].ToString() });
                }
            }
            TempData["Error"] = "Invalid login attempt.";
        }
        return View(model);
    }
    public IActionResult Register()
    {
        return View();
    }
    public IActionResult CommonRegistration()
    {
        return PartialView("_commonRegistration");
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
                ModelState.AddModelError(string.Empty, error.Description);
        }
        return View("Register", model);
    }
    public IActionResult VendorBusinessDetails(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return PartialView("_vendorBusiness", model);

        HttpContext.Session.SetString("VendorStep1", JsonConvert.SerializeObject(model));
        // TempData["VendorStep1"] = JsonConvert.SerializeObject(model);

        return PartialView("_vendorBusiness", new VendorViewModel());
    }
    public IActionResult VendorDocuments(VendorViewModel model)
    {
        foreach (var key in new[] { "FileUrl", "DocumentType" })
        {
            ModelState.Remove(key);
        }

        if (!ModelState.IsValid)
            return PartialView("_vendorDocuments", model);

        HttpContext.Session.SetString("VendorStep2", JsonConvert.SerializeObject(model));
        // TempData["VendorStep2"] = JsonConvert.SerializeObject(model);
        return PartialView("_vendorDocuments", model);
    }
    public async Task<IActionResult> RegisterVendor(VendorViewModel model)
    {
        foreach (var key in new[] { "BusinessName", "GSTNumber" })
        {
            ModelState.Remove(key);
        }

        if (!ModelState.IsValid)
            return PartialView("_vendorDocuments", model);

        var step1Json = HttpContext.Session.GetString("VendorStep1");
        var step2Json = HttpContext.Session.GetString("VendorStep2");

        // var step1Json = TempData["VendorStep1"]?.ToString();
        // var step2Json = TempData["VendorStep2"]?.ToString();

        if (string.IsNullOrWhiteSpace(step1Json) || string.IsNullOrWhiteSpace(step2Json))
        {
            ModelState.AddModelError("", "Session expired. Please complete steps again.");
            return RedirectToAction("Register");
        }

        var step1 = JsonConvert.DeserializeObject<RegisterViewModel>(step1Json);
        var step2 = JsonConvert.DeserializeObject<VendorViewModel>(step2Json);

        IdentityUser user = new()
        {
            UserName = step1.Email,
            Email = step1.Email,
            PhoneNumber = step1.Phone,
        };
        IdentityResult result = await _userManager.CreateAsync(user, step1.Password);
        if (result.Succeeded)
        {
            var user1 = new RegisterViewModel
            {
                FirstName = step1.FirstName,
                LastName = step1.LastName,
                IdentityUserId = user.Id,
            };
            var newUserId = _userService.AddUser(user1);

            var fullVendor = new VendorViewModel
            {
                VendorId = newUserId,
                BusinessName = step2.BusinessName,
                BusinessAddress = step2.BusinessAddress,
                GSTNumber = step2.GSTNumber,
                DocumentType = step2.DocumentType,
                FileUrl = model.FileUrl
            };

            _userService.AddVendor(fullVendor);

            await _userManager.AddToRoleAsync(user, "Vendor");
            await _signInManager.SignInAsync(user, isPersistent: false);

            TempData["Message"] = "Vendor Registered Successfully";

            HttpContext.Session.Remove("VendorStep1");
            HttpContext.Session.Remove("VendorStep2");

            return RedirectToAction("Login", "Account");
        }
        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View("Register", model);
    }
}