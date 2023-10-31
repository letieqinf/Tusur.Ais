using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request
{
    public class RegisterRequestModel
    {
        [Required] public string Name { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string? Patronymic { get; set; }

        [Required] public string UserName { get; set; }
        [Required, EmailAddress] public string EmailAddress { get; set;}
        [Required, ProtectedPersonalData] public string Password { get; set; }
        [Required] public IEnumerable<string> Roles { get; set; }
    }
}
