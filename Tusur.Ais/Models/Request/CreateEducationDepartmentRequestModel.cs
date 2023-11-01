using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateEducationDepartmentRequestModel
{
    [Required] public string? Name { get; set; }
    [Required] public string? LastName { get; set; }
    [Required] public string? Patronymic { get; set; }
}