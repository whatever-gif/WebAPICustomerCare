using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class QuaTrinhXuLyEticket
{
    public int MaXuLy { get; set; }
    public string MaTicket { get; set; } = null!;
    public string NoiDungXuLy { get; set; } = null!;
    public string NguoiXuLy { get; set; } = null!;
    public DateTime ThoiGianXuLy { get; set; }
    public string TenNguoiXuLy { get; set; } = null!;
    public string TrangThai { get; set; } = null!;

}



