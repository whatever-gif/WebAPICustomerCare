using DATN.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {

        private readonly IWebHostEnvironment _hostingEnvironment;

        public UploadController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        //...
        //string fileUrl = Path.Combine(_hostingEnvironment.WebRootPath, "UploadedFiles", file.FileName);


        [HttpPost]
        public async Task<ActionResult<Response<UploadFile>>> UploadFile(IFormFile file)
        {
            var response = new Response<UploadFile>();

            if (file == null || file.Length == 0)
            {
                response.Error = "Không có file nào được chọn!";
                return response;
            }

            // Remove all spaces from the filename
            var fileName = file.FileName.Replace(" ", "");

            var path = Path.Combine(
                        _hostingEnvironment.WebRootPath, "UploadedFiles");

            // Create the directory if it does not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = Path.Combine(path, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            response.Success = true;

            var request = Request;
            var fileUrl = $"{request.Scheme}://{request.Host}/UploadedFiles/{fileName}";

            response.Data = new UploadFile { FileName = fileName, FilePath = fileUrl };

            return response;
        }



    }
}
