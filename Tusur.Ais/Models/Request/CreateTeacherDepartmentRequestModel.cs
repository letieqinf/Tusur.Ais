using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateTeacherDepartmentRequestModel
{
    [Required, Key] public Guid TeacherId { get; set; }
    [Required] public string FacultyName { get; set; }
    [Required] public string DepartmentName { get; set; }
}