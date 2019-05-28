using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Trojantrading.Models;
using Trojantrading.Repositories;
using Trojantrading.Service;
using Trojantrading.Util;
using IronPdf;

namespace Trojantrading.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class PdfBoardController : Controller
    {
        private readonly IPdfBoardRepository pdfBoardRepository;
        private readonly TrojantradingDbContext trojantradingDbContext;
        private readonly IShare share;
        private readonly IUserRepository userRepository;
        private readonly IShoppingCartRepository shoppingCartRepository;

        public PdfBoardController(
            IPdfBoardRepository pdfBoardRepository,
            TrojantradingDbContext trojantradingDbContext,
            IShare share,
            IUserRepository userRepository,
            IShoppingCartRepository shoppingCartRepository)
        {
            this.pdfBoardRepository = pdfBoardRepository;
            this.trojantradingDbContext = trojantradingDbContext;
            this.share = share;
            this.userRepository = userRepository;
            this.shoppingCartRepository = shoppingCartRepository;
        }

        [HttpGet("GetPdfBoards")]
        [NoCache]
        [ProducesResponseType(typeof(List<PdfBoard>), 200)]
        public IActionResult GetPdfBoards()
        {
            return Ok(pdfBoardRepository.GetPdfBoards());
        }

        [HttpPost("DeletePdfBoards")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult DeletePdfBoards([FromBody]PdfBoard pdf)
        {
            return Ok(pdfBoardRepository.DeletePdfBoards(pdf));
        }

        #region SavePdf
        [HttpPost("SavePdf"), DisableRequestSizeLimit]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult SavePdf(string type)
        {
            try
            {
                var httpRequest = HttpContext.Request;
                if (httpRequest.Form.Files.Count > 0)
                {
                    var uploadFile = httpRequest.Form.Files[0];  // get the uploaded file
                    if (uploadFile != null && uploadFile.Length > 0)
                    {

                        var path = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot",
                                    type.ToUpperInvariant() + " PRICE LIST" + Path.GetExtension(uploadFile.FileName).ToLowerInvariant());

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            uploadFile.CopyTo(stream);
                            stream.Close();
                        }

                        PdfBoard pdfBoard = new PdfBoard()
                        {
                            Title = type.ToUpperInvariant() + " PRICE LIST" + Path.GetExtension(uploadFile.FileName).ToLowerInvariant(),
                            Path = path
                        };

                        if (trojantradingDbContext.PdfBoards.Where(pdf => pdf.Path == pdfBoard.Path).Count() == 0)
                        {
                            trojantradingDbContext.PdfBoards.Add(pdfBoard);
                            trojantradingDbContext.SaveChanges();
                            trojantradingDbContext.Dispose();
                        }

                        return Ok(new ApiResponse
                        {
                            Status = "success",
                            Message = type.ToUpperInvariant() + " PRICE LIST Pdf file uploaded."
                        });
                    }
                }
                return Ok(new ApiResponse
                {
                    Status = "fail",
                    Message = "Upload file is empty"
                });
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse
                {
                    Status = "fail",
                    Message = ex.Message
                });
            }
        }
        #endregion

        #region Upload Users
        [HttpPost("UploadUsers"), DisableRequestSizeLimit]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult UploadUsers()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                if (httpRequest.Form.Files.Count > 0)
                {
                    var uploadFile = httpRequest.Form.Files[0];  // get the uploaded file
                    if (uploadFile != null && uploadFile.Length > 0)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ImportUsers.xlsx");
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            uploadFile.CopyTo(stream);
                            stream.Close();
                        }
                        FileInfo file = new FileInfo(path);
                        using (ExcelPackage package = new ExcelPackage(file))
                        {
                            ExcelWorksheet workSheet = package.Workbook.Worksheets["Sheet1"];
                            int totalRows = workSheet.Dimension.Rows;

                            List<User> userList = new List<User>();

                            for (int i = 3; i <= totalRows; i++)
                            {
                                if (workSheet.Cells[i, 1].Value != null)
                                {
                                    StringBuilder builder = new StringBuilder();
                                    builder.Append(share.RandomString(4, true));
                                    builder.Append(share.RandomNumber(1000, 9999));
                                    builder.Append(share.RandomString(2, false));
                                    userList.Add(new User()
                                    {
                                        Account = workSheet.Cells[i, 1].Value == null ? "" : workSheet.Cells[i, 1].Value.ToString(),
                                        Password = builder.ToString(),
                                        BussinessName = workSheet.Cells[i, 2].Value == null ? "" : workSheet.Cells[i, 2].Value.ToString(),
                                        BillingStreetNumber = workSheet.Cells[i, 3].Value == null ? "" : workSheet.Cells[i, 3].Value.ToString(),
                                        BillingAddressLine = workSheet.Cells[i, 4].Value == null ? "" : workSheet.Cells[i, 4].Value.ToString(),
                                        BillingSuburb = workSheet.Cells[i, 5].Value == null ? "" : workSheet.Cells[i, 5].Value.ToString(),
                                        BillingState = workSheet.Cells[i, 6].Value == null ? "" : workSheet.Cells[i, 6].Value.ToString(),
                                        BillingPostCode = workSheet.Cells[i, 7].Value == null ? "" : workSheet.Cells[i, 7].Value.ToString(),
                                        ShippingStreetNumber = workSheet.Cells[i, 8].Value == null ? "" : workSheet.Cells[i, 8].Value.ToString(),
                                        ShippingAddressLine = workSheet.Cells[i, 9].Value == null ? "" : workSheet.Cells[i, 9].Value.ToString(),
                                        ShippingSuburb = workSheet.Cells[i, 10].Value == null ? "" : workSheet.Cells[i, 10].Value.ToString(),
                                        ShippingState = workSheet.Cells[i, 11].Value == null ? "" : workSheet.Cells[i, 11].Value.ToString(),
                                        ShippingPostCode = workSheet.Cells[i, 12].Value == null ? "" : workSheet.Cells[i, 12].Value.ToString(),
                                        Phone = workSheet.Cells[i, 13].Value == null ? "" : workSheet.Cells[i, 13].Value.ToString(),
                                        CompanyPhone = workSheet.Cells[i, 14].Value == null ? "" : workSheet.Cells[i, 14].Value.ToString(),
                                        Mobile = workSheet.Cells[i, 15].Value == null ? "" : workSheet.Cells[i, 15].Value.ToString(),
                                        Email = workSheet.Cells[i, 16].Value == null ? "" : workSheet.Cells[i, 16].Value.ToString(),
                                        Role = workSheet.Cells[i, 17].Value == null ? "" : workSheet.Cells[i, 17].Value.ToString()
                                    });
                                }
                            }
                            package.Stream.Close();
                            package.Dispose();
                            trojantradingDbContext.Users.AddRange(userList);
                            trojantradingDbContext.SaveChanges();
                            trojantradingDbContext.Dispose();
                        }
                        return Ok(new ApiResponse
                        {
                            Status = "success",
                            Message = " PRICE LIST Pdf file uploaded."
                        });
                    }
                }
                return Ok(new ApiResponse
                {
                    Status = "fail",
                    Message = "Upload file is empty"
                });
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse
                {
                    Status = "fail",
                    Message = ex.Message
                });
            }
        }
        #endregion

        #region write pdf
        [HttpGet("WritePdf")]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult WritePdf(int orderId, double gst, double priceExclGst, double discount, int userId)
        {
            try
            {
                var order = trojantradingDbContext.Orders.Where(x => x.Id == orderId).FirstOrDefault();
                var cart = shoppingCartRepository.GetShoppingCartByID(order.ShoppingCartId, userId);
                var currentUser = userRepository.GetUserByAccount(userId);
                double priceIncGst = priceExclGst + gst;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<html lang='en'><head><style>");
                stringBuilder.Append(".clearfix:after{content:'';display: table;clear: both;} a{color: #5D6975;text-decoration: underline;} body{position: relative;width: 21cm;height: 29.7cm;margin: 0 auto;color: #001028;background: #FFFFFF;font-family: Arial, sans-serif;font-size: 12px;font-family: Arial;}");
                stringBuilder.Append("header{padding: 10px 0;margin-bottom: 30px;}#logo{text-align: center;margin-bottom: 10px; font-size: 2em}#logo img{width: 90px;}h1 {border-top: 1px solid #5D6975;border-bottom: 1px solid #5D6975;color: #5D6975;font-size: 1.5em;line-height: 1.4em;font-weight: normal;text-align: center;margin: 0 0 20px 0;}");
                stringBuilder.Append("#project {float: left; font-size: 18px}#project span {color: #5D6975;text-align:left;display: inline-block;font-size: 0.8em;}#company {float: right;text-align: right; font-size: 18px}#company span {color: #5D6975;text-align: right;display: inline-block;font-size: 0.8em;}#project div,#company div {white-space: nowrap;}");
                stringBuilder.Append("table {width: 100%;border-collapse: collapse;border-spacing: 0;margin-bottom: 20px;}table tr:nth-child(2n-1) td {background: #F5F5F5;}table th,table td {text-align: center;}table th {padding: 5px 20px;color: #5D6975;border-bottom: 1px solid #C1CED9;white-space: nowrap;font-weight: normal;}table.service,table.desc {text-align: left;}table td {padding: 20px;text-align: right;}table td.service,table td.desc {vertical-align: top;}table td.unit,table td.qty,table td.total {font-size: 1.2em;}table td.grand {border-top: 1px solid #5D6975;}");
                stringBuilder.Append("#notices,#notices .notice {color: #5D6975;font-size: 1.2em;}footer {font-size: 1.5em;color: #5D6975;width: 100%;height: 30px;position: absolute;bottom: 0;border-top: 1px solid #C1CED9;padding: 8px 0;text-align: center;}");
                stringBuilder.Append("</style></head>");
                stringBuilder.Append("<body><header class='clearfix'><div id='logo'><p>Trojan Trading Company PTY LTD</p></div>");
                stringBuilder.Append("<h1>Your Order #" + orderId + " for Customer " + currentUser.Account + "</h1>");
                stringBuilder.Append("<div id='company' class='clearfix'><div>SHIPPING ADDRESS</div><div><span>ACCOUNT:&nbsp;&nbsp;</span>" + currentUser.Account + "</div><div><span>CUSTOMER:&nbsp;&nbsp;</span>" + currentUser.ShippingCustomerName + "</div><div><span>ADDRESS:&nbsp;&nbsp;</span>" + currentUser.ShippingStreetNumber + " " + currentUser.ShippingAddressLine + "</div><div>" + currentUser.ShippingSuburb + ", " + currentUser.ShippingState + ", " + currentUser.ShippingPostCode + "</div><div><span>EMAIL:&nbsp;&nbsp;</span> <a href='" + currentUser.Email + "' target='_blank'>" + currentUser.Email + "</a></div><div><span>PHONE:&nbsp;&nbsp;</span>" + currentUser.Phone + "</div></div>");
                stringBuilder.Append("<div id='project'><div>BILLING ADDRESS</div><div><span>ACCOUNT:&nbsp;&nbsp;</span>" + currentUser.Account + "</div><div><span>CUSTOMER:&nbsp;&nbsp;</span>" + currentUser.BillingCustomerName + "</div><div><span>ADDRESS:&nbsp;&nbsp;</span>" + currentUser.BillingStreetNumber + " " + currentUser.BillingAddressLine + "</div><div>" + currentUser.BillingSuburb + ", " + currentUser.BillingState + ", " + currentUser.BillingPostCode + "</div><div><span>EMAIL:&nbsp;&nbsp;</span> <a href='" + currentUser.Email + "' target='_blank'>" + currentUser.Email + "</a></div><div><span>PHONE:&nbsp;&nbsp;</span>" + currentUser.Phone + "</div></div></header>");
                stringBuilder.Append("<main><table><thead><tr><th>Product Name</th><th>WLP ex.GST</th><th>Buy Price ex.GST</th><th>Order Qty</th><th>Line Amount ex.GST</th></tr></thead><tbody>");
                foreach (var item in cart.ShoppingItems)
                {
                    stringBuilder.Append("<tr><td class='service'>#" + item.Product.Id + " " + item.Product.Name + "</td><td class='total'>$" + item.Product.OriginalPrice + "</td>");
                    if (currentUser.Role.ToLower() == "agent")
                    {
                        stringBuilder.Append("<td class='unit'>$" + String.Format("{0:0.00}", item.Product.AgentPrice) + "</td>");
                    }
                    else if (currentUser.Role.ToLower() == "wholesaler")
                    {
                        stringBuilder.Append("<td class='unit'>$" + String.Format("{0:0.00}", item.Product.WholesalerPrice) + "</td>");
                    }
                    stringBuilder.Append("<td class='qty'>" + item.Amount + "</td>");
                    if (currentUser.Role.ToLower() == "agent")
                    {
                        stringBuilder.Append("<td class='total'>$" + String.Format("{0:0.00}", item.Product.AgentPrice) + "</td></tr>");
                    }
                    else if (currentUser.Role.ToLower() == "wholesaler")
                    {
                        stringBuilder.Append("<td class='total'>$" + String.Format("{0:0.00}", item.Product.WholesalerPrice) + "</td></tr>");
                    }
                }
                stringBuilder.Append("<tr><td colspan='4'> Payment Method</ td ><td class='total'>" + String.Format("{0}", cart.PaymentMethod == "onaccount" ? "On Account" : "Prepayment") + "</td></tr>");
                stringBuilder.Append("<tr><td colspan='4'> You Will Pay Excl.GST</ td ><td class='total'>" + String.Format("{0:0.00}", priceExclGst) + "</td></tr>");
                stringBuilder.Append("<tr><td colspan='4'> GST</ td ><td class='total'>" + String.Format("{0:0.00}", gst) + "</td></tr>");
                stringBuilder.Append("<tr><td colspan='4'> Total Discount Earned</ td ><td class='total'>" + String.Format("{0:0.00}", discount) + "</td></tr>");
                stringBuilder.Append("<tr><td colspan='4' class='grand total'> You will pay Inc.GST</ td ><td class='grand total'>" + String.Format("{0:0.00}", priceIncGst) + "</td></tr>");
                stringBuilder.Append("</tbody></table>");
                stringBuilder.Append("<div id='notices'><div>NOTICE:</div><div class='notice'>A finance charge of 1.5% will be made on unpaid balances after 30 days.</div></div>");
                stringBuilder.Append("</main>");
                stringBuilder.Append("<footer><a href='' target='_blank'>http://xxxxxxxx</a></footer>");
                stringBuilder.Append("</body></html>");
                string pdfBody = stringBuilder.ToString();
                HtmlToPdf Renderer = new HtmlToPdf();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "order_" + orderId + ".pdf");
                var PDF = Renderer.RenderHtmlAsPdf(pdfBody);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    PDF.Stream.CopyTo(stream);
                    stream.Close();
                }
                return Ok(new ApiResponse
                {
                    Status = "success",
                    Message = "Successfully write order to pdf file",
                });
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse
                {
                    Status = "fail",
                    Message = ex.Message
                });
            }
        }
        #endregion

        #region SaveImage
        [HttpPost("SaveImage"), DisableRequestSizeLimit]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public IActionResult SaveImage()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                if (httpRequest.Form.Files.Count > 0)
                {
                    var uploadFile = httpRequest.Form.Files[0];  // get the uploaded file
                    if (uploadFile != null && uploadFile.Length > 0)
                    {

                        var path = Path.Combine(
                                    Directory.GetCurrentDirectory(), "wwwroot", "img", uploadFile.FileName);

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            uploadFile.CopyTo(stream);
                            stream.Close();
                        }

                        if (trojantradingDbContext.HeadInformations.Where(img => img.ImagePath == path).Count() == 0)
                        {
                            HeadInformation imageModel = new HeadInformation();
                            imageModel.Content = "";
                            imageModel.ImagePath = uploadFile.FileName;
                            trojantradingDbContext.HeadInformations.Add(imageModel);
                            trojantradingDbContext.SaveChanges();
                            trojantradingDbContext.Dispose();
                        }

                        return Ok(new ApiResponse
                        {
                            Status = "success",
                            Message = "Image file uploaded."
                        });
                    }
                }
                return Ok(new ApiResponse
                {
                    Status = "fail",
                    Message = "Upload file is empty"
                });
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse
                {
                    Status = "fail",
                    Message = ex.Message
                });
            }
        }
        #endregion
    }
}