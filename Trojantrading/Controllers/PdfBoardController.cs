using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trojantrading.Models;
using Trojantrading.Repositories;
using Trojantrading.Repositories.Generic;
using Trojantrading.Service;
using Trojantrading.Util;
using Trojantrading.Utilities;

namespace Trojantrading.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class PdfBoardController : Controller
    {
        private readonly IPdfBoardRepository pdfBoardRepository;
        private readonly IShare share;
        private readonly IUserRepository userRepository;
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;
        private readonly IRepositoryV2<Order> _orderDataRepository;
        private readonly IRepositoryV2<PdfBoard> _pdfBoardDataRepository;
        private readonly IRepositoryV2<User> _userDataRepository;
        private readonly IRepositoryV2<Product> _productDataRepository;
        private readonly IRepositoryV2<PackagingList> _packagingListDataRepository;
        private readonly IRepositoryV2<HeadInformation> _headInformationDataRepository;

        public PdfBoardController(
            IPdfBoardRepository pdfBoardRepository,
            IShare share,
            IUserRepository userRepository,
            IShoppingCartRepository shoppingCartRepository,
            IProductRepository productRepository,
            IRepositoryV2<Order> orderDataRepository,
            IRepositoryV2<PdfBoard> pdfBoardDataRepository,
            IRepositoryV2<User> userDataRepository,
            IRepositoryV2<Product> productDataRepository,
            IRepositoryV2<PackagingList> packagingListDataRepository,
            IRepositoryV2<HeadInformation> headInformationDataRepository)
        {
            this.pdfBoardRepository = pdfBoardRepository;
            this.share = share;
            this.userRepository = userRepository;
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
            _orderDataRepository = orderDataRepository;
            _pdfBoardDataRepository = pdfBoardDataRepository;
            _userDataRepository = userDataRepository;
            _productDataRepository = productDataRepository;
            _packagingListDataRepository = packagingListDataRepository;
            _headInformationDataRepository = headInformationDataRepository;
        }

        [HttpGet("GetPdfBoards")]
        [NoCache]
        [ProducesResponseType(typeof(List<PdfBoard>), 200)]
        public async Task<IActionResult> GetPdfBoards()
        {
            return Ok(await pdfBoardRepository.GetPdfBoards());
        }

        [HttpPost("DeletePdfBoards")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> DeletePdfBoards([FromBody]PdfBoard pdf)
        {
            return Ok(await pdfBoardRepository.DeletePdfBoards(pdf));
        }

        #region SavePdf
        [HttpPost("SavePdf"), DisableRequestSizeLimit]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> SavePdf(string type)
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

                        if (_pdfBoardDataRepository.Queryable.Where(pdf => pdf.Path == pdfBoard.Path).Count() == 0)
                        {
                            _pdfBoardDataRepository.Create(pdfBoard);
                            await _pdfBoardDataRepository.SaveChangesAsync();
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
        public async Task<IActionResult> UploadUsers()
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
                            _userDataRepository.CreateRange(userList);
                            await _userDataRepository.SaveChangesAsync();
                        }
                        return Ok(new ApiResponse
                        {
                            Status = "success",
                            Message = "Users uploaded."
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


        #region Upload Products
        [HttpPost("UploadProducts"), DisableRequestSizeLimit]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<IActionResult> UploadProducts()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                if (httpRequest.Form.Files.Count > 0)
                {
                    var uploadFile = httpRequest.Form.Files[0];  // get the uploaded file
                    if (uploadFile != null && uploadFile.Length > 0)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ImportProducts.xlsx");
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
                            ExcelWorksheet workSheet = package.Workbook.Worksheets["Trojan Product items as at 2407"];
                            int totalRows = workSheet.Dimension.Rows;

                            List<Product> productList = new List<Product>();
                            List<Product> updateProductList = new List<Product>();

                            for (int i = 2; i <= totalRows; i++)
                            {
                                if (workSheet.Cells[i, 2].Value != null)
                                {
                                    if (_productDataRepository.Queryable.Where(x => x.Name == workSheet.Cells[i, 3].Value.ToString()).Count() < 1)
                                    {
                                        productList.Add(new Product()
                                        {
                                            ItemCode = workSheet.Cells[i, 2].Value == null ? "" : workSheet.Cells[i, 2].Value.ToString(),
                                            Name = workSheet.Cells[i, 3].Value == null ? "" : workSheet.Cells[i, 3].Value.ToString(),
                                            Category = workSheet.Cells[i, 4].Value == null ? "" : workSheet.Cells[i, 4].Value.ToString().Trim().Replace(' ', '-'),
                                            OriginalPrice = workSheet.Cells[i, 6].Value == null ? 0 : double.Parse(workSheet.Cells[i, 6].Value.ToString().Replace("$", "").Trim()),
                                            AgentPrice = workSheet.Cells[i, 7].Value == null ? 0 : double.Parse(workSheet.Cells[i, 7].Value.ToString().Replace("$", "").Trim()),
                                            WholesalerPrice = workSheet.Cells[i, 8].Value == null ? 0 : double.Parse(workSheet.Cells[i, 8].Value.ToString().Replace("$", "").Trim()),
                                            PrepaymentDiscount = workSheet.Cells[i, 9].Value == null ? 0 : double.Parse(workSheet.Cells[i, 9].Value.ToString().Replace("$", "").Trim()),
                                            Status = workSheet.Cells[i, 10].Value == null ? "" : workSheet.Cells[i, 10].Value.ToString()
                                        });
                                    }
                                    else {
                                        var product = _productDataRepository.Queryable.Where(x => x.Name == workSheet.Cells[i, 3].Value.ToString()).FirstOrDefault();
                                        product.ItemCode = workSheet.Cells[i, 2].Value == null ? "" : workSheet.Cells[i, 2].Value.ToString();
                                        product.Name = workSheet.Cells[i, 3].Value == null ? "" : workSheet.Cells[i, 3].Value.ToString();
                                        product.Category = workSheet.Cells[i, 4].Value == null ? "" : workSheet.Cells[i, 4].Value.ToString().Trim().Replace(' ', '-');
                                        product.OriginalPrice = workSheet.Cells[i, 6].Value == null ? 0 : double.Parse(workSheet.Cells[i, 6].Value.ToString().Replace("$", "").Trim());
                                        product.AgentPrice = workSheet.Cells[i, 7].Value == null ? 0 : double.Parse(workSheet.Cells[i, 7].Value.ToString().Replace("$", "").Trim());
                                        product.WholesalerPrice = workSheet.Cells[i, 8].Value == null ? 0 : double.Parse(workSheet.Cells[i, 8].Value.ToString().Replace("$", "").Trim());
                                        product.PrepaymentDiscount = workSheet.Cells[i, 9].Value == null ? 0 : double.Parse(workSheet.Cells[i, 9].Value.ToString().Replace("$", "").Trim());
                                        product.Status = workSheet.Cells[i, 10].Value == null ? "" : workSheet.Cells[i, 10].Value.ToString();
                                        _productDataRepository.Update(product);
                                        await _productDataRepository.SaveChangesAsync();
                                    }
                                }
                            }
                            if (productList.Count>0) {
                                _productDataRepository.CreateRange(productList);
                                await _productDataRepository.SaveChangesAsync();
                            }

                            for (int i = 2; i <= totalRows; i++)
                            {
                                if (workSheet.Cells[i, 2].Value != null)
                                {
                                    if (_packagingListDataRepository.Queryable.Where(x => x.ProductId == _productDataRepository.Queryable.Where(y => y.Name == workSheet.Cells[i, 3].Value.ToString()).FirstOrDefault().Id).Count() < 1)
                                    {
                                        List<PackagingList> PackageNames = new List<PackagingList>();
                                        string PackageName = workSheet.Cells[i, 5].Value == null ? "" : workSheet.Cells[i, 5].Value.ToString();
                                        int productId = _productDataRepository.Queryable.Where(x => x.Name == workSheet.Cells[i, 3].Value.ToString()).FirstOrDefault().Id;
                                        if (PackageName.ToLower().Contains("op"))
                                        {
                                            PackageNames.Add(new PackagingList()
                                            {
                                                ProductId = productId,
                                                PackageName = "OP"
                                            });
                                        }
                                        if (PackageName.ToLower().Contains("pp"))
                                        {
                                            PackageNames.Add(new PackagingList()
                                            {
                                                ProductId = productId,
                                                PackageName = "PP"
                                            });
                                        }
                                        _packagingListDataRepository.CreateRange(PackageNames);
                                        await _packagingListDataRepository.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        var updatePackageNames = _packagingListDataRepository.Queryable.Where(x => x.ProductId == _productDataRepository.Queryable.Where(y => y.Name == workSheet.Cells[i, 3].Value.ToString()).FirstOrDefault().Id).FirstOrDefault();
                                        string PackageName = workSheet.Cells[i, 5].Value == null ? "" : workSheet.Cells[i, 5].Value.ToString();
                                        int productId = _productDataRepository.Queryable.Where(x => x.Name == workSheet.Cells[i, 3].Value.ToString()).FirstOrDefault().Id;
                                        if (PackageName.ToLower().Contains("op"))
                                        {
                                            updatePackageNames.ProductId = productId;
                                            updatePackageNames.PackageName = "OP";
                                        }
                                        else if (PackageName.ToLower().Contains("pp"))
                                        {
                                            updatePackageNames.ProductId = productId;
                                            updatePackageNames.PackageName = "PP";
                                        }
                                        else {
                                            _packagingListDataRepository.Delete(updatePackageNames);
                                            await _packagingListDataRepository.SaveChangesAsync();
                                        }
                                        _packagingListDataRepository.Update(updatePackageNames);
                                        await _packagingListDataRepository.SaveChangesAsync();
                                    }
                                }
                            }
                            package.Stream.Close();
                            package.Dispose();
                        }
                        return Ok(new ApiResponse
                        {
                            Status = "success",
                            Message = "Products uploaded."
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
        public async Task<IActionResult> WritePdf(int orderId, double gst, double priceExclGst, double discount, int userId)
        {
            try
            {
                var order = await _orderDataRepository.Queryable.Where(x => x.Id == orderId).GetFirstOrDefaultAsync();
                var cart = await shoppingCartRepository.GetShoppingCartByID(order.ShoppingCartId, userId);
                var currentUser = await userRepository.GetUserByAccount(userId);
                double priceIncGst = priceExclGst + gst;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<!DOCTYPE html><html lang='en'><head><meta charset='utf-8'><style>");
                stringBuilder.Append(".clearfix:after{content:'';display: table;clear: both;} a{color: #5D6975;text-decoration: underline;} body{position: relative;width: 21cm;height: 29.7cm;margin: 0 auto;color: #001028;background: #FFFFFF;font-family: Arial, sans-serif;font-size: 12px;}");
                stringBuilder.Append("header{padding: 10px 0;margin-bottom: 30px;}#logo{text-align: center;margin-bottom: 10px; font-size: 2em}#logo img{width: 90px;}h1 {border-top: 1px solid #5D6975;border-bottom: 1px solid #5D6975;color: #5D6975;font-size: 1.5em;line-height: 1.4em;font-weight: normal;text-align: center;margin: 0 0 20px 0;}");
                stringBuilder.Append("#project {float: left; font-size: 18px}#project span {color: #5D6975;text-align:left;display: inline-block;font-size: 0.8em;}#company {float: right;text-align: right; font-size: 18px}#company span {color: #5D6975;text-align: right;display: inline-block;font-size: 0.8em;}#project div,#company div {white-space: nowrap;}");
                stringBuilder.Append("table {width: 100%;border-collapse: collapse;border-spacing: 0;margin-bottom: 20px;}table tr:nth-child(2n-1) td {background: #F5F5F5;}table th,table td {text-align: center;white-space:pre-wrap;word-wrap:break-word;}table th {padding: 5px 20px;color: #5D6975;border-bottom: 1px solid #C1CED9;font-weight: normal;}table .service,table .desc {text-align: left;}table td{padding: 20px;text-align: right;}table td.service,table td.desc {vertical-align: top;}table td.unit,table td.qty,table td.total {font-size: 1.2em;}table td.grand {border-top: 1px solid #5D6975;}");
                stringBuilder.Append("#notices,#notices .notice {color: #5D6975;font-size: 1.2em;}footer {font-size: 1.5em;color: #5D6975;width: 100%;height: 30px;position: absolute;bottom: 0;border-top: 1px solid #C1CED9;padding: 8px 0;text-align: center;}");
                stringBuilder.Append("</style></head>");
                stringBuilder.Append("<body><header class='clearfix'><div id='logo'><p>Trojan Trading Company PTY LTD</p></div>");
                stringBuilder.Append("<h1>Order #" + orderId + " for Customer " + currentUser.Account + "</h1>");
                stringBuilder.Append("<div id='company' class='clearfix'><div>SHIPPING ADDRESS</div><div><span>Business Name:&nbsp;&nbsp;</span>" + currentUser.BussinessName + "</div><div><span>ADDRESS:&nbsp;&nbsp;</span>" + currentUser.ShippingStreetNumber + " " + currentUser.ShippingAddressLine + "</div><div>" + currentUser.ShippingSuburb + ", " + currentUser.ShippingState + ", " + currentUser.ShippingPostCode + "</div><div><span>EMAIL:&nbsp;&nbsp;</span> <a href='" + currentUser.Email + "' target='_blank'>" + currentUser.Email + "</a></div><div><span>PHONE:&nbsp;&nbsp;</span>" + currentUser.Phone + "</div></div>");
                stringBuilder.Append("<div id='project'><div>BILLING ADDRESS</div><div><span>Business Name:&nbsp;&nbsp;</span>" + currentUser.BussinessName + "</div><div><span>ADDRESS:&nbsp;&nbsp;</span>" + currentUser.BillingStreetNumber + " " + currentUser.BillingAddressLine + "</div><div>" + currentUser.BillingSuburb + ", " + currentUser.BillingState + ", " + currentUser.BillingPostCode + "</div><div><span>EMAIL:&nbsp;&nbsp;</span> <a href='" + currentUser.Email + "' target='_blank'>" + currentUser.Email + "</a></div><div><span>PHONE:&nbsp;&nbsp;</span>" + currentUser.Phone + "</div></div></header>");
                stringBuilder.Append("<main><table><thead><tr><th class='service' style='width: 10%'>Item Code</th><th class='desc' style='width: 40%'>Product Name</th><th style='width: 10%'>Packaging</th><th style='width: 10%'>Original Price</th><th style='width: 10%'>Buy Price</th><th style='width: 10%'>Order Qty</th><th style='width: 10%'>Line Amount</th></tr></thead><tbody>");
                foreach (var item in cart.ShoppingItems)
                {
                    stringBuilder.Append("<tr><td class='service'>" + item.Product.ItemCode + "</td><td class='desc'>" + item.Product.Name + "</td><td class='total'>" + item.Packaging + "</td><td class='total'>$" + item.Product.OriginalPrice + "</td>");
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
                        stringBuilder.Append("<td class='total'>$" + String.Format("{0:0.00}", item.Product.AgentPrice * item.Amount) + "</td></tr>");
                    }
                    else if (currentUser.Role.ToLower() == "wholesaler")
                    {
                        stringBuilder.Append("<td class='total'>$" + String.Format("{0:0.00}", item.Product.WholesalerPrice * item.Amount) + "</td></tr>");
                    }
                }
                stringBuilder.Append("<tr><td colspan='4' style='text-align:right'> Payment Method</td><td class='total' colspan='3'>" + String.Format("{0}", cart.PaymentMethod == "onaccount" ? "On Account" : "Prepayment") + "</td></tr>");
                stringBuilder.Append("<tr><td colspan='4' style='text-align:right'> Total Price Excl.GST</td><td class='total' colspan='3'>$" + String.Format("{0:0.00}", priceExclGst) + "</td></tr>");
                stringBuilder.Append("<tr><td colspan='4' style='text-align:right'> GST</td><td class='total' colspan='3'>$" + String.Format("{0:0.00}", gst) + "</td></tr>");
                stringBuilder.Append("<tr><td colspan='4' style='text-align:right'> Total Discount Earned</td><td class='total' colspan='3'>$" + String.Format("{0:0.00}", discount) + "</td></tr>");
                stringBuilder.Append("<tr><td colspan='4' style='text-align:right' class='grand total'> Total Price Inc.GST</td><td class='grand total' colspan='3'>$" + String.Format("{0:0.00}", priceIncGst) + "</td></tr>");
                stringBuilder.Append("</tbody></table>");
                stringBuilder.Append("<div id='notices'><div>NOTICE:</div><div class='notice'>A finance charge of 1.5% will be made on unpaid balances after 30 days.</div></div>");
                stringBuilder.Append("</main>");
                stringBuilder.Append("<footer><a href='http://xxxxxxxx' target='_blank'>http://xxxxxxxx</a></footer>");
                stringBuilder.Append("</body></html>");
                string pdfBody = stringBuilder.ToString();
                HtmlToPdf Renderer = new HtmlToPdf();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "order_" + orderId + ".pdf");
                var pdf = Renderer.ConvertHtmlString(pdfBody);

                MemoryStream stream = new MemoryStream();
                pdf.Save(stream);
                pdf.Close();
                byte[] docBytes = stream.ToArray();

                return File(docBytes, "application/pdf");
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
        public async Task<IActionResult> SaveImage()
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

                        if (!System.IO.File.Exists(path))
                        {
                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                uploadFile.CopyTo(stream);
                                stream.Close();
                            }
                        }

                        if (_headInformationDataRepository.Queryable.Where(img => img.ImagePath == path).Count() == 0)
                        {
                            HeadInformation imageModel = new HeadInformation();
                            imageModel.Content = "";
                            imageModel.ImagePath = uploadFile.FileName;
                            _headInformationDataRepository.Create(imageModel);
                            await _headInformationDataRepository.SaveChangesAsync();
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
