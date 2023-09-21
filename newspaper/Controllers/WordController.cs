using BLL.Exceptions;
using BLL.Functions;
using DTO.Repository;
using Microsoft.AspNetCore.Mvc;


namespace newspaper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IFuncs _funcs;
        public static IWebHostEnvironment _environment;

        public WordController(IWebHostEnvironment environment, IFuncs funcs)
        {
            this._funcs = funcs;
            _environment = environment;
        }

        [HttpGet("Shabetz/{date}")]
        public IActionResult Shabetz(DateTime date)
        {
            NewspapersPublishedDTO newspapersPublishedDTO = _funcs.Shabets(date);
            return Ok(newspapersPublishedDTO);
        }

        [HttpGet("ClosingNewspaper/{date}/{countPages}")]
        public IActionResult ClosingNewspaper(DateTime date, int countPages)
        {
            try
            {
                _funcs.ClosingNewspaper(date, countPages);
                return Ok();
            }
            catch (DateAlreadyExistsException)
            {
                return StatusCode(409);
            }
            catch (NewspaperNotGeneratedException)
            {
                return StatusCode(408);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

    }
}
