using Microsoft.AspNetCore.Identity;
using Tusur.Ais.Data.Entities.Users;

namespace Tusur.Ais.Services
{
    public interface IDatabaseInitializeService
    {
        Task InitializeAsync();
    }
}
