namespace Hempact.Models;

public class Order
{
    public int Id { get; set; }

    public string? CustomerName { get; set; }

    public string? Email { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Status { get; set; }
}