using BLL.functions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using A = DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;



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

        [HttpGet("FirstWord")]
        public IActionResult FirstWord()
        {
            string f = myPath + "myFile1.docx";
            string[] strs = { "Title 1", "Hi my name is Yael snd I live in Rabi Tzadok 10 Dira 1", "Title 2", "Hi my name is Yael snd I live in Rabi Tzadok 10 Dira 1","I just want to see if you realy can to right it like a man.", "Title 3", "Hi my name is Yael snd I live in Rabi Tzadok 10 Dira 1", };
            _funcs.FirstWord(f, strs);
            return Ok("FirstWord is finish");
        }

        [HttpGet("ConvertWordPDF")]
        public IActionResult ConvertWordPDF()
        {
            _funcs.convertWordPFD(myPath + "myFile1.docx", myPath + "MyFile.pdf");
            return Ok("Love");
        }

<<<<<<< HEAD
        [HttpGet("Shabetz")]
        public IActionResult Shabetz()
        {
            _funcs.Shabets();
            return Ok();
        }

=======

        [HttpGet("ConvertPdfToWord")]
        public IActionResult ConvertPdfToWord()
        {
            string pdfFilePath = _environment.WebRootPath + "\\NewspapersPdf\\finalNewspaper2.pdf";
            string wordFilePath = _environment.WebRootPath + "\\TempWord\\finalNewspaper2.docx";
            _funcs.ConvertPdfToWord(pdfFilePath, wordFilePath);
            return Ok("hello");
        }
>>>>>>> b3ff3dd7ea6f5569d3e0151668b2107c679663a0
    }
}
