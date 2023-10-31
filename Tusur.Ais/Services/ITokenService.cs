using Microsoft.AspNetCore.Identity;
using Tusur.Ais.Data.Entities.Users;

namespace Tusur.Ais.Services
{
    public interface ITokenService
    {
        public string CreateToken(User user, List<string> roles);
    }
}
