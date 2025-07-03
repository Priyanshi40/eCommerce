using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Services;

public class UserService : IUserService
{
    private readonly IUserRepo _userRepo;
    public UserService(IUserRepo userRepo)
    {
        _userRepo = userRepo;
    }
    public RegisterViewModel GetUserById(string id)
    {
        UserDetails user = _userRepo.GetUserByIdentityId(id);
        RegisterViewModel userDetail = new()
        {
            UserId = user.Id,
            FirstName = user.Firstname,
            LastName = user.Lastname,
            Email = user.IUser.Email,
            Phone = user.IUser.PhoneNumber,
        };
        return userDetail;
    }
    public int AddUser(RegisterViewModel user)
    {
        if (user != null)
        {
            UserDetails oldUser = new()
            {
                Firstname = user.FirstName,
                Lastname = user.LastName,
                IdentityUserId = user.IdentityUserId,
            };
            if (user.UserId != 0 || user.IdentityUserId != null)
            {
                oldUser.Id = user.UserId;
                oldUser.IdentityUserId = user.IdentityUserId;
            }
            return _userRepo.AddUser(oldUser);
        }
        return -1;
    }

    public void AddVendor(VendorViewModel vendor)
    {
        if (vendor != null)
        {
            VendorDetails newVendor = new()
            {
                BusinessName = vendor.BusinessName,
                BusinessAddress = vendor.BusinessAddress,
                // DocumentType = vendor.DocumentType,
                DocumentType = (int)Enum.Parse(typeof(VendorDocuments), vendor.DocumentType.ToString()),
                // DocumentName = vendor.DocumentName??vendor.DocumentType.ToString(),
                GSTNumber = vendor.GSTNumber,
                FileUrl = vendor.FileUrl,
                VendorId = vendor.VendorId
            };

            if (vendor.Id != 0)
            {
                newVendor.Id = vendor.Id;
            }

            _userRepo.AddVendor(newVendor); 
        }
    }

}
