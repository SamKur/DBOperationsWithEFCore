using Microsoft.EntityFrameworkCore;

namespace DBOperationsWithEFCoreApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
            
        }

        //Create Operation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Currency>().HasData(
                //c => c.CurrencyCode
                new Currency() { Id = 1, CurrencyType = "INR", Description = "Indian Rupee" },
                new Currency() { Id = 2, CurrencyType = "USD", Description = "US Dollar" },
                new Currency() { Id = 3, CurrencyType = "RIN", Description = "Malayesian Ringit" },
                new Currency() { Id = 5, CurrencyType = "TAK", Description = "Bangladeshi Taka" }
                );

            modelBuilder.Entity<Language>().HasData(
                new Language() {Id=1 , Title="Hindi", Description= "Hindi" },
                new Language() {Id=2 , Title="English" , Description= "English" },
                new Language() {Id=3 , Title="Bengali" , Description= "Bengali" },
                new Language() {Id=4, Title="Tamil" , Description= "Tamil" }

                );
        }
        // in c# code table name would be `book` but actual table => Books (below mentioned)
        public DbSet<Book> Books { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<BookPrice> BookPrices { get; set; }
        public DbSet<Currency> Currencies { get; set; } 
    }
}
