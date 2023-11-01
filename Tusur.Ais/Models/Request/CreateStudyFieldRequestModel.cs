using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateStudyFieldRequestModel
{
    [Required] public string StudyFieldName { get; set; }
}