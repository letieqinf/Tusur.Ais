using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tusur.Ais.Models.Defaults;

namespace Tusur.Ais.Data.Entities.UniversityStructure
{
    [Index(nameof(StudyProfileId), nameof(FacultyName), nameof(Form), IsUnique = true)]
    public class StudyPlan
    {
        [Key] public Guid Id { get; set; }

        // Foreign Keys

        public Guid StudyProfileId { get; set; }
        public string FacultyName { get; set; }

        // Fields

        public StudyForms Form { get; set; }

        // Dependencies

        [ForeignKey(nameof(StudyProfileId))]
        public StudyProfile StudyProfile { get; set; }

        [ForeignKey(nameof(FacultyName))]
        public Faculty Faculty { get; set; }
    }
}
