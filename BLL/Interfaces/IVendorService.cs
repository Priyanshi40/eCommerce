
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IVendorService
{
    VendorDetailsViewModel GetVendorsService(string searchString, int pageNumber, int pageSize);
    VendorDetailsViewModel GetVendorDetailsService(int vendorId);
}
