
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IWishlistService
{
    int GetWishlistCount(int userId);
    ProductViewModel GetWishlistItems(int userId, int pageNumber, int pageSize);
    bool AddProductToWishlist(int productId, int userId);
}