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

        [HttpPost("FinishOrder")]
        public IActionResult FinishOrder([FromBody] FinishOrderDTO finishOrder)
        {
            _funcs.FinishOrder(finishOrder.Customer, finishOrder.ListDates, finishOrder.ListOrderDetails);
            return Ok("finishOrder");
        }

        [HttpPost("FinishOrderAdWords")]
        public IActionResult FinishOrderAdWords([FromBody] FinishOrderDTO finishOrder)
        {
            _funcs.FinishOrderAdWords(finishOrder.Customer, finishOrder.ListDates, finishOrder.ListOrderDetails);
            return Ok("finishOrder");
        }
    }
}
