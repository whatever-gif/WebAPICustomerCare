using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DATN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanHuyenController : ControllerBase
    {
        private readonly QuanTriHeThongContext _context;
        public QuanHuyenController(QuanTriHeThongContext context)
        {
            _context = context;
        }

        // Lấy tất cả quận huyện
        [HttpPost("search")]
        public async Task<ActionResult<Response<QuanHuyen>>> GetAllTinhTP()
        {
            var response = new Response<QuanHuyen>();

            try
            {
                var list = await _context.QuanHuyen.ToListAsync();
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


        // Lấy tỉnh tp theo mã tỉnh tp
        [HttpPost("getByMaTinhTP")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<QuanHuyen>>> GetTinhTPByMaTinhTP([FromForm] IFormCollection form)
        {

            var response = new Response<QuanHuyen>();

            Microsoft.Extensions.Primitives.StringValues maTinhTPValues;

            string maTinhTP = "";

            form.TryGetValue("MaTinhTP", out maTinhTPValues);
            if (maTinhTPValues.Count > 0)
            {
                maTinhTP = maTinhTPValues[0] ?? "";
            }

            if (string.IsNullOrEmpty(maTinhTP))
            {
                response.Success = true;
                return response;
            }

            try
            {
                var list = await _context.QuanHuyen.FromSqlRaw("SELECT * FROM QuanHuyen WHERE MaTinhTP = {0}", maTinhTP).ToListAsync();
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
