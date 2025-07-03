using DAL.ViewModels;

namespace BLL.Interfaces;

public interface IUserService
{
    RegisterViewModel GetUserById(string id);
    int AddUser(RegisterViewModel user);
    void AddVendor(VendorViewModel vendor);
}
