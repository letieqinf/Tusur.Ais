using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateTrainingProfileRequestModel
{
    [Required] public string TrainingProfileName { get; set; }
}