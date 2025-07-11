using DAL.Models;

namespace BLL.Interfaces;

public interface IProductRepo
{
    IQueryable<Product> GetQueryableProducts(string? searchString);
    Product? GetProductDetails(int productId);
    Product UpSertProduct(Product product);
    bool DeleteProduct(Product product);
    int ApproveProduct(Product product);
    int AddProductToWishlist(int productId, int userId);
}
