using Tusur.Ais.Data.Entities.Applications;
using Tusur.Ais.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Tusur.Ais.Data.Entities.Users;
using Tusur.Ais.Models.Defaults;
using Tusur.Ais.Data.Entities.UniversityStructure;

namespace Tusur.Ais.Extensions
{
    public static class ContextExtensions
    {
        #region Student Methods

        public static async Task<Group> GetStudentGroupAsync(this ApplicationDbContext context, User user)
        {
            if (await context.Students.FindAsync(user.Id) is null)
                throw new ArgumentException("Provided user does not exists.");

            var studentGroup = await context.StudentGroups.FirstOrDefaultAsync(sg => sg.StudentId == user.Id);
            if (studentGroup == null)
                throw new ArgumentException("Provided user does not have any group relations.");

            var group = await context.Groups.FirstOrDefaultAsync(g => g.Id == studentGroup.GroupId);
            if (group is null)
                throw new Exception("Cannot reach group because there is no group with such Group Id.");

            return group;
        }

        public static async Task<RecruitmentYear> GetStudentRecruitmentYearAsync(this ApplicationDbContext context, User user)
        {
            var group = await context.GetStudentGroupAsync(user);
            var year = await context.RecruitmentYears.FirstOrDefaultAsync(y => y.Id == group.RecruitmentYearId);
            if (year is null)
                throw new Exception("Group exists but not binded to recruitment year.");

            return year;
        }

        public static async Task<StudyPlan> GetStudentStudyPlanAsync(this ApplicationDbContext context, User user)
        {
            var year = await context.GetStudentRecruitmentYearAsync(user);
            var studyPlan = await context.StudyPlans.FirstOrDefaultAsync(sp => sp.StudyProfileId == year.StudyPlanId);
            if (studyPlan is null)
                throw new Exception();

            return studyPlan;
        }

        public static async Task<StudyProfile> GetStudentStudyProfileAsync(this ApplicationDbContext context, User user)
        {
            var studyPlan = await context.GetStudentStudyPlanAsync(user);

            var studyProfile = await context.StudyProfiles.FirstOrDefaultAsync(sp => sp.Id == studyPlan.StudyProfileId);
            if (studyProfile is null)
                throw new Exception();

            return studyProfile;
        }

        public static async Task<StudyForms> GetStudentStudyFormAsync(this ApplicationDbContext context, User user)
        {
            var studyPlan = await context.GetStudentStudyPlanAsync(user);
            return studyPlan.Form;
        }

        #endregion

        #region Department Methods

        public static async Task<IEnumerable<User>> GetDepartmentTeachersAsync(this ApplicationDbContext context, Department department)
        {
            if (await context.FindAsync<Department>(department.Id) is null)
                throw new ArgumentException("Provided department does not exists.");

            var teacherDepartments = await context.TeacherDepartments.FirstOrDefaultAsync(td => td.DepartmentId == department.Id);
            if (teacherDepartments is null)
                throw new ArgumentException("Provided department has no teachers.");

            var teachers = context.Users.Where(t => t.Id == teacherDepartments.TeacherId);

            return teachers;
        }

        #endregion

        #region Signatory Methods

        public static async Task<Signatory> GetSignatory(this ApplicationDbContext context)
        {
            var proxy = await context.Proxies.OrderByDescending(p => p.DateStart).FirstOrDefaultAsync();
            if (proxy == null)
                throw new Exception();

            var signatory = await context.Signatories.FirstOrDefaultAsync(s => s.Id == proxy.SignatoryId);
            if (signatory is null)
                throw new Exception();

            return signatory;
        }

        #endregion
    }
}
