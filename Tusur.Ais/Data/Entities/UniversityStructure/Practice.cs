using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tusur.Ais.Data.Entities.UniversityStructure
{
    [Index(nameof(RecruitmentYearId), nameof(PracticeTypeId), nameof(Semester), IsUnique = true)]
    public class Practice
    {
        [Key] public Guid Id { get; set; }

        // Foreign Keys

        public Guid RecruitmentYearId { get; set; }
        public Guid PracticeTypeId { get; set; }

        // Fields

        public string Semester { get; set; }

        // Dependencies

        [ForeignKey(nameof(RecruitmentYearId))]
        public RecruitmentYear RecruitmentYear { get; set; }

        [ForeignKey(nameof(PracticeTypeId))]
        public PracticeType PracticeType { get; set; }
    }
}
