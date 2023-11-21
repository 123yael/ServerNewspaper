using BLL.Functions;
using DTO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IFuncs _funcs;
        public static IWebHostEnvironment _environment;

        public OrderController(IFuncs funcs, IWebHostEnvironment environment)
        {
            _funcs = funcs;
            _environment = environment;

        }

        [HttpPost("FinishOrder/{token}")]
        public IActionResult FinishOrder(string token, [FromBody] FinishOrderDTO finishOrder)
        {
            _funcs.FinishOrder(token, finishOrder.ListDates, finishOrder.ListOrderDetails);
            return Ok("finishOrder");
        }

        [HttpPost("FinishOrderAdWords/{token}")]
        public IActionResult FinishOrderAdWords(string token, [FromBody] FinishOrderDTO finishOrder)
        {
            _funcs.FinishOrderAdWords(token, finishOrder.ListDates, finishOrder.ListOrderDetails);
            return Ok("finishOrder");
        }

        [HttpPost("CalculationOfOrderPrice")]
        public IActionResult CalculationOfOrderPrice([FromBody] List<OrderDetailDTO> listOrderDetails)
        {
            return Ok(_funcs.CalculationOfOrderPrice(listOrderDetails));
        }

        [HttpPost("CalculationOfOrderWordsPrice")]
        public IActionResult CalculationOfOrderWordsPrice([FromBody] List<OrderDetailDTO> listOrderDetails)
        {
            return Ok(_funcs.CalculationOfOrderWordsPrice(listOrderDetails));
        }
    }
}
