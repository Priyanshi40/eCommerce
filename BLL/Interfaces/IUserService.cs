using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IUserService
{
    RegisterViewModel GetUserById(string id);
    void AddUser(RegisterViewModel user);
}
