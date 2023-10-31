using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tusur.Ais.Data.Entities.UniversityStructure
{
    [Index(nameof(PracticeKindName), nameof(Name), IsUnique = true)]
    public class PracticeType
    {
        [Key] public Guid Id { get; set; }

        // Foreign Keys

        public string PracticeKindName { get; set; }

        // Fields

        public string Name { get; set; }

        // Dependencies

        [ForeignKey(nameof(PracticeKindName))]
        public PracticeKind PracticeKind { get; set; } 
    }
}