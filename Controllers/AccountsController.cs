using Microsoft.AspNetCore.Mvc;
using TravelOrganization2.Model.Dto.Common;
using TravelOrganization2.Model.Dto.Login;
using TravelOrganization2.Model.Dto.Register;
using TravelOrganization2.Model.Dto.Repository;
using TravelOrganization2.Model.Services;
using TravelOrganization2.Model.Services.Email;
using TravelOrganization2.Models.Services;

namespace TravelOrganization2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly UserTokenRepository userTokenRepository;
        private readonly EmailRepository _emailRepository;
        public AccountsController(UserRepository _userRepository,  UserTokenRepository userTokenRepository, EmailRepository _emailRepository)
        {
            this._userRepository = _userRepository;
            this.userTokenRepository = userTokenRepository;
            this._emailRepository = _emailRepository;
        }

        [HttpPost("SignUp")]
        public IActionResult SignUp([FromBody] RegisterRequestUserDto request)
        {

            var result = _userRepository.Add(new RepositoryRequestUserDto()
            {
                Name = request.Name,
                Family = request.Family,
                Password = request.Password,
                Re_Password = request.Re_Password,
                Email = request.Email,
                PhoneNo = request.PhoneNo,
            });

            string url = Url.Action("Post", "User", result, Request.Scheme);

            if (result.IsSuccess == true)
                return Created(url, result);
            else
                return ValidationProblem(new ValidationProblemDetails()
                {
                    Title = "Validation Failed",
                    Detail = result.Message,
                });
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequestUserDto request)
        {
            var result = _userRepository.Login(request);

            if (result.IsSuccess == false)
                return Unauthorized(result.Message);

            return Ok(result);
        }

        [HttpPost]
        [Route("RefreshToken")]
        public IActionResult RefreshToken(string Refreshtoken)
        {
            var result = userTokenRepository.FindRefreshToken(Refreshtoken);
            if (result == null)
            {
                return Unauthorized("Refresh token not found");
            }
            if (result.RefreshTokenExp < DateTime.Now)
            {
                return Unauthorized("Token Expired");
            }

            var token = userTokenRepository.CreateToken(result.User);
            userTokenRepository.DeleteToken(Refreshtoken);

            return Ok(token);
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email,string subject,string body)
        {
            var result = await _emailRepository.SendEmail(email, subject, body);

            if (result.IsSuccess == true)
                return Ok(result);

            return BadRequest(result);
        }
        // GET: api/<UserController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<UserController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<UserController>

        //// PUT api/<UserController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UserController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
