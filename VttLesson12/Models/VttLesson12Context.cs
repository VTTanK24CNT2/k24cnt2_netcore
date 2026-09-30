using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VttLesson12.Models;

public partial class VttLesson12Context : DbContext
{
    public VttLesson12Context()
    {
    }

    public VttLesson12Context(DbContextOptions<VttLesson12Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Banner> Banners { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS02;Database=VttLesson12;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.VttId).HasName("PK__Product__29F81078E48EAF85");

            entity.ToTable("Product");

            entity.Property(e => e.VttId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.VttCategoryId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.VttCreateDate).HasColumnType("datetime");
            entity.Property(e => e.VttDescription).HasMaxLength(500);
            entity.Property(e => e.VttImages).HasMaxLength(255);
            entity.Property(e => e.VttName).HasMaxLength(100);
            entity.Property(e => e.VttPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VttSalePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.VttStatus).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
