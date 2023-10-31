using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tusur.Ais.Data.Entities.Users;

namespace Tusur.Ais.Extensions
{
    public static class JwtBearerExtensions
    {
        public static IEnumerable<Claim> CreateClaims(this User user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            return claims;
        }

        public static SigningCredentials CreateSigningCredentials(this IConfiguration configuration)
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)
                ),
                SecurityAlgorithms.HmacSha256
            );
        }

        public static JwtSecurityToken CreateJwtToken(this IEnumerable<Claim> claims, IConfiguration configuration)
        {
            var expire = configuration.GetSection("Jwt:Expire").Get<int>();

            return new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(expire),
                signingCredentials: configuration.CreateSigningCredentials()
            );
        }

        public static JwtSecurityToken CreateToken(this IConfiguration configuration, List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!));
            var tokenValidityInMinutes = configuration.GetSection("Jwt:TokenValidityInMinutes").Get<int>();

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        public static IEnumerable<Claim> GetJwtTokenClaims(this HttpRequest request)
        {
            var authorizationHeader = request.Headers.Authorization.ToString();
            if (authorizationHeader is null)
                throw new ArgumentException("Provided request does not contain authorization header.");

            var jwtToken = authorizationHeader.Split(' ')[1];
            var decodedJwt = new JwtSecurityToken(jwtEncodedString: jwtToken);

            return decodedJwt.Claims;
        }

        public static string GetEmailFromClaims(this IEnumerable<Claim> claims)
        {
            var email = claims
                .Where(claim => claim.Type == ClaimTypes.Email)
                .Select(claim => claim.Value)
                .FirstOrDefault();

            if (email is null)
                throw new ArgumentException("Provided JWT token claims do not contain the email claim.");

            return email;
        }

        public static IEnumerable<string> GetRolesFromClaims(this IEnumerable<Claim> claims)
        {
            var roles = claims
                .Where(claim => claim.Type == ClaimTypes.Role)
                .Select(claim => claim.Value);

            if (roles is null)
                throw new ArgumentException("Provided JWT token claims do not contain any role claims.");

            return roles;
        }
    }
}
