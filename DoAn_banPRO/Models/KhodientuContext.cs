using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DoAn_banPRO.Models;

namespace DoAn_banPRO.Models
{
    public class KhodientuContext : DbContext
    {
        public DbSet<LoaiHang> LoaiHangs { get; set; }
        public DbSet<LinhKien> LinhKiens { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<PhieuNhap> PhieuNhaps { get; set; }
        public DbSet<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
        public DbSet<PhieuXuat> PhieuXuats { get; set; }
        public DbSet<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=QL_KHODIENTU;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoaiHang>(entity =>
            {
                entity.HasKey(e => e.MaLoai);
                entity.ToTable("LoaiHang");
                entity.Property(e => e.MaLoai).HasMaxLength(6).IsUnicode(false).IsFixedLength();
                entity.Property(e => e.TenLoai).HasMaxLength(100);
            });

            modelBuilder.Entity<NhaCungCap>(entity =>
            {
                entity.HasKey(e => e.MaNCC);
                entity.ToTable("NhaCungCap");
                entity.Property(e => e.MaNCC).HasMaxLength(6).IsUnicode(false).IsFixedLength();
                entity.Property(e => e.TenNCC).HasMaxLength(100);
            });

            modelBuilder.Entity<LinhKien>(entity =>
            {
                entity.HasKey(e => e.MaLK);
                entity.ToTable("LinhKien");
                entity.Property(e => e.MaLK).HasMaxLength(6).IsUnicode(false).IsFixedLength();
                entity.Property(e => e.TenLK).HasMaxLength(100);
                entity.Property(e => e.TonKho).HasColumnName("TONKHO");
                
                entity.Ignore(e => e.TenLoai);
                entity.Ignore(e => e.TenNCC);
            });

            modelBuilder.Entity<NguoiDung>(entity =>
            {
                entity.HasKey(e => e.MaND);
                entity.ToTable("NguoiDung");
                entity.Property(e => e.MaND).HasMaxLength(6).IsUnicode(false).IsFixedLength();
                entity.Property(e => e.TaiKhoan).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.MatKhau).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.HoTen).HasMaxLength(100);
                entity.Property(e => e.Quyen).HasMaxLength(50);
            });

            modelBuilder.Entity<PhieuNhap>(entity =>
            {
                entity.HasKey(e => e.MaPhieu);
                entity.ToTable("PhieuNhap");
                entity.Property(e => e.MaPhieu).HasMaxLength(6).IsUnicode(false).IsFixedLength();
                entity.Property(e => e.MaND).HasMaxLength(6).IsUnicode(false).IsFixedLength();
                entity.Property(e => e.NgayLap).HasColumnType("datetime");
                entity.Property(e => e.TongTien).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<ChiTietPhieuNhap>(entity =>
            {
                entity.HasKey(e => e.MaCTPN);
                entity.ToTable("ChiTietPhieuNhap");
                entity.Property(e => e.MaPN).HasMaxLength(6).IsUnicode(false).IsFixedLength();
                entity.Property(e => e.MaLK).HasMaxLength(6).IsUnicode(false).IsFixedLength();
                entity.Property(e => e.DonGiaNhap).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<PhieuXuat>(entity =>
            {
                entity.HasKey(e => e.MaPhieu);
                entity.ToTable("PhieuXuat");
                entity.Property(e => e.MaPhieu).HasMaxLength(6).IsUnicode(false).IsFixedLength();
                entity.Property(e => e.MaND).HasMaxLength(6).IsUnicode(false).IsFixedLength();
                entity.Property(e => e.NgayLap).HasColumnType("datetime");
                entity.Property(e => e.TongTien).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<ChiTietPhieuXuat>(entity =>
            {
                entity.HasKey(e => e.MaCTPX);
                entity.ToTable("ChiTietPhieuXuat");
                entity.Property(e => e.MaPX).HasMaxLength(6).IsUnicode(false).IsFixedLength();
                entity.Property(e => e.MaLK).HasMaxLength(6).IsUnicode(false).IsFixedLength();
                entity.Property(e => e.DonGiaXuat).HasColumnType("decimal(18,2)");
            });
        }
    }
}