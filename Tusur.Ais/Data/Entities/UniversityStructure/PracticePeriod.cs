using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tusur.Ais.Data.Entities.UniversityStructure
{
    [Index(nameof(PracticeId), nameof(StartDate), IsUnique = true)]
    public class PracticePeriod
    {
        [Key] public Guid Id { get; set; }

        // Foreign Keys

        public Guid PracticeId { get; set; }

        // Fields

        [Required] public DateTime StartDate { get; set; }
        [Required] public DateTime EndDate { get; set; }

        // Dependencies

        [ForeignKey(nameof(PracticeId))]
        public Practice Practice { get; set; }
    }
}
