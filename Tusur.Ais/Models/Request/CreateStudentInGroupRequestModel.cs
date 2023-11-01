using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateStudentInGroupRequestModel
{
    [Required] public string GroupName { get; set; }
    [Required] public string? Name { get; set; }
    [Required] public string? LastName { get; set; }
    [Required] public string? Patronymic { get; set; }
}