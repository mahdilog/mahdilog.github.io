using TravelOrganization2.Model.Dto.Token;

namespace TravelOrganization2.Model.Dto.Login
{
    public class LoginResponseUserDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public LoginReceiveResultDto Token { get; set; }
    }
}
