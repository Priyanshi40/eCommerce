using DAL.Models;

namespace BLL.Interfaces;

public interface IUserRepo
{
    UserDetails GetUserByIdentityId(string id);
    UserDetails GetUserById(int id);
    List<Country> GetCountry();
    IEnumerable<State> GetState(int? countryId);
    IEnumerable<City> GetCity(int? stateId);
    List<Address> GetUserAddresses(int userId);
    Task<IQueryable<UserDetails>> GetQueryableUsers(string? searchString);
    int AddUser(UserDetails user);
    int AddUserAddress(Address address);
}
