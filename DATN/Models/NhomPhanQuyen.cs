using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class NhomPhanQuyen
{
    public string MaNhomPhanQuyen { get; set; } = null!;

    public string TenNhomPhanQuyen { get; set; } = null!;

    public string MoTa { get; set; } = null!;

    public string TrangThai { get; set; } = null!;

}

public class NhomPhanQuyenDTO
{
    public string MaNhomPhanQuyen { get; set; }
    public string TenNhomPhanQuyen { get; set; }
    public string MoTa { get; set; }
    public string TrangThai { get; set; }
}

