using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Request;

public class CreateRecruitmentYearRequestModel
{
    [Required] public DateTime Year { get; set; }
    [Required] public string FacultyName { get; set; }

}

public class CreateGroupRequestModel
{
    [Required] public DateTime Year { get; set; }
    [Required] public string FacultyName { get; set; }
    [Required] public string GroupName { get; set; }
    [Required] public string DepartmentName { get; set; }
}