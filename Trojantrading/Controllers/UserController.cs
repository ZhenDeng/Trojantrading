using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Trojantrading.Models;
using Trojantrading.Repositories;
using Trojantrading.Service;
using Trojantrading.Util;

namespace Trojantrading.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IShare _share;
        private readonly AppSettings _appSettings;

        public UserController(
            IUserRepository userRepository,
            IOptions<AppSettings> appSettings,
            IShare share)
        {
            _userRepository = userRepository;
            _share = share;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [NoCache]
        [ProducesResponseType(typeof(UserResponse), 200)]
        public IActionResult Authenticate([FromBody]User userModel)
        {
            User user = _userRepository.GetUserWithRole(userModel.Account);
            if (user.Status.ToLower() == "active")
            {
                if (userModel.Account != user.Account || userModel.Password != user.Password)
                {
                    return Unauthorized();
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Name, userModel.Account)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(20),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info and token to store client side
                return Ok(new UserResponse
                {
                    UserName = user.Account,
                    Token = tokenString,
                    Role = user.Role.Name
                });
            }
            else
            {
                return Ok(new UserResponse
                {
                    UserName = "",
                    Token = "",
                    Role = ""
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("PasswordRecover")]
        [NoCache]
        [ProducesResponseType(typeof(UserResponse), 200)]
        public IActionResult PasswordRecover(string email, string userName)
        {
            User userModel = _userRepository.Get(userName);
            if (userModel.Status.ToLower() == "active")
            {
                if (userModel.Account != userName)
                {
                    return Unauthorized();
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                new Claim(ClaimTypes.Email, userModel.Email)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                bool isUpdated = _share.SendEmail(_share.GetConfigKey("EmailFrom"), email, "Trojantrading Password Reset", string.Format("Click url below to reset password:\r\n\r\n{0}", "http://localhost:56410/recover/" + tokenString));
                // return basic user info and token to store client side
                return Ok(new UserResponse
                {
                    UserName = userModel.Account,
                    Token = tokenString
                });
            }
            else
            {
                return Ok(new UserResponse
                {
                    UserName = "",
                    Token = ""
                });
            }
        }

        [AllowAnonymous]
        [HttpGet("ValidateEmail")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public IActionResult ValidateEmail(string email)
        {
            var result = _userRepository.ValidateEmail(email);
            return Ok(result);
        }

        [HttpGet("UpdatePassword")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<IActionResult> UpdatePassword(string userName, string password)
        {
            var result = await _userRepository.UpdatePassword(userName, password);
            return Ok(result);
        }


        [Route("/info")]
        public async Task<IActionResult> Info()
        {
            return new JsonResult("123");
        }

        [HttpPost("/logout")]
        public async Task<IActionResult> Logout()
        {
            return StatusCode(201);
        }

        [Route("/update")]
        public async Task<IActionResult> Update(User user)
        {
            _userRepository.Update(user);
            return null;
        }

        [Route("/delete")]
        public async Task<IActionResult> Remove(int id)
        {
            _userRepository.Delete(id);
            return null;
        }
    }
}