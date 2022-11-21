using MessengerData.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MessengerData.Extensions;
using MessengerModel.UserModels;
using Microsoft.AspNetCore.Authorization;
using MessengerData;
using System.Xml.Linq;
using System.IO.Compression;

namespace Messenger.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserProvider _provider;
        
        public UserController(ApplicationDbContext context, UserProvider provider)
        {
            _provider = provider;
            _context = context;
        }

        [HttpGet("~/api/test")]
        public IActionResult GetTest()
        {
            //var user = _provider.GetRepository().Get(null, null,
            //    x => x
            //        .Include(y => y.Contacts).ThenInclude(y => y.Contact)
            //        .Include(y => y.IAsContact).ThenInclude(y => y.User))
            //    .ToArray();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (!_provider.GetUserGuid(User, out var userGuid))
            {
                return StatusCode(500);
            }

            User? user = await _context.Users
                .Include(x => x.UserChats).ThenInclude(x => x.Chat).ThenInclude(x => x.ChatUsers).ThenInclude(x => x.User)
                .Include(x => x.Contacts).ThenInclude(x => x.Contact)
                .FirstOrDefaultAsync(x => x.Guid == userGuid);

            if (user == null)
            {
                return StatusCode(500);
            }

            return Ok(_provider.ToUserDTO(user));
        }

        [Authorize]
        [HttpGet("~/api/GetUsers")]
        [ActionName("GetUsers")]
        public async Task<IActionResult> GetUsers(string name, UserOrderBy orderby = UserOrderBy.Name, int pageindex = 0, int pagesize = 20)
        {
            var nameSearch = name.ToLower();
            Expression<Func<User, bool>> filter = (x) =>
                x.Name.Contains(nameSearch)
                || x.FirstName.ToLower().Contains(nameSearch)
                || x.LastName.ToLower().Contains(nameSearch);

            var TotalCount = _context.Users.Where(filter).Count();
            User[] users = await _context.Users.Where(filter).OrderBy(orderby.ToString()).Skip(pageindex * pagesize).Take(pagesize).Select(x => x).ToArrayAsync();

            return Ok(new {Contacts = _provider.ToContactDTO(users) , TotalCount = TotalCount});
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserDTO newUserDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == newUserDTO.Name.ToLowerInvariant());
            if (user != null)
            {
                return BadRequest(new { errors = new { Name = new string[] { "User with this name exists" }}});
            }

            var result = await _provider.CreateUserAsync(newUserDTO);
            if (result)
            {
                return StatusCode(201, _provider.ToUserDTO(result.Entity));
            }
            else
            {
                return StatusCode(500, result.ErrorMessage);
            }
        }

        [Authorize]
        [HttpPost("~/api/PostContact/")]
        public async Task<IActionResult> PostContact([FromBody] CreateContactDTO createContactDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_provider.GetUserGuid(User, out var userGuid))
            {
                return StatusCode(500);
            }

            var result = await _provider.AddContact(userGuid, createContactDTO.Name);
            if (result)
            {
                return StatusCode(201, result.Entity);
            }
            else
            {
                return StatusCode(500, result.ErrorMessage);
            }
        }

        [Authorize]
        [HttpDelete("~/api/DeleteContact/{name}")]
        public async Task<IActionResult> DeleteContact(string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_provider.GetUserGuid(User, out var userGuid))
            {
                return StatusCode(500);
            }

            var result = await _provider.DeleteContact(userGuid, name);
            if (result)
            {
                return StatusCode(204);
            }
            else
            {
                return StatusCode(500, result.ErrorMessage);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateUserDTO updateUserDTO)
        {
            if (!_provider.GetUserGuid(User, out var userGuid))
            {
                return StatusCode(500);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _provider.UpdateUserAsync(userGuid, updateUserDTO);
            if (result)
            {
                return Ok(_provider.ToUserDTO(result.Entity));
            }
            else
            {
                return StatusCode(500, result.ErrorMessage);
            }
        }

        [Authorize]
        [HttpPut("~/api/ChangePassword")]
        [ActionName("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] string password)
        {
            if (!_provider.GetUserGuid(User, out var userGuid))
            {
                return StatusCode(500);
            }

            var result = await _provider.ChangePasswordAsync(userGuid, password);
            if (!result)
            {
                return StatusCode(500, result.ErrorMessage);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("~/api/PostChat/")]
        public async Task<IActionResult> PostChat([FromBody] CreateChatDTO createChatDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (!_provider.GetUserGuid(User, out var userGuid))
            {
                return StatusCode(500);
            }

            var result = await _provider.AddChat(userGuid, createChatDTO);
            if (result)
            {
                return StatusCode(201, _provider.ToChatDTO(result.Entity));
            }
            else
            {
                return StatusCode(500, result.ErrorMessage);
            }
        }

        public enum UserOrderBy
        {
            Name,
            FirstName,
            LastName,
            PhoneNumber
        }

    }
}
