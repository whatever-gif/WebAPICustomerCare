using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class PhanLoaiTicket
{
    public string MaPhanLoaiTicket { get; set; } = null!;

    public string TenPhanLoaiTicket { get; set; } = null!;

}

public class PhanLoaiTicketDTO
{
    public string MaPhanLoaiTicket { get; set; }

    public string TenPhanLoaiTicket { get; set; }
}

