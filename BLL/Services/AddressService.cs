using AutoMapper;
using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepo _addRepo;
    private readonly IUserRepo _userRepo;
    // private readonly IMapper _mapper;
    public AddressService(IAddressRepo addRepo, IUserRepo userRepo)
    {
        _addRepo = addRepo;
        _userRepo = userRepo;
        // _mapper = mapper;
    }
    public List<Country> GetCountryService()
    {
        return _addRepo.GetCountry();
    }
    public IEnumerable<State> GetStateService(int? countryId)
    {
        return _addRepo.GetState(countryId);
    }
    public IEnumerable<City> GetCityService(int? stateId)
    {
        return _addRepo.GetCity(stateId);
    }
    public AddressViewModel GetAddressById(int AddressId,int userId)
    {
        Address address = _addRepo.GetAddressById(AddressId);
        UserDetails user = _userRepo.GetUserById(userId);
        AddressViewModel addDetails = new ();
        
        if (address != null)
        {
            // addDetails = _mapper.Map<Address>(AddressViewModel);
            addDetails.Id = address.Id;
            addDetails.HouseName = address.HouseName;
            addDetails.Street = address.Street;
            addDetails.Landmark = address.Landmark;
            addDetails.StateId = address.StateId;
            addDetails.CityId = address.CityId;
            addDetails.CountryId = address.CountryId;
            addDetails.StateName = address.State.Name;
            addDetails.CityName = address.City.Name;
            addDetails.CountryName = address.Country.Name;
            addDetails.PostalCode = address.PostalCode;
            addDetails.IsDefault = address.IsDefault;
            addDetails.FirstName = user.Firstname;
            addDetails.LastName = user.Lastname;
        }
        return addDetails;
    }
    public AddressViewModel GetUserAddresses(int userId)
    {
        List<Address> addresses = _addRepo.GetUserAddresses(userId);
        UserDetails user = _userRepo.GetUserById(userId);
        AddressViewModel address = new()
        {
            Address = addresses.Select(a => new AddressViewModel
            {
                Id = a.Id,
                Street = a.Street,
                Landmark = a.Landmark,
                CityId = a.CityId,
                StateId = a.StateId,
                CountryId = a.CountryId,
                CityName = a.City?.Name,
                StateName = a.State?.Name,
                CountryName = a.Country?.Name,
                PostalCode = a.PostalCode,
                IsDefault = a.IsDefault,
                HouseName = a.HouseName,
                FirstName = user.Firstname,
                LastName = user.Lastname,
            }).ToList(),
        };
        return address;
    }

    public int AddUserAddress(AddressViewModel address)
    {
        if (address != null)
        {
            Address oldAdd = new()
            {
                Street = address.Street,
                Landmark = address.Landmark,
                PostalCode = address.PostalCode,
                HouseName = address.HouseName,
                CityId = address.CityId,
                StateId = address.StateId,
                CountryId = address.CountryId,
                ModifiedBy = address.ModifiedBy,
                IsDefault = address.IsDefault,
            };
            if (address.Id != 0)
            {
                oldAdd.Id = address.Id;
            }
            return _addRepo.AddUserAddress(oldAdd);
        }
        return -1;
    }
}
