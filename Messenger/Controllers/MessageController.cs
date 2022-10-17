using MessengerData.Providers;
using MessengerData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
