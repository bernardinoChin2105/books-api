using BookService.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookService.Data
{
    public class BooksDbContext : DbContext
    {
        public BooksDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Rates)
                .WithOne(r => r.Book);
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Rate> Rates { get; set; }
    }
}
