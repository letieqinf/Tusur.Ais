using System.ComponentModel.DataAnnotations;
using Tusur.Ais.Models.Defaults;

namespace Tusur.Ais.Data.Entities.Applications
{
    public class Application
    {
        [Key] public Guid Id { get; set; }

        // Fields

        [Required] public ApplicationStatuses Status { get; set; }
        
        // Dependencies

        public Contract? Contract { get; set; } 
    }
}
