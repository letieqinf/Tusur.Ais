using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateGroupRequestModel
{
    [Required] public string Name { get; set; }
}