using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Tusur.Ais.Data.Entities.Users;
using Tusur.Ais.Services;

namespace TUSUR.AIS.Practice;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

                var databaseInitializeService = new DatabaseInitializeService(userManager, roleManager);

                await databaseInitializeService.InitializeAsync();
            }
            catch (Exception e)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(e, "An error has occured while creating database defaults.");
            }
        }

        await host.RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}