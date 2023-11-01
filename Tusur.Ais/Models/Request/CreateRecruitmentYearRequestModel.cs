using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateRecruitmentYearRequestModel
{
    [Required] public DateTime Year { get; set; }
}