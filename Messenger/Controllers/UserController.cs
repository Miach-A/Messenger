using MessengerData.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MessengerData.Extensions;
using MessengerModel.UserModels;
using Microsoft.AspNetCore.Authorization;

namespace Messenger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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


            Func<IQueryable<User>, IOrderedQueryable<User>>? order = 
                orderby == null 
                ? null 
                : (x) => x.OrderBy(orderby);

            return Ok(await _provider.GetRepository().Get(filter, order).ToArrayAsync());
        }
        public async Task<IActionResult> Post([FromBody] NewUserDTO newUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _provider.CreateUserAsync(newUserDTO);
            if (result.Result)
            {
                return StatusCode(201);
            }
            else
            {
                return StatusCode(500, result.ErrorMessage);
            }
        }

        [HttpPut("{guid}")]
        public async Task<IActionResult> Put(Guid guid, [FromBody] NewUserDTO newUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _provider.CreateUserAsync(newUserDTO);
            if (result.Result)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500, result.ErrorMessage);
            }

        }

    }
}
