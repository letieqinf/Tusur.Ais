using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tusur.Ais.Data;
using Tusur.Ais.Data.Entities.Users;
using Tusur.Ais.Extensions;
using Tusur.Ais.Models.Request;
using Tusur.Ais.Models.Response;

namespace Tusur.Ais.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<User> _userManager;

        public AccountController(ApplicationDbContext context, RoleManager<IdentityRole<Guid>> roleManager, UserManager<User> userManager) 
        { 
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var jwtTokenClaims = Request.GetJwtTokenClaims();
            var email = jwtTokenClaims.GetEmailFromClaims();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok(new GetProfileResponseModel
            {
                UserName = user.UserName,
                Email = email,
                Name = user.Name,
                LastName = user.LastName,
                Patronymic = user.Patronymic
            });
        }

        [HttpPatch]
        [Route("profile/edit")]
        public async Task<IActionResult> EditProfile([FromBody] EditRequestModel model)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var jwtTokenClaims = Request.GetJwtTokenClaims();
            var email = jwtTokenClaims.GetEmailFromClaims();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            user.Name = model.Name;
            user.LastName = model.LastName;
            user.Patronymic = model.Patronymic;

            await _context.SaveChangesAsync();

            return Ok(new EditResponseModel
            {
                Name = user.Name, 
                LastName = user.LastName, 
                Patronymic = user.Patronymic
            });
        }

        [HttpGet]
        [Route("profile/roles")]
        public IActionResult GetRoles()
        {
            var jwtTokenClaims = Request.GetJwtTokenClaims();
            var roles = jwtTokenClaims.GetRolesFromClaims();

            return Ok(roles);
        }
    }
}
