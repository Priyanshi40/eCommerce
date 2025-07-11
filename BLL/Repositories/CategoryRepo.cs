using BLL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repositories;

public class CategoryRepo : ICategoryRepo
{
    private readonly E_CommerceContext _context;
    public CategoryRepo(E_CommerceContext context)
    {
        _context = context;
    }
    public List<Category> GetCategories()
    {
        return _context.Category.Where(c => c.IsActive).ToList();
    }
    public IQueryable<Category> GetCategoryQueryable(string? searchString)
    {
        IQueryable<Category> category = _context.Category
                                                .Include(u => u.Products)
                                                .Where(u => !u.IsDeleted)
                                                .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
            category = category.Where(u => u.Name.ToLower().Contains(searchString.ToLower().Trim()));

        return category;
    }
    public Category GetCategoryDetails(int catId)
    {
        return _context.Category.Include(c => c.Products).FirstOrDefault(b => b.Id == catId);
    }
    public int UpSertCategory(Category category)
    {
        if (category.Id == 0)
            _context.Category.Add(category);
        else
            _context.Category.Update(category);

        _context.SaveChanges();
        return category.Id;
    }
    public void DeleteCategory(Category category)
    {
        _context.Category.Update(category);
        _context.SaveChanges();
    }
    public bool CheckCategory(string name, int id)
    {
        return _context.Category.Any(c => c.Name == name && c.Id != id);
    }
    
}
