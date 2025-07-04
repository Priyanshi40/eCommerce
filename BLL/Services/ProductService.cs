using BLL.Interfaces;
using BLL.Utility;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class ProductService : IProductService
{
    private readonly IProductRepo _productRepo;
    private readonly ImageService _imgService;

    public ProductService(IProductRepo productRepo, ImageService imgService)
    {
        _productRepo = productRepo;
        _imgService = imgService;
    }
    public ProductViewModel GetProductsService(string searchString,int category,string statusFilter,int pageNumber, int pageSize,int userId = 0)
    {
        IQueryable<Product> queyableProducts = _productRepo.GetQueryableProducts(searchString);
        if(userId > 0)
        {
            queyableProducts = queyableProducts.Where(p => p.CreatedBy == userId);
        }
        if (category > 0)
        {
            queyableProducts = queyableProducts.Where(p => p.CategoryId == category);
        }
        if (!string.IsNullOrEmpty(statusFilter))
        {
            if (Enum.TryParse<ProductStatus>(statusFilter, out var parsedStatus))
            {
                queyableProducts = queyableProducts.Where(p => p.Status == parsedStatus);
            }
        }
        ProductViewModel productsView = new();
        if (queyableProducts != null)
        {
            int totalRecords = queyableProducts.Count();
            var paginatedProducts = queyableProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

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
            productDetails.CategoryId = productData.CategoryId;
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
        {
            existingProduct = _productRepo.GetProductDetails(product.Id);
        }

        List<string> existingImageUrls = existingProduct?.Images.Select(i => i.ImageUrl).ToList() ?? new List<string>();

        if (product.RemovedImages != null)
        {
            foreach (var img in product.RemovedImages)
            {
                existingImageUrls.Remove(img);
            }
        }

        if (product.GalleryImages != null && product.GalleryImages.Any())
        {
            foreach (var file in product.GalleryImages)
            {
                var saved = _imgService.SaveImageService(file);
                existingImageUrls.Add(saved);
            }
        }

        Product newProduct = new()
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            ModifiedBy = product.ModifiedBy,
            CreatedBy = product.CreatedBy,
            CoverImage = product.CoverImage,
            Images = existingImageUrls.Select(i => new ProductImage
            {
                ImageUrl = i
            }).ToList()
            // Images = new List<ProductImage>()
        };
        if (product.Id != 0)
        {
            newProduct.Id = product.Id;
        }
        ;
        // if (product.GalleryImages != null && product.GalleryImages.Count > 0)
        // {
        //     foreach (var image in product.GalleryImages)
        //     {
        //         var savedImage = _imgService.SaveImageService(image);
        //         newProduct.Images.Add(new ProductImage
        //         {
        //             ImageUrl = savedImage,
        //         });
        //     }
        // }
        return _productRepo.UpSertProduct(newProduct);
    }
    public bool DeleteProduct(Product product)
    {
        return _productRepo.DeleteProduct(product);
    }
    public bool ApproveProduct(Product product)
    {
        return _productRepo.ApproveProduct(product);
    }
}
