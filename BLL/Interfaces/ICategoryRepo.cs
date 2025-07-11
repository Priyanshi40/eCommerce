using DAL.Models;

namespace BLL.Interfaces;

public interface ICategoryRepo
{
    List<Category> GetCategories();
    IQueryable<Category> GetCategoryQueryable(string? searchString);
    Category GetCategoryDetails(int catId);
    int UpSertCategory(Category category);
    void DeleteCategory(Category category);
    bool CheckCategory(string name, int id);
}
