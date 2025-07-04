using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IProductService
{
    ProductViewModel GetProductsService(string searchString,int category,string statusFilter,int pageNumber, int pageSize,int userId = 0);
    ProductViewModel GetProductDetailsService(int productId);
    Product UpSertProduct(ProductViewModel product);
    bool DeleteProduct(Product product);
    bool ApproveProduct(Product product);
}
