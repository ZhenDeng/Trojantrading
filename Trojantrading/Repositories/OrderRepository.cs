using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trojantrading.Models;
using Trojantrading.Repositories.Generic;
using Trojantrading.Service;
using Trojantrading.Utilities;

namespace Trojantrading.Repositories
{

    public interface IOrderRepository
    {
        Task<ApiResponse> AddOrder(ShoppingCart cart, double gst, double priceExclGst, double discount);
        Task<Order> Get(int id);
        Task<ApiResponse> DeleteOrder(int id);
        Task<ApiResponse> UpdateOrder(Order order);
        Task<Order> GetOrdersWithShoppingItems(int orderId);
        Task<List<Order>> GetOrdersByUserID(int userId, string dateFrom, string dateTo);
        Task<List<Order>> GetOrdersByDate(string dateFrom, string dateTo);
    }

    public class OrderRepository : IOrderRepository
    {
        private readonly IUserRepository userRepository;
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IShare share;
        private readonly IRepositoryV2<Order> _orderDataRepository;
        private readonly IRepositoryV2<ShoppingCart> _shoppingCartDataRepository;
        private readonly IRepositoryV2<ShoppingItem> _shoppingItemDataRepository;

        public OrderRepository(
            IUserRepository userRepository,
            IShoppingCartRepository shoppingCartRepository,
            IShare share,
            IRepositoryV2<Order> orderDataRepository,
            IRepositoryV2<ShoppingCart> shoppingCartDataRepository,
            IRepositoryV2<ShoppingItem> shoppingItemDataRepository
            )
        {
            this.userRepository = userRepository;
            this.shoppingCartRepository = shoppingCartRepository;
            this.share = share;
            _orderDataRepository = orderDataRepository;
            _shoppingCartDataRepository = shoppingCartDataRepository;
            _shoppingItemDataRepository = shoppingItemDataRepository;
        }

