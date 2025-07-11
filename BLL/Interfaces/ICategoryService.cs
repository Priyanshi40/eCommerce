using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface ICategoryService
{
    List<Category> GetCategoriesService();
    CategoryViewModel GetCategories(string? searchString, SortOrder sort, string statusFilter, int pageNumber, int pageSize);
    CategoryViewModel GetCategoryDetailsService(int catId);
    int UpSertCategory(CategoryViewModel model, int userId);
    bool CheckCategoryService(string name, int id);
    void SoftDeleteOrDeactivate(int catId, bool? isActive, bool isDelete, int userId);
}
