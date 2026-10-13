using Microsoft.EntityFrameworkCore;
using GlowBeauty_Shop.Models;

namespace GlowBeauty_Shop.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}