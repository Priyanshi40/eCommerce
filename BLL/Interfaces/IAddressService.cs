using DAL.Models;
using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IAddressService
{
    List<Country> GetCountryService();
    IEnumerable<State> GetStateService(int? countryId);
    IEnumerable<City> GetCityService(int? stateId);
    AddressViewModel GetAddressById(int AddressId, int userId);
    AddressViewModel GetUserAddresses(int userId);
    int AddUserAddress(AddressViewModel address);

}
