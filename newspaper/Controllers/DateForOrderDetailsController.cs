using BLL.Functions;
using DAL.Models;
using DTO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DateForOrderDetailsController : ControllerBase
    {
        private readonly IFuncs _funcs;

        public DateForOrderDetailsController(IFuncs funcs)
        {
            this._funcs = funcs;
        }

        private DateTime ConvertDateFromStringToDateTime(string date)
        {
            DateTime dateTime;
            DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            return dateTime;
        }

        [HttpPut("UpdateStatus/{id}/{status}/{date}")]
        public async Task<IActionResult> UpdateStatus(int id, bool status, [FromRoute] string date, [FromQuery] PaginationParams @params)
        {
            _funcs.UpdateStatus(id, status);

            DateTime dateTime = ConvertDateFromStringToDateTime(date);

            return Ok(_funcs.GetAllOrderDetailsTableByDate(dateTime, @params.Page, @params.ItemsPerPage));
        }

        [HttpPut("UpdateStatusWords/{id}/{status}/{date}")]
        public async Task<IActionResult> UpdateStatusWords(int id, bool status, [FromRoute] string date, [FromQuery] PaginationParams @params)
        {
            _funcs.UpdateStatus(id, status);

            DateTime dateTime = ConvertDateFromStringToDateTime(date);

            return Ok(_funcs.GetAllDetailsWordsTableByDate(dateTime, @params.Page, @params.ItemsPerPage));
        }
    }
}
