using DAL.Models;

namespace BLL.Interfaces;

public interface IUserRepo
{
    UserDetails GetUserByIdentityId(string id);
    UserDetails GetUserById(int id);
    Task<IQueryable<UserDetails>> GetQueryableUsers(string? searchString);
    int AddUser(UserDetails user);
}
