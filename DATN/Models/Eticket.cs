using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class Eticket
{
    public string MaTicket { get;set; } = null!;
    public string MaTrangThaiTicket { get;set; } = null!;
    public string TenTicket { get;set; } = null!;
    public string MaKhachHang { get;set; } = null!;
    public string ChiTiet { get;set; } = null!;
    public string MaPhanLoaiTicket { get;set; } = null!;
    public DateTime TicketDeadline { get; set; }
    public DateTime? ThoiDiemTiepNhan { get;set; }
    public string? NguoiTao { get;set; } = null!;
    public DateTime? ThoiGianTao { get;set; }
    public string? NguoiCapNhat { get;set; } = null!;
    public DateTime? ThoiGianCapNhat { get; set; }
    public string? TrangThai { get; set; }
    public string? TenTrangThaiTicket { get; set; }
    public string? TenPhanLoaiTicket { get; set; }
    public string? TenKhachHang { get; set; } = null;
    public string? SoDienThoai { get; set; } = null;
    public string? TenNguoiTao { get; set; } = null;
    public string? TenNguoiCapNhat { get; set; } = null;
    public string? ThoiGianXuLy { get; set; }
    public int? FlagQuaHanXuLy { get; set; }
}

public partial class EticketDTO : Eticket
{
    public string TicketDeadline { get; set; }
    public string ThoiDiemTiepNhan { get; set; }
    public string ThoiGianTao { get; set; }
    public string ThoiGianCapNhat { get; set; }

}

public partial class EticketSearch
{
    public string MaTicket { get; set; } 
    public string TenTicket { get; set; }
    public string MaKhachHang { get; set; }
    public string MaPhanLoaiTicket { get; set; }
    public string MaTrangThaiTicket { get; set; }
    public string FlagQuaHanXuLy { get; set; }
    public string? ThoiGianTaoTu { get; set; }
    public string? ThoiGianTaoDen { get; set; }
    public string NguoiTao { get; set; }
}

public partial class EticketUpdate
{
    public string MaTicket { get; set; }
    public string NguoiCapNhat { get; set; }
}

public partial class ThongTinEticket
{
    // Khởi tạo đối tượng Eticket
    public Eticket Eticket { get; set; } = new Eticket();

    public KhachHang KhachHang { get; set; } = new KhachHang();

    public List<QuaTrinhXuLyEticket> ListQuaTrinhXuLy { get; set; } = new List<QuaTrinhXuLyEticket>();
}


