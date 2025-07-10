using BLL.Interfaces;
using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _catRepo;

    public CategoryService(ICategoryRepo catRepo)
    {
        _catRepo = catRepo;
    }
    public List<Category> GetCategoriesService()
    {
        return _catRepo.GetCategories();
    }
    public IQueryable<Category> GetQueryableCategories(string? searchString,SortOrder sort, string statusFilter)
    {
        IQueryable<Category> catQuery = _catRepo.GetCategoryQueryable(searchString);
        if (!string.IsNullOrEmpty(statusFilter))
        {
            if (statusFilter == CategoryStatus.Active.ToString())
            {
                catQuery = catQuery.Where(u => u.IsActive);
            }
            else if(statusFilter == CategoryStatus.Deactive.ToString())
            {
                catQuery = catQuery.Where(u => !u.IsActive);
            }
        }
        catQuery = sort switch
        {
            SortOrder.Name => catQuery.OrderByDescending(u => u.Name),
            SortOrder.Description => catQuery.OrderByDescending(u => u.Description),
            SortOrder.TotalProducts => catQuery.OrderByDescending(u => u.Products.Count),
            _ => catQuery.OrderBy(u => u.Name),
        };
        return catQuery;
    }
    public CategoryViewModel GetCategoryDetailsService(int catId)
    {
        Category cat = _catRepo.GetCategoryDetails(catId);
        CategoryViewModel catDetails = new CategoryViewModel();
        if (cat != null)
        {
            catDetails.Id = cat.Id;
            catDetails.Name = cat.Name;
            catDetails.Description = cat.Description;
            catDetails.CoverImage = cat.CoverImage;
            catDetails.IsActive = cat.IsActive;
        }
        return catDetails;
    }

    public Category UpSertCategory(CategoryViewModel category)
    {
        Category newCategory = new Category
        {
            Name = category.Name,
            Description = category.Description,
            ModifiedBy = category.UpdatedBy,
            CreatedBy = category.CreatedBy,
            CoverImage = category.CoverImage,
        };
        if (category.Id != 0)
        {
            newCategory.Id = category.Id;
        };
        return _catRepo.UpSertCategory(newCategory);
    }
    public void DeleteCategoryService(Category category)
    {
        _catRepo.DeleteCategory(category);
    }
    public bool CheckCategoryService(string name,int id)
    {
        return _catRepo.CheckCategory(name,id);
    }
}
