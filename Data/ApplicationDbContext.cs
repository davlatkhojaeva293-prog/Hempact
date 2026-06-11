using Microsoft.EntityFrameworkCore;
using Hempact.Models;

namespace Hempact.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<Subscription> Subscriptions { get; set; }

    public DbSet<ContactMessage> ContactMessages { get; set; }

    public DbSet<Order> Orders { get; set; }

    public DbSet<User> Users { get; set; }
}