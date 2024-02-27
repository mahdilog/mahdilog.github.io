using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelOrganization2.Model.Context;
using TravelOrganization2.Model.Dto.Token;
using TravelOrganization2.Model.Entities;
using TravelOrganization2.Model.Security.Helper;

namespace TravelOrganization2.Models.Services
{
    public class UserTokenRepository
    {

        private readonly DataBaseContext context;
        private readonly IConfiguration configuration;
        public UserTokenRepository(DataBaseContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }

        public LoginReceiveResultDto CreateToken(User request)
        {
            SecurityHelper securityHelper = new SecurityHelper();

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,  request.Name),
                    new Claim(ClaimTypes.Surname,request.Family),
                    new Claim(ClaimTypes.NameIdentifier,request.Id.ToString())
                };

            string key = configuration["JWtConfig:Key"];

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenexp = DateTime.Now.AddMinutes(int.Parse(configuration["JWtConfig:expires"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWtConfig:issuer"],
                audience: configuration["JWtConfig:audience"],
                expires: tokenexp,
                notBefore: DateTime.Now,
                claims: claims,
                signingCredentials: credentials
                );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshToken = Guid.NewGuid();

            SaveToken(new UserToken()
            {
                TokenExp = tokenexp,
                TokenHash = securityHelper.Getsha256Hash(jwtToken),
                RefreshToken = securityHelper.Getsha256Hash(refreshToken.ToString()),
                RefreshTokenExp = DateTime.Now.AddDays(30),
                User = request
            });

            return new LoginReceiveResultDto()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.ToString()
            };
        }
        public void SaveToken(UserToken userToken)
        {
            context.UserTokens.Add(userToken);
            context.SaveChanges();
        }
        public UserToken FindRefreshToken(string RefreshToken)
        {
            SecurityHelper securityHelper = new SecurityHelper();

            string RefreshTokenHash = securityHelper.Getsha256Hash(RefreshToken);
            var usertoken = context.UserTokens.Include(p=> p.User)
                .SingleOrDefault(p => p.RefreshToken == RefreshTokenHash);

            return usertoken;
        }
        public void DeleteToken(string RefreshToken)
        {
            var token = FindRefreshToken(RefreshToken);
            if(token != null)
            {
                context.UserTokens.Remove(token);
                context.SaveChanges();
            }
        }
    }
}
