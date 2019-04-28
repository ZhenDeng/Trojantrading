using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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
        private readonly TrojantradingDbContext trojantradingDbContext;
        private readonly AppSettings _appSettings;

        public UserController(
            IUserRepository userRepository,
            IOptions<AppSettings> appSettings,
            IShare share,
            TrojantradingDbContext trojantradingDbContext)
        {
            _userRepository = userRepository;
            _share = share;
            this.trojantradingDbContext = trojantradingDbContext;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [NoCache]
        [ProducesResponseType(typeof(UserResponse), 200)]
        public IActionResult Authenticate([FromBody]User userModel)
        {
            int userId = trojantradingDbContext.Users.Where(u => u.Account == userModel.Account && u.Password == userModel.Password).FirstOrDefault().Id;
            User user = _userRepository.GetUserWithRole(userId);
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
                    UserId = user.Id,
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
        public IActionResult PasswordRecover(string email, int userId)
        {
            User userModel = _userRepository.GetUserByAccount(userId);
            if (userModel.Status.ToLower() == "active")
            {
                if (userModel.Id != userId)
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
                    UserId = userModel.Id,
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
    }
}