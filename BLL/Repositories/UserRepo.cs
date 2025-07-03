using BLL.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repositories;

public class UserRepo : IUserRepo
{
    private readonly E_CommerceContext _context;
    public UserRepo(E_CommerceContext context)
    {
        _context = context;
    }
    public UserDetails GetUserByIdentityId(string id)
    {
        return _context.UserDetails.Include(u => u.IUser).FirstOrDefault(u => u.IdentityUserId == id)!;
    }
    public int AddUser(UserDetails user)
    {
        try
        {
            if (user != null)
            {
                UserDetails oldUser = _context.UserDetails.FirstOrDefault(u => u.Id == user.Id || u.IdentityUserId == user.IdentityUserId);
                if (oldUser != null)
                {
                    oldUser.Firstname = user.Firstname ?? oldUser.Firstname;
                    oldUser.Lastname = user.Lastname ?? oldUser.Lastname;
                    // oldUser.ProfileImage = user.ProfileImage ?? oldUser.ProfileImage;
                    _context.UserDetails.Update(oldUser);
                    _context.SaveChanges();
                    return oldUser.Id;
                }
                else
                {
                    _context.UserDetails.Add(user);
                    _context.SaveChanges();
                    return user.Id;
                }
            }
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Adding User: {ex.Message}");
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
