using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Tusur.Ais.Data.Entities.Users;
using Tusur.Ais.Extensions;

namespace Tusur.Ais.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(User user, List<string> roles)
        {
            var token = user
                .CreateClaims(roles)
                .CreateJwtToken(_configuration);

            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(token);
        }
    }
}
