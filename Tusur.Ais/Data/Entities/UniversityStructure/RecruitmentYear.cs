using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tusur.Ais.Data.Entities.UniversityStructure
{
    [Index(nameof(Year), IsUnique = true)]
    public class RecruitmentYear
    {
        [Key] public Guid Id { get; set; }

        // Foreign Keys

        public Guid StudyPlanId { get; set; }

        // Fields

        [Required] public DateTime Year { get; set; }

        // Dependencies

        [ForeignKey(nameof(StudyPlanId))]
        public StudyPlan StudyPlan { get; set; }
    }
}
