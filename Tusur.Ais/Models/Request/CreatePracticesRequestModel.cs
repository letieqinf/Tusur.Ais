using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tusur.Ais.Data.Entities.UniversityStructure;

namespace Tusur.Ais.Models.Request;

public class CreatePracticesRequestModel
{
    // Practice Kind
    
    [Required] public string PracticeKindName { get; set; }

    // Practice Type
    
    [Required, Key] public Guid Id { get; set; }
    [Required] public string PracticeTypeName { get; set; }
    
}