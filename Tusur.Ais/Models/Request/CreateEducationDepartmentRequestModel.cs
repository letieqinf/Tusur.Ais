using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateEducationDepartmentRequestModel
{
    [Required, Key] public Guid UserId { get; set; } 
}