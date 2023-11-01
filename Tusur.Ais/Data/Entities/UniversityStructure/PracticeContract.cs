using System.ComponentModel.DataAnnotations.Schema;
using Tusur.Ais.Data.Entities.Applications;

namespace Tusur.Ais.Data.Entities.UniversityStructure;

public class PracticeContract
{
    public Guid ContractId { get; set; }
    public Guid PracticeId { get; set; }
    
    // Dependencies
    [ForeignKey(nameof(ContractId))]
    public Contract Contract { get; set; }
    
    [ForeignKey(nameof(PracticeId))]
    public Practice Practice { get; set; }
}