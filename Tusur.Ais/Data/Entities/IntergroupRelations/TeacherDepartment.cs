using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tusur.Ais.Data.Entities.UniversityStructure;
using Tusur.Ais.Data.Entities.Users;

namespace Tusur.Ais.Data.Entities.IntergroupRelations
{
    public class TeacherDepartment
    {
        public Guid TeacherId { get; set; }
        public Guid DepartmentId { get; set; }

        // Dependecies

        [ForeignKey(nameof(TeacherId))]
        public User Teacher { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
    }
}
