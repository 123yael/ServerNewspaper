using BLL.Functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewspapersPublishedController : ControllerBase
    {

        private readonly IFuncs _funcs;

        public NewspapersPublishedController(IFuncs funcs)
        {
            this._funcs = funcs;
        }

        [HttpGet("GetAllNewspapersPublished")]
        public IActionResult GetAllNewspapersPublished()
        {
            return Ok(_funcs.GetAllNewspapersPublished());
        }
    }
}
