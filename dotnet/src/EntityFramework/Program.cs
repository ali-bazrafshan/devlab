using Microsoft.EntityFrameworkCore;

namespace Sandbox.EntityFramwork
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public virtual Shop Shop { get; set; } = null!;
    }

    public class AppDbContext : DbContext
    {
        public DbSet<Shop> Shop => Set<Shop>();
        public DbSet<Product> Product => Set<Product>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=db/shop.db");
    }

    class Program
    {
        static void Main()
        {
            using var db = new AppDbContext();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Insert
            var shop = new Shop { Name = "Test Shop" };
            db.Shop.Add(shop);
            db.Product.Add(new Product
            {
                Name = "Test Product",
                Shop = shop
            });
            db.SaveChanges();

            // Query
            var products = db.Product.AsNoTracking().ToList();
            foreach (var p in products)
                //Console.WriteLine($"{p.Id}: {p.Name} is in {p.Shop.Name}");
                Console.WriteLine($"{p.Id}: {p.Name}");
        }
    }
}