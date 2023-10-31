using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tusur.Ais.Data.Entities.Applications;
using Tusur.Ais.Data.Entities.Users;

namespace Tusur.Ais.Data.Entities.IntergroupRelations
{
    public class StudentApplication
    {
        public Guid StudentId { get; set; }
        public Guid ApplicationId { get; set; }

        // Dependencies

        [ForeignKey(nameof(StudentId))]
        public User Student { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        public Application Application { get; set; }
    }
}
