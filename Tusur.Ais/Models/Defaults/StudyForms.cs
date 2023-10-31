using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Defaults
{
    public enum StudyForms
    {
        [Display(Name = "Очная")]
        FullTime,
    
        [Display(Name = "Заочная")]
        PartTime,
    
        [Display(Name = "Очно-заочная")]
        FullPartTime,
    
        [Display(Name = "Очно-заочная с применением дистанционных технологий")]
        Distant
    }
}
