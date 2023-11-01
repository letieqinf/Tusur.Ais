using System.ComponentModel.DataAnnotations.Schema;

namespace Tusur.Ais.Data.Entities.UniversityStructure;

public class TrainingProfile
{
    public Guid TrainingProfileId { get; set; }
    
    // Foreign Keys
    public string StudyFieldName { get; set; }
    
    // Fields
    public string TrainingProfileName { get; set; }
    
    // Dependencies
    [ForeignKey(nameof(StudyFieldName))]
    public StudyField StudyField { get; set; }
}