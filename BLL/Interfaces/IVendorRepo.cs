using DAL.Models;

namespace BLL.Interfaces;

public interface IVendorRepo
{
    IQueryable<VendorDetails> GetQueryableVendors(string? searchString);
    VendorDetails? GetVendorDetails(int vendorId);
    string ApproveVendor(UserDetails user);
    void AddVendor(VendorDetails vendor);
}
