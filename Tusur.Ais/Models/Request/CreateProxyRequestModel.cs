using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateProxyRequestModel
{
    [Required] public DateTime DateStart { get; set; }
    [Required] public int Number { get; set; }
}