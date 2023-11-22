using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateStudyPlanRequestModel
{
    [Required] public string FacultyName { get; set; }
    [Required] public string StudyForm { get; set; }
    [Required] public string StudyProfileName { get; set; }
}