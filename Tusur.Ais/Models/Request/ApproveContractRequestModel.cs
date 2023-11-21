using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace Tusur.Ais.Models.Request;

public class ApproveContractRequestModel
{
    [Required, Key] public Guid ApplicationId { get; set; }
}