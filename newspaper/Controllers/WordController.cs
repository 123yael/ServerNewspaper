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


        [HttpGet("ConvertWordPDF")]
        public IActionResult ConvertWordPDF()
        {
            _funcs.ConvertFromWordToPdf(myPath + "myFile1.docx", myPath + "MyFile.pdf");
            return Ok("Love");
        }


        [HttpGet("Shabetz")]
        public IActionResult Shabetz()
        {
            string pdfFilePath = _environment.WebRootPath + "\\NewspapersPdf\\pdfTemplate.pdf";
            _funcs.Shabets(pdfFilePath);
            return Ok();
        }



        [HttpGet("ConvertPdfToWord")]
        public IActionResult ConvertPdfToWord()
        {
            string pdfFilePath = _environment.WebRootPath + "\\NewspapersPdf\\finalNewspaper2.pdf";
            string wordFilePath = _environment.WebRootPath + "\\TempWord\\finalNewspaper2.docx";
            _funcs.ConvertPdfToWord(pdfFilePath, wordFilePath);
            return Ok("hello");
        }

        [HttpGet("CompleteWordTemplate")]
        public IActionResult CompleteWordTemplate()
        {
            string wordFilePath = _environment.WebRootPath + @"\TempWord\wordsLeft.dotx";
            _funcs.CompleteWordTemplate(wordFilePath, _environment.WebRootPath + @"\TempWord");
            return Ok("i finish to replace");
        }


        [HttpGet("CreateWordAd")]
        public IActionResult CreateWordAd()
        {
            string wordFilePath = _environment.WebRootPath + @"\TempWord\wordsTemplate.dotx";
            _funcs.CreateWordAd(wordFilePath, _environment.WebRootPath + @"\TempWord");
            return Ok("i finish to replace CreateWordAd");
        }

    }
}
