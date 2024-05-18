using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DATN.Models;

public partial class QuanTriHeThongContext : DbContext
{
    public QuanTriHeThongContext()
    {
    }

    public QuanTriHeThongContext(DbContextOptions<QuanTriHeThongContext> options)
        : base(options)
    {
    }

    public virtual DbSet<QuanHuyen> QuanHuyen { get; set; }
    public virtual DbSet<QuocGia> QuocGia { get; set; }
    public virtual DbSet<TinhTp> TinhTp { get; set; }
    public virtual DbSet<PhuongXa> PhuongXa { get; set; }
    public virtual DbSet<GTTT> GTTT { get; set; }
    public virtual DbSet<NguonKhach> NguonKhach { get; set; }
    public virtual DbSet<TrangThaiTicket> TrangThaiTicket { get; set; }
    public virtual DbSet<PhanLoaiTicket> PhanLoaiTicket { get; set; }
    public virtual DbSet<NguoiDung> NguoiDung { get; set; }
    public virtual DbSet<NhomPhanQuyen> NhomPhanQuyen { get; set; }
    public virtual DbSet<Eticket> Eticket { get; set; }
    public virtual DbSet<KhachHang> KhachHang { get; set; }
    public virtual DbSet<QuaTrinhXuLyEticket> QuaTrinhXuLyEticket { get; set; }
    public virtual DbSet<BaoCaoKPIEticket> BaoCaoKPIEticket { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(local)\\SQLSV2019EXP;Database=QuanTriHeThong;Trusted_Connection=True;TrustServerCertificate=True;User ID=sa;pwd=SqlSv2019");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuanHuyen>(entity =>
        {
            entity.HasKey(e => e.MaQuanHuyen).HasName("PK__QuanHuye__B86B827A847270EE");

            entity.ToTable("QuanHuyen");

            entity.Property(e => e.MaQuanHuyen)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.MaTinhTp)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MaTinhTP");
            entity.Property(e => e.TenQuanHuyen).HasMaxLength(50);

        });

        modelBuilder.Entity<QuocGia>(entity =>
        {
            entity.HasKey(e => e.MaQuocGia).HasName("PK__QuocGia__30D69ACB9DCA7BEA");

            entity.Property(e => e.MaQuocGia)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TenQuocGia).HasMaxLength(50);
        });

        modelBuilder.Entity<TinhTp>(entity =>
        {
            entity.HasKey(e => e.MaTinhTp).HasName("PK__TinhTP__8BF4DB21E7A8117E");

            entity.ToTable("TinhTP");

            entity.Property(e => e.MaTinhTp)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MaTinhTP");
            entity.Property(e => e.MaQuocGia)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TenTinhTp)
                .HasMaxLength(50)
                .HasColumnName("TenTinhTP");
        });

        modelBuilder.Entity<PhuongXa>(entity =>
        {
            entity.HasKey(e => e.MaPhuongXa).HasName("PK__PhuongXa__55F1EFA9BB304E58");

            entity.ToTable("PhuongXa");

            entity.Property(e => e.MaPhuongXa)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MaPhuongXa");
            entity.Property(e => e.MaQuanHuyen)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TenPhuongXa).HasMaxLength(50).HasColumnName("TenPhuongXa"); ;

        });

        modelBuilder.Entity<GTTT>(entity =>
        {
            entity.HasKey(e => e.MaGTTT).HasName("PK__GTTT");

            entity.ToTable("GTTT");

            entity.Property(e => e.MaGTTT)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaGTTT");
            entity.Property(e => e.TenGTTT)
                .HasMaxLength(20)
                .IsUnicode(true);
        });

        modelBuilder.Entity<NguonKhach>(entity =>
        {
            entity.HasKey(e => e.MaNguonKhach).HasName("PK__NguonKhach");

            entity.ToTable("NguonKhach");

            entity.Property(e => e.MaNguonKhach)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaNguonKhach");
            entity.Property(e => e.TenNguonKhach)
                .HasMaxLength(30)
                .IsUnicode(true);
        });

        modelBuilder.Entity<TrangThaiTicket>(entity =>
        {
            entity.HasKey(e => e.MaTrangThaiTicket).HasName("PK_TrangThaiTicket");

            entity.ToTable("TrangThaiTicket ");

            entity.Property(e => e.MaTrangThaiTicket)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaTrangThaiTicket");
            entity.Property(e => e.TenTrangThaiTicket)
                .HasMaxLength(30)
                .IsUnicode(true);
        });

        modelBuilder.Entity<PhanLoaiTicket>(entity =>
        {
            entity.HasKey(e => e.MaPhanLoaiTicket).HasName("PK_PhanLoaiTicket");

            entity.ToTable("PhanLoaiTicket");

            entity.Property(e => e.MaPhanLoaiTicket)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaPhanLoaiTicket");
            entity.Property(e => e.TenPhanLoaiTicket)
                .HasMaxLength(50)
                .IsUnicode(true);
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("PK_NguoiDung");

            entity.ToTable("NguoiDung");

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Email");
            entity.Property(e => e.TenNguoiDung)
                .HasMaxLength(30)
                .IsUnicode(true);
            entity.Property(e => e.Avatar)
                .HasColumnType("text");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasComputedColumnSql("isnumeric([SoDienThoai])"); // accept 0 -> 9
            entity.Property(e => e.MatKhau)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.FlagSysAdmin)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TrangThaiHoatDong)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.TrangThaiXoa)
                .HasMaxLength(1)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NhomPhanQuyen>(entity =>
        {
            entity.HasKey(e => e.MaNhomPhanQuyen).HasName("PK_NhomPhanQuyen");

            entity.ToTable("NhomPhanQuyen");

            entity.Property(e => e.MaNhomPhanQuyen)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MaNhomPhanQuyen");
            entity.Property(e => e.TenNhomPhanQuyen)
                .HasMaxLength(50)
                .IsUnicode(true);
            entity.Property(e => e.MoTa)
                .HasColumnType("text");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasComputedColumnSql("isnumeric([TrangThai])");
        });

        // build ETicket model
        modelBuilder.Entity<Eticket>(entity =>
        {
            entity.HasKey(e => e.MaTicket).HasName("PK_Eticket");

            entity.ToTable("Eticket");            

            entity.Property(e => e.MaTicket)
                 .HasMaxLength(10)
                 .IsUnicode(false)
                 .HasColumnName("MaTicket");
            entity.Property(e => e.MaTrangThaiTicket)
                 .HasMaxLength(10)
                 .IsUnicode(false)
                 .HasColumnName("MaTrangThaiTicket");
            entity.Property(e => e.TenTicket)
                .HasMaxLength(250)
                .IsUnicode(true)
                .HasColumnName("TenTicket");
            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaKhachHang");
            entity.Property(e => e.ChiTiet)
                .HasColumnName("ChiTiet");
            entity.Property(e => e.MaPhanLoaiTicket)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaPhanLoaiTicket");
            entity.Property(e => e.TicketDeadline)
                .HasColumnName("TicketDeadline");
            entity.Property(e => e.ThoiDiemTiepNhan)
                .HasColumnName("ThoiDiemTiepNhan");
            entity.Property(e => e.NguoiTao)
                .HasColumnName("NguoiTao");
            entity.Property(e => e.ThoiGianTao)
                .HasColumnName("ThoiGianTao");
            entity.Property(e => e.ThoiGianTao)
                .HasColumnName("ThoiGianTao");
        });

        // build KhachHang model
        modelBuilder.Entity<KhachHang>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK_KhachHang");

            entity.ToTable("KhachHang");

            entity.Property(e => e.MaKhachHang)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaKhachHang");
            entity.Property(e => e.TenKhachHang)
                .HasMaxLength(30)
                .IsUnicode(true)
                .HasColumnName("TenKhachHang");
            entity.Property(e => e.NgaySinh)
                .HasColumnName("NgaySinh");
            entity.Property(e => e.GioiTinh)
                .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("GioiTinh");
            entity.Property(e => e.MaQuocGia)
                .HasColumnName("MaQuocGia");
            entity.Property(e => e.MaTinhTP)
                .HasColumnName("MaTinhTP");
            entity.Property(e => e.MaQuanHuyen)
                .HasColumnName("MaQuanHuyen");
            entity.Property(e => e.MaPhuongXa)
                .HasColumnName("MaPhuongXa");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(250)
                .IsUnicode(true)
                .HasColumnName("DiaChi");
            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SoDienThoai");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Email");
            entity.Property(e => e.SoGTTT)
                .IsUnicode(false)
                .HasColumnName("SoGTTT");
            entity.Property(e => e.MaGTTT)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaGTTT");
            entity.Property(e => e.NgheNghiep)
                .HasMaxLength(30)
                .IsUnicode(true)
                .HasColumnName("NgheNghiep");
            entity.Property(e => e.MaNguonKhach)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.AnhDaiDien)
                .HasColumnName("AnhDaiDien");
            entity.Property(e => e.NgayTao)
            .HasColumnName("NgayTao");
            entity.Property(e => e.NguoiTao)
                .HasColumnName("NguoiTao");
            entity.Property(e => e.TrangThai)
            .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("TrangThai");
            entity.Property(e => e.TrangThaiXoa)
            .HasMaxLength(1)
                .IsUnicode(false)
                .HasColumnName("TrangThaiXoa");
        });

        // build QuaTrinhXuLyEticket model
        modelBuilder.Entity<QuaTrinhXuLyEticket>(entity =>
        {
            entity.HasKey(e => e.MaXuLy).HasName("PK_MaXuLy");

            entity.ToTable("QuaTrinhXuLyEticket");

            entity.Property(e => e.MaXuLy)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaXuLy");
            entity.Property(e => e.MaTicket)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("MaTicket");
            entity.Property(e => e.NoiDungXuLy)
                .HasColumnName("NoiDungXuLy");
            entity.Property(e => e.NguoiXuLy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NguoiXuLy");
            entity.Property(e => e.ThoiGianXuLy)
                .HasColumnName("ThoiGianXuLy");
            entity.Property(e => e.TenNguoiXuLy)
                .HasColumnName("TenNguoiXuLy");
        });

        modelBuilder.Entity<BaoCaoKPIEticket>(entity =>
        {
            entity.HasNoKey();
        });




        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
