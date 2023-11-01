using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Tusur.Ais.Data.Entities.Applications;
using Tusur.Ais.Data.Entities.CompanyStructure;
using Tusur.Ais.Data.Entities.IntergroupRelations;
using Tusur.Ais.Data.Entities.UniversityStructure;
using Tusur.Ais.Data.Entities.Users;

namespace Tusur.Ais.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PracticeApplication>()
                .HasKey(entity => new { entity.PracticeId, entity.ApplicationId, entity.Type });

            builder.Entity<StudentApplication>()
                .HasKey(entity => new { entity.StudentId, entity.ApplicationId });

            builder.Entity<StudentGroup>()
                .HasKey(entity => new { entity.StudentId, entity.GroupId });

            builder.Entity<TeacherDepartment>()
                .HasKey(entity => new { entity.TeacherId, entity.DepartmentId });

            base.OnModelCreating(builder);
        }

        #region Profiles

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Secretary> Secretaries { get; set; }
        public DbSet<EducationDepartment> EducationDepartments { get; set; }

        #endregion

        #region University Structure

        public DbSet<Department> Departments { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Practice> Practices { get; set; }
        public DbSet<PracticeKind> PracticeKinds { get; set; }
        public DbSet<PracticeType> PracticeTypes { get; set; }
        public DbSet<PracticePeriod> PracticePeriods { get; set; }
        public DbSet<RecruitmentYear> RecruitmentYears { get; set; }
        public DbSet<StudyField> StudyFields { get; set; }
        public DbSet<StudyProfile> StudyProfiles { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<TrainingProfile> TrainingProfiles { get; set; }
        public DbSet<StudentContract> StudentContracts { get; set; }
        public DbSet<StudentInGroup> StudentInGroups { get; set; }
        public DbSet<EducationPlan> EducationPlans { get; set; }
        public DbSet<PracticeContract> PracticeContracts { get; set; }

        #endregion

        #region Company Structure

        public DbSet<Company> Companies { get; set; }
        public DbSet<ContactFace> ContactFaces { get; set; }

        #endregion

        #region Applications

        public DbSet<Application> Applications { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Signatory> Signatories { get; set; }
        public DbSet<Proxy> Proxies { get; set; }

        #endregion

        #region Intergroup Relations

        public DbSet<PracticeApplication> PracticeApplications { get; set; }
        public DbSet<StudentApplication> StudentApplications { get; set; }
        public DbSet<StudentGroup> StudentGroups { get; set; }
        public DbSet<TeacherDepartment> TeacherDepartments { get; set; }

        #endregion
    }
}
