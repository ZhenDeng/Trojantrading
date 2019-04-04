using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trojantrading.Repositories;
using Trojantrading.Models;

namespace Trojantrading.Controllers
{
    [Route("[Controller]")]
    public class UserController :Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        [Route("/info")]
        public async Task<IActionResult> Info()
        {
            return new JsonResult("123");
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register()
        {
            return StatusCode(201);
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login()
        {
            return StatusCode(201);
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

        public async Task<IActionResult> Edit(int id)
        {
            var user = _userRepository.Get(id);
            return null;
        }

        public async Task<IActionResult> Create(User user)
        {
            _userRepository.Add(user);
            return null;
        }

        public async Task<IActionResult> GetAll()
        {
            var users = _userRepository.GetAll();
            return null;
        }
    }
}