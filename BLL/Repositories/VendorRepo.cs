using BLL.Interfaces;
using DAL.Enums;
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
    public string ApproveVendor(UserDetails user)
    {
        try
        {
            UserDetails oldUser = _context.UserDetails.Include(u => u.IUser).FirstOrDefault(b => b.Id == user.Id);
            if (oldUser != null)
            {
                oldUser.Status = user.Status;
                oldUser.AdminComment = user.AdminComment;
                if (user.Status == ProductStatus.Approved)
                {
                    oldUser.IsApproved = true;
                }
                _context.UserDetails.Update(oldUser);
                _context.SaveChanges();
                return oldUser.IUser.Id;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error While Approving/Rejecting User: {ex.Message}");
            throw;
        }
    }
    public void AddVendor(VendorDetails vendor)
    {
        try
        {
            if (vendor != null)
            {
                VendorDetails oldVendor = _context.VendorDetails.FirstOrDefault(u => u.Id == vendor.Id || u.VendorId == vendor.VendorId);
                if (oldVendor != null)
                {
                    oldVendor.BusinessName = vendor.BusinessName ?? oldVendor.BusinessName;
                    oldVendor.BusinessAddress = vendor.BusinessAddress ?? oldVendor.BusinessAddress;
                    oldVendor.GSTNumber = vendor.GSTNumber ?? oldVendor.GSTNumber;
                    oldVendor.DocumentType = vendor.DocumentType != 0 ? vendor.DocumentType : oldVendor.DocumentType;
                    oldVendor.FileUrl = vendor.FileUrl ?? oldVendor.FileUrl;
                    _context.VendorDetails.Update(oldVendor);
                }
                else
                {
                    _context.VendorDetails.Add(vendor);
                }
                _context.SaveChanges();

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Adding Vendor: {ex.Message}");
            throw;
        }
    }
}
