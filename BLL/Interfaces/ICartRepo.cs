using DAL.Models;

namespace BLL.Interfaces;

public interface ICartRepo
{
    int GetCartItemsCount(int userId);
    bool CheckUserCart(int userId, int productId);
    Cart GetCartWithItemsByUserId(int userId);
    void AddCart(Cart cart);
    void UpdateCart(Cart cart);
    public void RemoveCartItem(CartItem cartItem);
    public void RemoveCart(Cart cart);
    void Save();
}
