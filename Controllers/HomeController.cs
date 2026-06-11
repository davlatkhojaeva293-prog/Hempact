using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Hempact.Models;
using Hempact.Data;
using System.Text.Json;


namespace Hempact.Controllers;

public class HomeController : Controller
{
 private readonly ILogger<HomeController> _logger;
private readonly ApplicationDbContext _context;

public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
{
    _logger = logger;
    _context = context;
}

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
public IActionResult Products()
{
    var products = _context.Products.ToList();

    return View(products);
}
public IActionResult Subscription()
{
    return View();
}
[HttpPost]
public IActionResult AddSubscription(string fullName, string email, string planType)
{
    var subscription = new Subscription
    {
        FullName = fullName,
        Email = email,
        PlanType = planType
    };

    _context.Subscriptions.Add(subscription);
    _context.SaveChanges();

    return RedirectToAction("Admin");
}
public IActionResult Contact()
{
    return View();
}
[HttpPost]
public IActionResult AddContactMessage(string fullName, string email, string message)
{
    var contactMessage = new ContactMessage
    {
        FullName = fullName,
        Email = email,
        Message = message
    };

    _context.ContactMessages.Add(contactMessage);
    _context.SaveChanges();

    return RedirectToAction("Admin");
}
public IActionResult Admin()
{
    var viewModel = new AdminDashboardViewModel
{
    Products = _context.Products.ToList(),
    Subscriptions = _context.Subscriptions.ToList(),
    ContactMessages = _context.ContactMessages.ToList(),
    Orders = _context.Orders.ToList(),

    TotalProducts = _context.Products.Count(),
    TotalSubscriptions = _context.Subscriptions.Count(),
    TotalMessages = _context.ContactMessages.Count(),
    TotalOrders = _context.Orders.Count()
};

    return View(viewModel);
}
[HttpPost]
public IActionResult AddProduct(string name, decimal price, string description, int stock, string imageUrl)

{
  var product = new Product
{
    Name = name,
    Price = price,
    Description = description,
    Stock = stock,
    ImageUrl = imageUrl
};

    _context.Products.Add(product);
    _context.SaveChanges();

    return RedirectToAction("Admin");
}
[HttpPost]
public IActionResult DeleteProduct(int id)
{
    var product = _context.Products.Find(id);

    if (product != null)
    {
        _context.Products.Remove(product);
        _context.SaveChanges();
    }

    return RedirectToAction("Admin");
}
[HttpPost]
public IActionResult DeleteOrder(int id)
{
    var order = _context.Orders.Find(id);

    if (order != null)
    {
        _context.Orders.Remove(order);
        _context.SaveChanges();
    }

    return RedirectToAction("Admin");
}

[HttpPost]
public IActionResult DeleteSubscription(int id)
{
    var subscription = _context.Subscriptions.Find(id);

    if (subscription != null)
    {
        _context.Subscriptions.Remove(subscription);
        _context.SaveChanges();
    }

    return RedirectToAction("Admin");
}

[HttpPost]
public IActionResult DeleteContactMessage(int id)
{
    var message = _context.ContactMessages.Find(id);

    if (message != null)
    {
        _context.ContactMessages.Remove(message);
        _context.SaveChanges();
    }

    return RedirectToAction("Admin");
}
public IActionResult EditProduct(int id)
{
    var product = _context.Products.Find(id);

    if (product == null)
    {
        return RedirectToAction("Admin");
    }

    return View(product);
}

[HttpPost]
public IActionResult EditProduct(Product product)
{
    _context.Products.Update(product);
    _context.SaveChanges();

    return RedirectToAction("Admin");
}

public IActionResult ProductDetails(int id)
{
    var product = _context.Products.Find(id);

    if (product == null)
    {
        return RedirectToAction("Products");
    }

    return View(product);
}
public IActionResult AddToCart(int id)
{
    var product = _context.Products.Find(id);

    if (product == null)
    {
        return RedirectToAction("Products");
    }

    var cartJson = HttpContext.Session.GetString("Cart");
var cart = string.IsNullOrEmpty(cartJson)
    ? new List<CartItem>()
    : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

    var existingItem = cart.FirstOrDefault(x => x.ProductId == id);

    if (existingItem != null)
    {
        existingItem.Quantity++;
    }
    else
    {
        cart.Add(new CartItem
{
    ProductId = product.Id,
    ProductName = product.Name,
    ImageUrl = product.ImageUrl,
    Price = product.Price,
    Quantity = 1
});
    }

    HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

    return RedirectToAction("Cart");
}
public IActionResult IncreaseQuantity(int id)
{
    var cartJson = HttpContext.Session.GetString("Cart");

    var cart = string.IsNullOrEmpty(cartJson)
        ? new List<CartItem>()
        : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

    var item = cart.FirstOrDefault(x => x.ProductId == id);

    if (item != null)
    {
        item.Quantity++;
    }

    HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

    return RedirectToAction("Cart");
}

public IActionResult DecreaseQuantity(int id)
{
    var cartJson = HttpContext.Session.GetString("Cart");

    var cart = string.IsNullOrEmpty(cartJson)
        ? new List<CartItem>()
        : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

    var item = cart.FirstOrDefault(x => x.ProductId == id);

    if (item != null)
    {
        item.Quantity--;

        if (item.Quantity <= 0)
        {
            cart.Remove(item);
        }
    }

    HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

    return RedirectToAction("Cart");
}
public IActionResult Cart()
{
    var cartJson = HttpContext.Session.GetString("Cart");

    var cart = string.IsNullOrEmpty(cartJson)
        ? new List<CartItem>()
        : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

    return View(cart);
}
public IActionResult RemoveFromCart(int id)
{
    var cartJson = HttpContext.Session.GetString("Cart");
    var cart = string.IsNullOrEmpty(cartJson)
        ? new List<CartItem>()
        : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

    var item = cart.FirstOrDefault(x => x.ProductId == id);

    if (item != null)
    {
        cart.Remove(item);
    }

    HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));

    return RedirectToAction("Cart");
}

public IActionResult Checkout()
{
    var cartJson = HttpContext.Session.GetString("Cart");
    var cart = string.IsNullOrEmpty(cartJson)
        ? new List<CartItem>()
        : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

    var totalAmount = cart.Sum(item => item.Total);

    ViewBag.TotalAmount = totalAmount;

    return View();
}
[HttpPost]
public IActionResult CreateOrder(string customerName, string email)
{
    var cartJson = HttpContext.Session.GetString("Cart");
    var cart = string.IsNullOrEmpty(cartJson)
        ? new List<CartItem>()
        : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();

    var totalAmount = cart.Sum(item => item.Total);

    var order = new Order
    {
        CustomerName = customerName,
        Email = email,
        TotalAmount = totalAmount,
        Status = "Paid"
    };

    _context.Orders.Add(order);
    _context.SaveChanges();

    HttpContext.Session.Remove("Cart");

    return RedirectToAction("OrderConfirmation");
}
public IActionResult OrderConfirmation()
{
    return View();
}
public IActionResult Login()
{
    return View();
}

public IActionResult Register()
{
    return View();
}

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
