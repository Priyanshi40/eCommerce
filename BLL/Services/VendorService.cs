using BLL.Interfaces;
using BLL.Utility;
using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Services;

public class VendorService : IVendorService
{
    private readonly IVendorRepo _vendorRepo;
    private readonly ImageService _imgService;
    public VendorService(IVendorRepo vendorRepo, ImageService imgService)
    {
        _vendorRepo = vendorRepo;
        _imgService = imgService;
    }
    public VendorDetailsViewModel GetVendorsService(string searchString,SortOrder sort, string statusFilter, int pageNumber, int pageSize)
    {
        IQueryable<VendorDetails> queyableVendors = _vendorRepo.GetQueryableVendors(searchString);
        if (!string.IsNullOrEmpty(statusFilter))
        {
            if (Enum.TryParse<ProductStatus>(statusFilter, out var parsedStatus))
            {
                queyableVendors = queyableVendors.Where(p => p.UserNavigation.Status == parsedStatus);
            }
        }
        queyableVendors = sort switch
        {
            SortOrder.Name => queyableVendors.OrderByDescending(u => u.UserNavigation.Firstname),
            SortOrder.BusinessName => queyableVendors.OrderByDescending(u => u.BusinessName),
            SortOrder.Email => queyableVendors.OrderByDescending(u => u.UserNavigation.IUser.Email),
            _ => queyableVendors.OrderBy(u => u.BusinessName),
        };
        VendorDetailsViewModel vendorsView = new();
        if (queyableVendors != null)
        {
            int totalRecords = queyableVendors.Count();
            var paginatedVendors = queyableVendors.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            vendorsView.Vendors = paginatedVendors;
            vendorsView.PageSize = pageSize;
            vendorsView.PageNumber = pageNumber;
            vendorsView.TotalRecords = totalRecords;
            vendorsView.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        }
        return vendorsView;
    }
    public VendorDetailsViewModel GetVendorDetailsService(int vendorId)
    {
        VendorDetails vendorData = _vendorRepo.GetVendorDetails(vendorId);
        VendorDetailsViewModel vendorDetails = new();
        if (vendorData != null)
        {
            vendorDetails.VendorId = vendorId;
            vendorDetails.Email = vendorData.UserNavigation.IUser.Email;
            vendorDetails.Phone = vendorData.UserNavigation.IUser.PhoneNumber;
            vendorDetails.Firstname = vendorData.UserNavigation.Firstname;
            vendorDetails.Lastname = vendorData.UserNavigation.Lastname;
            vendorDetails.BusinessName = vendorData.BusinessName;
            vendorDetails.GSTNumber = vendorData.GSTNumber;
            vendorDetails.BusinessAddress = vendorData.BusinessAddress;
            vendorDetails.DocumentType = (VendorDocuments)vendorData.DocumentType;
            vendorDetails.FileUrl = vendorData.FileUrl;
        }
        return vendorDetails;
    }
    public string ApproveVendor(UserDetails vendor)
    {
        return _vendorRepo.ApproveVendor(vendor);
    }
    public void AddVendor(VendorViewModel vendor)
    {
        if (vendor != null)
        {
            VendorDetails newVendor = new()
            {
                BusinessName = vendor.BusinessName,
                BusinessAddress = vendor.BusinessAddress,
                DocumentType = (int)vendor.DocumentType,
                GSTNumber = vendor.GSTNumber,
                FileUrl = vendor.FileUrl,
                VendorId = vendor.VendorId
            };

            if (vendor.Id != 0)
            {
                newVendor.Id = vendor.Id;
            }

            _vendorRepo.AddVendor(newVendor); 
        }
    }
}
