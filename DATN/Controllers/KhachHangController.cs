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
    public class KhachHangController : ControllerBase
    {

        // gen context
        private readonly QuanTriHeThongContext _context;

        // constructor for context
        public KhachHangController(QuanTriHeThongContext context)
        {
            _context = context;
        }

        // Check mã khách hàng
        public async Task<bool> CheckKhachHangTonTai(string maKhachHang)
        {
            try
            {
                var list = await _context.KhachHang.FromSqlRaw("EXEC CheckKhachHangTonTai @MaKhachHang",
                    new SqlParameter("@MaKhachHang", maKhachHang)).ToListAsync();

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

        // Tìm kiếm khách hàng
        [HttpPost("search")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<KhachHang>>> TimKiemKhachHang([FromForm] IFormCollection form)
        {

            var response = new Response<KhachHang>();

            Microsoft.Extensions.Primitives.StringValues maKhachHangValues;
            Microsoft.Extensions.Primitives.StringValues tenKhachHangValues;
            Microsoft.Extensions.Primitives.StringValues soDienThoaiValues;
            Microsoft.Extensions.Primitives.StringValues emailValues;
            Microsoft.Extensions.Primitives.StringValues nguoiTaoValues;
            Microsoft.Extensions.Primitives.StringValues thoiGianTaoTuValues;
            Microsoft.Extensions.Primitives.StringValues thoiGianTaoDenValues;


            string maKhachHang = "";
            string tenKhachHang = "";
            string soDienThoai = "";
            string email = "";
            string nguoiTao = "";
            string? thoiGianTaoTu = "" == "" ? null : "";
            string? thoiGianTaoDen = "" == "" ? null : "";

            form.TryGetValue("MaKhachHang", out maKhachHangValues);
            if (maKhachHangValues.Count > 0)
            {
                maKhachHang = maKhachHangValues[0] ?? "";
            }

            form.TryGetValue("TenKhachHang", out tenKhachHangValues);
            if (tenKhachHangValues.Count > 0)
            {
                tenKhachHang = tenKhachHangValues[0] ?? "";
            }


            form.TryGetValue("SoDienThoai", out soDienThoaiValues);
            if (soDienThoaiValues.Count > 0)
            {
                soDienThoai = soDienThoaiValues[0] ?? "";
            }

            form.TryGetValue("Email", out emailValues);
            if (emailValues.Count > 0)
            {
                email = emailValues[0] ?? "";
            }

            form.TryGetValue("NguoiTao", out nguoiTaoValues);
            if (nguoiTaoValues.Count > 0)
            {
                nguoiTao = nguoiTaoValues[0] ?? "";
            }

            form.TryGetValue("ThoiGianTaoTu", out thoiGianTaoTuValues);
            if (thoiGianTaoTuValues.Count > 0)
            {
                DateTime temp;

                if (thoiGianTaoTuValues[0] == "" || DateTime.TryParse(thoiGianTaoTuValues[0], out temp))
                {
                    thoiGianTaoTu = thoiGianTaoTuValues[0];
                }
                else
                {
                    return new Response<KhachHang>
                    {
                        DataList = new List<KhachHang>(),
                        Count = 0,
                        Error = "Thời gian tạo từ không hợp lệ!",
                        Success = false
                    };
                }
            }

            form.TryGetValue("ThoiGianTaoDen", out thoiGianTaoDenValues);
            if (thoiGianTaoDenValues.Count > 0)
            {
                DateTime temp;
                if (thoiGianTaoDenValues[0] == "" || DateTime.TryParse(thoiGianTaoDenValues[0], out temp))
                {
                    thoiGianTaoDen = thoiGianTaoDenValues[0];
                }
                else
                {
                    return new Response<KhachHang>
                    {
                        DataList = new List<KhachHang>(),
                        Count = 0,
                        Error = "Thời gian tạo đến không hợp lệ!",
                        Success = false
                    };
                }
            }

            try
            {
             var list = await _context.KhachHang.FromSqlRaw("EXEC TimKiemKhachHang @MaKhachHang, @TenKhachHang, @SoDienThoai, @Email, @ThoiGianTaoTu, @ThoiGianTaoDen, @NguoiTao",
                 new SqlParameter("@MaKhachHang", maKhachHang),
                 new SqlParameter("@TenKhachHang", tenKhachHang),
                 new SqlParameter("@SoDienThoai", soDienThoai),
                 new SqlParameter("@Email", email),
                 new SqlParameter("@NguoiTao", nguoiTao),
                 new SqlParameter("@ThoiGianTaoTu", string.IsNullOrEmpty(thoiGianTaoTu) ? (object)DBNull.Value : thoiGianTaoTu),
                 new SqlParameter("@ThoiGianTaoDen", string.IsNullOrEmpty(thoiGianTaoDen) ? (object)DBNull.Value : thoiGianTaoDen)
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

        // Tạo mới khách hàng
        [HttpPost("create")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<KhachHang>>> TaoMoiKhachHang([FromForm] IFormCollection form)
        {

            var result = new Response<KhachHang>
            {
                DataList = new List<KhachHang>(),
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
                var objectPost = JsonConvert.DeserializeObject<KhachHang>(strJson);

                 // Kiểm tra MaKhachHang
                 if (string.IsNullOrEmpty(objectPost.MaKhachHang))
                 {
                    return Utils.Utils.ReturnErrorResponse<KhachHang>("Mã khách hàng không hợp lệ!", null);
                }


                // Kiểm tra TenKhachHang
                if (string.IsNullOrEmpty(objectPost.TenKhachHang))
                {
                    return Utils.Utils.ReturnErrorResponse<KhachHang>("Tên khách hàng không hợp lệ!", null);

                }

                // Kiểm tra SoDienThoai
                if (string.IsNullOrEmpty(objectPost.SoDienThoai))
                {
                    return Utils.Utils.ReturnErrorResponse<KhachHang>("Số điện thoại không hợp lệ!", null);
                }

                // Kiểm tra MaKhachHang đã tồn tại hay chưa
                var checkTonTai = await CheckKhachHangTonTai(objectPost.MaKhachHang);
                if (checkTonTai == true)
                {
                    return Utils.Utils.ReturnErrorResponse<KhachHang>("Khách hàng đã tồn tại!", null);
                }

                var ngaySinh = (object)DBNull.Value;

                // Check ngày sinh nếu có dữ liệu thì chuyển về kiểu DateTime không thì null
                
                if (objectPost.NgaySinh != null)
                {
                    ngaySinh = objectPost.NgaySinh.Value;
                }

                // Kiểm tra giới tính có phải là Male, Female hay Other không

               

                // Bắt đầu thêm mới khách hàng

                var list = await _context.KhachHang.FromSqlRaw("EXEC TaoMoiKhachHang @MaKhachHang, @TenKhachHang, @NgaySinh, @GioiTinh, @MaQuocGia, @MaTinhTP, @MaQuanHuyen, @MaPhuongXa, @DiaChi ," +
                    "@SoDienThoai, @Email, @SoGTTT, @MaGTTT, @NgheNghiep, @MaNguonKhach, @AnhDaiDien, @NguoiTao, @TrangThai",
                    new SqlParameter("@MaKhachHang", objectPost.MaKhachHang),
                    new SqlParameter("@TenKhachHang", objectPost.TenKhachHang),
                    new SqlParameter("@NgaySinh", ngaySinh),
                    new SqlParameter("@GioiTinh", objectPost.GioiTinh),
                    new SqlParameter("@MaQuocGia", objectPost.MaQuocGia ?? (object)DBNull.Value),
                    new SqlParameter("@MaTinhTP", objectPost.MaTinhTP ?? (object)DBNull.Value),
                    new SqlParameter("@MaQuanHuyen", objectPost.MaQuanHuyen ?? (object)DBNull.Value),
                    new SqlParameter("@MaPhuongXa", objectPost.MaPhuongXa ?? (object)DBNull.Value),
                    new SqlParameter("@DiaChi", objectPost.DiaChi ?? (object)DBNull.Value),
                    new SqlParameter("@SoDienThoai", objectPost.SoDienThoai),
                    new SqlParameter("@Email", objectPost.Email ?? (object)DBNull.Value),
                    new SqlParameter("@SoGTTT", objectPost.SoGTTT ?? (object)DBNull.Value),
                    new SqlParameter("@MaGTTT", objectPost.MaGTTT ?? (object)DBNull.Value),
                    new SqlParameter("@NgheNghiep", objectPost.NgheNghiep ?? (object)DBNull.Value),
                    new SqlParameter("@MaNguonKhach", objectPost.MaNguonKhach ?? (object)DBNull.Value),
                    new SqlParameter("@AnhDaiDien", objectPost.AnhDaiDien ?? (object)DBNull.Value),
                    new SqlParameter("@NguoiTao", objectPost.NguoiTao),
                    new SqlParameter("@TrangThai", "1")
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
                return Utils.Utils.ReturnErrorResponse<KhachHang>("Dữ liệu đầu vào không hợp lệ!", ex.Message);
            }


        }

        // Chi tiết khách hàng
        [HttpPost("getByMaKhachHang")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<KhachHang>>> ChiTietKhachHang([FromForm] IFormCollection form)
        {

            var response = new Response<KhachHang>();

            Microsoft.Extensions.Primitives.StringValues maKhachHangValues;

            string maKhachHang = "";

            form.TryGetValue("MaKhachHang", out maKhachHangValues);
            if (maKhachHangValues.Count > 0)
            {
                maKhachHang = maKhachHangValues[0] ?? "";
            }

            if (string.IsNullOrEmpty(maKhachHang))
            {
                response.Success = true;
                return response;
            }

            try
            {
                var list = await _context.KhachHang.FromSqlRaw("EXEC TimKiemThongTinKhachHang @MaKhachHang", 
                    new SqlParameter("@MaKhachHang", maKhachHang)).ToListAsync();

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

        // Cập nhật khách hàng
        [HttpPost("update")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<KhachHang>>> CapNhatKhachHang([FromForm] IFormCollection form)
        {

            var result = new Response<KhachHang>
            {
                DataList = new List<KhachHang>(),
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
                var objectPost = JsonConvert.DeserializeObject<KhachHang>(strJson);

                // Kiểm tra MaKhachHang
                if (string.IsNullOrEmpty(objectPost.MaKhachHang))
                {
                    return Utils.Utils.ReturnErrorResponse<KhachHang>("Mã khách hàng không hợp lệ!", null);
                }


                // Kiểm tra TenKhachHang
                if (string.IsNullOrEmpty(objectPost.TenKhachHang))
                {
                    return Utils.Utils.ReturnErrorResponse<KhachHang>("Tên khách hàng không hợp lệ!", null);

                }

                // Kiểm tra SoDienThoai
                if (string.IsNullOrEmpty(objectPost.SoDienThoai))
                {
                    return Utils.Utils.ReturnErrorResponse<KhachHang>("Số điện thoại không hợp lệ!", null);
                }

                // Kiểm tra MaKhachHang đã tồn tại hay chưa
                var checkTonTai = await CheckKhachHangTonTai(objectPost.MaKhachHang);
                if (checkTonTai == false)
                {
                    return Utils.Utils.ReturnErrorResponse<KhachHang>("Khách hàng không tồn tại!", null);

                }

                var ngaySinh = (object)DBNull.Value;
                var trangThai = "1";

                // Check ngày sinh nếu có dữ liệu thì chuyển về kiểu DateTime không thì null

                if (objectPost.NgaySinh != null)
                {
                    ngaySinh = objectPost.NgaySinh.Value;
                }

                // Check trạng thái 0 - 1
                
                if (objectPost.TrangThai != null)
                {
                    if (!Utils.Utils.IsValidTrangThai(objectPost.TrangThai))
                    {
                        return Utils.Utils.ReturnErrorResponse<KhachHang>("Trạng thái không hợp lệ!", null);
                    }
                    trangThai = objectPost.TrangThai;
                }



                // Bắt đầu cập nhật khách hàng

                var update = await _context.Database.ExecuteSqlRawAsync("EXEC CapNhatKhachHang @MaKhachHang, @TenKhachHang, @NgaySinh, @GioiTinh, @MaQuocGia, @MaTinhTP, @MaQuanHuyen, @MaPhuongXa, @DiaChi ," +
                    "@SoDienThoai, @Email, @SoGTTT, @MaGTTT, @NgheNghiep, @MaNguonKhach, @AnhDaiDien, @TrangThai",
                    new SqlParameter("@MaKhachHang", objectPost.MaKhachHang),
                    new SqlParameter("@TenKhachHang", objectPost.TenKhachHang),
                    new SqlParameter("@NgaySinh", ngaySinh),
                    new SqlParameter("@GioiTinh", objectPost.GioiTinh),
                    new SqlParameter("@MaQuocGia", objectPost.MaQuocGia ?? (object)DBNull.Value),
                    new SqlParameter("@MaTinhTP", objectPost.MaTinhTP ?? (object)DBNull.Value),
                    new SqlParameter("@MaQuanHuyen", objectPost.MaQuanHuyen ?? (object)DBNull.Value),
                    new SqlParameter("@MaPhuongXa", objectPost.MaPhuongXa ?? (object)DBNull.Value),
                    new SqlParameter("@DiaChi", objectPost.DiaChi ?? (object)DBNull.Value),
                    new SqlParameter("@SoDienThoai", objectPost.SoDienThoai),
                    new SqlParameter("@Email", objectPost.Email ?? (object)DBNull.Value),
                    new SqlParameter("@SoGTTT", objectPost.SoGTTT ?? (object)DBNull.Value),
                    new SqlParameter("@MaGTTT", objectPost.MaGTTT ?? (object)DBNull.Value),
                    new SqlParameter("@NgheNghiep", objectPost.NgheNghiep ?? (object)DBNull.Value),
                    new SqlParameter("@MaNguonKhach", objectPost.MaNguonKhach ?? (object)DBNull.Value),
                    new SqlParameter("@AnhDaiDien", objectPost.AnhDaiDien ?? (object)DBNull.Value),
                    new SqlParameter("@TrangThai", trangThai)
                );

                // check it run correct
                if (update > 0)
                {
                    result.Success = true;
                }

                return result;


            }
            catch (Exception ex)
            {
                return Utils.Utils.ReturnErrorResponse<KhachHang>("Dữ liệu đầu vào không hợp lệ!", ex.Message);
            }


        }

        // Chi tiết khách hàng (gồm eticket)
        [HttpPost("getAllInfoByMaKhachHang")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<ThongTinKhachHang>>> ChiTietKhachHangVaTicket([FromForm] IFormCollection form)
        {

            var response = new Response<ThongTinKhachHang>();

            response.Data = new ThongTinKhachHang();

            response.Data.KhachHang = new KhachHang();
            response.Data.Ticket = new List<Eticket>();

            Microsoft.Extensions.Primitives.StringValues maKhachHangValues;

            string maKhachHang = "";

            form.TryGetValue("MaKhachHang", out maKhachHangValues);
            if (maKhachHangValues.Count > 0)
            {
                maKhachHang = maKhachHangValues[0] ?? "";
            }

            if (string.IsNullOrEmpty(maKhachHang))
            {
                response.Success = true;
                return response;
            }

            try
            {
                var list = await _context.KhachHang.FromSqlRaw("EXEC TimKiemThongTinKhachHang @MaKhachHang",
                    new SqlParameter("@MaKhachHang", maKhachHang)).ToListAsync();

                if (list.Count == 0)
                {
                    response.Success = false;
                    return response;
                }
                else
                {
                    response.Success = true;
                    response.Data.KhachHang = list[0];
                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
            }

            try
            {
                var list = await _context.Eticket.FromSqlRaw("EXEC TimKiemThongTinTicketKhachHang @MaKhachHang",
                    new SqlParameter("@MaKhachHang", maKhachHang)).ToListAsync();
              
                response.Success = true;
                response.Data.Ticket = list.ToList();
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
            }

            return response;
        }

        // Xóa khách hàng
        [HttpPost("delete")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<KhachHang>>> XoaKhachHang([FromForm] IFormCollection form)
        {

            var response = new Response<KhachHang>();

            Microsoft.Extensions.Primitives.StringValues maKhachHangValues;

            string maKhachHang = "";

            form.TryGetValue("MaKhachHang", out maKhachHangValues);
            if (maKhachHangValues.Count > 0)
            {
                maKhachHang = maKhachHangValues[0] ?? "";
            }

            if (string.IsNullOrEmpty(maKhachHang))
            {
                response.Success = false;
                response.Error = "Mã khách hàng không hợp lệ!";
                return response;
            }

            // Check khách hàng đã tồn tại hay chưa
            var checkTonTai = await CheckKhachHangTonTai(maKhachHang);
            
            if (checkTonTai == false)
            {
                response.Success = false;
                response.Error = "Khách hàng không tồn tại!";
                return response;
            }

            try
            {
                var del = await _context.Database.ExecuteSqlRawAsync("UPDATE KhachHang SET TrangThaiXoa = 1 WHERE MaKhachHang = {0}", maKhachHang);

                if (del == 0)
                {
                    response.Success = false;
                }
                else
                {
                    response.Success = true;
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
