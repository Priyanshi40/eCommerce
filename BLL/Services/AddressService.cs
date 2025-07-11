using AutoMapper;
using BLL.Interfaces;
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Services;

public class AddressService : IAddressService
{
    private readonly IAddressRepo _addRepo;
    private readonly IUserRepo _userRepo;
    private readonly IMapper _mapper;
    public AddressService(IAddressRepo addRepo, IUserRepo userRepo, IMapper mapper)
    {
        _addRepo = addRepo;
        _userRepo = userRepo;
        _mapper = mapper;
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
    public AddressViewModel GetAddressById(int AddressId, int userId)
    {
        Address address = _addRepo.GetAddressById(AddressId);
        UserDetails user = _userRepo.GetUserById(userId);
        AddressViewModel viewModel = new();

        if (user != null)
        {
            viewModel.FirstName = user.Firstname;
            viewModel.LastName = user.Lastname;
            viewModel.UserId = userId;
        }

        if (address != null)
        {
            viewModel = _mapper.Map<AddressViewModel>(address);
            viewModel.FirstName = user?.Firstname;
            viewModel.LastName = user?.Lastname;
        }

        return viewModel;
    }
    public AddressViewModel GetUserAddresses(int userId)
    {
        List<Address> addresses = _addRepo.GetUserAddresses(userId);
        UserDetails user = _userRepo.GetUserById(userId);
        List<AddressViewModel> mappedAddresses = _mapper.Map<List<AddressViewModel>>(addresses);
        foreach (AddressViewModel addr in mappedAddresses)
        {
            addr.FirstName = user.Firstname;
            addr.LastName = user.Lastname;
        }
        return new AddressViewModel
        {
            Address = mappedAddresses
        };
    }
    public int AddUserAddress(AddressViewModel address)
    {
        if (address == null)
            return -1;

        Address oldAdd = _mapper.Map<Address>(address);
        if (address.Id != 0)
        {
            oldAdd.Id = address.Id;
            oldAdd.Modifiedat = DateTime.Now;
        }
        else
            oldAdd.CreatedBy = address.ModifiedBy;

        return _addRepo.AddUserAddress(oldAdd);
    }
}