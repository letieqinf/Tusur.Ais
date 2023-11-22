using Microsoft.AspNetCore.Identity;
using Tusur.Ais.Data.Entities.Users;
using Tusur.Ais.Models.Defaults;

namespace Tusur.Ais.Services
{
    public class DatabaseInitializeService : IDatabaseInitializeService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public DatabaseInitializeService(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager) 
        { 
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            foreach (var role in UserRoles.AvailableRoles)
                await CreateRole(role);

            var admin = new User
            {
                UserName = "admin",
                    Email = "admin@temp.com"
            };

            var user = await _userManager.FindByEmailAsync(admin.Email);
            if (user is null)
            {
                var result = await _userManager.CreateAsync(admin, "Admin1!");
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(admin, UserRoles.Admin);
            }
        }

        private async Task CreateRole(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
                await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
    }
}
