using BLL.Interfaces;
using DAL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace Ecommerce.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IAccountService _accountService;
    public AccountController(UserManager<IdentityUser> userManager,
                            SignInManager<IdentityUser> signInManager,
                            IAccountService accountService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _accountService = accountService;
    }

    [HttpGet]
    public IActionResult Login(string? ReturnUrl = null)
    {
        ViewData["ReturnUrl"] = ReturnUrl;
        if (User.Identity.IsAuthenticated && _signInManager.IsSignedIn(User))
        {
            IdentityUser? user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                IList<string> userRole = _userManager.GetRolesAsync(user).Result;

                if (userRole[0].ToString() == "User")
                    return RedirectToAction("Index", "Home");

                return RedirectToAction("Index", "Home", new { area = userRole[0].ToString() });
            }
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? ReturnUrl)
    {
        if (!ModelState.IsValid)
            return View(model);

        IdentityUser? user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                TempData["Message"] = "Welcome" + model.Email;
                IList<string> userRole = _userManager.GetRolesAsync(user).Result;

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    return Redirect(ReturnUrl);

                if (userRole[0].ToString() == "User")
                    return RedirectToAction("Index", "Home");

                return RedirectToAction("Index", "Home", new { area = userRole[0].ToString() });
            }
        }
        TempData["Error"] = "Invalid login attempt.";
        return View(model);
    }
    public IActionResult Register() => View();
    public IActionResult CommonRegistration() => PartialView("_commonRegistration");

    [HttpPost]
    public async Task<IActionResult> RegisterCustomer(RegisterViewModel model)
    {
        (bool Success, List<string> Errors) result = await _accountService.RegisterCustomer(model);
        if (result.Success)
        {
            TempData["Message"] = "User Registered Successfully";
            return RedirectToAction("Login");
        }

        foreach (string error in result.Errors)
            ModelState.AddModelError(string.Empty, error);

        return View("Register", model);
    }
    public IActionResult VendorBusinessDetails(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return PartialView("_vendorBusiness", model);

        HttpContext.Session.SetString("VendorStep1", JsonConvert.SerializeObject(model));

        return PartialView("_vendorBusiness", new VendorViewModel());
    }
    public IActionResult VendorDocuments(VendorViewModel model)
    {
        foreach (string? key in new[] { "File", "DocumentType" })
            ModelState.Remove(key);

        if (!ModelState.IsValid)
            return PartialView("_vendorDocuments", model);

        HttpContext.Session.SetString("VendorStep2", JsonConvert.SerializeObject(model));
        return PartialView("_vendorDocuments", model);
    }
    public async Task<IActionResult> RegisterVendor(VendorViewModel model)
    {
        ModelState.Remove("BusinessName");
        ModelState.Remove("GSTNumber");

        if (!ModelState.IsValid)
            return PartialView("_vendorDocuments", model);

        (bool Success, List<string> Errors) result = await _accountService.RegisterVendor(HttpContext, model);
        if (result.Success)
        {
            TempData["Message"] = "Vendor Registered Successfully";
            return RedirectToAction("Login");
        }

        foreach (string error in result.Errors)
            ModelState.AddModelError(string.Empty, error);

        return View("Register", model);
    }
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}