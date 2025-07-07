using DAL.Models;

namespace BLL.Interfaces;

public interface IVendorRepo
{
    IQueryable<VendorDetails> GetQueryableVendors(string? searchString);
    VendorDetails? GetVendorDetails(int vendorId);
    bool ApproveVendor(UserDetails user);
    void AddVendor(VendorDetails vendor);
}
