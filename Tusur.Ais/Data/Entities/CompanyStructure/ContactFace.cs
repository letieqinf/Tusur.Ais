using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tusur.Ais.Data.Entities.CompanyStructure
{
    public class ContactFace
    {
        [Key] public Guid Id { get; set; }

        // Foreign Keys

        public Guid CompanyId { get; set; }

        // Fields

        [Required] public string Name { get; set; }
        [Required] public string LastName { get; set; }
        public string? Patronymic { get; set; }

        [Required] public string JobTitle { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required, EmailAddress] public string Email { get; set; }

        // Dependencies

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }
    }
}
