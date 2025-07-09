using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IUserService
{
    RegisterViewModel GetUserById(string id);
    UserViewModel GetUserById(int id);
    List<Country> GetCountryService();
    IEnumerable<State> GetStateService(int? countryId);
    IEnumerable<City> GetCityService(int? stateId);
    AddressViewModel GetUserAddresses(int userId);
    Task<UserViewModel> GetUsersService(string searchString, SortOrder sortOrder, int pageNumber, int pageSize);
    int AddUser(RegisterViewModel user);
    int AddUserAddress(AddressViewModel address);
}
