using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Data.Entities.Users
{
    public class EducationDepartment
    {
        [Key]
        public Guid Id { get; set; }

        // Foreign Keys

        public Guid UserId { get; set; }

        // Dependencies

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
