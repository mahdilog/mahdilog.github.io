using System.Text;
using System.Security.Cryptography;

namespace TravelOrganization2.Model.Security.Helper
{
    public class SecurityHelper
    {
        public string Getsha256Hash(string value)
        {
            var algoritm = new SHA256CryptoServiceProvider();
            var byteValue = Encoding.UTF8.GetBytes(value);
            var byteHash = algoritm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}