        public async Task<ApiResponse> AddOrder(ShoppingCart cart, double gst, double priceExclGst, double discount)
        {
            try
            {
                Order order = new Order()
                {
                    CreatedDate = DateTime.Now,
                    TotalItems = cart.TotalItems,
                    TotalPrice = cart.TotalPrice,
                    OrderStatus = "Unprocessed",
                    UserId = cart.UserId,
                    ShoppingCartId = cart.Id,
                    InvoiceNo = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    ClientMessage = cart.PaymentMethod == "onaccount" ? "ON ACCOUNT" : "PREPAYMENT",
                    AdminMessage = "",
                    Balance = 0
                };
                _orderDataRepository.Create(order);
                int orderId;
                if ((await _orderDataRepository.Queryable.GetListAsync()).Count < 1)
                {
                    orderId = 1;
                }
                else {
                    orderId = _orderDataRepository.Queryable.Last().Id + 1;
                }
                var currentUser = await userRepository.GetUserByAccount(cart.UserId);
                double priceIncGst = priceExclGst + gst;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<div style='padding: 0; margin: 0; width:100%; height: 100%; background:#fbfaf7;'>");
                stringBuilder.Append("<table width='100%' height='100%' align='center' cellspacing='0' cellpadding='0' bgcolor='#fbfaf7'>");
                stringBuilder.Append("<tbody>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'>&nbsp;</td></tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='color:#343731;'><table width='600px' bgcolor='#ffffff' cellspacing='0' cellpadding='0' align='center' border='0' style='border:1px solid #232323;text-align:center'><tbody><tr><td style='padding:10px;color:#343731'>");
                stringBuilder.Append("<h2 style='font-weight:400;text-transform:uppercase'>Trojan Trading Company PTY LTD</h2>");
                stringBuilder.Append("</td></tr>");
                stringBuilder.Append("<tr style='text-align:left'><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'><table width='90%'><tbody>");
                stringBuilder.Append("<tr><td valign='top' width='50%' style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'><h3 style='margin:15px 20px 10px 20px;font-size:1.1em;color:#454545;text-align:left'>Shipping Address</h3>");
                stringBuilder.Append("<p style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545;margin:12px 20px 10px 20px;margin-bottom:0'>" + currentUser.BussinessName + "<br>");
                stringBuilder.Append(currentUser.ShippingStreetNumber + " " + currentUser.ShippingAddressLine + "<br> " + currentUser.ShippingSuburb + ", " + currentUser.ShippingState + ", " + currentUser.ShippingPostCode + "<br>");
                stringBuilder.Append("<strong>Email:</strong><a href='" + currentUser.Email + "' target='_blank'>" + currentUser.Email + "</a><br><strong>Phone:</strong>" + currentUser.Phone + "</p></td>");
                stringBuilder.Append("<td valign='top' width='50%' style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'><h3 style='margin:15px 20px 10px 20px;font-size:1.1em;color:#454545;text-align:left'>Billing Address</h3>");
                stringBuilder.Append("<p style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545;margin:12px 20px 10px 20px;margin-bottom:0'>" + currentUser.BussinessName + "<br>");
                stringBuilder.Append(currentUser.BillingStreetNumber + " " + currentUser.BillingAddressLine + "<br> " + currentUser.BillingSuburb + ", " + currentUser.BillingState + ", " + currentUser.BillingPostCode + "<br>");
                stringBuilder.Append("<strong>Email:</strong><a href='" + currentUser.Email + "' target='_blank'>" + currentUser.Email + "</a><br><strong>Phone:</strong>" + currentUser.Phone + "</p></td>");
                stringBuilder.Append("</tr></tbody></table></td></tr>");
                stringBuilder.Append("</tbody></table></td></tr>");
                stringBuilder.Append("<tr><td width='600' style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'>");
                stringBuilder.Append("<table cellspacing='0' cellpadding='0' width='600' align='center' bgcolor='#ffffff' border='0' style='border-right:1px solid #d3d3d3;border-left:1px solid #d3d3d3'>");
                stringBuilder.Append("<tbody><tr><td align='center' style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'>&nbsp;</td></tr>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'>");
                stringBuilder.Append("<h3 style='margin:15px 20px 10px 20px;font-size:1.1em;color:#454545;text-align:center'>Order #" + share.RandomString(4, true)+share.RandomNumber(1000, 9999).ToString() + " for Customer " + currentUser.Account + "</h3>");
                stringBuilder.Append("</td></tr>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'><table width='90%' cellspacing='0' cellpadding='0' align='center' bgcolor='#ffffff' border='0'>");
                stringBuilder.Append("<tbody><tr>");
                stringBuilder.Append("<td style='padding:1em 0.25em;border-bottom:1px solid #c4c4c4'></td>");
                stringBuilder.Append("<td style='padding:1em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong style ='font-size:10px'>Packaging</strong></td>");
                stringBuilder.Append("<td style='padding:1em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong style ='font-size:10px'>Original Price</strong></td>");
                stringBuilder.Append("<td style='padding:1em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong style='font-size:10px'>Buy Price</strong></td>");
                stringBuilder.Append("<td style='padding:1em 0.25em;border-bottom:1px solid #c4c4c4;text-align:center'><strong style ='font-size:10px'>Order Qty</strong></td>");
                stringBuilder.Append("<td style='padding:1em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong style='font-size:10px'>Line Amount</strong></td></tr>");
                foreach (var item in cart.ShoppingItems)
                {
                    stringBuilder.Append("<tr><td style='padding:0 0.25em;border-bottom:1px solid #c4c4c4'><h4 style='margin:0'>" + item.Product.Name + "</h4></td>");
                    stringBuilder.Append("<td style='padding:0 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><p style='font:12px/1.5 Arial,Helvetica,sans-serif;margin:0 0 0 0'>" + item.Packaging + "</p></td>");
                    stringBuilder.Append("<td style='padding:0 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><p style='font:12px/1.5 Arial,Helvetica,sans-serif;margin:0 0 0 0'>$" + item.Product.OriginalPrice + "</p></td>");
                    if (currentUser.Role.ToLower() == "agent")
                    {
                        stringBuilder.Append("<td style='padding:0 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong>$" + String.Format("{0:0.00}", item.Product.AgentPrice) + "</strong></td>");
                    }
                    else if (currentUser.Role.ToLower() == "wholesaler")
                    {
                        stringBuilder.Append("<td style='padding:0 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong>$" + String.Format("{0:0.00}", item.Product.WholesalerPrice) + "</strong></td>");
                    }
                    stringBuilder.Append("<td style='padding:0 0.25em;border-bottom:1px solid #c4c4c4;text-align:center'>" + item.Amount + "</td>");
                    if (currentUser.Role.ToLower() == "agent")
                    {
                        stringBuilder.Append("<td style='padding:0 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><span><strong>$" + String.Format("{0:0.00}", item.Product.AgentPrice * item.Amount) + "</strong></span></td></tr>");
                    }
                    else if (currentUser.Role.ToLower() == "wholesaler")
                    {
                        stringBuilder.Append("<td style='padding:0 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><span><strong>$" + String.Format("{0:0.00}", item.Product.WholesalerPrice * item.Amount) + "</strong></span></td></tr>");
                    }
                }
                stringBuilder.Append("</tbody>");
                stringBuilder.Append("<tfoot>");
                stringBuilder.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='3' width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4'>Payment Method</td>");
                stringBuilder.Append("<td width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong><span>" + String.Format("{0}", cart.PaymentMethod == "onaccount" ? "On Account" : "Prepayment") + "</span></strong></td></tr>");
                stringBuilder.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='3' width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4'>Total Price Excl.GST</td>");
                stringBuilder.Append("<td width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong>$<span>" + String.Format("{0:0.00}", priceExclGst) + "</span></strong></td></tr>");
                stringBuilder.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='3' width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4'>GST</td>");
                stringBuilder.Append("<td width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong>$<span>" + String.Format("{0:0.00}", gst) + "</span></strong></td></tr>");
                stringBuilder.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='3' width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4'>Total Price Inc.GST</td>");
                stringBuilder.Append("<td width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong>$<span>" + String.Format("{0:0.00}", priceIncGst) + "</span></strong></td></tr>");
                stringBuilder.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='3' width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4'>Total Discount Earned</td>");
                stringBuilder.Append("<td width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong>$<span>" + String.Format("{0:0.00}", discount) + "</span></strong></td></tr>");
                stringBuilder.Append("</tfoot></table></td></tr>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'>&nbsp;</td></tr>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545;text-align:center;border-bottom:1px solid #d3d3d3'><p style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#45659d;margin:12px 20px 10px 20px;text-decoration:none'>BSB: </p><p style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#45659d;margin:12px 20px 10px 20px;text-decoration:none'>Account Number: </p><p style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#45659d;margin:12px 20px 10px 20px;text-decoration:none'>Account Name: </p></td></tr>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545;text-align:center;'><p style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#45659d;margin:12px 20px 10px 20px;text-decoration:none'>Diclaimer</p></td></tr>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545;text-align:center;border-bottom:1px solid #d3d3d3'><p style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#45659d;margin:12px 20px 10px 20px;text-decoration:none'>xxxxxx</p></td></tr>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545;background:#f6f4ef;text-align:center;border-bottom:1px solid #d3d3d3'><p style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#45659d;margin:12px 20px 10px 20px;text-decoration:none'><a style='color:#454545;text-decoration:none' target='_blank'><strong>https://XXXXXXXX</strong></a></p></td></tr>");
                stringBuilder.Append("</tbody></table></td></tr>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'>&nbsp;</td></tr>");
                stringBuilder.Append("</tbody></table></div>");
                string emailBody = stringBuilder.ToString();
                share.SendEmail("testprojectemail2019@gmail.com", currentUser.Email, "test", emailBody, "", "", true);

                foreach (var si in cart.ShoppingItems)
                {
                    si.Status = "1";
                    si.Product = null;
                    si.ShoppingCartId = cart.Id;
                }

                _shoppingItemDataRepository.UpdateRange(cart.ShoppingItems.ToList());
                await _shoppingItemDataRepository.SaveChangesAsync();
                cart.Status = "1";
                _shoppingCartDataRepository.Update(cart);
                await _shoppingCartDataRepository.SaveChangesAsync();
                ShoppingCart sc = new ShoppingCart()
                {
                    TotalItems = 0,
                    OriginalPrice = 0,
                    TotalPrice = 0,
                    UserId = order.UserId,
                    Status = "0",
                    PaymentMethod = "onaccount"
                };

                _shoppingCartDataRepository.Create(sc);
                await _shoppingCartDataRepository.SaveChangesAsync();

                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully create order"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "fail",
                    Message = ex.Message
                };
            }

        }

