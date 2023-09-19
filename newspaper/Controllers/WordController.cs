using BLL.Functions;
using Microsoft.AspNetCore.Mvc;


namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IFuncs _funcs;
        public static IWebHostEnvironment _environment;

        public WordController(IWebHostEnvironment environment, IFuncs funcs)
        {
            this._funcs = funcs;
            _environment = environment;
        }

        private string myPath = "C:\\Users\\YAEL\\OneDrive\\שולחן העבודה\\";


        [HttpGet("Shabetz")]
        public IActionResult Shabetz()
        {
            string pdfFilePath = _environment.WebRootPath + "\\NewspapersPdf\\pdfTemplate.pdf";
            _funcs.Shabets(pdfFilePath);
            return Ok();
        }

    }
}
