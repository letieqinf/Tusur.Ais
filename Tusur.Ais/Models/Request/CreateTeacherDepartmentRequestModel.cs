using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateTeacherDepartmentRequestModel
{
    [Required] public string? Name { get; set; }
    [Required] public string? LastName { get; set; }
    [Required] public string? Patronymic { get; set; }
    [Required] public string FacultyName { get; set; }
    [Required] public string DepartmentName { get; set; }
}