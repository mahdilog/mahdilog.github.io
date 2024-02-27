namespace TravelOrganization2.Model.Entities
{
    public class UserToken
    {
        public int TokenId { get; set; }
        public string TokenHash { get; set; }
        public DateTime TokenExp { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExp { get; set; }

        public User User { get; set; }
        public long UserId { get; set; }

    }
}
