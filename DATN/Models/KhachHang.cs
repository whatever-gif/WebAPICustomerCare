using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class KhachHang
{
        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }
        public DateOnly? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string? MaQuocGia { get; set; }
        public string? MaTinhTP { get; set; }
        public string? MaQuanHuyen { get; set; }
        public string? MaPhuongXa { get; set; }
        public string? DiaChi { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? SoGTTT { get; set; }
        public string? MaGTTT { get; set; }
        public string? NgheNghiep { get; set; }
        public string? MaNguonKhach { get; set; }
        public string? AnhDaiDien { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; }
        public string TrangThai { get; set; }
        public string? TrangThaiXoa { get; set; }
        public string? TenQuocGia { get; set; } = null;
        public string? TenTinhTP { get; set; } = null;
        public string? TenQuanHuyen { get; set; } = null;
        public string? TenPhuongXa { get; set; } = null;
        public string? TenGTTT { get; set; } = null;
        public string? TenNguonKhach { get; set; } = null;
        public string? TenNguoiDung { get; set; } = null;
}
public partial class KhachHangSearch
{
    public string MaKhachHang { get; set; }
    public string TenKhachHang { get; set; }
    public string DienThoai { get; set; }
    public string Email { get; set; }
    public string? ThoiGianTaoTu { get; set; }
    public string? ThoiGianTaoDen { get; set; }
    public string NguoiTao { get; set; }
}

public partial class ThongTinKhachHang
{
    // Khởi tạo đối tượng KhachHang
    public KhachHang KhachHang { get; set; } = new KhachHang();

    public List<Eticket> Ticket { get; set; } = new List<Eticket>();
}