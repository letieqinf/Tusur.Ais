using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateStudentContractRequestModel
{
    [Required] public string? Number { get; set; }
    [Required] public DateTime? Date { get; set; }
    [Required] public string? Name { get; set; }
    [Required] public string? LastName { get; set; }
    [Required] public string? Patronymic { get; set; }
}