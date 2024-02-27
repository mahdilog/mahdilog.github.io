namespace TravelOrganization2.Model.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Password { get; set; }
        public string Re_Password { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Role { get; set; }

        public DateTime InsertTime { get; set; }
        public bool IsRemoved { get; set; }

        public ICollection<UserToken> UserTokens { get; set; }

    }
}
