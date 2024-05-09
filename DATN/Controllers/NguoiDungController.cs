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
        //[HttpPost("search")]
        //[Consumes("application/x-www-form-urlencoded")]
        //public async Task<ActionResult<Response<NguoiDung>>> TimKiemNguoiDung([FromForm] IFormCollection form)
        //{

        //    var response = new Response<NguoiDung>();

        //    Microsoft.Extensions.Primitives.StringValues EmailValues;
        //    Microsoft.Extensions.Primitives.StringValues tenNguoiDungValues;
        //    Microsoft.Extensions.Primitives.StringValues soDienThoaiValues;
        //    Microsoft.Extensions.Primitives.StringValues emailValues;
        //    Microsoft.Extensions.Primitives.StringValues nguoiTaoValues;
        //    Microsoft.Extensions.Primitives.StringValues thoiGianTaoTuValues;
        //    Microsoft.Extensions.Primitives.StringValues thoiGianTaoDenValues;


        //    string Email = "";
        //    string tenNguoiDung = "";
        //    string soDienThoai = "";
        //    string email = "";
        //    string nguoiTao = "";
        //    string? thoiGianTaoTu = "" == "" ? null : "";
        //    string? thoiGianTaoDen = "" == "" ? null : "";

        //    form.TryGetValue("Email", out EmailValues);
        //    if (EmailValues.Count > 0)
        //    {
        //        Email = EmailValues[0] ?? "";
        //    }

        //    form.TryGetValue("TenNguoiDung", out tenNguoiDungValues);
        //    if (tenNguoiDungValues.Count > 0)
        //    {
        //        tenNguoiDung = tenNguoiDungValues[0] ?? "";
        //    }


        //    form.TryGetValue("SoDienThoai", out soDienThoaiValues);
        //    if (soDienThoaiValues.Count > 0)
        //    {
        //        soDienThoai = soDienThoaiValues[0] ?? "";
        //    }

        //    form.TryGetValue("Email", out emailValues);
        //    if (emailValues.Count > 0)
        //    {
        //        email = emailValues[0] ?? "";
        //    }

        //    form.TryGetValue("NguoiTao", out nguoiTaoValues);
        //    if (nguoiTaoValues.Count > 0)
        //    {
        //        nguoiTao = nguoiTaoValues[0] ?? "";
        //    }

        //    form.TryGetValue("ThoiGianTaoTu", out thoiGianTaoTuValues);
        //    if (thoiGianTaoTuValues.Count > 0)
        //    {
        //        DateTime temp;

        //        if (thoiGianTaoTuValues[0] == "" || DateTime.TryParse(thoiGianTaoTuValues[0], out temp))
        //        {
        //            thoiGianTaoTu = thoiGianTaoTuValues[0];
        //        }
        //        else
        //        {
        //            return new Response<NguoiDung>
        //            {
        //                DataList = new List<NguoiDung>(),
        //                Count = 0,
        //                Error = "Thời gian tạo từ không hợp lệ!",
        //                Success = false
        //            };
        //        }
        //    }

        //    form.TryGetValue("ThoiGianTaoDen", out thoiGianTaoDenValues);
        //    if (thoiGianTaoDenValues.Count > 0)
        //    {
        //        DateTime temp;
        //        if (thoiGianTaoDenValues[0] == "" || DateTime.TryParse(thoiGianTaoDenValues[0], out temp))
        //        {
        //            thoiGianTaoDen = thoiGianTaoDenValues[0];
        //        }
        //        else
        //        {
        //            return new Response<NguoiDung>
        //            {
        //                DataList = new List<NguoiDung>(),
        //                Count = 0,
        //                Error = "Thời gian tạo đến không hợp lệ!",
        //                Success = false
        //            };
        //        }
        //    }

        //    try
        //    {
        //     var list = await _context.NguoiDung.FromSqlRaw("EXEC TimKiemNguoiDung @Email, @TenNguoiDung, @SoDienThoai, @Email, @ThoiGianTaoTu, @ThoiGianTaoDen, @NguoiTao",
        //         new SqlParameter("@Email", Email),
        //         new SqlParameter("@TenNguoiDung", tenNguoiDung),
        //         new SqlParameter("@SoDienThoai", soDienThoai),
        //         new SqlParameter("@Email", email),
        //         new SqlParameter("@NguoiTao", nguoiTao),
        //         new SqlParameter("@ThoiGianTaoTu", string.IsNullOrEmpty(thoiGianTaoTu) ? (object)DBNull.Value : thoiGianTaoTu),
        //         new SqlParameter("@ThoiGianTaoDen", string.IsNullOrEmpty(thoiGianTaoDen) ? (object)DBNull.Value : thoiGianTaoDen)
        //     ).ToListAsync();

        //        response.DataList = list;
        //        response.Count = list.Count;
        //        response.Success = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Error = ex.Message;
        //    }

        //    return response;
        //}

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
        //[HttpPost("getByEmail")]
        //[Consumes("application/x-www-form-urlencoded")]
        //public async Task<ActionResult<Response<NguoiDung>>> ChiTietNguoiDung([FromForm] IFormCollection form)
        //{

        //    var response = new Response<NguoiDung>();

        //    Microsoft.Extensions.Primitives.StringValues EmailValues;

        //    string Email = "";

        //    form.TryGetValue("Email", out EmailValues);
        //    if (EmailValues.Count > 0)
        //    {
        //        Email = EmailValues[0] ?? "";
        //    }

        //    if (string.IsNullOrEmpty(Email))
        //    {
        //        response.Success = true;
        //        return response;
        //    }

        //    try
        //    {
        //        var list = await _context.NguoiDung.FromSqlRaw("EXEC TimKiemThongTinNguoiDung @Email", 
        //            new SqlParameter("@Email", Email)).ToListAsync();

        //        if (list.Count == 0)
        //        {
        //            response.Success = false;
        //            return response;
        //        }
        //        else
        //        {
        //            response.Success = true;
        //            response.Data = list[0];
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        response.Error = ex.Message;
        //    }

        //    return response;
        //}

        //// Cập nhật người dùng
        //[HttpPost("update")]
        //[Consumes("application/x-www-form-urlencoded")]
        //public async Task<ActionResult<Response<NguoiDung>>> CapNhatNguoiDung([FromForm] IFormCollection form)
        //{

        //    var result = new Response<NguoiDung>
        //    {
        //        DataList = new List<NguoiDung>(),
        //        Count = 0,
        //        Error = null,
        //        Success = false
        //    };

        //    Microsoft.Extensions.Primitives.StringValues strJsonValues;

        //    string strJson = "";

        //    form.TryGetValue("strJson", out strJsonValues);
        //    if (strJsonValues.Count > 0)
        //    {
        //        strJson = strJsonValues[0] ?? "";
        //    }


        //    // Kiểm tra chuỗi strJson đầu vào

        //    try
        //    {
        //        var objectPost = JsonConvert.DeserializeObject<NguoiDung>(strJson);

        //        // Kiểm tra Email
        //        if (string.IsNullOrEmpty(objectPost.Email))
        //        {
        //            return Utils.Utils.ReturnErrorResponse<NguoiDung>("Email không hợp lệ!", null);
        //        }


        //        // Kiểm tra TenNguoiDung
        //        if (string.IsNullOrEmpty(objectPost.TenNguoiDung))
        //        {
        //            return Utils.Utils.ReturnErrorResponse<NguoiDung>("Tên người dùng không hợp lệ!", null);

        //        }

        //        // Kiểm tra SoDienThoai
        //        if (string.IsNullOrEmpty(objectPost.SoDienThoai))
        //        {
        //            return Utils.Utils.ReturnErrorResponse<NguoiDung>("Số điện thoại không hợp lệ!", null);
        //        }

        //        // Kiểm tra Email đã tồn tại hay chưa
        //        var checkTonTai = await CheckNguoiDungTonTai(objectPost.Email);
        //        if (checkTonTai == false)
        //        {
        //            return Utils.Utils.ReturnErrorResponse<NguoiDung>("người dùng không tồn tại!", null);

        //        }

        //        var ngaySinh = (object)DBNull.Value;
        //        var trangThai = "1";

        //        // Check ngày sinh nếu có dữ liệu thì chuyển về kiểu DateTime không thì null

        //        if (objectPost.NgaySinh != null)
        //        {
        //            ngaySinh = objectPost.NgaySinh.Value;
        //        }

        //        // Check trạng thái 0 - 1
                
        //        if (objectPost.TrangThai != null)
        //        {
        //            if (!Utils.Utils.IsValidTrangThai(objectPost.TrangThai))
        //            {
        //                return Utils.Utils.ReturnErrorResponse<NguoiDung>("Trạng thái không hợp lệ!", null);
        //            }
        //            trangThai = objectPost.TrangThai;
        //        }



        //        // Bắt đầu cập nhật người dùng

        //        var update = await _context.Database.ExecuteSqlRawAsync("EXEC CapNhatNguoiDung @Email, @TenNguoiDung, @NgaySinh, @GioiTinh, @MaQuocGia, @MaTinhTP, @MaQuanHuyen, @MaPhuongXa, @DiaChi ," +
        //            "@SoDienThoai, @Email, @SoGTTT, @MaGTTT, @NgheNghiep, @MaNguonKhach, @AnhDaiDien, @TrangThai",
        //            new SqlParameter("@Email", objectPost.Email),
        //            new SqlParameter("@TenNguoiDung", objectPost.TenNguoiDung),
        //            new SqlParameter("@NgaySinh", ngaySinh),
        //            new SqlParameter("@GioiTinh", objectPost.GioiTinh),
        //            new SqlParameter("@MaQuocGia", objectPost.MaQuocGia ?? (object)DBNull.Value),
        //            new SqlParameter("@MaTinhTP", objectPost.MaTinhTP ?? (object)DBNull.Value),
        //            new SqlParameter("@MaQuanHuyen", objectPost.MaQuanHuyen ?? (object)DBNull.Value),
        //            new SqlParameter("@MaPhuongXa", objectPost.MaPhuongXa ?? (object)DBNull.Value),
        //            new SqlParameter("@DiaChi", objectPost.DiaChi ?? (object)DBNull.Value),
        //            new SqlParameter("@SoDienThoai", objectPost.SoDienThoai),
        //            new SqlParameter("@Email", objectPost.Email ?? (object)DBNull.Value),
        //            new SqlParameter("@SoGTTT", objectPost.SoGTTT ?? (object)DBNull.Value),
        //            new SqlParameter("@MaGTTT", objectPost.MaGTTT ?? (object)DBNull.Value),
        //            new SqlParameter("@NgheNghiep", objectPost.NgheNghiep ?? (object)DBNull.Value),
        //            new SqlParameter("@MaNguonKhach", objectPost.MaNguonKhach ?? (object)DBNull.Value),
        //            new SqlParameter("@AnhDaiDien", objectPost.AnhDaiDien ?? (object)DBNull.Value),
        //            new SqlParameter("@TrangThai", trangThai)
        //        );

        //        // check it run correct
        //        if (update > 0)
        //        {
        //            result.Success = true;
        //        }

        //        return result;


        //    }
        //    catch (Exception ex)
        //    {
        //        return Utils.Utils.ReturnErrorResponse<NguoiDung>("Dữ liệu đầu vào không hợp lệ!", ex.Message);
        //    }


        //}

        //// Xóa người dùng
        //[HttpPost("delete")]
        //[Consumes("application/x-www-form-urlencoded")]
        //public async Task<ActionResult<Response<NguoiDung>>> XoaNguoiDung([FromForm] IFormCollection form)
        //{

        //    var response = new Response<NguoiDung>();

        //    Microsoft.Extensions.Primitives.StringValues EmailValues;

        //    string Email = "";

        //    form.TryGetValue("Email", out EmailValues);
        //    if (EmailValues.Count > 0)
        //    {
        //        Email = EmailValues[0] ?? "";
        //    }

        //    if (string.IsNullOrEmpty(Email))
        //    {
        //        response.Success = false;
        //        response.Error = "Email không hợp lệ!";
        //        return response;
        //    }

        //    // Check người dùng đã tồn tại hay chưa
        //    var checkTonTai = await CheckNguoiDungTonTai(Email);
            
        //    if (checkTonTai == false)
        //    {
        //        response.Success = false;
        //        response.Error = "người dùng không tồn tại!";
        //        return response;
        //    }

        //    try
        //    {
        //        var del = await _context.Database.ExecuteSqlRawAsync("UPDATE NguoiDung SET TrangThaiXoa = 1 WHERE Email = {0}", Email);

        //        if (del == 0)
        //        {
        //            response.Success = false;
        //        }
        //        else
        //        {
        //            response.Success = true;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        response.Error = ex.Message;
        //    }

        //    return response;
        //}
    }
}
