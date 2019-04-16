using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trojantrading.Repositories;
using Trojantrading.Util;
using Trojantrading.Models;

namespace Trojantrading.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
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
        

        [Route("/dashboard")]
        public IActionResult Dashboard()
        {
            string totalUserNumber = _userRepository.GetTotalUserNumber().ToString();//user
            string totalNewUserNumber = _userRepository.GetNewUserNumber().ToString();
            string totalProduct = _productRepository.GetTotalProducts().ToString();//product
            string totalOrder = _orderRepository.GetToatalOrderNumber().ToString();//order
            string totalNewOrder = _orderRepository.GetNewOrderNumber().ToString();//new order
            return null;
        }

        public IActionResult GetExcel()
        {
            return null;
        }

        [HttpGet("GetUserByAccount")]
        [NoCache]
        [ProducesResponseType(typeof(User), 200)]
        public IActionResult GetUserByAccount(string userName)
        {
            var userInfo = _userRepository.Get(userName);
            return Ok(userInfo);
        }

        [HttpGet("GetShippingAddress")]
        [NoCache]
        [ProducesResponseType(typeof(ShippingAddress), 200)]
        public IActionResult GetShippingAddress(int userId)
        {
            var result = _userRepository.GetShippingAddress(userId);
            return Ok(result);
        }

        [HttpPost("UpdateUser")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public IActionResult UpdateUser([FromBody]User user)
        {
            var result = _userRepository.Update(user);
            return Ok(result);
        }

        public async Task<IActionResult> GetProducts()
        {
            return null;
        }

        public async Task<IActionResult> GetOrders()
        {
            return null;
        }
    }
}