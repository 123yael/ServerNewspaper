using BLL.Exceptions;
using BLL.Functions;
using BLL.Jwt;
using DocumentFormat.OpenXml.Spreadsheet;
using DTO.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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

        [HttpGet("LogIn/{email}/{pass}")]
        public IActionResult LogIn(string email, string pass)
        {
            try
            {
                string token = _funcs.LogIn(email, pass);
                return Ok(token);
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

        [HttpPost("SignUp/{isRegistered}")]
        public IActionResult SignUp([FromBody] CustomerDTO cust, bool isRegistered)
        {
            try
            {
                string token = _funcs.SignUp(cust, isRegistered);
                return Ok(token);
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

        [HttpGet("IsAdmin/{token}")]
        public IActionResult IsAdmin(string token)
        {

            bool isAdmin = _funcs.IsAdmin(token);
            return Ok(isAdmin);
        }
    }
}
