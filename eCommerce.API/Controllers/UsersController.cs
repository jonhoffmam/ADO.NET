using eCommerce.API.Models;
using eCommerce.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_userRepository.Get());
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            User user = _userRepository.Get(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] User user)
        {
            _userRepository.Insert(user);
            return Created("", user);
        }

        [HttpPut]
        public IActionResult Update([FromBody] User user)
        {
            _userRepository.Update(user);
            return Ok(user);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            _userRepository.Delete(id);
            return NoContent();
        }
    }
}
