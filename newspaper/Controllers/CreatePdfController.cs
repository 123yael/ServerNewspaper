using BLL.functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatePdfController : ControllerBase
    {
        private readonly IFuncs _funcs;

        public CreatePdfController(IFuncs funcs)
        {
            this._funcs = funcs;
        }

        // הדפסת המילים שלום עולם
        [HttpGet("AddAdFileToPdf")]
        public IActionResult AddAdFileToPdf()
        {
            _funcs.AddAdFileToPdf();
            return Ok("finish!!!");
        }

        // הקובץ עם התמונות של הכדורי שוקולד
        [HttpGet("AddAdFileToPdf2")]
        public IActionResult AddAdFileToPdf2()
        {
            string pathPDF = "C:\\Users\\YAEL\\OneDrive\\שולחן העבודה\\pdfpic.pdf";
            string pathPIC = "C:\\Users\\YAEL\\OneDrive\\תמונות\\כככ.jpg";
            _funcs.GeneratePDF(pathPDF, pathPIC);
            return Ok("finish!!!");
        }

        // יצירת הקובץ של כל גדלי הדפים
        [HttpGet("AddAdFileToPdf3")]
        public IActionResult AddAdFileToPdf3()
        {
            _funcs.AddAdFileToPdf3();
            return Ok("finish!!!");
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            _funcs.Create("C:\\Users\\YAEL\\OneDrive\\שולחן העבודה\\finalNewspaper2.pdf");
            return Ok("finish!!!");
        }

        // הפונקציה של כתיבה נורמלית ל pdf
        [HttpGet("WriteToPdf")]
        public IActionResult WriteToPdf()
        {
            _funcs.WriteToPdf("C:\\Users\\YAEL\\OneDrive\\שולחן העבודה\\words.pdf");
            return Ok("finish!!!");
        }
    }
}
