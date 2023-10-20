using BLL.Functions;
using DTO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {

        private readonly IFuncs _funcs;

        public TableController(IFuncs funcs)
        {
            this._funcs = funcs;
        }

        private DateTime ConvertDateFromStringToDateTime(string date)
        {
            DateTime dateTime;
            DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            return dateTime;
        }

        [HttpGet("GetAllOrderDetailsTableByDate/{date}")]
        public async Task<IActionResult> GetAllOrderDetailsTableByDate([FromRoute] string date, [FromQuery] PaginationParams @params)
        {
            DateTime dateTime = ConvertDateFromStringToDateTime(date);
            var res = _funcs.GetAllOrderDetailsTableByDate(dateTime, @params.Page, @params.ItemsPerPage);
            return Ok(res);
        }

        [HttpGet("GetAllDetailsWordsTableByDate/{date}")]
        public async Task<IActionResult> GetAllDetailsWordsTableByDate([FromRoute] string date, [FromQuery] PaginationParams @params)
        {
            DateTime dateTime = ConvertDateFromStringToDateTime(date);
            return Ok(_funcs.GetAllDetailsWordsTableByDate(dateTime, @params.Page, @params.ItemsPerPage));
        }

    }
}
