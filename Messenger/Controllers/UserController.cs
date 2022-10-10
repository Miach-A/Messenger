using MessengerData.Providers;
using MessengerData.Repository;
using MessengerModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        [HttpGet("{guid:guid}")]
        public async Task<IActionResult> Get(Guid guid)
        {
            return Ok(await _provider.GetRepository().Get(x => x.Guid == guid).ToArrayAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Get(string? name, string? firstname, string? lastname, string? phonenumber,string? orderby, int pageindex = 0, int pagesize = 20)
        {
 
            Expression<Func<User, bool>> filter = (x) =>      
                name == null ? true : x.Name.Contains(name)
                && firstname == null ? true : x.FirstName.Contains(firstname!)
                && lastname == null ? true : x.LastName.Contains(lastname!)
                && phonenumber == null ? true : x.PhoneNumber.Contains(phonenumber!);
            
            return Ok(await _provider.GetRepository().Get(filter).ToArrayAsync());
        }

        [HttpPost]
        public Task<IActionResult> Post()
        {

            return Ok();
        }

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
