namespace Tusur.Ais.Models.Response
{
    public class GetContractBasicsResponseModel
    {
        public struct Information
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string? Patronymic { get; set; }
        }

        public string UserName { get; set; }
        public Information FullName { get; set; }
    }
}
