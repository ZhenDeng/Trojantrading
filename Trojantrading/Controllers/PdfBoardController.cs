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
                                        Email = workSheet.Cells[i, 16].Value == null ? "" : workSheet.Cells[i, 16].Value.ToString()
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
                stringBuilder.Append("<div style='padding: 0; margin: 0; height: 100%; background:#fbfaf7;'>");
                stringBuilder.Append("<table width='100%' height='100%' align='center' cellspacing='0' cellpadding='0' bgcolor='#fbfaf7'>");
                stringBuilder.Append("<tbody>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'>&nbsp;</td></tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='color:#343731;'><table width='600px' bgcolor='#ffffff' cellspacing='0' cellpadding='0' align='center' border='0' style='border:1px solid #232323;text-align:center'><tbody><tr><td style='padding:10px;color:#343731'>");
                stringBuilder.Append("<img src='https://via.placeholder.com/150' width='80px' style='margin:0 auto;display:block'>");
                stringBuilder.Append("<h2 style='font-weight:400;text-transform:uppercase'>Trojan Trading Company PTY LTD</h2><h4 style='font-weight:400;text-transform:uppercase'>Australia</h4>");
                stringBuilder.Append("</td></tr>");
                stringBuilder.Append("<tr style='text-align:left'><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'><table width='90%'><tbody>");
                stringBuilder.Append("<tr><td valign='top' width='50%' style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'><h3 style='margin:15px 20px 10px 20px;font-size:1.1em;color:#454545;text-align:left'>Shipping Address</h3>");
                stringBuilder.Append("<p style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545;margin:12px 20px 10px 20px;margin-bottom:0'>" + currentUser.Account + "<br>" + currentUser.ShippingCustomerName + "<br>");
                stringBuilder.Append(currentUser.ShippingStreetNumber + " " + currentUser.ShippingAddressLine + "<br> " + currentUser.ShippingSuburb + ", " + currentUser.ShippingState + ", " + currentUser.ShippingPostCode + "<br>");
                stringBuilder.Append("<strong>Email:</strong><a href='" + currentUser.Email + "' target='_blank'>" + currentUser.Email + "</a><br><strong>Phone:</strong>" + currentUser.Phone + "</p></td>");
                stringBuilder.Append("<td valign='top' width='50%' style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'><h3 style='margin:15px 20px 10px 20px;font-size:1.1em;color:#454545;text-align:left'>Billing Address</h3>");
                stringBuilder.Append("<p style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545;margin:12px 20px 10px 20px;margin-bottom:0'>" + currentUser.Account + "<br>" + currentUser.BillingCustomerName + "<br>");
                stringBuilder.Append(currentUser.BillingStreetNumber + " " + currentUser.BillingAddressLine + "<br> " + currentUser.BillingSuburb + ", " + currentUser.BillingState + ", " + currentUser.BillingPostCode + "<br>");
                stringBuilder.Append("<strong>Email:</strong><a href='" + currentUser.Email + "' target='_blank'>" + currentUser.Email + "</a><br><strong>Phone:</strong>" + currentUser.Phone + "</p></td>");
                stringBuilder.Append("</tr></tbody></table></td></tr>");
                stringBuilder.Append("</tbody></table></td></tr>");
                stringBuilder.Append("<tr><td width='600' style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'>");
                stringBuilder.Append("<table cellspacing='0' cellpadding='0' width='600' align='center' bgcolor='#ffffff' border='0' style='border-right:1px solid #d3d3d3;border-left:1px solid #d3d3d3'>");
                stringBuilder.Append("<tbody><tr><td align='center' style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'>&nbsp;</td></tr>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'>");
                stringBuilder.Append("<h3 style='margin:15px 20px 10px 20px;font-size:1.1em;color:#454545;text-align:center'>Your Order #" + orderId + " for Customer " + currentUser.Account + "</h3>");
                stringBuilder.Append("</td></tr>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'><table width='90%' cellspacing='0' cellpadding='0' align='center' bgcolor='#ffffff' border='0'>");
                stringBuilder.Append("<tbody><tr>");
                stringBuilder.Append("<td style='padding:1em 0.25em;border-bottom:1px solid #c4c4c4'></td>");
                stringBuilder.Append("<td style='padding:1em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong style ='font-size:10px'>WLP ex.GST</strong></td>");
                stringBuilder.Append("<td style='padding:1em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong style='font-size:10px'>Buy Price ex.GST</strong></td>");
                stringBuilder.Append("<td style='padding:1em 0.25em;border-bottom:1px solid #c4c4c4;text-align:center'><strong style ='font-size:10px'>Order Qty</strong></td>");
                stringBuilder.Append("<td style='padding:1em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong style='font-size:10px'>Line Amount ex.GST</strong></td></tr>");
                foreach (var item in cart.ShoppingItems)
                {
                    stringBuilder.Append("<tr><td style='padding:0 0.25em;border-bottom:1px solid #c4c4c4'><h4 style='margin:0'>#" + item.Product.Id + " " + item.Product.Name + "</h4></td>");
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
                stringBuilder.Append("<tr><td colspan='1'>&nbsp;</td><td colspan='3' width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4'>Payment Method</td>");
                stringBuilder.Append("<td width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong><span>" + order.ClientMessage + "</span></strong></td></tr>");
                stringBuilder.Append("<tr><td colspan='1'>&nbsp;</td><td colspan='3' width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4'>You Will Pay Excl.GST</td>");
                stringBuilder.Append("<td width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong>$<span>" + String.Format("{0:0.00}", priceExclGst) + "</span></strong></td></tr>");
                stringBuilder.Append("<tr><td colspan='1'>&nbsp;</td><td colspan='3' width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4'>GST</td>");
                stringBuilder.Append("<td width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong>$<span>" + String.Format("{0:0.00}", gst) + "</span></strong></td></tr>");
                stringBuilder.Append("<tr><td colspan='1'>&nbsp;</td><td colspan='3' width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4'>You will pay Inc.GST</td>");
                stringBuilder.Append("<td width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong>$<span>" + String.Format("{0:0.00}", priceIncGst) + "</span></strong></td></tr>");
                stringBuilder.Append("<tr><td colspan='1'>&nbsp;</td><td colspan='3' width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4'>Total Discount Earned</td>");
                stringBuilder.Append("<td width='100' style='padding:0.5em 0.25em;border-bottom:1px solid #c4c4c4;text-align:right'><strong>$<span>" + String.Format("{0:0.00}", discount) + "</span></strong></td></tr>");
                stringBuilder.Append("</tfoot></table></td></tr>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'>&nbsp;</td></tr>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545;background:#f6f4ef;text-align:center;border-bottom:1px solid #d3d3d3'><p style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#45659d;margin:12px 20px 10px 20px;text-decoration:none'><a style='color:#454545;text-decoration:none' target='_blank'><strong>https://XXXXXXXX</strong></a></p></td></tr>");
                stringBuilder.Append("</tbody></table></td></tr>");
                stringBuilder.Append("<tr><td style='font:12px/1.5 Arial,Helvetica,sans-serif;color:#454545'>&nbsp;</td></tr>");
                stringBuilder.Append("</tbody></table></div>");

                string pdfBody = stringBuilder.ToString();
                HtmlToPdf Renderer = new HtmlToPdf();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "order_"+orderId+".pdf");
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
    }
}