using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhuongXaController : ControllerBase
    {

        // gen context
        private readonly QuanTriHeThongContext _context;

        // constructor for context
        public PhuongXaController(QuanTriHeThongContext context)
        {
            _context = context;
        }

        // Lấy tất cả phường xã
        [HttpPost("search")]
        public async Task<ActionResult<Response<PhuongXa>>> GetAllTinhTP()
        {
            var response = new Response<PhuongXa>();

            try
            {
                var list = await _context.PhuongXa.ToListAsync();
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

        // Lấy phường xa theo quận huyện
        [HttpPost("getByMaQuanHuyen")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<PhuongXa>>> GetTinhTPByMaTinhTP([FromForm] IFormCollection form)
        {

            var response = new Response<PhuongXa>();

            Microsoft.Extensions.Primitives.StringValues maQuanHuyenValues;

            string maQuanHuyen = "";

            form.TryGetValue("MaQuanHuyen", out maQuanHuyenValues);
            if (maQuanHuyenValues.Count > 0)
            {
                maQuanHuyen = maQuanHuyenValues[0] ?? "";
            }

            if (string.IsNullOrEmpty(maQuanHuyen))
            {
                response.Success = true;
                return response;
            }

            try
            {
                var list = await _context.PhuongXa.FromSqlRaw("SELECT * FROM PhuongXa WHERE MaQuanHuyen = {0}", maQuanHuyen).ToListAsync();
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
