using BLL.Interfaces;
using DAL.Models;

namespace BLL.Repositories;

public class UserRepo : IUserRepo
{
    private readonly E_CommerceContext _context;
    public UserRepo(E_CommerceContext context)
    {
        _context = context;
    }
    public void AddUser(UserDetails user)
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
                    // oldUser.ProfileImage = user.ProfileImage ?? oldUser.ProfileImage;
                    _context.UserDetails.Update(oldUser);
                }
                else
                {
                    _context.UserDetails.Add(user);
                }
                _context.SaveChanges();

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error Adding User: {ex.Message}");
            throw;
        }
    }
}
