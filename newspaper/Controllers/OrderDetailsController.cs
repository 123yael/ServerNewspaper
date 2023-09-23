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

        [HttpGet("GetOrderDetailsByDate/{date}")]
        public async Task<IActionResult> GetOrderDetailsByDate([FromRoute] string date, [FromQuery] PaginationParams @params)
        {

            DateTime dateTime;
            DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);

            var orderDetails = _funcs.GetAllReleventOrdersDTO(dateTime);

            var paginationMetadata = new PaginationMetadata(orderDetails.Count(), @params.Page, @params.ItemsPerPage);

            var items = orderDetails.Skip((@params.Page - 1) * @params.ItemsPerPage)
                .Take(@params.ItemsPerPage);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(new { List = items, PaginationMetadata = paginationMetadata });

        }
    }
}
