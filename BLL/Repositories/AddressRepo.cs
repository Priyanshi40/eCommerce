using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repositories;

public class AddressRepo : IAddressRepo
{
    private readonly E_CommerceContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    public AddressRepo(E_CommerceContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public List<Country> GetCountry()
    {
        return _context.Country.ToList();
    }
    public IEnumerable<State> GetState(int? countryId)
    {
        List<State> stateList = _context.State.Where(c => c.CountryId == countryId).ToList();
        if (stateList == null)
            return _context.State.ToList();

        return stateList;
    }
    public IEnumerable<City> GetCity(int? stateId)
    {
        List<City> cityList = _context.City.Where(c => c.StateId == stateId).ToList();
        if (cityList == null)
            return _context.City.ToList();
        return cityList;
    }
    public Address GetAddressById(int AddressId)
    {
        Address? addresses = _context.Addresses
            .Include(a => a.City)
            .Include(a => a.State)
            .Include(a => a.Country)
            .FirstOrDefault(a => a.Id == AddressId);

        return addresses;
    }
    public List<Address> GetUserAddresses(int userId)
    {
        List<Address> addresses = _context.Addresses
            .Include(a => a.City)
            .Include(a => a.State)
            .Include(a => a.Country)
            .Where(a => a.UserId == userId)
            .ToList();

        return addresses;
    }
    public int AddUserAddress(Address address)
    {
        try
        {
            if (address == null) return 0;
            Address oldAddress = _context.Addresses.FirstOrDefault(u => u.Id == address.Id);
            if (oldAddress != null)
            {
                if (address.IsDefault)
                {
                    List<Address> otherAdd = _context.Addresses.Where(a => a.UserId == oldAddress.UserId).ToList();
                    foreach (Address add in otherAdd)
                        add.IsDefault = false;

                    oldAddress.IsDefault = true;
                }
                
                oldAddress.Street = address.Street ?? oldAddress.Street;
                oldAddress.Landmark = address.Landmark ?? oldAddress.Landmark;
                oldAddress.PostalCode = address.PostalCode ?? oldAddress.PostalCode;
                oldAddress.HouseName = address.HouseName ?? oldAddress.HouseName;
                oldAddress.CityId = address.CityId != 0 ? address.CityId : oldAddress.CityId;
                oldAddress.CountryId = address.CountryId != 0 ? address.CountryId : oldAddress.CountryId;
                oldAddress.StateId = address.StateId != 0 ? address.StateId : oldAddress.StateId;
                oldAddress.ModifiedBy = address.ModifiedBy;
                oldAddress.Modifiedat = DateTime.Now;
                _context.Addresses.Update(oldAddress);
                _context.SaveChanges();
                return oldAddress.Id;
            }
            else
            {
                _context.Addresses.Add(address);
                _context.SaveChanges();
                return address.Id;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Adding Address: {ex.Message}");
            throw;
        }
    }
}