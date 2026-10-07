using Microsoft.EntityFrameworkCore;

namespace VttLesson14CoreA7.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<BannerBlog> BannerBlogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình bảng Category: Name không trùng (Unique)
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(e => e.Name).IsUnique();
                entity.Property(e => e.Status).HasDefaultValue((byte)1);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
            });

            // Cấu hình bảng Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(d => d.Category)
                      .WithMany(p => p.Products)
                      .HasForeignKey(d => d.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Cấu hình bảng BannerBlog
            modelBuilder.Entity<BannerBlog>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
            });
        }
    }
}
