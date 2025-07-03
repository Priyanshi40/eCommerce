using DAL.Models;

namespace BLL.Interfaces;

public interface IVendorRepo
{
    IQueryable<VendorDetails> GetQueryableVendors(string? searchString);
    VendorDetails? GetVendorDetails(int vendorId);
}
