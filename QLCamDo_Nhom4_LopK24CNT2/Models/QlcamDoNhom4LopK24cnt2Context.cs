using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLCamDo_Nhom4_LopK24CNT2.Models;

public partial class QlcamDoNhom4LopK24cnt2Context : DbContext
{
    public QlcamDoNhom4LopK24cnt2Context()
    {
    }

    public QlcamDoNhom4LopK24cnt2Context(DbContextOptions<QlcamDoNhom4LopK24cnt2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<GiaoDich> GiaoDiches { get; set; }

    public virtual DbSet<HopDongCam> HopDongCams { get; set; }

    public virtual DbSet<KhachHang> KhachHangs { get; set; }

    public virtual DbSet<LoaiTaiSan> LoaiTaiSans { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TaiSan> TaiSans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS02;Database=QLCamDo_Nhom4_LopK24CNT2;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GiaoDich>(entity =>
        {
            entity.HasKey(e => e.MaGd).HasName("PK__GiaoDich__2725AE811D356EDA");

            entity.ToTable("GiaoDich");

            entity.Property(e => e.MaGd).HasColumnName("MaGD");
            entity.Property(e => e.LoaiGd)
                .HasMaxLength(50)
                .HasColumnName("LoaiGD");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.NgayGd)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("NgayGD");
            entity.Property(e => e.NhanVienThu).HasMaxLength(100);
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.GiaoDiches)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GiaoDich_HopDong");
        });

        modelBuilder.Entity<HopDongCam>(entity =>
        {
            entity.HasKey(e => e.MaHd).HasName("PK__HopDongC__2725A6E09B6112A0");

            entity.ToTable("HopDongCam");

            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.LaiSuat).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.NgayCam).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TienCam).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(50)
                .HasDefaultValue("đang cầm");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.HopDongCams)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HopDong_KhachHang");
        });

        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKh).HasName("PK__KhachHan__2725CF1E7B8B02E9");

            entity.ToTable("KhachHang");

            entity.HasIndex(e => e.SoCccd, "UQ__KhachHan__8A547D3AED2A0777").IsUnique();

            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.AnhCccd)
                .HasMaxLength(500)
                .HasColumnName("AnhCCCD");
            entity.Property(e => e.DiaChi).HasMaxLength(255);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.SoCccd)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SoCCCD");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(15)
                .IsUnicode(false);
        });

        modelBuilder.Entity<LoaiTaiSan>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__LoaiTaiS__730A57597576C6B0");

            entity.ToTable("LoaiTaiSan");

            entity.Property(e => e.LaiSuatNgay).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.TenLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTk).HasName("PK__TaiKhoan__27250070F8D404DA");

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.TenDangNhap, "UQ__TaiKhoan__55F68FC08353FBB6").IsUnique();

            entity.Property(e => e.MaTk).HasColumnName("MaTK");
            entity.Property(e => e.MaKh).HasColumnName("MaKH");
            entity.Property(e => e.MatKhauHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VaiTro)
                .HasMaxLength(50)
                .HasDefaultValue("Nhân viên");

            entity.HasOne(d => d.MaKhNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaKh)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TaiKhoan_KhachHang");
        });

        modelBuilder.Entity<TaiSan>(entity =>
        {
            entity.HasKey(e => e.MaTs).HasName("PK__TaiSan__27250078786243E9");

            entity.ToTable("TaiSan");

            entity.Property(e => e.MaTs).HasColumnName("MaTS");
            entity.Property(e => e.AnhTaiSan).HasMaxLength(500);
            entity.Property(e => e.GiaTriDinhGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaHd).HasColumnName("MaHD");
            entity.Property(e => e.TenTaiSan).HasMaxLength(150);

            entity.HasOne(d => d.MaHdNavigation).WithMany(p => p.TaiSans)
                .HasForeignKey(d => d.MaHd)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaiSan_HopDong");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.TaiSans)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaiSan_LoaiTaiSan");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
