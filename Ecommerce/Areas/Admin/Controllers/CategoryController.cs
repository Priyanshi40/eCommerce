using BLL.Interfaces;
using BLL.Utility;
using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class CategoryController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ICategoryService _catService;
    private readonly IUserService _userService;
    private readonly ImageService _imgService;
    public CategoryController(UserManager<IdentityUser> userManager,
                              ICategoryService catService,
                              IUserService userService,
                              ImageService imgService)
    {
        _userManager = userManager;
        _catService = catService;
        _userService = userService;
        _imgService = imgService;
    }
    private string? GetUserIdentityId() => _userManager.GetUserId(User);
    private int GetAppUserId(string identityId) => _userService.GetUserById(identityId).UserId;
    private bool IsAuthenticated() => !string.IsNullOrEmpty(GetUserIdentityId());
    public IActionResult Index() => View();
    public IActionResult CatList(string statusFilter, SortOrder sortOrder, string searchString, int pageNumber = 1, int pageSize = 5)
    {
        CategoryViewModel CategoryView = _catService.GetCategories(searchString, sortOrder, statusFilter,pageNumber,pageSize);
        return PartialView("_catList", CategoryView);
    }
    public IActionResult CatModal(int catId)
    {
        CategoryViewModel catDetails = _catService.GetCategoryDetailsService(catId);
        return PartialView("_catModal", catDetails);
    }
    public IActionResult AddCategory(CategoryViewModel catToAdd, IFormFile? categoryImage)
    {
        if (!ModelState.IsValid)
            return Ok(new { status = AjaxError.Error.ToString() });

        if (categoryImage != null)
            catToAdd.CoverImage = _imgService.SaveImageService(categoryImage);

        _catService.UpSertCategory(catToAdd, GetAppUserId(GetUserIdentityId()));
        return RedirectToAction("CatList");
    }
    public IActionResult DeleteCategory(int catId, bool? isActive, bool isDelete)
    {
        if (catId == 0)
            return Ok(new { status = AjaxError.ValidationError.ToString() });

        if (!IsAuthenticated())
            return Ok(new { status = AjaxError.UnAuthorized.ToString() });

        _catService.SoftDeleteOrDeactivate(catId, isActive, isDelete, GetAppUserId(GetUserIdentityId()));
        return RedirectToAction("CatList", "Category");
    }
    public JsonResult CheckDuplicateName(string name, int Id)
    {
        bool exists = _catService.CheckCategoryService(name, Id);
        return Json(!exists);
    }
}
