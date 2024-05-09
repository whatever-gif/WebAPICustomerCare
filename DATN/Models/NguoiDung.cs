using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class NguoiDung
{
    public string Email { get; set; } = null!;

    public string TenNguoiDung { get; set; } = null!;

    public string? Avatar { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string? MatKhau { get; set; } = null!;

    public string FlagSysAdmin { get; set; }

    public string TrangThaiHoatDong { get; set; } = null!;

    public string TrangThaiXoa { get; set; } = null!;



}



