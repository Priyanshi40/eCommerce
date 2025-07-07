using DAL.Models;

namespace DAL.ViewModels;

public class VendorDetailsViewModel
{
    public ICollection<VendorDetailsViewModel> VendorDetails { get; set; } = new List<VendorDetailsViewModel>();
    public ICollection<VendorDetails> Vendors { get; set; } = new List<VendorDetails>();
    public int Id { get; set; }
    public string? IdentityUserId { get; set; }
    public int VendorId { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Firstname { get; set; }
    public string? Lastname { get; set; }
    public bool IsApproved { get; set; }
    public string BusinessName { get; set; }
    public string? GSTNumber { get; set; }
    public string? BusinessAddress { get; set; }
    public VendorDocuments DocumentType { get; set; }
    public string FileUrl { get; set; }

    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
}
