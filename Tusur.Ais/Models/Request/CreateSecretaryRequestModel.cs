using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateSecretaryRequestModel
{
    [Required, Key] public Guid SecretaryId { get; set; } 
}