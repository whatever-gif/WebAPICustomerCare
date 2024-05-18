using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.WebSockets;


namespace DATN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {

        // gen context
        private readonly QuanTriHeThongContext _context;

        // constructor for context
        public NguoiDungController(QuanTriHeThongContext context)
        {
            _context = context;
        }

        // Check email
        public async Task<bool> CheckNguoiDungTonTai(string email)
        {
            try
            {
                var list = await _context.NguoiDung.FromSqlRaw("EXEC CheckNguoiDungTonTai @Email",
                    new SqlParameter("@Email", email)).ToListAsync();

                if (list.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        // Tìm kiếm người dùng
        [HttpPost("search")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<NguoiDung>>> TimKiemNguoiDung([FromForm] IFormCollection form)
        {

            var response = new Response<NguoiDung>();

            Microsoft.Extensions.Primitives.StringValues keywordValues;

            string Keyword = "";

            form.TryGetValue("Keyword", out keywordValues);
            if (keywordValues.Count > 0)
            {
                Keyword = keywordValues[0] ?? "";
            }
        
            try
            {
                var list = await _context.NguoiDung.FromSqlRaw("EXEC TimKiemNguoiDung @Keyword",
                    new SqlParameter("@Keyword", Keyword)
                ).ToListAsync();

                response.DataList = list;
                response.Count = list.Count;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }

            return response;
        }

        // Tìm kiếm người dùng active
        [HttpPost("getActive")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<NguoiDung>>> TimKiemNguoiDungHoatDong()
        {
            var response = new Response<NguoiDung>();
            try
            {
                var list = await _context.NguoiDung.FromSqlRaw("EXEC TimKiemNguoiDungHoatDong"
                ).ToListAsync();

                response.DataList = list;
                response.Count = list.Count;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }

            return response;
        }

        // Tạo mới người dùng
        [HttpPost("create")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<NguoiDung>>> TaoMoiNguoiDung([FromForm] IFormCollection form)
        {

            var result = new Response<NguoiDung>
            {
                DataList = new List<NguoiDung>(),
                Count = 0,
                Error = null,
                Success = false
            };  

            Microsoft.Extensions.Primitives.StringValues strJsonValues;

            string strJson = "";

            form.TryGetValue("strJson", out strJsonValues);
            if (strJsonValues.Count > 0)
            {
                strJson = strJsonValues[0] ?? "";
            }


            // Kiểm tra chuỗi strJson đầu vào

            try
            {
                var objectPost = JsonConvert.DeserializeObject<NguoiDung>(strJson);

                 // Kiểm tra Email
                 if (string.IsNullOrEmpty(objectPost.Email))
                 {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Email không hợp lệ!", null);
                }


                // Kiểm tra TenNguoiDung
                if (string.IsNullOrEmpty(objectPost.TenNguoiDung))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Tên người dùng không hợp lệ!", null);

                }

                // Kiểm tra SoDienThoai
                if (string.IsNullOrEmpty(objectPost.SoDienThoai))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Số điện thoại không hợp lệ!", null);
                }

                // Kiểm tra MatKhau
                if (string.IsNullOrEmpty(objectPost.MatKhau))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Mật khẩu không hợp lệ!", null);
                }

                // Kiểm tra cờ FlagSysAdmin
                if (string.IsNullOrEmpty(objectPost.FlagSysAdmin))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Cờ quản trị hệ thống không hợp lệ!", null);
                }

                // Kiểm tra Email đã tồn tại hay chưa
                var checkTonTai = await CheckNguoiDungTonTai(objectPost.Email);
                if (checkTonTai == true)
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Người dùng đã tồn tại!", null);
                }

                // Bắt đầu thêm mới người dùng

                var list = await _context.NguoiDung.FromSqlRaw("EXEC TaoMoiNguoiDung @Email, @TenNguoiDung, @Avatar, @SoDienThoai, @FlagSysAdmin, @MatKhau",
                    new SqlParameter("@Email", objectPost.Email),
                    new SqlParameter("@TenNguoiDung", objectPost.TenNguoiDung),
                    new SqlParameter("@Avatar", objectPost.Avatar  ?? (object)DBNull.Value),
                    new SqlParameter("@SoDienThoai", objectPost.SoDienThoai),
                    new SqlParameter("@MatKhau", objectPost.MatKhau),
                    new SqlParameter("@FlagSysAdmin", objectPost.FlagSysAdmin)
                ).ToListAsync();

                if (list.Count > 0)
                {
                    result.Data = list[0];
                    result.Success = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                return Utils.Utils.ReturnErrorResponse<NguoiDung>("Đã có lỗi xảy ra!", ex.Message);
            }


        }

        // Chi tiết người dùng
        [HttpPost("getByEmail")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<NguoiDung>>> ChiTietNguoiDung([FromForm] IFormCollection form)
        {

            var response = new Response<NguoiDung>();

            Microsoft.Extensions.Primitives.StringValues emailValues;

            string email = "";

            form.TryGetValue("Email", out emailValues);
            if (emailValues.Count > 0)
            {
                email = emailValues[0] ?? "";
            }

            if (string.IsNullOrEmpty(email))
            {
                response.Success = false;
                return response;
            }

            try
            {
                var list = await _context.NguoiDung.FromSqlRaw("EXEC TimKiemThongTinNguoiDung @Email",
                    new SqlParameter("@Email", email)).ToListAsync();

                if (list.Count == 0)
                {
                    response.Success = false;
                    return response;
                }
                else
                {
                    response.Success = true;
                    response.Data = list[0];
                }

            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }

            return response;
        }

        // Cập nhật người dùng
        [HttpPost("update")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<NguoiDung>>> CapNhatNguoiDung([FromForm] IFormCollection form)
        {

            var result = new Response<NguoiDung>
            {
                DataList = new List<NguoiDung>(),
                Count = 0,
                Error = null,
                Success = false
            };

            Microsoft.Extensions.Primitives.StringValues strJsonValues;

            string strJson = "";

            form.TryGetValue("strJson", out strJsonValues);
            if (strJsonValues.Count > 0)
            {
                strJson = strJsonValues[0] ?? "";
            }


            // Kiểm tra chuỗi strJson đầu vào

            try
            {
                var objectPost = JsonConvert.DeserializeObject<NguoiDung>(strJson);

                // Kiểm tra Email
                if (string.IsNullOrEmpty(objectPost.Email))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Email không hợp lệ!", null);
                }


                // Kiểm tra TenNguoiDung
                if (string.IsNullOrEmpty(objectPost.TenNguoiDung))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Tên người dùng không hợp lệ!", null);

                }

                // Kiểm tra SoDienThoai
                if (string.IsNullOrEmpty(objectPost.SoDienThoai))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Số điện thoại không hợp lệ!", null);
                }

                // Kiểm tra MatKhau
                if (string.IsNullOrEmpty(objectPost.MatKhau))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Mật khẩu không hợp lệ!", null);
                }

                // Kiểm tra cờ FlagSysAdmin
                if (string.IsNullOrEmpty(objectPost.FlagSysAdmin))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Cờ quản trị hệ thống không hợp lệ!", null);
                }

                // Kiểm tra Email đã tồn tại hay chưa
                var checkTonTai = await CheckNguoiDungTonTai(objectPost.Email);
                if (checkTonTai == false)
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Người dùng không tồn tại!", null);
                }

