﻿using BLL.Functions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IFuncs _funcs;

        public EmailController(IFuncs funcs)
        {
            this._funcs = funcs;
        }

        [HttpGet("SentEmail/{name}/{email}/{message}/{subject}")]
        public IActionResult SentEmail(string name, string email, string message, string subject)
        {
            string t = _funcs.SentEmail(name, email, message, subject);
            return Ok(t);
        }
    }
}