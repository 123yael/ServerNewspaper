
using Microsoft.AspNetCore.Mvc;

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