using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tusur.Ais.Data;
using Tusur.Ais.Data.Entities.Users;
using Tusur.Ais.Models.Defaults;
using Tusur.Ais.Models.Request;
using Tusur.Ais.Models.Response;
using Tusur.Ais.Services;

namespace Tusur.Ais.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public AuthController(ApplicationDbContext context, RoleManager<IdentityRole<Guid>> roleManager, ITokenService tokenService, UserManager<User> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user, roles.ToList());

            return Ok(new LoginResponseModel
            {
                UserName = user.UserName,
                Email = user.Email,
                JwtToken = token
            });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = UserRoles.Admin)]
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            foreach (var role in model.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    ModelState.AddModelError(string.Empty, $"Role {role} does not exist.");
                    return BadRequest(ModelState);
                }
            }

            if ((await _userManager.FindByEmailAsync(model.EmailAddress)) is not null)
                return StatusCode(StatusCodes.Status500InternalServerError, $"User with this email already exists.");

            var user = new User
            {
                Name = model.Name,
                LastName = model.LastName,
                Patronymic = model.Patronymic,
                UserName = model.UserName,
                Email = model.EmailAddress
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);

            foreach (var role in model.Roles)
            {
                await CreateRoleProfile(user, role);
                await _userManager.AddToRoleAsync(user, role);
            }

            await _context.SaveChangesAsync();

            return Ok(new RegisterResponseModel { Message = $"User {user.UserName} was created successfully." });
        }

        private async Task CreateRoleProfile(User user, string role)
        {
            if (role == "student")
                await _context.AddAsync(new Student { UserId = user.Id });

            if (role == "teacher")
                await _context.AddAsync(new Teacher { UserId = user.Id });

            if (role == "secretary")
                await _context.AddAsync(new Secretary { UserId = user.Id });

            if (role == "education_department")
                await _context.AddAsync(new EducationDepartment { UserId = user.Id });
        }
    }
}
