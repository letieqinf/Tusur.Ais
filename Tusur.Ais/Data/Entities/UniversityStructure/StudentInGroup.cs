using System.ComponentModel.DataAnnotations.Schema;
using Tusur.Ais.Data.Entities.Users;
using Tusur.Ais.Models.Response;

namespace Tusur.Ais.Data.Entities.UniversityStructure;

public class StudentInGroup
{
    public Guid GroupId { get; set; }
    public Guid StudentId { get; set; }
    
    // Dependencies
    [ForeignKey(nameof(GroupId))]
    public Group Group { get; set; }
    
    [ForeignKey(nameof(StudentId))]
    public Student Student { get; set; }
}