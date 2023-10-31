using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tusur.Ais.Data.Entities.Users
{
    public class Teacher
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
