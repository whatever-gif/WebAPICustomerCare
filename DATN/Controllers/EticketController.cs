using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data.Common;
using System.Net.WebSockets;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace DATN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EticketController : ControllerBase
    {

        // gen context
        private readonly QuanTriHeThongContext _context;

        // constructor for context
        public EticketController(QuanTriHeThongContext context)
        {
            _context = context;
        }

        // Check mã ticket
        public async Task<bool> CheckTicketTonTai(string maTicket)
        {
            try
            {
                var list = await _context.Eticket.FromSqlRaw("EXEC CheckTicketTonTai @MaTicket",
                    new SqlParameter("@MaTicket", maTicket)).ToListAsync();

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

        // Tìm kiếm Eticket
        [HttpPost("search")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<Eticket>>> TimKiemEticket([FromForm] IFormCollection form)
        {

            var response = new Response<Eticket>();

            Microsoft.Extensions.Primitives.StringValues maTicketValues;
            Microsoft.Extensions.Primitives.StringValues tenTicketValues;
            Microsoft.Extensions.Primitives.StringValues maKhachHangValues;
            Microsoft.Extensions.Primitives.StringValues maPhanLoaiTicketValues;
            Microsoft.Extensions.Primitives.StringValues nguoiTaoValues;
            Microsoft.Extensions.Primitives.StringValues flagQuaHanXuLyValues;
            Microsoft.Extensions.Primitives.StringValues thoiGianTaoTuValues;
            Microsoft.Extensions.Primitives.StringValues thoiGianTaoDenValues;


            string maTicket = "";
            string tenTicket = "";
            string maTrangThaiTicket = "";
            string maKhachHang = "";
            string maPhanLoaiTicket = "";
            string nguoiTao = "";
            string flagQuaHanXuLy = "";
            string? thoiGianTaoTu = "" == "" ? null : "";
            string? thoiGianTaoDen = "" == "" ? null : "";

            form.TryGetValue("MaTicket", out maTicketValues);
            if (maTicketValues.Count > 0)
            {
                maTicket = maTicketValues[0] ?? "";
            }

            form.TryGetValue("TenTicket", out tenTicketValues);
            if (tenTicketValues.Count > 0)
            {
                tenTicket = tenTicketValues[0] ?? "";
            }

            form.TryGetValue("MaKhachHang", out maKhachHangValues);
            if (maKhachHangValues.Count > 0)
            {
                maKhachHang = maKhachHangValues[0] ?? "";
            }

            form.TryGetValue("MaPhanLoaiTicket", out maPhanLoaiTicketValues);
            if (maPhanLoaiTicketValues.Count > 0)
            {
                maPhanLoaiTicket = maPhanLoaiTicketValues[0] ?? "";
            }

            form.TryGetValue("NguoiTao", out nguoiTaoValues);
            if (nguoiTaoValues.Count > 0)
            {
                nguoiTao = nguoiTaoValues[0] ?? "";
            }

            form.TryGetValue("FlagQuaHanXuLy", out flagQuaHanXuLyValues);
            if (flagQuaHanXuLyValues.Count > 0 )
            {
                flagQuaHanXuLy = flagQuaHanXuLyValues[0] ?? "";
                if (flagQuaHanXuLyValues[0] != "0" && flagQuaHanXuLyValues[0] != "1")
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Cờ quá hạn xử lý không hợp lệ!", null);
                }
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
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Thời gian tạo từ không hợp lệ!", null);
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
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Thời gian tạo đến không hợp lệ!", null);
                }
            }

            
            try
            {
                var list = await _context.Eticket.FromSqlRaw("EXEC TimKiemEticket @MaTicket, @TenTicket, @MaKhachHang, @MaPhanLoaiTicket, @MaTrangThaiTicket, @FlagQuaHanXuLy, @ThoiGianTaoTu, @ThoiGianTaoDen, @NguoiTao",
                        new SqlParameter("@MaTicket", maTicket),
                        new SqlParameter("@TenTicket", tenTicket),
                        new SqlParameter("@MaKhachHang", maKhachHang),
                        new SqlParameter("@MaPhanLoaiTicket", maPhanLoaiTicket),
                        new SqlParameter("@MaTrangThaiTicket", maTrangThaiTicket),
                        new SqlParameter("@FlagQuaHanXuLy", flagQuaHanXuLy),
                        new SqlParameter("@NguoiTao", nguoiTao),
                        new SqlParameter("@ThoiGianTaoTu", string.IsNullOrEmpty(thoiGianTaoTu) ? (object)DBNull.Value : thoiGianTaoTu),
                        new SqlParameter("@ThoiGianTaoDen", string.IsNullOrEmpty(thoiGianTaoDen) ? (object)DBNull.Value : thoiGianTaoDen)
                        ).ToListAsync();

                response.DataList = list.ToList();
                response.Count = list.Count;
                response.Error = null;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                response.Success = false;
                return new Response<Eticket>
                {
                    DataList = new List<Eticket>(),
                    Count = 0,
                    Error = ex.Message,
                    Success = false
                };
            }

            return response;

        }

        // Chi tiết ticket
        [HttpPost("getByMaTicket")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<ThongTinEticket>>> ChiTietTicket([FromForm] IFormCollection form)
        {

            var response = new Response<ThongTinEticket>();

            response.Data = new ThongTinEticket();
            response.Data.Eticket = new Eticket();
            response.Data.KhachHang = new KhachHang();
            response.Data.ListQuaTrinhXuLy = new List<QuaTrinhXuLyEticket>();

            Microsoft.Extensions.Primitives.StringValues maTicketValues;

            string maTicket = "";

            form.TryGetValue("MaTicket", out maTicketValues);
            if (maTicketValues.Count > 0)
            {
                maTicket = maTicketValues[0] ?? "";
            }

            if (string.IsNullOrEmpty(maTicket))
            {
                response.Success = true;
                return response;
            }

            try
            {
                var list = await _context.Eticket.FromSqlRaw("EXEC TimKiemThongTinTicket @MaTicket",
                    new SqlParameter("@MaTicket", maTicket)).ToListAsync();

                if (list.Count == 0)
                {
                    response.Success = false;
                    response.Data.Eticket = null;
                    response.Error = "Ticket không tồn tại!";
                    return response;
                }
                else
                {
                    var result = list[0];

                    response.Success = true;
                    response.Data.Eticket = result;

                    try
                    {
                        var listKH = await _context.KhachHang.FromSqlRaw("EXEC TimKiemThongTinKhachHang @MaKhachHang",
                            new SqlParameter("@MaKhachHang", result.MaKhachHang)).ToListAsync();

                        if (list.Count == 0)
                        {
                            response.Success = false;
                            response.Data.Eticket = null;
                            response.Data.KhachHang = null;
                            response.Error = "Khách hàng không tồn tại!";
                            return response;
                        }
                        else
                        {
                            response.Success = true;
                            response.Data.KhachHang = listKH[0];
                        }

                    }
                    catch (Exception ex)
                    {
                        response.Data = null;
                        response.Success = false;
                        response.Error = ex.Message;
                        return response;
                    }

                }

            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Error = ex.Message;
                return response;
            }


            try
            {
                var list = await _context.QuaTrinhXuLyEticket.FromSqlRaw("EXEC TimKiemThongTinQTXL @MaTicket",
                    new SqlParameter("@MaTicket", maTicket)).ToListAsync();

                response.Data.ListQuaTrinhXuLy = list.ToList();

            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Error = ex.Message;
                return response;
            }

            return response;
        }

        // Thêm Quá trình xử lý
        [HttpPost("createProcess")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<QuaTrinhXuLyEticket>>> ThemQuanTrinhXuLy([FromForm] IFormCollection form)
        {
            var response = new Response<QuaTrinhXuLyEticket>();

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
                var objectPost = JsonConvert.DeserializeObject<QuaTrinhXuLyEticket>(strJson);

                // Kiểm tra MaTicket
                if (string.IsNullOrEmpty(objectPost.MaTicket))
                {
                    return Utils.Utils.ReturnErrorResponse<QuaTrinhXuLyEticket>("Mã ticket không hợp lệ!", null);
                }

                // Kiểm tra NguoiXuLy
                if (string.IsNullOrEmpty(objectPost.NguoiXuLy))
                {
                    return Utils.Utils.ReturnErrorResponse<QuaTrinhXuLyEticket>("Người xử lý không hợp lệ!", null);

                }

                // Kiểm tra MaTicket đã tồn tại hay chưa
                var checkTonTai = await CheckTicketTonTai(objectPost.MaTicket);
                if (checkTonTai == false)
                {
                    return Utils.Utils.ReturnErrorResponse<QuaTrinhXuLyEticket>("Ticket không tồn tại!", null);
                }

                // Bắt đầu thêm qúa trính xử lý

                var add = await _context.Database.ExecuteSqlRawAsync("EXEC ThemQuaTrinhXuLy @MaTicket, @NoiDungXuLy, @NguoiXuLy",
                    new SqlParameter("@MaTicket", objectPost.MaTicket),
                    new SqlParameter("@NoiDungXuLy", objectPost.NoiDungXuLy),
                    new SqlParameter("@NguoiXuLy", objectPost.NguoiXuLy)
                );

                if (add > 0)
                {
                    response.Success = true;

                    var list = await _context.QuaTrinhXuLyEticket.FromSqlRaw("EXEC TimKiemThongTinQTXL @MaTicket",
                                               new SqlParameter("@MaTicket", objectPost.MaTicket)).ToListAsync();

                    response.DataList = list.ToList();
                }
                else
                {
                    response.Success = false;
                }

                return response;


            }
            catch (Exception ex)
            {
                return Utils.Utils.ReturnErrorResponse<QuaTrinhXuLyEticket>("Đã có lỗi xảy ra!", ex.Message);
            }
        }

        // Tạo mới ticket
        [HttpPost("create")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<Eticket>>> TaoMoiTicket([FromForm] IFormCollection form)
        {

            var response = new Response<Eticket>();

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
                var objectPost = JsonConvert.DeserializeObject<Eticket>(strJson);

                // Kiểm tra MaTrangThaiTicket
                if (string.IsNullOrEmpty(objectPost.MaTrangThaiTicket))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Mã trạng thái ticket không hợp lệ!", null);
                }


                // Kiểm tra TenTicket
                if (string.IsNullOrEmpty(objectPost.TenTicket))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Tên ticket không hợp lệ!", null);

                }

                // Kiểm tra MaKhachHang
                if (string.IsNullOrEmpty(objectPost.MaKhachHang))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Mã khách hàng không hợp lệ!", null);
                }

                // Kiểm tra MaPhanLoaiTicket
                if (string.IsNullOrEmpty(objectPost.MaPhanLoaiTicket))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Mã phân loại ticket không hợp lệ!", null);
                }

                // Check TicketDeadline
                if (objectPost.TicketDeadline == null)
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Ticket deadline không hợp lệ!", null);
                }

                // Kiểm tra NguoiTao
                if (string.IsNullOrEmpty(objectPost.NguoiTao))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Người tạo không hợp lệ!", null);
                }

                // Bắt đầu thêm mới ticket
                try
                {
                    var list = await _context.Eticket.FromSqlRaw("EXEC TaoMoiEticket @MaTrangThaiTicket, @TenTicket, @MaKhachHang, @ChiTiet, @MaPhanLoaiTicket, @TicketDeadline, @NguoiTao",
                        new SqlParameter("@MaTrangThaiTicket", objectPost.MaTrangThaiTicket),
                        new SqlParameter("@TenTicket", objectPost.TenTicket),
                        new SqlParameter("@MaKhachHang", objectPost.MaKhachHang),
                        new SqlParameter("@ChiTiet", objectPost.ChiTiet ?? ""),
                        new SqlParameter("@MaPhanLoaiTicket", objectPost.MaPhanLoaiTicket),
                        new SqlParameter("@TicketDeadline", objectPost.TicketDeadline),
                        new SqlParameter("@NguoiTao", objectPost.NguoiTao)
                    ).ToListAsync();

                    if (list.Count > 0)
                    {
                        response.Data = list[0];
                        response.Success = true;
                    }
                    else
                    {
                        response.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Error = ex.Message;
                    response.Data = null;
                }

                return response;

            }
            catch (Exception ex)
            {
                return Utils.Utils.ReturnErrorResponse<Eticket>("Đã có lỗi xảy ra!", ex.Message);
            }


        }

        // Cập nhật ticket
        [HttpPost("update")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<Eticket>>> CapNhatEticket([FromForm] IFormCollection form)
        {

            var response = new Response<Eticket>();

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
                var objectPost = JsonConvert.DeserializeObject<Eticket>(strJson);

                // Kiểm tra MaTicket
                if (string.IsNullOrEmpty(objectPost.MaTicket))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Mã ticket không hợp lệ!", null);
                }

                // Kiểm tra MaTrangThaiTicket
                if (string.IsNullOrEmpty(objectPost.MaTrangThaiTicket))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Mã trạng thái ticket không hợp lệ!", null);
                }


                // Kiểm tra TenTicket
                if (string.IsNullOrEmpty(objectPost.TenTicket))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Tên ticket không hợp lệ!", null);

                }

                // Kiểm tra MaKhachHang
                if (string.IsNullOrEmpty(objectPost.MaKhachHang))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Mã khách hàng không hợp lệ!", null);
                }

                // Kiểm tra MaPhanLoaiTicket
                if (string.IsNullOrEmpty(objectPost.MaPhanLoaiTicket))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Mã phân loại ticket không hợp lệ!", null);
                }

                // Check TicketDeadline
                if (objectPost.TicketDeadline == null)
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Ticket deadline không hợp lệ!", null);
                }

                // Kiểm tra NguoiCapNhat
                if (string.IsNullOrEmpty(objectPost.NguoiCapNhat))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Người cập nhật không hợp lệ!", null);
                }

                // Bắt đầu cập nhật ticket
                try
                {
                    var update = await _context.Database.ExecuteSqlRawAsync("EXEC CapNhatEticket @MaTicket, @MaTrangThaiTicket, @TenTicket, @MaKhachHang, @ChiTiet, @MaPhanLoaiTicket, @TicketDeadline, @NguoiCapNhat",
                        new SqlParameter("@MaTicket", objectPost.MaTicket),
                        new SqlParameter("@MaTrangThaiTicket", objectPost.MaTrangThaiTicket),
                        new SqlParameter("@TenTicket", objectPost.TenTicket),
                        new SqlParameter("@MaKhachHang", objectPost.MaKhachHang),
                        new SqlParameter("@ChiTiet", objectPost.ChiTiet),
                        new SqlParameter("@MaPhanLoaiTicket", objectPost.MaPhanLoaiTicket),
                        new SqlParameter("@TicketDeadline", objectPost.TicketDeadline),
                        new SqlParameter("@NguoiCapNhat", objectPost.NguoiCapNhat)
                    );

                    if (update > 0)
                    {
                        response.Success = true;
                    }
                    else
                    {
                        response.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Error = ex.Message;
                    response.Data = null;
                }

                return response;

            }
            catch (Exception ex)
            {
                return Utils.Utils.ReturnErrorResponse<Eticket>("Đã có lỗi xảy ra!", ex.Message);
            }


        }

        // Cập nhật trạng thái ticket
        [HttpPost("updateTrangThai")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<Eticket>>> CapNhatTrangThaiEticket([FromForm] IFormCollection form)
        {

            var response = new Response<Eticket>();

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
                var objectPost = JsonConvert.DeserializeObject<Eticket>(strJson);

                // Kiểm tra MaTicket
                if (string.IsNullOrEmpty(objectPost.MaTicket))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Mã ticket không hợp lệ!", null);
                }

                // Kiểm tra MaTrangThaiTicket
                if (string.IsNullOrEmpty(objectPost.MaTrangThaiTicket))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Mã trạng thái ticket không hợp lệ!", null);
                }

                // Kiểm tra NguoiCapNhat
                if (string.IsNullOrEmpty(objectPost.NguoiCapNhat))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Người cập nhật không hợp lệ!", null);
                }

                // Bắt đầu cập nhật trạng thái
                try
                {
                    var update = await _context.Database.ExecuteSqlRawAsync("EXEC CapNhatTrangThaiEticket @MaTicket, @MaTrangThaiTicket, @NguoiCapNhat",
                        new SqlParameter("@MaTicket", objectPost.MaTicket),
                        new SqlParameter("@MaTrangThaiTicket", objectPost.MaTrangThaiTicket),
                        new SqlParameter("@NguoiCapNhat", objectPost.NguoiCapNhat)
                    );

                    if (update > 0)
                    {
                        response.Success = true;
                    }
                    else
                    {
                        response.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Error = ex.Message;
                    response.Data = null;
                }

                return response;

            }
            catch (Exception ex)
            {
                return Utils.Utils.ReturnErrorResponse<Eticket>("Đã có lỗi xảy ra!", ex.Message);
            }


        }

        // Xóa ticket
        [HttpPost("delete")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<Eticket>>> XoaTicket([FromForm] IFormCollection form)
        {

            var response = new Response<Eticket>();

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
                var objectPost = JsonConvert.DeserializeObject<Eticket>(strJson);

                // Kiểm tra MaTicket
                if (string.IsNullOrEmpty(objectPost.MaTicket))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Mã ticket không hợp lệ!", null);
                }

                // Kiểm tra NguoiCapNhat
                if (string.IsNullOrEmpty(objectPost.NguoiCapNhat))
                {
                    return Utils.Utils.ReturnErrorResponse<Eticket>("Người cập nhật không hợp lệ!", null);
                }

                // Xóa ticket
                try
                {
                    var update = await _context.Database.ExecuteSqlRawAsync("EXEC XoaEticket @MaTicket, @NguoiCapNhat",
                        new SqlParameter("@MaTicket", objectPost.MaTicket),
                        new SqlParameter("@NguoiCapNhat", objectPost.NguoiCapNhat)
                    );

                    if (update > 0)
                    {
                        response.Success = true;
                    }
                    else
                    {
                        response.Success = false;
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Error = ex.Message;
                    response.Data = null;
                }

                return response;

            }
            catch (Exception ex)
            {
                return Utils.Utils.ReturnErrorResponse<Eticket>("Đã có lỗi xảy ra!", ex.Message);
            }


        }

        // Xóa quá trình xử lý
        [HttpPost("deleteProcess")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<QuaTrinhXuLyEticket>>> XoaQuaTrinhXuLy([FromForm] IFormCollection form)
        {

            var response = new Response<QuaTrinhXuLyEticket>();

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
                var objectPost = JsonConvert.DeserializeObject<QuaTrinhXuLyEticket>(strJson);

                // Kiểm tra MaXuLy

                int number;
                if (int.TryParse(objectPost.MaXuLy.ToString(), out number) == false)
                {
                    return Utils.Utils.ReturnErrorResponse<QuaTrinhXuLyEticket>("Mã xử lý không hợp lệ!", null);
                }

                // Kiểm tra MaTicket
                if (string.IsNullOrEmpty(objectPost.MaTicket))
                {
                    return Utils.Utils.ReturnErrorResponse<QuaTrinhXuLyEticket>("Mã ticket không hợp lệ!", null);
                }

                // Xóa ticket
                try
                {
                    var del = await _context.Database.ExecuteSqlRawAsync("EXEC XoaQTXL @MaXuLy, @MaTicket",
                        new SqlParameter("@MaXuLy", objectPost.MaXuLy),
                        new SqlParameter("@MaTicket", objectPost.MaTicket)
                    );


                    if (del > 0)
                    {
                        response.Success = true;

                        var list = await _context.QuaTrinhXuLyEticket.FromSqlRaw("EXEC TimKiemThongTinQTXL @MaTicket",
                                                                          new SqlParameter("@MaTicket", objectPost.MaTicket)).ToListAsync();

                        response.DataList = list.ToList();
                    }
                    else
                    {
                        response.Success = false;
                    }

                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Error = ex.Message;
                    response.Data = null;
                }

                return response;

            }
            catch (Exception ex)
            {
                return Utils.Utils.ReturnErrorResponse<QuaTrinhXuLyEticket>("Đã có lỗi xảy ra!", ex.Message);
            }
        }


        // Báo cáo KPI Eticket
        [HttpPost("reportKPI")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<ActionResult<Response<BaoCaoKPIEticket>>> BaoCaoKPI([FromForm] IFormCollection form)
        {

            var response = new Response<BaoCaoKPIEticket>();

            Microsoft.Extensions.Primitives.StringValues nguoiTaoValues;
            Microsoft.Extensions.Primitives.StringValues maPhanLoaiTicketValues;
            Microsoft.Extensions.Primitives.StringValues thoiGianTaoTuValues;
            Microsoft.Extensions.Primitives.StringValues thoiGianTaoDenValues;


            string nguoiTao = "";
            string maPhanLoaiTicket = "";
            string? thoiGianTaoTu = "" == "" ? null : "";
            string? thoiGianTaoDen = "" == "" ? null : "";

            form.TryGetValue("NguoiTao", out nguoiTaoValues);
            if (nguoiTaoValues.Count > 0)
            {
                nguoiTao = nguoiTaoValues[0] ?? "";
            }

            form.TryGetValue("MaPhanLoaiTicket", out maPhanLoaiTicketValues);
            if (maPhanLoaiTicketValues.Count > 0)
            {
                maPhanLoaiTicket = maPhanLoaiTicketValues[0] ?? "";
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
                    return Utils.Utils.ReturnErrorResponse<BaoCaoKPIEticket>("Thời gian tạo từ không hợp lệ!", null);
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
                    return Utils.Utils.ReturnErrorResponse<BaoCaoKPIEticket>("Thời gian tạo đến không hợp lệ!", null);
                }
            }


            try
            {
                var list = await _context.BaoCaoKPIEticket.FromSqlRaw("EXEC BaoCaoKPI @NguoiTao, @MaPhanLoaiTicket, @ThoiGianTaoTu, @ThoiGianTaoDen",
                        new SqlParameter("@NguoiTao", nguoiTao),
                        new SqlParameter("@MaPhanLoaiTicket", maPhanLoaiTicket),
                        new SqlParameter("@ThoiGianTaoTu", string.IsNullOrEmpty(thoiGianTaoTu) ? (object)DBNull.Value : thoiGianTaoTu),
                        new SqlParameter("@ThoiGianTaoDen", string.IsNullOrEmpty(thoiGianTaoDen) ? (object)DBNull.Value : thoiGianTaoDen)
                        ).ToListAsync();

                response.DataList = list;
                response.Count = list.Count;
                response.Error = null;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                response.Success = false;
                return new Response<BaoCaoKPIEticket>
                {
                    DataList = new List<BaoCaoKPIEticket>(),
                    Count = 0,
                    Error = ex.Message,
                    Success = false
                };
            }

            return response;

        }
    }
}
