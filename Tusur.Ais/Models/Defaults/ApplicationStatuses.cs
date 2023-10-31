using System.ComponentModel.DataAnnotations;

namespace Tusur.Ais.Models.Defaults
{
    public enum ApplicationStatuses
    {
        [Display(Name = "Редактируется")]
        Editing,

        [Display(Name = "Отправлен на согласование")]
        Sent,

        [Display(Name = "Согласован руководителем практики от кафедры")]
        ApprovedByTeacher,

        [Display(Name = "Согласован руководителем учебного управления")]
        ApprovedByEducationDepartment,

        [Display(Name = "Принят секретарём")]
        AcceptedBySecretary,

        [Display(Name = "Отправлен в центр карьеры")]
        SentToCareerCenter,

        [Display(Name = "Подписан")]
        Signed,

        [Display(Name = "Принят кафедрой")]
        AcceptedByDepartment
    }
}
