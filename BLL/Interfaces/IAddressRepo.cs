using DAL.Models;

namespace BLL.Interfaces;

public interface IAddressRepo
{
    List<Country> GetCountry();
    IEnumerable<State> GetState(int? countryId);
    IEnumerable<City> GetCity(int? stateId);
    Address GetAddressById(int AddressId);
    List<Address> GetUserAddresses(int userId);
    int AddUserAddress(Address address);
}
