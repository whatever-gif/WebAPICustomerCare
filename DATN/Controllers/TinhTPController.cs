using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TinhTPController : ControllerBase
    {
        private readonly QuanTriHeThongContext _context;

        public TinhTPController(QuanTriHeThongContext context)
        {
            _context = context;
        }

        // Lấy tất cả quốc gia
        [HttpPost("search")]
        public async Task<ActionResult<Response<TinhTp>>> GetAllTinhTP()
        {
            var response = new Response<TinhTp>();

            try
            {
                var list = await _context.TinhTp.ToListAsync();
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


        // Lấy tỉnh tp theo mã quốc gia
        [HttpPost("getByMaQuocGia")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<TinhTp>>> GetTinhTPByMaQuocGia([FromForm] IFormCollection form)
        {

            var response = new Response<TinhTp>();

            Microsoft.Extensions.Primitives.StringValues maQuocGiaValues;

            string maQuocGia = "";

            form.TryGetValue("MaQuocGia", out maQuocGiaValues);
            if (maQuocGiaValues.Count > 0)
            {
                maQuocGia = maQuocGiaValues[0] ?? "";
            }

            if (string.IsNullOrEmpty(maQuocGia))
            {
                response.Success = true;
                return response;
            }

            try
            {
                var list = await _context.TinhTp.FromSqlRaw("SELECT * FROM TinhTp WHERE MaQuocGia = {0}", maQuocGia).ToListAsync();
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
