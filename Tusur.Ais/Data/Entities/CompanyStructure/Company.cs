using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Tusur.Ais.Models.Defaults;

namespace Tusur.Ais.Data.Entities.CompanyStructure
{
    [Index(nameof(Inn), IsUnique = true)]
    public class Company
    {
        [Key] public Guid Id { get; set; }

        // Fields

        [Required] public int Inn { get; set; }

        [Required] public string Name { get; set; }
        [Required] public string ShortName { get; set; }
        [Required] public string Address { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required, EmailAddress] public string Email { get; set; }

        [Required] public string DirectorName { get; set; }
        [Required] public string DirectorLastName { get; set; }
        public string? DirectorPatronymic { get; set; }

        [Required] public CompanyConfirmationStatuses Status { get; set; }
    }
}
