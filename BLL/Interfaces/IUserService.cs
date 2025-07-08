using DAL.Enums;
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IUserService
{
    RegisterViewModel GetUserById(string id);
    UserViewModel GetUserById(int id);
    Task<UserViewModel> GetUsersService(string searchString,SortOrder sortOrder, string statusFilter, int pageNumber, int pageSize);
    int AddUser(RegisterViewModel user);
}
