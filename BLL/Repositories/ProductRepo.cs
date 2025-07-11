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
        IQueryable<Product> products = _context.Product
                                                    .Include(p => p.Category)
                                                    .Include(p => p.WishList)
                                                    .Where(u => !u.IsDeleted)
                                                    .OrderBy(u => u.Name)
                                                    .AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
            products = products.Where(u => u.Name.ToLower().Contains(searchString.ToLower().Trim()));

        return products;
    }
    public Product? GetProductDetails(int productId)
    {
        return _context.Product
                            .Include(b => b.Category)
                            .Include(p => p.Images)
                            .FirstOrDefault(b => b.Id == productId);
    }
    public Product UpsertProduct(Product product)
    {
        if (product.Id > 0)
            _context.Product.Update(product);
        else
            _context.Product.Add(product);

        _context.SaveChanges();
        return product;
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
                oldProduct.StockQuantity = product.StockQuantity > 0 ? product.StockQuantity : oldProduct.StockQuantity;
                oldProduct.CategoryId = product.CategoryId > 0 ? product.CategoryId : oldProduct.CategoryId;
                oldProduct.ModifiedBy = product.ModifiedBy;
                oldProduct.ModifiedAt = DateTime.Now;

                if (product.Images != null)
                {
                    _context.ProductImage.RemoveRange(oldProduct.Images);
                    oldProduct.Images = product.Images;
                }

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
    public bool DeleteProduct(Product product)
    {
        try
        {
            Product oldProduct = _context.Product.FirstOrDefault(b => b.Id == product.Id);
            if (oldProduct == null) return false;

            oldProduct.ModifiedBy = product.ModifiedBy;
            oldProduct.ModifiedAt = DateTime.Now;
            oldProduct.IsDeleted = true;

            _context.Product.Update(oldProduct);
            _context.SaveChanges();
            return true;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error While Deleting Product: {ex.Message}");
            throw;
        }
    }
    public int ApproveProduct(Product product)
    {
        try
        {
            Product oldProduct = _context.Product.FirstOrDefault(b => b.Id == product.Id);
            if (oldProduct == null) return -1;

            oldProduct.ModifiedBy = product.ModifiedBy;
                oldProduct.ModifiedAt = DateTime.Now;
                oldProduct.Status = product.Status;
                oldProduct.AdminComment = product.AdminComment;

                _context.Product.Update(oldProduct);
                _context.SaveChanges();
                return oldProduct.CreatedBy;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error While Approving/Rejecting Product: {ex.Message}");
            throw;
        }
    }
    public int AddProductToWishlist(int productId, int userId)
    {
        try
        {
            Wishlist? wishlistItem = _context.Wishlist.FirstOrDefault(w => w.ProductId == productId && w.UserId == userId);
            if (wishlistItem == null)
            {
                wishlistItem = new Wishlist
                {
                    ProductId = productId,
                    UserId = userId,
                };
                _context.Wishlist.Add(wishlistItem);
            }
            else
                _context.Wishlist.Remove(wishlistItem);

            _context.SaveChanges();
            return wishlistItem.Product.CategoryId;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling product to wishlist: {ex.Message}");
            throw;
        }
    }
}
