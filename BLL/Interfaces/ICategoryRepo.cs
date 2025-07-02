using DAL.Models;

namespace BLL.Interfaces;

public interface ICategoryRepo
{
    List<Category> GetCategories();
    IQueryable<Category> GetCategoryQueryable(string? searchString);
    Category GetCategoryDetails(int catId);
    Category UpSertCategory(Category category);
    bool CheckCategory(string name, int id);
    void DeleteCategory(Category category);
}
