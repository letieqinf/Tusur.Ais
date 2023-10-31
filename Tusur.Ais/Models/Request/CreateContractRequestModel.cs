using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request
{
    public class CreateContractRequestModel
    {
        // Teacher

        [Required] public string TeacherUserName { get; set; }

        // Company

        [Required] public int Inn { get; set; }
        [Required] public string CompanyName { get; set; }
        [Required] public string CompanyShortName { get; set; }
        [Required] public string CompanyAddress { get; set; }
        [Required] public string CompanyPhoneNumber { get; set; }
        [Required, EmailAddress] public string CompanyEmail { get; set; }

        [Required] public string CompanyDirectorName { get; set; }
        [Required] public string CompanyDirectorLastName { get; set; }
        public string? CompanyDirectorPatronymic { get; set; }

        // Contact Face

        [Required] public string ContactFaceName { get; set; }
        [Required] public string ContactFaceLastName { get; set; }
        public string? ContactFacePatronymic { get; set; }
        [Required] public string ContactFaceJobTitle { get; set; }
        [Required, EmailAddress] public string ContactFaceEmail { get; set; }
        [Required] public string ContactFacePhoneNumber { get; set; }
    }
}
