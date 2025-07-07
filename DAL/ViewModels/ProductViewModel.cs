using System.ComponentModel.DataAnnotations;
using DAL.Models;
using Microsoft.AspNetCore.Http;

namespace DAL.ViewModels;

public class ProductViewModel
{
    public ICollection<Product> Products { get; set; } = new List<Product>();
    public int Id { get; set; }

    [Required]
    [MaxLength(200, ErrorMessage = "Name can't be longer than 200 characters")]
    public string Name { get; set; }

    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    public ProductStatus Status { get; set; }
    public string? CoverImage { get; set; }
    public IFormFile? Cover { get; set; }
    public bool IsDeleted { get; set; }
    public int CreatedBy { get; set; }
    public int ModifiedBy { get; set; }

    [Required(ErrorMessage = "Category is required")]
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
    public List<IFormFile>? GalleryImages { get; set; }
    public List<string> ProductImages { get; set; } = new List<string>();
    public List<string> RemovedImages { get; set; } = new List<string>();
    public List<int> WishlistProductIds { get; set; } = new List<int>();
}
