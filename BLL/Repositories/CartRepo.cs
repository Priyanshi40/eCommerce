using BLL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repositories;

public class CartRepo : ICartRepo
{
    private readonly E_CommerceContext _context;
    public CartRepo(E_CommerceContext context)
    {
        _context = context;
    }
    public int GetCartItemsCount(int userId)
    {
        return (from c in _context.Cart
                    join ci in _context.CartItem
                    on c.Id equals ci.CartId
                    where c.UserId == userId && !ci.IsDeleted
                    select ci).Count();
    }
    public bool CheckUserCart(int userId,int productId)
    {
        return (from c in _context.Cart
                    join ci in _context.CartItem
                    on c.Id equals ci.CartId
                    where c.UserId == userId && !ci.IsDeleted && ci.ProductId == productId
                    select ci).Any();
    }
    public Cart GetCartWithItemsByUserId(int userId)
    {
        return _context.Cart
            .Include(c => c.CartItems.Where(c => !c.IsDeleted))
            .FirstOrDefault(c => c.UserId == userId);
    }
    public void AddCart(Cart cart)
    {
        _context.Cart.Add(cart);
    }
    public void UpdateCart(Cart cart)
    {
        _context.Cart.Update(cart);
    }
    public void Save()
    {
        _context.SaveChanges();
    }
        
}
