using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateDepartmentRequestModel
{
    [Required] public string Name { get; set; }
}