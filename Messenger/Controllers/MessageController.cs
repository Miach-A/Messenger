using MessengerData.Providers;
using MessengerData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessengerModel.MessageModels;
using System.Threading;
using System.Linq;

namespace Messenger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly MessageProvider _provider;
        public MessageController(ApplicationDbContext context, MessageProvider provider)
        {
            _provider = provider;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid chatGuid, int count = 20, DateTime? date = null)
        {
            if (!_provider.GetUserGuid(User, out var userGuid))
            {
                return BadRequest();
            }

            //_provider.GetDeletedMessages(chatGuid, userGuid, date).Contains(x)
            //&& x.Contains(_provider.GetDeletedMessages(chatGuid, userGuid, date))

            var message = await _context.Messages
                //.Include(x => x.DeletedMessages)
                .Include(x => x.CommentedMessage)
                    #pragma warning disable CS8602
                    .ThenInclude(x => x.CommentedMessage)
                    #pragma warning restore CS8602
                    .ThenInclude(x => x.User)
                .Include(x => x.User)
                .Where(x => (date == null ? true : x.Date < date) 
                            && (x.ChatGuid == chatGuid))
                .Where(x => !_provider.GetDeletedMessages(chatGuid, userGuid, date).Contains(x))
                .OrderByDescending(x => x.Date)
                .Take(count)
                .ToArrayAsync();

            return Ok(_provider.ToMessageDTO(message));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateMessageDTO createMessageDTO)
        {
            var result = await _provider.CreateMessageAsync(createMessageDTO, User);
            if (!result)
            {
                return StatusCode(500);                
            }

            return Ok(_provider.ToMessageDTO(result.Entity));
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateMessageDTO updateMessageDTO)
        {
            var result = await _provider.UpdateMessageAsync(updateMessageDTO, User);
            if (!result)
            {
                return StatusCode(500);
            }

            return Ok(_provider.ToMessageDTO(result.Entity));
        }
    }
}
