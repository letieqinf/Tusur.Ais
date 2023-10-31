using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request
{
    public class LoginRequestModel
    {
        [Required] public string UserName { get; set; }
        [Required] public string Password { get; set; }
    }
}
