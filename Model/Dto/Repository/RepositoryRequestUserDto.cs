using TravelOrganization2.Model.Dto.Token;

namespace TravelOrganization2.Model.Dto.Repository
{
    public class RepositoryRequestUserDto
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string Password { get; set; }
        public string Re_Password { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Role { get; set; }
        public LoginReceiveResultDto TokenSendReceive { get; set; }

        public DateTime InsertTime { get; set; }
        public bool IsRemoved { get; set; }
    }
}
