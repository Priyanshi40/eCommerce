using BLL.Interfaces;
using BLL.Utility;
using DAL.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IVendorService _vendorService;
        private readonly ImageService _imageService;

        public AccountService(UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager,
                              IUserService userService,
                              IVendorService vendorService,
                              ImageService imageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _vendorService = vendorService;
            _imageService = imageService;
        }
        public async Task<(bool Success, List<string> Errors)> RegisterCustomer(RegisterViewModel model)
        {
            IdentityUser user = new()
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToList());

            model.IdentityUserId = user.Id;
            _userService.AddUser(model);

            await _userManager.AddToRoleAsync(user, "User");
            await _signInManager.SignInAsync(user, false);

            return (true, new());
        }
        public async Task<(bool Success, List<string> Errors)> RegisterVendor(HttpContext context, VendorViewModel model)
        {
            string? step1Json = context.Session.GetString("VendorStep1");
            string? step2Json = context.Session.GetString("VendorStep2");

            if (string.IsNullOrEmpty(step1Json) || string.IsNullOrEmpty(step2Json))
                return (false, new List<string> { "Session expired. Please try again." });

            RegisterViewModel? step1 = JsonConvert.DeserializeObject<RegisterViewModel>(step1Json);
            VendorViewModel? step2 = JsonConvert.DeserializeObject<VendorViewModel>(step2Json);

            IdentityUser user = new()
            {
                UserName = step1.Email,
                Email = step1.Email,
                PhoneNumber = step1.Phone
            };

            IdentityResult result = await _userManager.CreateAsync(user, step1.Password);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description).ToList());

            RegisterViewModel newUser = new()
            {
                FirstName = step1.FirstName,
                LastName = step1.LastName,
                IdentityUserId = user.Id
            };

            int userId = _userService.AddUser(newUser);
            string fileUrl = _imageService.SaveImageService(model.File, "vendor_docs");

            VendorViewModel vendor = new()
            {
                VendorId = userId,
                BusinessName = step2.BusinessName,
                BusinessAddress = step2.BusinessAddress,
                GSTNumber = step2.GSTNumber,
                DocumentType = model.DocumentType,
                FileUrl = fileUrl
            };

            _vendorService.AddVendor(vendor);
            await _userManager.AddToRoleAsync(user, "Vendor");
            await _signInManager.SignInAsync(user, false);

            context.Session.Remove("VendorStep1");
            context.Session.Remove("VendorStep2");

            return (true, new());
        }
    }
}