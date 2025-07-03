using BLL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repositories;

public class ProductRepo : IProductRepo
{
    private readonly E_CommerceContext _context;
    public ProductRepo(E_CommerceContext context)
    {
        _context = context;
    }
    public IQueryable<Product> GetQueryableProducts(string? searchString)
    {
        var products = _context.Product.Include(p => p.Category).Where(u => !u.IsDeleted).OrderBy(u => u.Name).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            products = products.Where(u => u.Name.ToLower().Contains(searchString.ToLower().Trim()));
        }
        return products;
    }
    public Product GetProductDetails(int productId)
    {
        return _context.Product.Include(b => b.Category).Include(p => p.Images).FirstOrDefault(b => b.Id == productId);
    }
    public Product UpSertProduct(Product product)
    {
        try
        {
            Product oldProduct = _context.Product.FirstOrDefault(b => b.Id == product.Id);
            if (oldProduct != null)
            {
                oldProduct.Name = product.Name ?? oldProduct.Name;
                oldProduct.Description = product.Description ?? oldProduct.Description;
                oldProduct.CoverImage = product.CoverImage ?? oldProduct.CoverImage;
                oldProduct.Price = product.Price > 0 ? product.Price : oldProduct.Price;
                oldProduct.CategoryId = product.CategoryId > 0 ? product.CategoryId : oldProduct.CategoryId;
                oldProduct.Images = product.Images ?? oldProduct.Images;
                oldProduct.ModifiedBy = product.ModifiedBy;
                oldProduct.ModifiedAt = DateTime.Now;
                _context.Product.Update(oldProduct);
                _context.SaveChanges();
                return oldProduct;
            }
            else
            {
                _context.Product.Add(product);
                _context.SaveChanges();
                return product;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding Product: {ex.Message}");
            throw;
        }
    }
}
