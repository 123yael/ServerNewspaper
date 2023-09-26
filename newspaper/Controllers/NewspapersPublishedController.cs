using BLL.Functions;
using DTO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

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

        [HttpGet("GetAllNewspapersPublished/{sheet}/{date}")]
        public async Task<IActionResult> GetAllNewspapersPublished([FromRoute] string sheet, [FromRoute] string date, [FromQuery] PaginationParams @params)
        {
            var newspapersPublishedDTOs = _funcs.GetAllNewspapersPublished();
            if(sheet != "0")
                newspapersPublishedDTOs = newspapersPublishedDTOs.Where(n => n.NewspaperId.ToString().StartsWith(sheet)).ToList();
            if(date != "0")
                newspapersPublishedDTOs = newspapersPublishedDTOs.Where(n => n.PublicationDate.StartsWith(date)).ToList();

            var paginationMetadata = new PaginationMetadata(newspapersPublishedDTOs.Count(), @params.Page, @params.ItemsPerPage);

            var items = newspapersPublishedDTOs.Skip((@params.Page - 1) * @params.ItemsPerPage)
                .Take(@params.ItemsPerPage);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(new { list = items, paginationMetadata = paginationMetadata });
        }

    }
}
