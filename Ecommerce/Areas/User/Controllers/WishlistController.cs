using BLL.Interfaces;
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
        int userId = _userService.GetUserById(_userManager.GetUserId(User)).UserId;
        int count = _wishService.GetWishlistCount(userId);
        return Json(new { count });
    }
    public IActionResult Index(int pageNumber = 1, int pageSize = 8)
    {
        int userId = _userService.GetUserById(_userManager.GetUserId(User)).UserId;
        ProductViewModel viewModel = _wishService.GetWishlistItems(userId, pageNumber, pageSize);
        return View(viewModel);
    }
    public IActionResult RemoveFromWishlist(int productId)
    {
        int userId = _userService.GetUserById(_userManager.GetUserId(User)).UserId;
        if (userId < 0)
            return Unauthorized();

        bool result = _wishService.AddProductToWishlist(productId, userId);
        if (result)
            return Ok(new { success = true });
        else
            return BadRequest();
        
    }
    public IActionResult ToggleWishlist(int productId,int categoryId)
    {
        int userId = _userService.GetUserById(_userManager.GetUserId(User)).UserId;
        if (userId < 0)
            TempData["Error"] = "Please log in to use wishlist.";

        bool result = _wishService.AddProductToWishlist(productId, userId);
        if (result)
            TempData["Message"] = "Wishlist updated !!";
        else
            TempData["Error"] = "Product not found !!";
        return RedirectToAction("ProductByCategory","Home",new{area = "" , category = categoryId});
    }

}