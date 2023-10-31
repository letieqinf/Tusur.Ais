namespace Tusur.Ais.Models.Defaults
{
    public static class UserRoles
    {
        public const string User = "user";
        public const string Student = "student";
        public const string Teacher = "teacher";
        public const string Secretary = "secretary";
        public const string EducationDepartment = "education_department";
        public const string Admin = "admin";

        public static IEnumerable<string> AvailableRoles = new[] 
        {
            User, 
            Student,
            Teacher,
            Secretary,
            EducationDepartment,
            Admin
        };
    }
}
