using BLL.Interfaces;
using DAL.Enums;
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
    public UserViewModel GetUserById(int id)
    {
        UserDetails user = _userRepo.GetUserById(id);
        UserViewModel userDetail = new()
        {
            Id = user.Id,
            IdentityUserId = user.IdentityUserId,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            Email = user.IUser.Email,
            Phone = user.IUser.PhoneNumber,
        };
        return userDetail;
    }

    public async Task<UserViewModel> GetUsersService(string searchString, SortOrder sort, int pageNumber, int pageSize)
    {
        IQueryable<UserDetails> queyableUsers = await _userRepo.GetQueryableUsers(searchString);
        queyableUsers = sort switch
        {
            SortOrder.Name => queyableUsers.OrderByDescending(u => u.Firstname),
            SortOrder.Email => queyableUsers.OrderByDescending(u => u.IUser.Email),
            _ => queyableUsers.OrderBy(u => u.Firstname),
        };
        UserViewModel vendorsView = new();
        if (queyableUsers != null)
        {
            int totalRecords = queyableUsers.Count();
            List<UserDetails> paginatedUsers = queyableUsers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            vendorsView.Users = paginatedUsers;
            vendorsView.PageSize = pageSize;
            vendorsView.PageNumber = pageNumber;
            vendorsView.TotalRecords = totalRecords;
            vendorsView.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }
        return vendorsView;
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
                Status = ProductStatus.Approved,
                ModifiedBy = user.ModifiedBy
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
}
