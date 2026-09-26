using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VttLesson11.Models;

public partial class VttEmployee2410900069Context : DbContext
{
    public VttEmployee2410900069Context()
    {
    }

    public VttEmployee2410900069Context(DbContextOptions<VttEmployee2410900069Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Vtt2410900069> Vtt2410900069s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS02;Database=VttEmployee_2410900069;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vtt2410900069>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Vtt24109__3214EC073F0195F8");

            entity.ToTable("Vtt2410900069");

            entity.Property(e => e.VttActive).HasDefaultValue(true);
            entity.Property(e => e.VttEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.VttGender).HasMaxLength(10);
            entity.Property(e => e.VttName).HasMaxLength(50);
            entity.Property(e => e.VttPhone)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
