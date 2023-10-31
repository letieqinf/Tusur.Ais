using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tusur.Ais.Data.Entities.UniversityStructure;
using Tusur.Ais.Data.Entities.Users;

namespace Tusur.Ais.Data.Entities.IntergroupRelations
{
    public class StudentGroup
    {
        public Guid StudentId { get; set; }
        public Guid GroupId { get; set; }

        // Dependencies

        [ForeignKey(nameof(StudentId))]
        public User Student { get; set; }

        [ForeignKey(nameof(GroupId))]
        public Group Group { get; set; }
    }
}
