using System.Threading.Tasks;
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

    public async Task<UserViewModel> GetUsersService(string searchString, string statusFilter, int pageNumber, int pageSize)
    {
        IQueryable<UserDetails> queyableUsers = await _userRepo.GetQueryableUsers(searchString);
        if (!string.IsNullOrEmpty(statusFilter))
        {
            if (Enum.TryParse<ProductStatus>(statusFilter, out var parsedStatus))
            {
                queyableUsers = queyableUsers.Where(p => p.Status == parsedStatus);
            }
        }
        UserViewModel vendorsView = new();
        if (queyableUsers != null)
        {
            int totalRecords = queyableUsers.Count();
            var paginatedUsers = queyableUsers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

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
