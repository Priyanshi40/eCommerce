using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface ICategoryService
{
    List<Category> GetCategoriesService();
    IQueryable<Category> GetQueryableCategories(string? searchString, string statusFilter);
    CategoryViewModel GetCategoryDetailsService(int catId);
    Category UpSertCategory(CategoryViewModel category);
    bool CheckCategoryService(string name, int id);
    void DeleteCategoryService(Category category);
}
