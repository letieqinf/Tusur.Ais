namespace Tusur.Ais.Models.Response
{
    public class GetProfileResponseModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }
    }
}
