using DATN.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DATN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly QuanTriHeThongContext _context;

        public AuthController(QuanTriHeThongContext context)
        {
            _context = context;
        }

        // Đăng nhập
        [HttpPost("login")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<NguoiDung>>> Login([FromForm] IFormCollection form)
        {

            var response = new Response<NguoiDung>();

            Microsoft.Extensions.Primitives.StringValues emailValues;
            Microsoft.Extensions.Primitives.StringValues matKhauValues;

            string email = "";
            string matKhau = "";

            form.TryGetValue("Email", out emailValues);
            if (emailValues.Count > 0)
            {
                email = emailValues[0] ?? "";
            }

            form.TryGetValue("MatKhau", out matKhauValues);
            if (matKhauValues.Count > 0)
            {
                matKhau = matKhauValues[0] ?? "";
            }

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(matKhau))
            {
                return Utils.Utils.ReturnErrorResponse<NguoiDung>("Email hoặc mật khẩu không được hợp lệ!", null);
            }

            try
            {
                var list = await _context.NguoiDung.FromSqlRaw("EXEC DangNhap @Email, @MatKhau", 
                    new SqlParameter("@Email", email),
                    new SqlParameter("@MatKhau", matKhau))
                    .ToListAsync();
                
                if (list.Count == 0)
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Email hoặc mật khẩu không chính xác!", null);
                }
                else
                {
                    var result = list[0];

                    if (result.TrangThaiHoatDong == "0")
                    {
                        return Utils.Utils.ReturnErrorResponse<NguoiDung>("Người dùng không hoạt động. Vui lòng liên hệ Quản trị viên để được hỗ trợ!", null);
                    }
                    else
                    {
                        response.Success = true;
                        response.Data = result;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }

            return response;
        }
    }
}

