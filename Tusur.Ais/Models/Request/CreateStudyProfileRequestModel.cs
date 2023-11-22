using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateStudyProfileRequestModel
{
    [Required] public string StudyFieldName { get; set; }
    [Required] public string StudyProfileName { get; set; }
}