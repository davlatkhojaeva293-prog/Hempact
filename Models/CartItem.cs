namespace Hempact.Models;

public class CartItem
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public decimal Total
    {
        get { return Price * Quantity; }
    }
    public string? ImageUrl { get; set; }
}