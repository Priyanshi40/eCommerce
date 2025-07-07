using BLL.Interfaces;
using BLL.Utility;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class WishlistService : IWishlistService
{
    private readonly IWishlistRepo _wishlistRepo;

    public WishlistService(IWishlistRepo wishlistRepo)
    {
        _wishlistRepo = wishlistRepo;
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
        var wishlistedProducts = _wishlistRepo.GetWishlistItems(userId);

        int totalRecords = wishlistedProducts.Count();

        var pagedProducts = wishlistedProducts
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new ProductViewModel
        {
            Products = pagedProducts,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
        };
    }

}