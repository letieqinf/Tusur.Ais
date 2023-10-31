using Microsoft.AspNetCore.Identity;

namespace Tusur.Ais.Data.Entities.Users
{
    public class User : IdentityUser<Guid>
    {
        // Fields

        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }

        // Dependencies

        public Student? Student { get; set; }
        public Teacher? Teacher { get; set; }
        public Secretary? Secretary { get; set; }
        public EducationDepartment? EducationDepartment { get; set; }
    }
}