                // Kiểm tra trạng thái hoạt động , nếu khác 0 hoặc 1 thì trả về lỗi
                if (objectPost.TrangThaiHoatDong != "0" && objectPost.TrangThaiHoatDong != "1")
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Trạng thái hoạt động không hợp lệ!", null);
                }


                // Bắt đầu cập nhật

                var update = await _context.Database.ExecuteSqlRawAsync("EXEC CapNhatNguoiDung @Email, @TenNguoiDung, @Avatar, @MatKhau, @SoDienThoai, @FlagSysAdmin, @TrangThaiHoatDong",
                    new SqlParameter("@Email", objectPost.Email),
                    new SqlParameter("@TenNguoiDung", objectPost.TenNguoiDung),
                    new SqlParameter("@Avatar", objectPost.Avatar ?? (object)DBNull.Value),
                    new SqlParameter("@SoDienThoai", objectPost.SoDienThoai),
                    new SqlParameter("@MatKhau", objectPost.MatKhau),
                    new SqlParameter("@FlagSysAdmin", objectPost.FlagSysAdmin),
                    new SqlParameter("@TrangThaiHoatDong", objectPost.TrangThaiHoatDong)
                );

                if (update > 0)
                {
                    result.Success = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                return Utils.Utils.ReturnErrorResponse<NguoiDung>("Đã có lỗi xảy ra!", ex.Message);
            }
        }

        // Xóa người dùng
        [HttpPost("delete")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<NguoiDung>>> XoaNguoiDung([FromForm] IFormCollection form)
        {

            var result = new Response<NguoiDung>
            {
                DataList = new List<NguoiDung>(),
                Count = 0,
                Error = null,
                Success = false
            };

            Microsoft.Extensions.Primitives.StringValues emailValues;

            string email = "";

            form.TryGetValue("Email", out emailValues);
            if (emailValues.Count > 0)
            {
                email = emailValues[0] ?? "";
            }


            // Kiểm tra chuỗi strJson đầu vào

            try
            {

                // Kiểm tra Email
                if (string.IsNullOrEmpty(email))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Email không hợp lệ!", null);
                }


                // Kiểm tra Email đã tồn tại hay chưa
                var checkTonTai = await CheckNguoiDungTonTai(email);
                if (checkTonTai == false)
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Người dùng không tồn tại!", null);
                }

                // Bắt đầu xóa

                var update = await _context.Database.ExecuteSqlRawAsync("EXEC XoaNguoiDung @Email",
                    new SqlParameter("@Email", email)
                );

                if (update > 0)
                {
                    result.Success = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                return Utils.Utils.ReturnErrorResponse<NguoiDung>("Đã có lỗi xảy ra!", ex.Message);
            }
        }


        // Cập nhật người dùng (thay đổi tên, sđt)
        [HttpPost("updateNormal")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<NguoiDung>>> CapNhatNguoiDungThongThuong([FromForm] IFormCollection form)
        {

            var result = new Response<NguoiDung>
            {
                DataList = new List<NguoiDung>(),
                Count = 0,
                Error = null,
                Success = false
            };

            Microsoft.Extensions.Primitives.StringValues strJsonValues;

            string strJson = "";

            form.TryGetValue("strJson", out strJsonValues);
            if (strJsonValues.Count > 0)
            {
                strJson = strJsonValues[0] ?? "";
            }


            // Kiểm tra chuỗi strJson đầu vào

            try
            {
                var objectPost = JsonConvert.DeserializeObject<NguoiDung>(strJson);

                // Kiểm tra Email
                if (string.IsNullOrEmpty(objectPost.Email))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Email không hợp lệ!", null);
                }


                // Kiểm tra TenNguoiDung
                if (string.IsNullOrEmpty(objectPost.TenNguoiDung))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Tên người dùng không hợp lệ!", null);

                }

                // Kiểm tra SoDienThoai
                if (string.IsNullOrEmpty(objectPost.SoDienThoai))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Số điện thoại không hợp lệ!", null);
                }

                // Bắt đầu cập nhật

                var update = await _context.Database.ExecuteSqlRawAsync("UPDATE NguoiDung set TenNguoiDung = {0}, SoDienThoai = {1} WHERE Email = {2} AND TrangThaiXoa = 0",
                    objectPost.TenNguoiDung, objectPost.SoDienThoai, objectPost.Email
                );

                if (update > 0)
                {
                    result.Success = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                return Utils.Utils.ReturnErrorResponse<NguoiDung>("Đã có lỗi xảy ra!", ex.Message);
            }
        }


        // Cập nhật mật khẩu
        [HttpPost("updatePassword")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<NguoiDung>>> CapNhatMatKhau([FromForm] IFormCollection form)
        {

            var result = new Response<NguoiDung>
            {
                DataList = new List<NguoiDung>(),
                Count = 0,
                Error = null,
                Success = false
            };

            Microsoft.Extensions.Primitives.StringValues strJsonValues;

            string strJson = "";

            form.TryGetValue("strJson", out strJsonValues);
            if (strJsonValues.Count > 0)
            {
                strJson = strJsonValues[0] ?? "";
            }

            // Kiểm tra chuỗi strJson đầu vào

            try
            {
                var objectPost = JsonConvert.DeserializeObject<CapNhatMatKhau>(strJson);

                // Kiểm tra Email
                if (string.IsNullOrEmpty(objectPost.Email))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Email không hợp lệ!", null);
                }


                // Kiểm tra MatKhau
                if (string.IsNullOrEmpty(objectPost.MatKhau))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Mật khẩu không hợp lệ!", null);

                }

                // Kiểm tra MatKhauMoi
                if (string.IsNullOrEmpty(objectPost.MatKhauMoi))
                {
                    return Utils.Utils.ReturnErrorResponse<NguoiDung>("Mật khẩu mới không hợp lệ!", null);
                }

                // Bắt đầu cập nhật

                var update = await _context.Database.ExecuteSqlRawAsync("UPDATE NguoiDung set MatKhau = {0} WHERE Email = {1} AND MatKhau = {2} AND TrangThaiXoa = 0",
                    objectPost.MatKhauMoi, objectPost.Email, objectPost.MatKhau
                );

                if (update > 0)
                {
                    result.Success = true;
                }
                else
                {
                    result.Error = "Mật khẩu cũ không chính xác!";
                }

                return result;
            }
            catch (Exception ex)
            {
                return Utils.Utils.ReturnErrorResponse<NguoiDung>("Đã có lỗi xảy ra!", ex.Message);
            }

        }
    }
}
