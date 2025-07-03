using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models;

public class ProductImage
{
    [Key]
    public int Id { get; set; }

    public string ImageUrl { get; set; }

    public int? ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public virtual Product Product { get; set; }
}