using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguonKhachController : ControllerBase
    {

        // gen context
        private readonly QuanTriHeThongContext _context;

        // constructor for context
        public NguonKhachController(QuanTriHeThongContext context)
        {
            _context = context;
        }

        // Lấy tất cả nguồn khách
        [HttpPost("search")]
        public async Task<ActionResult<Response<NguonKhach>>> GetAllNguonKhach()
        {
            var response = new Response<NguonKhach>();

            try
            {
                var list = await _context.NguonKhach.ToListAsync();
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
