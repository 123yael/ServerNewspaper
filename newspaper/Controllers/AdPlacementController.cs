using BLL.functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdPlacementController : ControllerBase
    {
        private readonly IFuncs _funcs;

        public AdPlacementController(IFuncs funcs)
        {
            this._funcs = funcs;
        }

        [HttpGet("GetAllAdPlacement")]
        public IActionResult GetAllAdPlacement()
        {
            return Ok(_funcs.GetAllAdPlacement());
        }

        [HttpGet("IsDisablePlacment")]
        public IActionResult IsDisablePlacment()
        {
            return Ok(_funcs.IsDisablePlacment());
        }
    }
}
