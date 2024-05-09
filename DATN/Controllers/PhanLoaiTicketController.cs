using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhanLoaiTicketController : ControllerBase
    {

        // gen context
        private readonly QuanTriHeThongContext _context;

        // constructor for context
        public PhanLoaiTicketController(QuanTriHeThongContext context)
        {
            _context = context;
        }

        // Lấy tất cả phân loại Ticket
        [HttpPost("search")]
        public async Task<ActionResult<Response<PhanLoaiTicket>>> GetAllPhanLoaiTicket()
        {
            var response = new Response<PhanLoaiTicket>();

            try
            {
                var list = await _context.PhanLoaiTicket.ToListAsync();
                response.DataList = list;
                response.Success = true;
                response.Count = list.Count;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }

            return response;
        }
    }
}
