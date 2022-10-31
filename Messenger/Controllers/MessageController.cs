using MessengerData.Providers;
using MessengerData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MessengerModel.MessageModels;

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

        //[HttpGet]
        //public async Task<IActionResult> GetMessage(DateTime date,Guid guid)
        //{
        //    var message = await _context.Messages.Include(x => x.CommentedMessage).FirstOrDefaultAsync(x => x.Date == date && x.Guid == guid);
        //    if (message == null)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(_provider.ToMessageDTO(message));             

        //}

        [HttpGet]
        public async Task<IActionResult> Get(DateTime date, Guid chatGuid, int count)
        {
            var message = await _context.Messages.Where(x => x.Date < date && x.ChatGuid == chatGuid).OrderByDescending(x => x.Date).Take(count).ToArrayAsync();
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
