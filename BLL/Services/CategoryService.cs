using AutoMapper;
using BLL.Interfaces;
using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepo _catRepo;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepo catRepo, IMapper mapper)
    {
        _catRepo = catRepo;
        _mapper = mapper;
    }
    public List<Category> GetCategoriesService()
    {
        return _catRepo.GetCategories();
    }
    public CategoryViewModel GetCategories(string? searchString, SortOrder sort, string statusFilter, int pageNumber, int pageSize)
    {
        IQueryable<Category> catQuery = _catRepo.GetCategoryQueryable(searchString);
        if (!string.IsNullOrEmpty(statusFilter))
        {
            catQuery = statusFilter == CategoryStatus.Active.ToString()
                ? catQuery.Where(c => c.IsActive)
                : catQuery.Where(c => !c.IsActive);
        }
        catQuery = sort switch
        {
            SortOrder.Name => catQuery.OrderByDescending(u => u.Name),
            SortOrder.Description => catQuery.OrderByDescending(u => u.Description),
            SortOrder.TotalProducts => catQuery.OrderByDescending(u => u.Products.Count),
            _ => catQuery.OrderBy(u => u.Name),
        };

        CategoryViewModel catView = new();
        if (catQuery != null)
        {
            int totalRecords = catQuery.Count();
            List<Category> paginated = catQuery
                                            .Skip((pageNumber - 1) * pageSize)
                                            .Take(pageSize)
                                            .ToList();

            catView.Categories = paginated;
            catView.PageSize = pageSize;
            catView.PageNumber = pageNumber;
            catView.TotalRecords = totalRecords;
            catView.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }
        return catView;
    }
    public CategoryViewModel GetCategoryDetailsService(int catId)
    {
        Category cat = _catRepo.GetCategoryDetails(catId);
        CategoryViewModel catDetails = new ();
        if (cat != null)
        {
            catDetails = _mapper.Map<CategoryViewModel>(cat);
        }
        return catDetails;
    }
    public int UpSertCategory(CategoryViewModel model, int userId)
    {
        Category category = _mapper.Map<Category>(model);
        if (model.Id == 0)
        {
            category.CreatedAt = DateTime.Now;
            category.CreatedBy = userId;
        }
        // if (model.CoverImage != null) {
        //     category.CoverImage = model.CoverImage; 
        // }
        category.ModifiedAt = DateTime.Now;
        category.ModifiedBy = userId;

        return _catRepo.UpSertCategory(category);
    }
    public bool CheckCategoryService(string name, int id)
    {
        return _catRepo.CheckCategory(name, id);
    }
    public void SoftDeleteOrDeactivate(int catId, bool? isActive, bool isDelete, int userId)
    {
        Category category = _catRepo.GetCategoryDetails(catId);
        category.IsDeleted = isDelete;
        category.IsActive = isActive ?? true;
        category.ModifiedBy = userId;
        category.ModifiedAt = DateTime.Now;
        _catRepo.DeleteCategory(category);
    }
}
