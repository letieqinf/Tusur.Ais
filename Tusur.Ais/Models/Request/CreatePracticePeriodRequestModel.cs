using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreatePracticePeriodRequestModel
{
    [Required] public string PracticeTypeName { get; set; }
    [Required] public DateTime Year { get; set; }
    [Required] public string Semester { get; set; }
    [Required] public DateTime DateStart { get; set; }
    [Required] public DateTime DateEnd { get; set; }
}