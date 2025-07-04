using BLL.Interfaces;
using BLL.Utility;
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
    public CategoryController(UserManager<IdentityUser> userManager, ICategoryService catService, IUserService userService, ImageService imgService)
    {
        _userManager = userManager;
        _catService = catService;
        _userService = userService;
        _imgService = imgService;
    }
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult CatList(string statusFilter, string searchString, int pageNumber = 1, int pageSize = 5)
    {
        var queyableCategories = _catService.GetQueryableCategories(searchString, statusFilter);

        int totalRecords = queyableCategories.Count();
        var paginatedCategories = queyableCategories.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        var CategoryView = new CategoryViewModel
        {
            Categories = paginatedCategories,
            PageSize = pageSize,
            PageNumber = pageNumber,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
        };
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
        {
            return Ok(new { status = AjaxError.Error.ToString() });
        }
        else
        {
            if (categoryImage != null)
                catToAdd.CoverImage = _imgService.SaveImageService(categoryImage);

            var user = _userService.GetUserById(_userManager.GetUserId(User));
            if (catToAdd.Id == 0)
            {
                catToAdd.CreatedBy = user.UserId;
                catToAdd.UpdatedBy = user.UserId;
            }
            else
                catToAdd.UpdatedBy = user.UserId;

            _catService.UpSertCategory(catToAdd);
            return RedirectToAction("CatList", "Category", new { area = "Admin" });
        }
    }
    public IActionResult DeleteCategory(int catId, bool? isActive, bool isDelete)
    {
        if (catId != 0)
        {
            var user = _userService.GetUserById(_userManager.GetUserId(User));
            if (user == null)
                return Ok(new { status = AjaxError.UnAuthorized.ToString() });
            else
            {
                Category category = new()
                {
                    Id = catId,
                    IsDeleted = isDelete,
                    ModifiedBy = user.UserId,
                };
                if (isActive != null)
                    category.IsActive = isActive.Value;

                _catService.DeleteCategoryService(category);
                return RedirectToAction("CatList", "Category");
            }
        }
        else
            return Ok(new { status = AjaxError.ValidationError.ToString() });
    }
    public JsonResult CheckDuplicateName(string name, int Id)
    {
        bool exists = _catService.CheckCategoryService(name, Id);
        return Json(!exists);
    }
}
