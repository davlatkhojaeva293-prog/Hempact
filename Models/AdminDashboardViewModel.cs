namespace Hempact.Models;

public class AdminDashboardViewModel
{
    public List<Product> Products { get; set; } = new List<Product>();

    public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public List<ContactMessage> ContactMessages { get; set; } = new List<ContactMessage>();

    public int TotalProducts { get; set; }

    public int TotalSubscriptions { get; set; }

    public int TotalMessages { get; set; }
    public List<Order> Orders { get; set; } = new List<Order>();

public int TotalOrders { get; set; }
}