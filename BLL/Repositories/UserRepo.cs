using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BLL.Repositories;

public class UserRepo : IUserRepo
{
    private readonly E_CommerceContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    public UserRepo(E_CommerceContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public UserDetails GetUserByIdentityId(string id)
    {
        return _context.UserDetails.Include(u => u.IUser).FirstOrDefault(u => u.IdentityUserId == id)!;
    }
    public UserDetails GetUserById(int id)
    {
        return _context.UserDetails.Include(u => u.IUser).FirstOrDefault(u => u.Id == id)!;
    }

    public List<Country> GetCountry()
    {
        return _context.Country.ToList();
    }
    public IEnumerable<State> GetState(int? countryId)
    {
        List<State> stateList = _context.State.Where(c => c.CountryId == countryId).ToList();
        if (stateList == null)
        {
            return _context.State.ToList();
        }
        return stateList;
    }
    public IEnumerable<City> GetCity(int? stateId)
    {
        List<City> cityList = _context.City.Where(c => c.StateId == stateId).ToList();
        if (cityList == null)
        {
            return _context.City.ToList();
        }
        return cityList;
    }
    public List<Address> GetUserAddresses(int userId)
    {
        var addresses = _context.Addresses
            .Include(a => a.City)
            .ThenInclude(c => c.State)
            .ThenInclude(s => s.Country)
            .Include(a => a.State)
            .Include(a => a.Country)
            .Where(a => a.UserId == userId)
            .ToList();

        return addresses;
    }
    public async Task<IQueryable<UserDetails>> GetQueryableUsers(string? searchString)
    {
        IList<IdentityUser> customers = await _userManager.GetUsersInRoleAsync("User");
        var customerIds = customers.Select(c => c.Id).ToList();
        var users = _context.UserDetails.Include(p => p.IUser).Where(p => customerIds.Contains(p.IUser.Id)).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            users = users.Where(u => u.Firstname.ToLower().Contains(searchString.ToLower().Trim())).AsQueryable();
        }
        return users;
    }
    public int AddUser(UserDetails user)
    {
        try
        {
            if (user != null)
            {
                UserDetails oldUser = _context.UserDetails.FirstOrDefault(u => u.Id == user.Id || u.IdentityUserId == user.IdentityUserId);
                if (oldUser != null)
                {
                    oldUser.Firstname = user.Firstname ?? oldUser.Firstname;
                    oldUser.Lastname = user.Lastname ?? oldUser.Lastname;
                    oldUser.ModifiedBy = user.ModifiedBy;
                    oldUser.Modifiedat = DateTime.Now;
                    _context.UserDetails.Update(oldUser);
                    _context.SaveChanges();
                    return oldUser.Id;
                }
                else
                {
                    _context.UserDetails.Add(user);
                    _context.SaveChanges();
                    return user.Id;
                }
            }
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Adding User: {ex.Message}");
            throw;
        }
    }
    public int AddUserAddress(Address address)
    {
        try
        {
            if (address != null)
            {
                Address oldAddress = _context.Addresses.FirstOrDefault(u => u.Id == address.Id);
                if (oldAddress != null)
                {
                    if (address.IsDefault)
                    {
                        var otherAdd = _context.Addresses.Where(a => a.UserId == oldAddress.UserId).ToList();
                        foreach (var add in otherAdd)
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
            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Adding Address: {ex.Message}");
            throw;
        }
    }
}
