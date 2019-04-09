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
using Trojantrading.Util;

namespace Trojantrading.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;

        public UserController(IUserRepository userRepository, IOptions<AppSettings> appSettings)
        {
            _userRepository = userRepository;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        [NoCache]
        [ProducesResponseType(typeof(UserResponse), 200)]
        public IActionResult Authenticate([FromBody]User userModel)
        {
            if (userModel.Account != "admin" || userModel.Password != "123")
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
                UserName = userModel.Account,
                Token = tokenString
            });
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

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var user = _userRepository.Get(id);
        //    return null;
        //}

        public async Task<IActionResult> GetAll()
        {
            var users = _userRepository.GetAll();
            return null;
        }
    }
}