using BLL.functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IFuncs _funcs;

        public WordController(IFuncs funcs)
        {
            this._funcs = funcs;
        }

        [HttpGet("FirstWord")]
        public IActionResult FirstWord()
        {
            string f = "C:\\Users\\שירה בוריה\\Desktop\\myFile1.docx";
            string[] strs = { "Title 1", "Hi my name is Yael snd I live in Rabi Tzadok 10 Dira 1", "Title 2", "Hi my name is Yael snd I live in Rabi Tzadok 10 Dira 1","I just want to see if you realy can to right it like a man.", "Title 3", "Hi my name is Yael snd I live in Rabi Tzadok 10 Dira 1", };
            _funcs.FirstWord(f, strs);
            return Ok("FirstWord is finish");
        }

        [HttpGet("ConvertWordPDF")]
        public IActionResult ConvertWordPDF()
        {
            _funcs.convertWordPFD("C:\\Users\\שירה בוריה\\Desktop\\myFile1.docx", "C:\\Users\\שירה בוריה\\Desktop\\MyFile.pdf");
            return Ok("Love");
        }
    }
}
