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


        //בכל מקרה שמתמשים בפונקציה הזאת בתוך הפונקציה של השיבוץ

        //[HttpGet("Create")]
        //public IActionResult Create()
        //{
        //    _funcs.Create("C:\\Users\\YAEL\\OneDrive\\שולחן העבודה\\finalNewspaper2.pdf");
        //    return Ok("finish!!!");
        //}

    }
}
