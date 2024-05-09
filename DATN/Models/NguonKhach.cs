using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class NguonKhach
{
    public string MaNguonKhach { get; set; } = null!;

    public string TenNguonKhach { get; set; } = null!;

}

public class NguonKhachDTO
{
    public string MaNguonKhach { get; set; }

    public string TenNguonKhach { get; set; }
}


