using AutoMapper;
using BLL.Interfaces;
using DAL.Enums;
using DAL.Models;
using DAL.ViewModels;

namespace BLL.Services;

public class VendorService : IVendorService
{
    private readonly IVendorRepo _vendorRepo;
    private readonly IMapper _mapper;
    public VendorService(IVendorRepo vendorRepo, IMapper mapper)
    {
        _vendorRepo = vendorRepo;
        _mapper = mapper;
    }
    public VendorDetailsViewModel GetVendorsService(string searchString,SortOrder sort, string statusFilter, int pageNumber, int pageSize)
    {
        IQueryable<VendorDetails> queyableVendors = _vendorRepo.GetQueryableVendors(searchString);
        if (!string.IsNullOrEmpty(statusFilter))
        {
            if (Enum.TryParse(statusFilter, out ProductStatus parsedStatus))
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
            List<VendorDetails> paginatedVendors = queyableVendors.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

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
            vendorDetails = _mapper.Map<VendorDetailsViewModel>(vendorData);

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
            VendorDetails vendorMapped = _mapper.Map<VendorDetails>(vendor);
            if (vendor.Id != 0)
                vendorMapped.Id = vendor.Id;

            _vendorRepo.AddVendor(vendorMapped); 
        }
    }
}
