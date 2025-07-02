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
    public void AddUser(RegisterViewModel user)
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
            _userRepo.AddUser(oldUser);
        }
    }
}
