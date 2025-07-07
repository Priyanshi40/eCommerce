
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IVendorService
{
    VendorDetailsViewModel GetVendorsService(string searchString, string statusFilter, int pageNumber, int pageSize);
    VendorDetailsViewModel GetVendorDetailsService(int vendorId);
    bool ApproveVendor(UserDetails vendor);
    void AddVendor(VendorViewModel vendor);
}
