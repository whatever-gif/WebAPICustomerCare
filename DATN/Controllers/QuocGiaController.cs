using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuocGiaController : ControllerBase
    {

        private readonly QuanTriHeThongContext _context;

        public QuocGiaController(QuanTriHeThongContext context)
        {
            _context = context;
        }


        // Lấy tất cả quốc gia
        [HttpPost("search")]
        public async Task<ActionResult<Response<QuocGia>>> GetAllQuocGia()
        {
            var response = new Response<QuocGia>();

            try
            {
                var list = await _context.QuocGia.ToListAsync();
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
