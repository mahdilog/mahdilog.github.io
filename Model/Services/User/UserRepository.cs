using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using TravelOrganization2.Model.Context;
using TravelOrganization2.Model.Dto.Common;
using TravelOrganization2.Model.Dto.Login;
using TravelOrganization2.Model.Dto.Register;
using TravelOrganization2.Model.Dto.Repository;
using TravelOrganization2.Model.Entities;
using TravelOrganization2.Model.Security.Helper;
using TravelOrganization2.Models.Services;

namespace TravelOrganization2.Model.Services
{
    public class UserRepository
    {
        private readonly DataBaseContext _context;
        private readonly UserTokenRepository userTokenRepository;
        public UserRepository(DataBaseContext _context,UserTokenRepository userTokenRepository)
        {
            this._context = _context;
            this.userTokenRepository = userTokenRepository;
        }

        private ResultDto Validation(RepositoryRequestUserDto requestuser)
        {
            if (string.IsNullOrWhiteSpace(requestuser.Name))
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "Empty name field",
                };

            if (string.IsNullOrWhiteSpace(requestuser.Family))
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "Empty family field",
                };

            if (string.IsNullOrWhiteSpace(requestuser.Name))
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "Empty name field",
                };

            if (requestuser.Password.Length < 8)
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "Lower than 8 length password",
                };

            if (requestuser.Password != requestuser.Re_Password)
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "Password and Repassword is not equal",
                };

            string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";
            var emialmatch = Regex.Match(requestuser.Email, emailRegex, RegexOptions.IgnoreCase);
            if (!emialmatch.Success)
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "Wrong format for email",
                };

            string phoneregex = "^09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}$";
            var phonematch = Regex.Match(requestuser.PhoneNo, phoneregex, RegexOptions.IgnoreCase);
            if (!phonematch.Success)
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "Wrong format for phone number",
                };

            return new ResultDto()
            {
                IsSuccess = true,
            };
        }
        private User GetUser(string email)
        {
            var user = _context.users.SingleOrDefault(p => p.Email == email);
            return user;
        }

        public ResultDto<RegisterResponseUserDto> Add(RepositoryRequestUserDto requestuser)
        {
            try
            {
                var validationresult = Validation(requestuser);

                if (validationresult.IsSuccess == true)
                {
                    User user = new User()
                    {
                        Name = requestuser.Name,
                        Family = requestuser.Family,
                        Password = new SecurityHelper().Getsha256Hash(requestuser.Password),
                        Re_Password =new SecurityHelper().Getsha256Hash(requestuser.Re_Password),
                        Email = requestuser.Email,
                        PhoneNo = requestuser.PhoneNo,
                        Role = "Customer",
                        InsertTime = DateTime.Now,
                        IsRemoved = false,
                    };

                    _context.users.Add(user);
                    _context.SaveChanges();

                    var token = userTokenRepository.CreateToken(user);

                    return new ResultDto<RegisterResponseUserDto>()
                    {
                        Data = new RegisterResponseUserDto()
                        {
                            Name = user.Name,
                            Family = user.Family,
                            Role = user.Role,
                            Token = token
                        },
                        IsSuccess = true,
                        Message = "Registered Successful"
                    };

                }

                return new ResultDto<RegisterResponseUserDto>()
                {
                    Data = new RegisterResponseUserDto()
                    {
                    },
                    IsSuccess = false,
                    Message = validationresult.Message
                };
            }
            catch (DbUpdateException)
            {
                return new ResultDto<RegisterResponseUserDto>()
                {
                    Data = new RegisterResponseUserDto()
                    {
                    },
                    IsSuccess = false,
                    Message = "Email already exists"
                };
            }

        }
        public ResultDto<LoginResponseUserDto> Login(LoginRequestUserDto request)
        {
            var user = GetUser(request.Email);
            if (user == null)
            {
                return new ResultDto<LoginResponseUserDto>
                {
                    Data = new LoginResponseUserDto()
                    {
                    } ,
                    IsSuccess = false,
                    Message = "Username or password not correct",
                };
            }

            var token = userTokenRepository.CreateToken(user);

            return new ResultDto<LoginResponseUserDto>()
            {
                Data = new LoginResponseUserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    Token = token
                },
                IsSuccess = true,
                Message = "Login successful"
            };
        }

    }
}
