using BLL.Interfaces;
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
    public IQueryable<Category> GetQueryableCategories(string? searchString, string statusFilter)
    {

        var catQuery = _catRepo.GetCategoryQueryable(searchString);
        if (statusFilter != "All" && !string.IsNullOrEmpty(statusFilter))
        {
            if (statusFilter == "Active")
            {
                catQuery = catQuery.Where(u => u.IsActive);
            }
            else if(statusFilter == "Deactive")
            {
                catQuery = catQuery.Where(u => !u.IsActive);
            }
        }
        return catQuery;
    }
    public CategoryViewModel GetCategoryDetailsService(int catId)
    {
        var cat = _catRepo.GetCategoryDetails(catId);
        var catDetails = new CategoryViewModel();
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
