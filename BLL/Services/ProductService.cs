using BLL.Interfaces;
using BLL.Utility;
using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class ProductService : IProductService
{
    private readonly IProductRepo _productRepo;
    private readonly IUserRepo _userRepo;
    private readonly ImageService _imgService;

    public ProductService(IProductRepo productRepo, ImageService imgService, IUserRepo userRepo)
    {
        _productRepo = productRepo;
        _imgService = imgService;
        _userRepo = userRepo;
    }
    public ProductViewModel GetProductsService(string searchString, SortOrder sort, int category, string statusFilter, int pageNumber, int pageSize, int userId = 0)
    // public ProductViewModel GetProductsService(string searchString, SortOrder sort, bool isAscending, int category, string statusFilter, int pageNumber, int pageSize, int userId = 0)
    {
        IQueryable<Product> queyableProducts = _productRepo.GetQueryableProducts(searchString);
        if (userId > 0)
            queyableProducts = queyableProducts.Where(p => p.CreatedBy == userId);

        if (category > 0)
            queyableProducts = queyableProducts.Where(p => p.CategoryId == category);

        if (!string.IsNullOrEmpty(statusFilter))
        {
            if (Enum.TryParse(statusFilter, out ProductStatus parsedStatus))
                queyableProducts = queyableProducts.Where(p => p.Status == parsedStatus);
        }
        // if (isAscending)
        // {
        //     queyableProducts = sort switch
        //     {
        //         SortOrder.Name => queyableProducts.OrderBy(p => p.Name),
        //         SortOrder.Category => queyableProducts.OrderBy(p => p.Category.Name),
        //         SortOrder.Price => queyableProducts.OrderBy(p => p.Price),
        //         SortOrder.Quantity => queyableProducts.OrderBy(p => p.StockQuantity),
        //         _ => queyableProducts.OrderBy(p => p.Name),
        //     };
        // }
        // else
        // {
            queyableProducts = sort switch
            {
                SortOrder.Name => queyableProducts.OrderByDescending(p => p.Name),
                SortOrder.Category => queyableProducts.OrderByDescending(p => p.Category.Name),
                SortOrder.Price => queyableProducts.OrderByDescending(p => p.Price),
                SortOrder.Quantity => queyableProducts.OrderByDescending(p => p.StockQuantity),
                _ => queyableProducts.OrderByDescending(p => p.Name),
            };
        // }
        ProductViewModel productsView = new();
        if (queyableProducts != null)
        {
            int totalRecords = queyableProducts.Count();
            List<Product> paginatedProducts = queyableProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            productsView.Products = paginatedProducts;
            productsView.PageSize = pageSize;
            productsView.PageNumber = pageNumber;
            productsView.TotalRecords = totalRecords;
            productsView.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }
        return productsView;
    }

    public ProductViewModel GetProductDetailsService(int productId)
    {
        Product productData = _productRepo.GetProductDetails(productId);
        ProductViewModel productDetails = new ProductViewModel();
        if (productData != null)
        {
            productDetails.Id = productData.Id;
            productDetails.Name = productData.Name;
            productDetails.Description = productData.Description;
            productDetails.Price = productData.Price;
            productDetails.StockQuantity = productData.StockQuantity;
            productDetails.CategoryId = productData.CategoryId;
            productDetails.CategoryName = productData.Category.Name;
            productDetails.CoverImage = productData.CoverImage;
            productDetails.ProductImages = productData.Images.Select(img => img.ImageUrl).ToList();
        }
        return productDetails;
    }

    public IQueryable<Product> GetQueryableProducts(string? searchString)
    {

        return _productRepo.GetQueryableProducts(searchString);
    }
    public Product UpSertProduct(ProductViewModel product)
    {
        Product existingProduct = null;
        if (product.Id != 0)
            existingProduct = _productRepo.GetProductDetails(product.Id);

        List<string> existingImageUrls = existingProduct?.Images.Select(i => i.ImageUrl).ToList() ?? new List<string>();

        if (product.RemovedImages != null)
        {
            foreach (string img in product.RemovedImages)
            {
                existingImageUrls.Remove(img);
            }
        }

        if (product.GalleryImages != null && product.GalleryImages.Any())
        {
            foreach (IFormFile file in product.GalleryImages)
            {
                string saved = _imgService.SaveImageService(file);
                existingImageUrls.Add(saved);
            }
        }

        Product newProduct = new()
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            ModifiedBy = product.ModifiedBy,
            CreatedBy = product.CreatedBy,
            CoverImage = product.CoverImage,
            Images = existingImageUrls.Select(i => new ProductImage
            {
                ImageUrl = i
            }).ToList()
        };
        if (product.Id != 0)
            newProduct.Id = product.Id;
        return _productRepo.UpSertProduct(newProduct);
    }
    public bool DeleteProduct(Product product)
    {
        return _productRepo.DeleteProduct(product);
    }
    public string? ApproveProduct(Product product)
    {
        int UserId = _productRepo.ApproveProduct(product);
        if (UserId != -1)
        {
            return _userRepo.GetUserById(UserId).IdentityUserId;
        }
        return null;
    }

    public int AddProductToWishlist(int productId, int userId)
    {
        if (productId <= 0 || userId <= 0)
            return -1;

        return _productRepo.AddProductToWishlist(productId, userId);
    }
}
