using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tusur.Ais.Data.Entities.Applications
{
    [Index(nameof(Number), IsUnique = true)]
    public class Proxy
    {
        [Key] public Guid Id { get; set; }

        // Foreign Keys

        public Guid SignatoryId { get; set; }

        // Fields

        [Required] public DateTime DateStart { get; set; }
        [Required] public int Number { get; set; }

        // Dependencies

        [ForeignKey(nameof(SignatoryId))]
        public Signatory Signatory { get; set; }
    }
}
