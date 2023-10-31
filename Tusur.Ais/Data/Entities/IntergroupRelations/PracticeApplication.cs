using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tusur.Ais.Data.Entities.Applications;
using Tusur.Ais.Data.Entities.UniversityStructure;
using Tusur.Ais.Models.Defaults;

namespace Tusur.Ais.Data.Entities.IntergroupRelations
{
    public class PracticeApplication
    {
        public Guid PracticeId { get; set; }
        public Guid ApplicationId { get; set; }
        public ApplicationTypes Type { get; set; }

        // Dependencies

        [ForeignKey(nameof(PracticeId))]
        public Practice Practice { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        public Application Application { get; set; }
    }
}
