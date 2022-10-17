using MessengerData.Providers;
using MessengerData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetMessage(DateTime date,Guid guid)
        {
            var message = await _context.Messages.Include(x => x.MessageComment).FirstOrDefaultAsync(x => x.Date == date && x.Guid == guid);
            return Ok(message);             

        }

        [HttpGet]
        public async Task<IActionResult> Get(DateTime date, Guid chatGuid, int count)
        {
            var message = await _context.Messages.Where(x => x.Date < date && x.ChatGuid == chatGuid).OrderByDescending(x => x.Date).Take(count).ToArrayAsync();
            return Ok(message);

        }

    }
}
