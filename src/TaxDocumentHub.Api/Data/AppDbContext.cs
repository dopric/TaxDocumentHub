using Microsoft.EntityFrameworkCore;
using TaxDocumentHub.Api.Domain;

namespace TaxDocumentHub.Api.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<DocumentCategory> Categories => Set<DocumentCategory>();
        public DbSet<TaxDocument> Documents => Set<TaxDocument>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure relationships and constraints if needed
            modelBuilder.Entity<DocumentCategory>()
                .HasOne(c => c.User)
                .WithMany(c => c.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaxDocument>()
                .HasOne(d => d.User)
                .WithMany(d=> d.Documents)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaxDocument>()
                .HasOne(d => d.Category)
                .WithMany(c => c.Documents)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Kategorie darf nicht gelöscht werden, wenn Dokumente existieren
        }
    }
}
