﻿using BLL.Exceptions;
using BLL.Functions;
using DTO.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
        public IActionResult Shabetz(string date)
        {
            DateTime dateTime;
            DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
            NewspapersPublishedDTO newspapersPublishedDTO = _funcs.Shabets(dateTime);
            return Ok(newspapersPublishedDTO);
        }

        [HttpGet("ClosingNewspaper/{date}/{countPages}")]
        public IActionResult ClosingNewspaper(string date, int countPages)
        {
            try
            {
                DateTime dateTime;
                DateTime.TryParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                _funcs.ClosingNewspaper(dateTime, countPages);
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
