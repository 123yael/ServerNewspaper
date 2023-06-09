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
            string[] cc = { "Title 1", "1. Hi my name is Yael and I leave in Rabi Tadok 10 Dira 1.", "Title 2", "2. Hi my name is Yael and I leave in Rabi Tadok 10 Dira 1.","Title 3", "3. Hi my name is Yael and I leave in Rabi Tadok 10 Dira 1." };

            _funcs.FirstWord(f, cc);
            return Ok("FirstWord is finish");
        }
    }
}
