using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface ICartService
{
    int GetCartItemsCount(int userId);
    int UpdateCart(int productId, int userId, int quantity);
    bool DeleteCart(int userId, int productId);
    List<CartViewModel> GetCart(int userId);
    bool AddToCart(List<CartViewModel> item, int userId);
}
