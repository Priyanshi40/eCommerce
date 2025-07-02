using DAL.Models;

namespace BLL.Interfaces;

public interface IUserRepo
{
    void AddUser(UserDetails user);
}
