using files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
namespace files.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageUploadController : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        public ImageUploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }


        // פונקציה להעלאת תמונה לשרת
        [HttpPost("uploadImage")]
        public IActionResult UploadImage(IFormFile image)
        {
            if (image.Length > 0)
            {
                if (!Directory.Exists(_environment.WebRootPath + "\\Upload"))
                    Directory.CreateDirectory(_environment.WebRootPath + "\\Upload\\");
                using (FileStream filestream = System.IO.File.Create(_environment.WebRootPath + "\\Upload\\" + image.FileName))
                {
                    image.CopyTo(filestream);
                    filestream.Flush();
                }
            }
            return Ok("the image is upload!!!");
        }
    }
}