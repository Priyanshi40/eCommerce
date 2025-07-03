using DAL.Models;

namespace BLL.Interfaces;

public interface IUserRepo
{
    UserDetails GetUserByIdentityId(string id);
    int AddUser(UserDetails user);
    void AddVendor(VendorDetails vendor);
}
