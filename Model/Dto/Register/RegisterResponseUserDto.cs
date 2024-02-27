using TravelOrganization2.Model.Dto.Token;

namespace TravelOrganization2.Model.Dto.Register
{
    public class RegisterResponseUserDto
    {
        public string Name { get; set; }
        public string Family { get; set; }
        public string Role { get; set; }
        public LoginReceiveResultDto Token { get; set; }
    }
}
