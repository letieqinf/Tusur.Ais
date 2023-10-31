using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tusur.Ais.Data.Entities.UniversityStructure
{
    [Index(nameof(StudyFieldName), nameof(Name), IsUnique = true)]
    public class StudyProfile
    {
        [Key] public Guid Id { get; set; }

        // Foreign Keys

        public string StudyFieldName { get; set; }

        // Fields

        [Required] public string Name { get; set; }

        // Dependencies

        [ForeignKey(nameof(StudyFieldName))]
        public StudyField StudyField { get; set; }
    }
}
