using MessengerData.Providers;
using MessengerModel.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewUserController : Controller
    {
        private UserProvider _provider;
        public NewUserController(UserProvider provider)
        {
            _provider = provider;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewUserDTO newUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _provider.CreateUserAsync(newUserDTO);
            if (result.Result)
            {
                return StatusCode(210, result.Entity);
            }
            else
            {
                return StatusCode(500, result.ErrorMessage);
            }
        }
    }
}
