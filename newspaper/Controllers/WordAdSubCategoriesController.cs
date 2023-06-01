using BLL.functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordAdSubCategoriesController : ControllerBase
    {
        private readonly IFuncs _funcs;

        public WordAdSubCategoriesController(IFuncs funcs)
        {
            this._funcs = funcs;
        }

        [HttpGet("GetAllWordAdSubCategories")]
        public IActionResult GetAllWordAdSubCategories()
        {
            return Ok(_funcs.GetAllWordAdSubCategories());
        }
    }
}
