using System.ComponentModel.DataAnnotations.Schema;

namespace Tusur.Ais.Data.Entities.UniversityStructure;

public class EducationPlan
{
    public Guid EducationPlanId { get; set; }
    
    // Foreign Keys
    public Guid TrainingProfileId { get; set; }
    public string FacultyName { get; set; }
    
    // Fields
    //public EducationForms EducationForm { get; set; }
    
    // Dependencies
    [ForeignKey(nameof(TrainingProfileId))]
    public TrainingProfile TrainingProfile { get; set; }
    
    [ForeignKey(nameof(FacultyName))]
    public Faculty Faculty { get; set; }
}