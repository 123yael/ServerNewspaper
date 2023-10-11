using BLL.Functions;
using DAL.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using DTO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Text.Json;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly IFuncs _funcs;

        public OrderDetailsController(IFuncs funcs)
        {
            this._funcs = funcs; 
        }

        [HttpGet("GetAllOrderDetails")]
        public IActionResult GetAllOrderDetails()
        {
            return Ok(_funcs.GetAllOrderDetails());
        }

    }
}
