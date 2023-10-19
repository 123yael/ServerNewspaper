using BLL.Functions;
using BLL.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly ICacheRedis _cacheRedis;

        public RedisController(ICacheRedis cacheRedis)
        {
            this._cacheRedis = cacheRedis;
        }

        [HttpPost("SetValue/{key}/{value}")]
        public IActionResult SetValue(string key, bool value)
        {
            Item item = new Item()
            {
                Email = key,
                IsRegistered = value,
            };
            _cacheRedis.SetItem(item);
            return Ok("Ok");
        }

        [HttpGet("GetAllEmails")]
        public IActionResult GetAllEmails()
        {          
            return Ok(_cacheRedis.GetAllItems());
        }

    }
}
