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

        [HttpGet("GetUsers")]
        [NoCache]
        [ProducesResponseType(typeof(List<User>), 200)]
        public IActionResult GetUsers()
        {
            var users = _userRepository.GetUsers();
            return Ok(users);
        }

        [HttpPost("UpdateUser")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public IActionResult UpdateUser([FromBody]User user)
        {
            var result = _userRepository.Update(user);
            return Ok(result);
        }

        [HttpPost("AddUser")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public IActionResult AddUser([FromBody]User user)
        {
            var result = _userRepository.AddUser(user);
            return Ok(result);
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