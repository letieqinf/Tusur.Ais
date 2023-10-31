using Tusur.Ais.Data.Entities.Applications;
using Tusur.Ais.Models.Defaults;

namespace Tusur.Ais.Models.Response
{
    public class GetContractsResponseModel
    {
        public struct Student
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string? Patronymic { get; set; }
            public string Group { get; set; }
            public string StudyField { get; set; }
            public string StudyProfile { get; set; }
            public StudyForms StudyForm { get; set; }
            public DateTime RecruitmentYear { get; set; }
        }

        public struct Teacher
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string? Patronymic { get; set; }
        }

        public struct Signatory
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string? Patronymic { get; set; }
            public string JobTitle { get; set; }
        }

        public struct Company
        {
            public int Inn { get; set; }
            public string Name { get; set; }
            public string ShortName { get; set; }
            public string Address { get; set; }
            public string DirectorName { get; set; }
            public string DirectorLastName { get; set; }
            public string? DirectorPatronymic { get; set; }
            public CompanyConfirmationStatuses Status { get; set; }
        }

        public struct ContactFace
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string? Patronymic { get; set; }
            public string JobTitle { get; set; }
        }

        public Guid ContractId { get; set; }
        public string? Number { get; set; }
        public DateTime? Date { get; set; }

        public Student StudentDescription { get; set; }
        public Teacher TeacherDescription { get; set; }
        public Signatory SignatoryDescription { get; set; }
        public Company CompanyDescription { get; set; }
        public ContactFace ContactFaceDescription { get; set; }

        public ApplicationStatuses ApplicationStatus { get; set; }
    }
}
