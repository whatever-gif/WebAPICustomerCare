using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GTTTController : ControllerBase
    {

        // gen context
        private readonly QuanTriHeThongContext _context;

        // constructor for context
        public GTTTController(QuanTriHeThongContext context)
        {
            _context = context;
        }

        // Lấy tất cả GTTT
        [HttpPost("search")]
        public async Task<ActionResult<Response<GTTT>>> GetAllGTTT()
        {
            var response = new Response<GTTT>();

            try
            {
                var list = await _context.GTTT.ToListAsync();
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
