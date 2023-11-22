using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tusur.Ais.Data;
using Tusur.Ais.Data.Entities.Applications;
using Tusur.Ais.Data.Entities.Users;
using Tusur.Ais.Models.Defaults;
using Tusur.Ais.Models.Response;

namespace Tusur.Ais.Extensions
{
    public static class UserContractExtensions
    {
        public static async Task<List<GetContractsResponseModel>> GetContractsAsync(this User user, UserManager<User> userManager, ApplicationDbContext context)
        {
            var contracts = new List<GetContractsResponseModel>();

            List<Contract>? contractList = null;
            var applications = context.Applications.ToList();

            if (await userManager.IsInRoleAsync(user, UserRoles.Student))
            {
                var studentApplications = context.StudentApplications.Where(sa => sa.StudentId == user.Id).ToList();
                applications = applications.IntersectBy(studentApplications.Select(sa => sa.ApplicationId), app => app.Id).ToList();
                contractList = applications.Where(a => a.Contract is not null).Select(a => a.Contract!).ToList();
            }
            else if (await userManager.IsInRoleAsync(user, UserRoles.Teacher))
            {
                contractList = applications.Where(a => a.Contract is not null).Select(a => a.Contract!).ToList();
                contractList = contractList.Where(cl => cl.TeacherId == user.Id).ToList();
            }
            else if (await userManager.IsInRoleAsync(user, UserRoles.EducationDepartment))
            {
                applications = applications.Where(a => a.Status == ApplicationStatuses.ApprovedByTeacher).ToList();
                contractList = applications.Where(a => a.Contract is not null).Select(a => a.Contract!).ToList();
            }
            else if (await userManager.IsInRoleAsync(user, UserRoles.Secretary))
            {
                applications = applications.Where(a => a.Status == ApplicationStatuses.ApprovedByEducationDepartment).ToList();
                contractList = applications.Where(a => a.Contract is not null).Select(a => a.Contract!).ToList();
            }
            else
            {
                contractList = applications.Where(a => a.Contract is not null).Select(a => a.Contract!).ToList();
            }

            contracts = await GetContractsAsync(contractList, context);

            return contracts;
        }

        private static async Task<List<GetContractsResponseModel>> GetContractsAsync(List<Contract>? contracts, ApplicationDbContext context)
        {
            if (contracts is null)
                return new List<GetContractsResponseModel>();

            var contractsResponses = new List<GetContractsResponseModel>();

            foreach (var contract in contracts) 
            {
                // Student
                var application = contract.Application;
                if (application is null) continue;

                var studentApplication = context.StudentApplications.FirstOrDefault(sa => sa.ApplicationId == application.Id);
                if (studentApplication is null) continue;

                var student = context.Users.FirstOrDefault(u => u.Id == studentApplication.StudentId);
                if (student is null) continue;

                // Student Data
                var group = await context.GetStudentGroupAsync(student);
                var year = await context.GetStudentRecruitmentYearAsync(student);
                var studyProfile = await context.GetStudentStudyProfileAsync(student);
                var studyForm = await context.GetStudentStudyFormAsync(student);

                // Contract Data
                var teacher = await context.Users.FirstOrDefaultAsync(t => t.Id == contract!.TeacherId);
                var signatory = await context.Signatories.FirstOrDefaultAsync(s => s.Id == contract!.SignatoryId);
                var company = await context.Companies.FirstOrDefaultAsync(c => c.Id == contract!.CompanyId);
                var contactFace = await context.ContactFaces.FirstOrDefaultAsync(c => c.Id == contract!.ContactFaceId);

                contractsResponses.Add(new GetContractsResponseModel
                {
                    ContractId = contract!.ApplicationId,
                    Number = contract!.Number,
                    Date = contract!.Date,
                    StudentDescription = new GetContractsResponseModel.Student
                    {
                        Name = student.Name!,
                        LastName = student.LastName!,
                        Patronymic = student.Patronymic,
                        Group = group.Name,
                        StudyField = studyProfile.StudyFieldName,
                        StudyProfile = studyProfile.Name,
                        StudyForm = studyForm,
                        RecruitmentYear = year.Year
                    },
                    TeacherDescription = new GetContractsResponseModel.Teacher
                    {
                        Name = teacher!.Name!,
                        LastName = teacher!.LastName!,
                        Patronymic = teacher!.Patronymic
                    },
                    SignatoryDescription = new GetContractsResponseModel.Signatory
                    {
                        Name = signatory!.Name,
                        LastName = signatory.LastName,
                        Patronymic = signatory.Patronymic,
                        JobTitle = signatory.JobTitle!
                    },
                    CompanyDescription = new GetContractsResponseModel.Company
                    {
                        Inn = company!.Inn,
                        Name = company!.Name,
                        ShortName = company!.ShortName,
                        Address = company!.Address,
                        DirectorName = company!.DirectorName,
                        DirectorLastName = company!.DirectorLastName,
                        DirectorPatronymic = company!.DirectorPatronymic,
                        Status = company!.Status
                    },
                    ContactFaceDescription = new GetContractsResponseModel.ContactFace
                    {
                        Name = contactFace!.Name,
                        LastName = contactFace!.LastName,
                        Patronymic = contactFace.Patronymic,
                        JobTitle = contactFace.JobTitle
                    },
                    ApplicationStatus = application!.Status
                });
            }

            return contractsResponses;
        }
    }
}
