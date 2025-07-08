using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IProductService
{
    ProductViewModel GetProductsService(string searchString,SortOrder sortOrder, int category, string statusFilter, int pageNumber, int pageSize, int userId = 0);
    // ProductViewModel GetProductsService(string searchString,SortOrder sortOrder, bool isAscending, int category, string statusFilter, int pageNumber, int pageSize, int userId = 0);
    ProductViewModel GetProductDetailsService(int productId);
    Product UpSertProduct(ProductViewModel product);
    bool DeleteProduct(Product product);
    string? ApproveProduct(Product product);
    int AddProductToWishlist(int productId, int userId);
}
