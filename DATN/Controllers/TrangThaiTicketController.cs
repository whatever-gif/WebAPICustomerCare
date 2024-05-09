using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrangThaiTicketController : ControllerBase
    {

        // gen context
        private readonly QuanTriHeThongContext _context;

        // constructor for context
        public TrangThaiTicketController(QuanTriHeThongContext context)
        {
            _context = context;
        }

        // Lấy tất cả trạng thái ticket
        [HttpPost("search")]
        public async Task<ActionResult<Response<TrangThaiTicket>>> GetTicketStatus()
        {
            var response = new Response<TrangThaiTicket>();

            try
            {
                var list = await _context.TrangThaiTicket.ToListAsync();
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
