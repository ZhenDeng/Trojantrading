using Microsoft.AspNetCore.Mvc;
using Trojantrading.Repositories;
using Trojantrading.Util;
using Trojantrading.Models;
using Microsoft.AspNetCore.Authorization;

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
        private readonly IHeadInformationRepository _headInformationRepository;
        private readonly IPdfBoardRepository _pdfBoardRepository;


        public AdminController(IOrderRepository orderRepository, IUserRepository userRepository,
            IProductRepository productRepository,
            IHeadInformationRepository headInformationRepository, IPdfBoardRepository pdfBoardRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _headInformationRepository = headInformationRepository;
            _pdfBoardRepository = pdfBoardRepository;
        }

        [HttpGet("GetUserByAccount")]
        [NoCache]
        [ProducesResponseType(typeof(User), 200)]
        public IActionResult GetUserByAccount(string userName)
        {
            var userInfo = _userRepository.GetUserByAccount(userName);
            return Ok(userInfo);
        }

        [HttpGet("GetUserWithAddress")]
        [NoCache]
        [ProducesResponseType(typeof(User), 200)]
        public IActionResult GetUserWithAddress(string userName)
        {
            var userInfo = _userRepository.GetUserWithAddress(userName);
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
        public IActionResult GetUserWithRole(string userName)
        {
            var userInfo = _userRepository.GetUserWithRole(userName);
            return Ok(userInfo);
        }

        [HttpGet("ValidatePassword")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public IActionResult ValidatePassword(string userName, string password)
        {
            var result = _userRepository.ValidatePassword(userName, password);
            return Ok(result);
        }

        [HttpGet("UpdatePassword")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public IActionResult UpdatePassword(string userName, string password)
        {
            var result = _userRepository.UpdatePassword(userName, password);
            return Ok(result);
        }
    }
}