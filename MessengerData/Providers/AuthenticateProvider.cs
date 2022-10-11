
using MessengerModel.Authenticate;
using MessengerModel.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MessengerData.Providers
{
    public class AuthenticateProvider
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthenticateProvider(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public MessengerSignInResult SignIn (AuthenticateDTO authenticateDTO)
        {
            var result = new MessengerSignInResult { Result = false };
            var user = _context.Users.Where(x => x.Name == authenticateDTO.Name).FirstOrDefault();

            if (user == null)
            {
                return result;
            }

            var hasher = new PasswordHasher<User>();
            var verifyResult = hasher.VerifyHashedPassword(user, user.PasswordHash, authenticateDTO.Password);

            if (verifyResult == PasswordVerificationResult.Failed)    
            {
                return result;
            }

            var AuthJwtKey = _configuration["AuthJwtKey"];

            if (AuthJwtKey == null || AuthJwtKey == String.Empty)
            {
                return result;
            }

            byte[] secretBytes = Encoding.UTF8.GetBytes(AuthJwtKey);
            var key = new SymmetricSecurityKey(secretBytes);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Guid.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Name)
            };

            var token = new JwtSecurityToken(
                _configuration["Issuer"], _configuration["Audience"], claims, DateTime.Now, DateTime.Now.AddHours(1), signingCredentials);

            result.Token = new JwtSecurityTokenHandler().WriteToken(token);

            return result;
        }
    }
}
