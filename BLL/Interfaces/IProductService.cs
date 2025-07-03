using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IProductService
{
    ProductViewModel GetProductsService(string searchString, int pageNumber, int pageSize);
    ProductViewModel GetProductDetailsService(int productId);
    Product UpSertProduct(ProductViewModel product);
}
