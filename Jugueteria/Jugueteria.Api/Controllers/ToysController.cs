using Jugueteria.Models;
using Jugueteria.Service.Repositories.ToysRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jugueteria.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToysController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IToysRepository _toysRepo;

        public ToysController(Serilog.ILogger logger, IToysRepository toysRepo)
        {
            _logger = logger;
            _toysRepo = toysRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var model = await _toysRepo.GetListAsync();
                if (model.Count() > 0)
                {
                    return Ok(model);
                }
                else
                {
                    _toysRepo.SeedData();
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, HttpContext.Request.Path);
                return BadRequest(ex);
            }
        }






      




    }
}
