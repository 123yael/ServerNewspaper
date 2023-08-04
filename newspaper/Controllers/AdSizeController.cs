using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using BLL.Functions;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdSizeController : ControllerBase
    {

        private readonly IFuncs _funcs;

        public AdSizeController(IFuncs funcs)
        {
            this._funcs = funcs;
        }

        [HttpGet("GetAllAdSize")]
        public IActionResult GetAllAdSize()
        {
            return Ok(_funcs.GetAllAdSize());
        }

    }
}
