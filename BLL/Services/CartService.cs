using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
namespace BLL.Services;

public class CartService : ICartService
{
    private readonly ICartRepo _cartRepo;
    public CartService(ICartRepo cartRepo)
    {
        _cartRepo = cartRepo;
    }
    public int GetCartItemsCount(int userId)
    {
        return _cartRepo.GetCartItemsCount(userId);
    }
    public int UpdateCart(int productId, int userId, int quantity)
    {
        Cart cart = _cartRepo.GetCartWithItemsByUserId(userId);
        if (cart != null)
        {
            CartItem? existingItem = cart.CartItems.SingleOrDefault(c => c.ProductId == productId);
            if (existingItem != null)
            {
                if (quantity == 0)
                    return -1;
                else
                    existingItem.Quantity = quantity;

                cart.ModifiedAt = DateTime.Now;
                _cartRepo.UpdateCart(cart);
                _cartRepo.Save();
                return cart.Id;
            }
        }
        return -1;
    }
    public bool DeleteCart(int userId, int productId)
    {
        Cart cart = _cartRepo.GetCartWithItemsByUserId(userId);
        if (cart == null) return false;

        CartItem? cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (cartItem == null) return false;

        _cartRepo.RemoveCartItem(cartItem);
        _cartRepo.Save();

        Cart updatedCart = _cartRepo.GetCartWithItemsByUserId(userId);
        if (updatedCart.CartItems == null || !updatedCart.CartItems.Any())
        {
            _cartRepo.RemoveCart(updatedCart);
            _cartRepo.Save();
        }
        return true;
    }
    public List<CartViewModel> GetCart(int userId)
    {
        Cart cart = _cartRepo.GetCartWithItemsByUserId(userId);
        List<CartViewModel> cartDetails = new List<CartViewModel>();
        if (cart != null)
        {
            cartDetails = cart.CartItems.Select(o => new CartViewModel
            {
                ProductId = o.ProductId,
                Name = o.ProductNavigation.Name,
                Price = o.Price,
                CoverImage = o.ProductNavigation.CoverImage,
                StockQuantity = o.ProductNavigation.StockQuantity,
                Quantity = o.Quantity,
            }).ToList();
        }
        return cartDetails;
    }
    public bool AddToCart(List<CartViewModel> items, int userId)
    {
        foreach (CartViewModel item in items)
        {
            Cart cart = _cartRepo.GetCartWithItemsByUserId(userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    CartItems = new List<CartItem>()
                };
                cart.CartItems.Add(new CartItem
                {
                    ProductId = item.Id,
                    Price = item.Price,
                    Quantity = 1
                });
                _cartRepo.AddCart(cart);
            }
            else
            {
                cart.ModifiedAt = DateTime.Now;

                CartItem? existingItem = cart.CartItems.SingleOrDefault(c => c.ProductId == item.Id);
                if (existingItem != null)
                {
                    existingItem.Quantity += 1;
                }
                else
                {
                    cart.CartItems.Add(new CartItem
                    {
                        ProductId = item.Id,
                        Price = item.Price,
                        Quantity = 1
                    });
                }
                _cartRepo.UpdateCart(cart);
            }
        }
        _cartRepo.Save();
        return true;
    }
}

