using MessengerData;
using MessengerData.Providers;
using MessengerModel.Authenticate;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        public readonly AuthenticateProvider _authenticateProvider;
        
        public AuthenticateController(AuthenticateProvider authenticateProvider)
        {
            _authenticateProvider = authenticateProvider; 
        }
        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthenticateDTO authenticateDTO)
        {

            var result = _authenticateProvider.Authenticate(authenticateDTO);
            if (!result.Result)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                access_token = result.Token
            });

        }

    }
}
