using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface ICartService
{
    int GetCartItemsCount(int userId);
    int UpdateCart(int productId, int userId, int quantity = 0);
    List<CartViewModel> GetCart(int userId);
    int AddToCart(CartViewModel item, int userId);
}
