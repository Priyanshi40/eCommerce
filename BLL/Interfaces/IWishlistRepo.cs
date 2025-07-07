
using DAL.Models;

namespace BLL.Interfaces;

public interface IWishlistRepo
{
    int GetWishlistCount(int userId);
    IQueryable<Product> GetWishlistItems(int userId);
    bool AddProductToWishlist(int productId, int userId);
}