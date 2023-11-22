using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tusur.Ais.Data.Entities.UniversityStructure;

namespace Tusur.Ais.Models.Request;

public class CreatePracticeTypeRequestModel
{
    // Practice Kind
    
    [Required] public string PracticeKindName { get; set; }

    // Practice Type
    
    [Required] public string PracticeTypeName { get; set; }
}

public class CreatePracticeKindRequestModel
{
    // Practice Kind
    
    [Required] public string PracticeKindName { get; set; }
}