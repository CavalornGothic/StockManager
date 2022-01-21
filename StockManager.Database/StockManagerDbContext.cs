using Microsoft.EntityFrameworkCore;

namespace StockManager.Database
{
    public class StockManagerDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<RowTypesDict> RowTypesDict { get; set; }
        public DbSet<Inventory> Inventories { get; set; }

        // tabele wiele do wielu
        public DbSet<RowCustomer> RowCustomers { get; set; }
        public DbSet<RowProduct> RowProducts { get; set; }

        public StockManagerDbContext(DbContextOptions options) : base (options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Row>()

            // wypełnienie słownika typów rzędów
            modelBuilder.Entity<RowTypesDict>().HasData(new RowTypesDict { Id = 1, Name = "K" }, new RowTypesDict { Id = 2, Name = "P" });
            // wypełnienie tablicy kontrahentów
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Korex", City = "Smolarz" }, 
                new Customer { Id = 2, Name = "Pol-kor", City = "Szczecinek"},
                new Customer { Id = 3, Name = "Drewex"});
            // wypełnienie tablicy rzędami
            modelBuilder.Entity<Row>().HasData(
                new Row { Id = 1, Height = 10.0, Length = 10.0, Width = 10.0, Number = 1, RowTypesDictFK = 1, Status = 0, Description = "Przykładowy rząd"},
                new Row { Id = 2, Height = 10.0, Length = 10.0, Width = 10.0, Number = 2, RowTypesDictFK = 1, Status = 0, Description = "Przykładowy rząd" },
                new Row { Id = 3, Height = 10.0, Length = 10.0, Width = 10.0, Number = 3, RowTypesDictFK = 1, Status = 0, Description = "Przykładowy rząd" }
            );
            // wypełnienie tablicy produktów
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "surowiec kory 1"},
                new Product { Id = 2, Name = "surowiec kory 2"},
                new Product { Id = 3, Name = "surowiec kory 3"},
                new Product { Id = 4, Name = "ziemia torfowa"},
                new Product { Id = 5, Name = "podłoże iglaste"}
                );

        }
    }
}
