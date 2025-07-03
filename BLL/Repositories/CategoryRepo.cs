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
        return _context.Category.Where(c =>c.IsActive).ToList();
    }
    public IQueryable<Category> GetCategoryQueryable(string? searchString)
    {
        var category = _context.Category.Where(u => !u.IsDeleted).OrderBy(u => u.Name).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            category = category.Where(u => u.Name.ToLower().Contains(searchString.ToLower().Trim()));
        }
        return category;
    }
    public Category GetCategoryDetails(int catId)
    {
        return _context.Category.Include(c => c.Products).FirstOrDefault(b => b.Id == catId);
    }
    public Category UpSertCategory(Category category)
    {
        try
        {
            Category oldCategory = _context.Category.FirstOrDefault(b => b.Id == category.Id);
            if (oldCategory != null)
            {
                oldCategory.Name = category.Name ?? oldCategory.Name;
                oldCategory.Description = category.Description ?? oldCategory.Description;
                oldCategory.CoverImage = category.CoverImage ?? oldCategory.CoverImage;
                oldCategory.ModifiedBy = category.ModifiedBy;
                oldCategory.ModifiedAt = DateTime.Now;
                _context.Category.Update(oldCategory);
                _context.SaveChanges();
                return oldCategory;
            }
            else
            {
                _context.Category.Add(category);
                _context.SaveChanges();
                return category;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding Category: {ex.Message}");
            throw;
        }
    }
    public void DeleteCategory(Category category)
    {
        try
        {
            Category oldCategory = _context.Category.FirstOrDefault(b => b.Id == category.Id);
            if (oldCategory != null)
            {
                if (category.IsDeleted)
                {
                    oldCategory.IsDeleted = category.IsDeleted;
                }
                else
                {
                    oldCategory.IsActive = category.IsActive;
                }
                oldCategory.ModifiedBy = category.ModifiedBy;
                oldCategory.ModifiedAt = DateTime.Now;
                _context.Category.Update(oldCategory);
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding Category: {ex.Message}");
            throw;
        }
    }
    public bool CheckCategory(string name, int id)
    {
        return _context.Category.Any(c => c.Name == name && c.Id != id);
    }
    
}
