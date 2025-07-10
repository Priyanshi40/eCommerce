using BLL.Interfaces;
using DAL.ViewModels;

namespace BLL.Services;

public class WishlistService : IWishlistService
{
    private readonly IWishlistRepo _wishlistRepo;
    private readonly ICartRepo _cartRepo;

    public WishlistService(IWishlistRepo wishlistRepo, ICartRepo cartRepo)
    {
        _wishlistRepo = wishlistRepo;
        _cartRepo = cartRepo;
    }
    public int GetWishlistCount(int userId)
    {
        return _wishlistRepo.GetWishlistCount(userId);
    }
    public bool AddProductToWishlist(int productId, int userId)
    {
        if (productId <= 0 || userId <= 0)
        {
            return false;
        }
        return _wishlistRepo.AddProductToWishlist(productId, userId);
    }

    public ProductViewModel GetWishlistItems(int userId, int pageNumber, int pageSize)
    {
        List<DAL.Models.Product> wishlistedProducts = _wishlistRepo.GetWishlistItems(userId);

        List<ProductViewModel> wishlist = new();
        if (wishlistedProducts != null)
        {
            wishlist = wishlistedProducts.Select(w => new ProductViewModel()
            {
                Id = w.Id,
                Name = w.Name,
                Price = w.Price,
                StockQuantity = w.StockQuantity,
                CategoryId = w.CategoryId,
                CategoryName = w.Category.Name,
                CoverImage = w.CoverImage,
                IsInCart = _cartRepo.CheckUserCart(userId,w.Id)
            }).ToList();
        }

        int totalRecords = wishlist.Count;

        List<ProductViewModel> pagedProducts = wishlist
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new ProductViewModel
        {
            ProductDetails = pagedProducts,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
        };
    }

}