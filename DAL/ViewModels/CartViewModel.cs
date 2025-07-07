namespace DAL.ViewModels;

public class CartViewModel
{
    public ICollection<CartViewModel> CartItems { get; set; } = new List<CartViewModel>();
    public int Id { get; set; }
    public int CartId { get; set; }
    public int? UserId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string CoverImage { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }
}
