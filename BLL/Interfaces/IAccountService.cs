using DAL.ViewModels;
using Microsoft.AspNetCore.Http;

namespace BLL.Interfaces;

public interface IAccountService
{
    Task<(bool Success, List<string> Errors)> RegisterCustomer(RegisterViewModel model);
    Task<(bool Success, List<string> Errors)> RegisterVendor(HttpContext context, VendorViewModel model);
}
