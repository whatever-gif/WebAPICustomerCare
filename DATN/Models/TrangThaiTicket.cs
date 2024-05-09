using System;
using System.Collections.Generic;

namespace DATN.Models;

public partial class TrangThaiTicket
{
    public string MaTrangThaiTicket { get; set; } = null!;

    public string TenTrangThaiTicket { get; set; } = null!;

}

public class TrangThaiTicketDTO
{
    public string MaTrangThaiTicket { get; set; }

    public string TenTrangThaiTicket { get; set; }
}

