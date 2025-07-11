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
    public async Task<IQueryable<UserDetails>> GetQueryableUsers(string? searchString)
    {
        IList<IdentityUser> customers = await _userManager.GetUsersInRoleAsync("User");
        List<string> customerIds = customers.Select(c => c.Id).ToList();
        IQueryable<UserDetails> users = _context.UserDetails.Include(p => p.IUser).Where(p => customerIds.Contains(p.IUser.Id)).AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
            users = users.Where(u => u.Firstname.ToLower().Contains(searchString.ToLower().Trim())).AsQueryable();

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
}
