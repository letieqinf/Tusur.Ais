using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Defaults
{
    public enum CompanyConfirmationStatuses
    {
        [Display(Name = "В обработке")]
        InProcess,

        [Display(Name = "Подтверждена")]
        Confirmed
    }
}
