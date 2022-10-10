using MessengerData.Providers;
using MessengerData.Repository;
using MessengerModel;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserProvider _provider;
        public UserController(UserProvider provider)
        {
            _provider = provider;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _provider.Test();
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{guid}")]
        public async Task<IActionResult> Get(Guid guid)
        {
            await Task.CompletedTask;
            return Ok();
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
