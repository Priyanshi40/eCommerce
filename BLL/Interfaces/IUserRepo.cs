using DAL.Models;

namespace BLL.Interfaces;

public interface IUserRepo
{
    UserDetails GetUserByIdentityId(string id);
    Task<IQueryable<UserDetails>> GetQueryableUsers(string? searchString);
    int AddUser(UserDetails user);
}
