using BLL.Exceptions;
using BLL.Functions;
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

        [HttpGet("LogIn/{email}/{pass}")]
        public IActionResult LogIn(string email, string pass)
        {
            try
            {
                CustomerDTO customerDTO = _funcs.LogIn(email, pass);
                return Ok(customerDTO);
            }
            catch (UserNotFoundException)
            {
                return StatusCode(404);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] CustomerDTO cust)
        {
            try
            {
                CustomerDTO customerDTO = _funcs.SignUp(cust);
                return Ok(customerDTO);
            }
            catch (UserAlreadyExistsException)
            {
                return StatusCode(409);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
