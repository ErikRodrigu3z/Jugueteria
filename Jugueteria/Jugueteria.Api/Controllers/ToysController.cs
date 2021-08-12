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
                    //descomentar esta linea para agregar 4 registros o comentar para que no agrege registros al refrescar
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

        [HttpDelete] 
        public async Task<IActionResult> Delete(int id)  
        {
            try 
            {
                await _toysRepo.DeleteToy(id);               
                return Ok();                
            }
            catch (Exception ex)
            {
                _logger.Error(ex, HttpContext.Request.Path);
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Toys toys) 
        {
            try
            {
                int id = await _toysRepo.AddToy(toys);
                //actualiza la imagen con el scope identity
                toys.Img = $"/img/toys/{toys.Id}.jpg";
                await _toysRepo.UpdateAsync(toys);
                return Ok(id); 
            }
            catch (Exception ex)
            {
                _logger.Error(ex, HttpContext.Request.Path);
                return BadRequest(ex);
            }
        }

        [HttpPut] 
        public async Task<IActionResult> Put([FromBody]Toys toys)
        {
            try
            {
                toys.Img = $"/img/toys/{toys.Id}.jpg";
                await _toysRepo.UpdateAsync(toys);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, HttpContext.Request.Path);
                return BadRequest(ex);
            }
        }






    }
}
