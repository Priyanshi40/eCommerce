using BLL.Interfaces;
using DAL.Enums;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repositories;

public class WishlistRepo : IWishlistRepo
{
    private readonly E_CommerceContext _context;
    public WishlistRepo(E_CommerceContext context)
    {
        _context = context;
    }
    public int GetWishlistCount(int userId)
    {
        var count = _context.Wishlist.Count(w => w.UserId == userId);
        return count;
    }

    public IQueryable<Product> GetWishlistItems(int userId)
    {
        var wishlistedProducts = _context.Wishlist
            .Include(w => w.Product)
                .ThenInclude(p => p.Category)
            .Include(w => w.Product.Images)
            .Where(w => w.UserId == userId && !w.Product.IsDeleted && w.Product.Status == ProductStatus.Approved)
            .Select(w => w.Product)
            .Distinct();

        return wishlistedProducts;

    }
    public bool AddProductToWishlist(int productId, int userId)
    {
        try
        {
            var wishlistItem = _context.Wishlist.FirstOrDefault(w => w.ProductId == productId && w.UserId == userId);
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
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling product to wishlist: {ex.Message}");
            throw;
        }
    }
}