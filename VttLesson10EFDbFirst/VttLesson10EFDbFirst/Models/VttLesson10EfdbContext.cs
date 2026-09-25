using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VttLesson10EFDbFirst.Models;

public partial class VttLesson10EfdbContext : DbContext
{
    public VttLesson10EfdbContext()
    {
    }

    public VttLesson10EfdbContext(DbContextOptions<VttLesson10EfdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<VttMember> VttMembers { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS02;Database=VttLesson10EFDb;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VttMember>(entity =>
        {
            entity.ToTable("VttMember");

            entity.Property(e => e.VttEmail)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VttFullName).HasMaxLength(50);
            entity.Property(e => e.VttPassword)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VttPhone)
                .HasMaxLength(12)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.VttUserName)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
