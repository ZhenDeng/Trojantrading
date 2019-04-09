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
        public async Task<IActionResult> Dashboard()
        {
            string totalUserNumber = _userRepository.GetTotalUserNumber().ToString();//user
            string totalNewUserNumber = _userRepository.GetNewUserNumber().ToString();
            string totalProduct = _productRepository.GetTotalProducts().ToString();//product
            string totalOrder = _orderRepository.GetToatalOrderNumber().ToString();//order
            string totalNewOrder = _orderRepository.GetNewOrderNumber().ToString();//new order
            return null;
        }

        public async Task<IActionResult> GetExcel()
        {
            return null;
        }

        [HttpGet("GetUserByAccount")]
        [NoCache]
        [ProducesResponseType(typeof(User), 200)]
        public async Task<IActionResult> GetUserByAccount(string userName)
        {
            var userInfo = await _userRepository.Get(userName);
            return Ok(userInfo);
        }

        [HttpPost("UpdateUser")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public async Task<IActionResult> UpdateUser([FromBody]User user)
        {
            var result = await _userRepository.Update(user);
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