using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreatePracticeRequestModel
{
    [Required] public DateTime RecruitmentYear { get; set; }
    [Required] public string Semester { get; set; }
    [Required] public string PracticeTypeName { get; set; }
}