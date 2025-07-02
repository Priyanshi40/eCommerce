using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IUserService
{
    void AddUser(RegisterViewModel user);
}