        public async Task<Order> Get(int id)
        {
            var order = await _orderDataRepository.Queryable
                .Where(o => o.Id == id)
                .GetFirstOrDefaultAsync();
            return order;
        }

        public async Task<ApiResponse> DeleteOrder(int id)
        {
            try
            {
                var order = await _orderDataRepository.Queryable.Where(u => u.Id == id).GetFirstOrDefaultAsync();
                _orderDataRepository.Delete(order);
                await _orderDataRepository.SaveChangesAsync();
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully delete order"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "fail",
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse> UpdateOrder(Order order)
        {
            try
            {
                _orderDataRepository.Update(order);
                await _orderDataRepository.SaveChangesAsync();
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully update order"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse()
                {
                    Status = "fail",
                    Message = ex.Message
                };
            }
        }

        public async Task<List<Order>> GetOrdersByUserID(int userId, string dateFrom, string dateTo)
        {

            DateTime fromDate = string.IsNullOrEmpty(dateFrom) ? DateTime.Now.AddMonths(-1).Date : DateTime.ParseExact(dateFrom, @"d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture).Date;
            DateTime toDate = string.IsNullOrEmpty(dateTo) ? DateTime.Now.AddDays(1).Date : DateTime.ParseExact(dateTo, @"d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(1).Date; // end date always next day midnight

            var orders = await _orderDataRepository.Queryable
                .Where(x => x.UserId == userId && DateTime.Compare(x.CreatedDate, fromDate) >= 0 && DateTime.Compare(x.CreatedDate, toDate) <= 0).GetListAsync();

            foreach (var order in orders)
            {
                var orderDetails = await GetOrdersWithShoppingItems(order.Id);
                order.ShoppingCart = orderDetails.ShoppingCart;
                order.User = orderDetails.User;
            }

            return orders;

        }

        public async Task<List<Order>> GetOrdersByDate(string dateFrom, string dateTo)
        {
            List<Order> orders = new List<Order>();

            DateTime fromDate = string.IsNullOrEmpty(dateFrom) ? DateTime.Now.AddMonths(-1).Date : DateTime.ParseExact(dateFrom, @"d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture).Date;
            DateTime toDate = string.IsNullOrEmpty(dateTo) ? DateTime.Now.AddDays(1).Date : DateTime.ParseExact(dateTo, @"d/M/yyyy", System.Globalization.CultureInfo.InvariantCulture).AddDays(1).Date; // end date always next day midnight

            orders = await _orderDataRepository.Queryable
                .Where(x => DateTime.Compare(x.CreatedDate, fromDate) >= 0 && DateTime.Compare(x.CreatedDate, toDate) <= 0).GetListAsync();

            foreach (var order in orders)
            {
                var orderDetails = await GetOrdersWithShoppingItems(order.Id);
                order.ShoppingCart = orderDetails.ShoppingCart;
                order.User = orderDetails.User;
            }
            return orders;

        }

        public async Task<Order> GetOrdersWithShoppingItems(int orderId)
        {
            var order = await _orderDataRepository.Queryable.Where(x => x.Id == orderId).GetFirstOrDefaultAsync();

            var shoppingCart = await shoppingCartRepository.GetShoppingCartByID(order.ShoppingCartId, order.UserId);

            var userDetail = await userRepository.GetUserByAccount(order.UserId);

            order.ShoppingCart = shoppingCart;
            order.User = userDetail;

            return order;
            
        }
    }
}