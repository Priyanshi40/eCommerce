using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BLL.Services;

public class CartService : ICartService
{
    private readonly ICartRepo _cartRepo;
    private readonly IProductRepo _productRepo;
    public CartService(ICartRepo cartRepo, IProductRepo productRepo)
    {
        _cartRepo = cartRepo;
        _productRepo = productRepo;
    }
    public int GetCartItemsCount(int userId)
    {
        return _cartRepo.GetCartItemsCount(userId);
    }
    public int UpdateCart(int productId, int userId, int quantity = 0)
    {
        Cart cart = _cartRepo.GetCartWithItemsByUserId(userId);
        if (cart != null)
        {
            CartItem? existingItem = cart.CartItems.SingleOrDefault(c => c.ProductId == productId);
            if (existingItem != null)
            {
                if (quantity == 0)
                    existingItem.IsDeleted = true;
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
    public List<CartViewModel> GetCart(int userId)
    {
        Cart cart = _cartRepo.GetCartWithItemsByUserId(userId);
        List<CartViewModel> cartDetails = new List<CartViewModel>();
        if (cart != null)
        {
            cartDetails = cart.CartItems.Select(o => new CartViewModel
            {
                ProductId = o.ProductId,
                Name = _productRepo.GetProductDetails(o.ProductId).Name,
                Price = o.Price,
                CoverImage = _productRepo.GetProductDetails(o.ProductId).CoverImage,
                Quantity = o.Quantity,
                StockQuantity = _productRepo.GetProductDetails(o.ProductId).StockQuantity,
            }).ToList();
        }
        return cartDetails;
    }
    public int AddToCart(CartViewModel item, int userId)
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
        _cartRepo.Save();
        return cart.Id;
    }
}

