using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly ImageService _imgService;

    public ProductService(IProductRepo productRepo, ImageService imgService, IUserRepo userRepo, IMapper mapper)
    {
        _productRepo = productRepo;
        _imgService = imgService;
        _userRepo = userRepo;
        _mapper = mapper;
    }
    public ProductViewModel GetProductsService(string searchString, SortOrder sort, int category, string statusFilter, int pageNumber, int pageSize, int userId = 0)
    {
        IQueryable<Product> queryProducts = _productRepo.GetQueryableProducts(searchString);

        if (userId > 0)
            queryProducts = queryProducts.Where(p => p.CreatedBy == userId);

        if (category > 0)
            queryProducts = queryProducts.Where(p => p.CategoryId == category);

        if (!string.IsNullOrEmpty(statusFilter) && Enum.TryParse(statusFilter, out ProductStatus parsedStatus))
            queryProducts = queryProducts.Where(p => p.Status == parsedStatus);

        queryProducts = sort switch
        {
            SortOrder.Name => queryProducts.OrderByDescending(p => p.Name),
            SortOrder.Category => queryProducts.OrderByDescending(p => p.Category.Name),
            SortOrder.Price => queryProducts.OrderByDescending(p => p.Price),
            SortOrder.Quantity => queryProducts.OrderByDescending(p => p.StockQuantity),
            _ => queryProducts.OrderByDescending(p => p.Name),
        };

        ProductViewModel pView = new();
        if (queryProducts != null)
        {
            int totalRecords = queryProducts.Count();
            List<Product> paginatedProducts = queryProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            pView.Products = paginatedProducts;
            pView.PageSize = pageSize;
            pView.PageNumber = pageNumber;
            pView.TotalRecords = totalRecords;
            pView.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }
        return pView;
    }

    // public ProductViewModel GetProductDetails(int id)
    // {
    //     Product entity = _productRepo.GetProductDetails(id);
    //     ProductViewModel viewDetails = _mapper.Map<ProductViewModel>(entity);
    //     viewDetails.ProductImages = entity.Images.Select(img => img.ImageUrl).ToList();
    //     return viewDetails;
    // }
    public ProductViewModel GetProductDetailsService(int productId)
    {
        Product productData = _productRepo.GetProductDetails(productId);
        ProductViewModel productDetails = new();
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
    // public Product UpSertProducts(ProductViewModel vm)
    // {
    //     var existing = vm.Id > 0 ? _productRepo.GetProductDetails(vm.Id) : null;
    //     var existingImages = existing?.Images.Select(i => i.ImageUrl).ToList() ?? new List<string>();

    //     if (vm.RemovedImages?.Any() == true)
    //         existingImages.RemoveAll(img => vm.RemovedImages.Contains(img));

    //     if (vm.GalleryImages?.Any() == true)
    //     {
    //         foreach (var file in vm.GalleryImages)
    //             existingImages.Add(_imgService.SaveImageService(file));
    //     }

    //     var entity = _mapper.Map<Product>(vm);
    //     entity.Images = existingImages.Select(i => new ProductImage { ImageUrl = i }).ToList();
    //     return _productRepo.UpSertProduct(entity);
    // }
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

    public bool DeleteProduct(Product product) => _productRepo.DeleteProduct(product);
    public string? ApproveProduct(Product product)
    {
        int UserId = _productRepo.ApproveProduct(product);
        return UserId != -1 ? _userRepo.GetUserById(UserId).IdentityUserId : null;
    }
    public int AddProductToWishlist(int productId, int userId)
    {
        if (productId <= 0 || userId <= 0) return -1;
        return _productRepo.AddProductToWishlist(productId, userId);
    }
}
