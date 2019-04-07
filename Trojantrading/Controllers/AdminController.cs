using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trojantrading.Repositories;

namespace Trojantrading.Controllers
{
    [Route("[Controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController:Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHeadInformationRepository _headInformationRepository;
        private readonly IPdfBoardRepository _pdfBoardRepository;


        public AdminController(OrderRepository orderRepository, UserRepository userRepository,
            ProductRepository productRepository,
            HeadInformationRepository headInformationRepository, PdfBoardRepository pdfBoardRepository)
        {
            this._orderRepository = orderRepository;
            this._userRepository = userRepository;
            this._productRepository = productRepository;
            this._headInformationRepository = headInformationRepository;
            this._pdfBoardRepository = pdfBoardRepository;
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


        public async Task<IActionResult> GetUsers()
        {
            return null;
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