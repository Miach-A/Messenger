﻿using MessengerData.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MessengerData.Extensions;
using MessengerModel.UserModels;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        [HttpGet("~/api/test")]
        public IActionResult GetTest()
        {
            var user = _provider.GetRepository().Get(null, null, x => x.Include(y => y.Contacts).ThenInclude(y => y.Contact).Include(y => y.IAsContact).ThenInclude(y => y.User)).ToArray();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            Guid userGuid;
            if (!_provider.GetUserGuid(User, out userGuid))
            {
                return StatusCode(500);
            }

            User? user = await _provider.GetRepository()
                .FirstOrDefaultAsync(x => x.Guid == userGuid
                ,x => x.Include(y => y.UserChats)
                        .Include(y => y.Contacts));

            if (user == null)
            {
                return StatusCode(500);
            }

            return Ok(_provider.ToUserDTO(user));

        }

        [Authorize]
        [HttpGet("~/api/GetUsers")]
        [ActionName("GetUsers")]
        public async Task<IActionResult> GetUsers(string? name, string? firstname, string? lastname, string? phonenumber, string? orderby, int pageindex = 0, int pagesize = 20)
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

            User[] users = await _provider.GetRepository().Get(filter, order).ToArrayAsync();

            return Ok(_provider.ToContactDTO(users));

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserDTO newUserDTO)
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

        [Authorize]
        [HttpPost("~/api/PostContact/")]
        public async Task<IActionResult> PostContact([FromBody] string contactName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Guid userGuid;
            if (!_provider.GetUserGuid(User, out userGuid))
            {
                return StatusCode(500);
            }

            var result = await _provider.AddContact(userGuid, contactName);
            if (result)
            {
                return StatusCode(200);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpDelete("~/api/DeleteContact/")]
        public async Task<IActionResult> DeleteContact([FromBody] string contactName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            Guid userGuid;
            if (!_provider.GetUserGuid(User, out userGuid))
            {
                return StatusCode(500);
            }

            var result = await _provider.DeleteContact(userGuid, contactName);
            if (result)
            {
                return StatusCode(204);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateUserDTO updateUserDTO)
        {
            Guid userGuid;
            if (!_provider.GetUserGuid(User, out userGuid))
            {
                return StatusCode(500);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _provider.UpdateUserAsync(userGuid, updateUserDTO);
            if (result.Result)
            {
                return Ok();
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

            Guid userGuid;
            if (!_provider.GetUserGuid(User, out userGuid))
            {
                return StatusCode(500);
            }

            var result = await _provider.ChangePasswordAsync(userGuid, password);
            if (!result)
            {
                return StatusCode(500);
            }

            return Ok();

        } 

    }
}
