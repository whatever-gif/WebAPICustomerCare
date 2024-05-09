using DATN.Models;
using System.Text.RegularExpressions;

namespace DATN.Utils
{
    public static class Utils
    {

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            //var phoneRegex = new Regex(@"^0[35789]\d{8}$");
            var phoneRegex = new Regex(@"^\d+$");
            return phoneRegex.IsMatch(phoneNumber);
        }
        public static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$");
            return emailRegex.IsMatch(email);
        }
        public static bool IsValidTrangThai(string trangThai)
        {
            var trangThaiRegex = new Regex(@"^(1|0)$");
            return trangThaiRegex.IsMatch(trangThai);
        }

        
        // Hàm trả dữ liệu khi lỗi

        public static Response<T> ReturnErrorResponse<T>(string error, string? errorDetail)
        {
            return new Response<T>
            {
                Error = error,
                ErrorDetail = errorDetail,
            };
        }
    }
}
