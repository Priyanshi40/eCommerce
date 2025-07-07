using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IUserService
{
    RegisterViewModel GetUserById(string id);
    Task<UserViewModel> GetUsersService(string searchString, string statusFilter, int pageNumber, int pageSize);
    int AddUser(RegisterViewModel user);
}
