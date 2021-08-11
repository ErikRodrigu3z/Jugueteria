using Jugueteria.Models.Segurity;
using Jugueteria.Service.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jugueteria.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IIdentityRepo _identity;
        public UserController(Serilog.ILogger logger, IIdentityRepo identity)
        {
            _logger = logger;
            _identity = identity;
        }
               


        // GET: api/<UserController>
        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var model = await _identity.GetUsersAsync();
                if (model.Count() > 0)
                {
                    return Ok(model);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, HttpContext.Request.Path);
                return BadRequest(ex);
            }
        }

        // GET api/<UserController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var model = await _identity.GetUserByIdAsync(id);
                if (model != null)
                {
                    return Ok(model);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, HttpContext.Request.Path);
                return BadRequest(ex);
            }
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Users model)
        {
            try
            {
                var result = await _identity.RegisterAsync(model);
                if (result.Succeeded)
                {
                    return Ok("Registro guardado");
                }
                else
                {
                    return BadRequest(result.Errors);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, HttpContext.Request.Path);
                return BadRequest(ex);
            }
        }

        // POST api/<UserController>
        [HttpGet("{email}/{password}")]
        public async Task<IActionResult> Get(string email, string password)
        {
            try
            {
                var user = await _identity.GetUserByEmailAsync(email);
                if (user != null)
                {
                    var result = await _identity.SignInJwtTokenAsync(user, password);
                    return Ok(result);
                }
                else
                {
                    return NotFound("");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, HttpContext.Request.Path);
                return BadRequest(ex);
            }
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Users user)
        {
            try
            {
                var userToUpdate = await _identity.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"No se encontro usuario con id: {id}");
                }
                userToUpdate.Email = user.Email;
                userToUpdate.UserName = user.Email;
                userToUpdate.Name = user.Name;
                userToUpdate.LastName = user.LastName;
                userToUpdate.PhoneNumber = user.PhoneNumber;
                userToUpdate.Active = user.Active;

                var result = await _identity.UpdateAsync(userToUpdate);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, HttpContext.Request.Path);
                return BadRequest(ex);
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _identity.DeleteUserAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, HttpContext.Request.Path);
                return BadRequest(ex);
            }
        }
    }
}
