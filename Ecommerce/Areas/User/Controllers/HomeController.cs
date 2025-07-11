using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles = "User")]
public class HomeController : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager;
    public HomeController(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
}
