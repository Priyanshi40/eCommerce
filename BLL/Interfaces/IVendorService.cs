
using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IVendorService
{
    VendorDetailsViewModel GetVendorsService(string searchString,SortOrder sort, string statusFilter, int pageNumber, int pageSize);
    VendorDetailsViewModel GetVendorDetailsService(int vendorId);
    string ApproveVendor(UserDetails vendor);
    void AddVendor(VendorViewModel vendor);
}
