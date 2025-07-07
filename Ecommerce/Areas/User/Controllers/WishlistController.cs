using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.User.Controllers;


[Area("User")]
[Authorize(Roles = "User")]
public class WishListController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserService _userService;
    private readonly IWishlistService _wishService;
    public WishListController(UserManager<IdentityUser> userManager, IUserService userService, IWishlistService wishService)
    {
        _userManager = userManager;
        _userService = userService;
        _wishService = wishService;
    }
    public IActionResult GetWishlistCount()
    {
        var userId = _userService.GetUserById(_userManager.GetUserId(User)).UserId;
        var count = _wishService.GetWishlistCount(userId);
        return Json(new { count });
    }
    public IActionResult Index(int pageNumber = 1, int pageSize = 8)
    {
        var userId = _userService.GetUserById(_userManager.GetUserId(User)).UserId;
        var viewModel = _wishService.GetWishlistItems(userId, pageNumber, pageSize);
        return View(viewModel);
    }
    public IActionResult ToggleWishlist(int productId)
    {
        var userId = _userService.GetUserById(_userManager.GetUserId(User)).UserId;
        if (userId < 0)
            return Ok(new { status = AjaxError.UnAuthorized.ToString() });

        bool result = _wishService.AddProductToWishlist(productId, userId);
        if (result)
            return Ok(new { status = AjaxError.Success.ToString() });
        else
            return Ok(new { status = AjaxError.NotFound.ToString() });
    }

}