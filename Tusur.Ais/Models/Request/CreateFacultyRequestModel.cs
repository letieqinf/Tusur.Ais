using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateFacultyRequestModel
{
    [Required] public string FacultyName { get; set; }
    [Required] public string Name { get; set; }
}