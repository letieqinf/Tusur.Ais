using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tusur.Ais.Data.Entities.UniversityStructure
{
    [Index(nameof(FacultyName), nameof(Name), IsUnique = true)]
    public class Department
    {
        [Key] public Guid Id { get; set; }

        // Foreign Keys

        public string FacultyName { get; set; }

        // Fields

        [Required] public string Name { get; set; }

        // Dependencies

        [ForeignKey(nameof(FacultyName))]
        public Faculty Faculty { get; set; }
    }
}
