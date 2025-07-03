using BLL.Interfaces;
using BLL.Utility;
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
    public VendorDetailsViewModel GetVendorsService(string searchString, int pageNumber, int pageSize)
    {
        IQueryable<VendorDetails> queyableVendors = _vendorRepo.GetQueryableVendors(searchString);
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
        }
        return vendorDetails;
    }
}
