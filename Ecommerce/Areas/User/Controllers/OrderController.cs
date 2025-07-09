using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.User.Controllers;

[Area("User")]
[Authorize(Roles = "User")]
public class OrderController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserService _userService;
    private readonly ICategoryService _catService;
    private readonly IProductService _proService;
    public OrderController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ICategoryService catService, IProductService proService, IUserService userService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _catService = catService;
        _proService = proService;
        _userService = userService;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult PlaceOrder()
    {
        return View("OrderCard");
    }

}
