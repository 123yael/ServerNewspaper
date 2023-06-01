using BLL.functions;
using DTO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IFuncs _funcs;

        public CustomerController(IFuncs funcs)
        {
            this._funcs = funcs;
        }

        [HttpPost("GetIdByCustomer")]
        public IActionResult GetIdByCustomer([FromBody] CustomerDTO cust)
        {
            return Ok(_funcs.GetIdByCustomer(cust));
        }

        [HttpGet("IsCustomerExists/{email}/{pass}")]
        public IActionResult IsCustomerExists(string email, string pass)
        {
            return Ok(_funcs.IsCustomerExists(email, pass));
        }
    }
}
