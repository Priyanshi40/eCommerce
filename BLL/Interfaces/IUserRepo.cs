using DAL.Models;

namespace BLL.Interfaces;

public interface IUserRepo
{
    UserDetails GetUserByIdentityId(string id);
    void AddUser(UserDetails user);
}
