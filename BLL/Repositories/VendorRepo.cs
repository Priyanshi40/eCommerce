using BLL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repositories;

public class VendorRepo : IVendorRepo
{
    private readonly E_CommerceContext _context;
    public VendorRepo(E_CommerceContext context)
    {
        _context = context;
    }
    public IQueryable<VendorDetails> GetQueryableVendors(string? searchString)
    {
        var vendors = _context.VendorDetails.Include(p => p.UserNavigation.IUser).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            vendors = vendors.Where(u => u.BusinessName.ToLower().Contains(searchString.ToLower().Trim()));
        }
        return vendors;
    }
    public VendorDetails? GetVendorDetails(int vendorId)
    {
        return _context.VendorDetails
            .Include(p => p.UserNavigation.IUser)
            .FirstOrDefault(v => v.Id == vendorId);
    }
}
