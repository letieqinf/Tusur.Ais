using System.ComponentModel.DataAnnotations.Schema;
using Tusur.Ais.Data.Entities.Applications;
using Tusur.Ais.Data.Entities.Users;

namespace Tusur.Ais.Data.Entities.UniversityStructure;

public class StudentContract
{
    public Guid ContractId { get; set; }
    public Guid StudentId { get; set; }
    
    // Dependencies
    [ForeignKey(nameof(ContractId))]
    public Contract Contract { get; set; }
    
    [ForeignKey(nameof(StudentId))]
    public Student Student { get; set; }
}