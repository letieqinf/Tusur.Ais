using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tusur.Ais.Data.Entities.UniversityStructure
{
    [Index(nameof(Name), IsUnique = true)]
    public class Group
    {
        [Key] public Guid Id { get; set; }

        // Foreign Keys

        public Guid RecruitmentYearId { get; set; }
        public Guid DepartmentId { get; set; }

        // Fields

        [Required] public string Name { get; set; }

        // Dependencies

        [ForeignKey(nameof(RecruitmentYearId))]
        public RecruitmentYear RecruitmentYear { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
    }
}
