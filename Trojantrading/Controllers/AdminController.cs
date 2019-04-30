using Microsoft.AspNetCore.Mvc;
using Trojantrading.Repositories;
using Trojantrading.Util;
using Trojantrading.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Trojantrading.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class AdminController:Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IPdfBoardRepository _pdfBoardRepository;


        public AdminController(IOrderRepository orderRepository, IUserRepository userRepository,
            IProductRepository productRepository,
            IPdfBoardRepository pdfBoardRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _pdfBoardRepository = pdfBoardRepository;
        }

        [HttpGet("GetUserByAccount")]
        [NoCache]
        [ProducesResponseType(typeof(User), 200)]
        public IActionResult GetUserByAccount(int userId)
        {
            var userInfo = _userRepository.GetUserByAccount(userId);
            return Ok(userInfo);
        }

        [HttpGet("GetUserWithAddress")]
        [NoCache]
        [ProducesResponseType(typeof(User), 200)]
        public IActionResult GetUserWithAddress(int userId)
        {
            var userInfo = _userRepository.GetUserWithAddress(userId);
            return Ok(userInfo);
        }

        [HttpPost("UpdateUser")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public IActionResult UpdateUser([FromBody]User user)
        {
            var result = _userRepository.Update(user);
            return Ok(result);
        }

        [HttpGet("GetUserWithRole")]
        [NoCache]
        [ProducesResponseType(typeof(User), 200)]
        public IActionResult GetUserWithRole(int userId)
        {
            var userInfo = _userRepository.GetUserWithRole(userId);
            return Ok(userInfo);
        }

        [HttpGet("GetUsersWithRole")]
        [NoCache]
        [ProducesResponseType(typeof(List<User>), 200)]
        public IActionResult GetUsersWithRole()
        {
            var users = _userRepository.GetUsersWithRole();
            return Ok(users);
        }

        [HttpGet("ValidatePassword")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public IActionResult ValidatePassword(int userId, string password)
        {
            var result = _userRepository.ValidatePassword(userId, password);
            return Ok(result);
        }

        [HttpGet("UpdatePassword")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public IActionResult UpdatePassword(int userId, string password)
        {
            var result = _userRepository.UpdatePassword(userId, password);
            return Ok(result);
        }
    }
}