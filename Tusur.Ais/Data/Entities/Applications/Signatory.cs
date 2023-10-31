using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Data.Entities.Applications
{
    public class Signatory
    {
        [Key] public Guid Id { get; set; }

        // Fields
        [Required] public string Name { get; set; }
        [Required] public string LastName { get; set; }
        public string? Patronymic { get; set; }

        [Required] public string? JobTitle { get; set; }
    }
}
