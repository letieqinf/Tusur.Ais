using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tusur.Ais.Data.Entities.CompanyStructure;
using Tusur.Ais.Data.Entities.Users;

namespace Tusur.Ais.Data.Entities.Applications
{
    [Index(nameof(Number), IsUnique = true)]
    public class Contract
    {
        [Key] public Guid ApplicationId { get; set; }

        // Foreign Keys

        public Guid CompanyId { get; set; }
        public Guid TeacherId { get; set; }
        public Guid SignatoryId { get; set; }
        public Guid ContactFaceId { get; set; }

        // Fields

        public string? Number { get; set; }
        public DateTime? Date { get; set; }

        // Dependencies

        [ForeignKey(nameof(ApplicationId))]
        public Application Application { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public User User { get; set; }

        [ForeignKey(nameof(SignatoryId))]
        public Signatory Signatory { get; set; }

        [ForeignKey(nameof(ContactFaceId))]
        public ContactFace ContactFace { get; set; }
    }
}
