using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Trojantrading.Models;
using Trojantrading.Repositories;
using Trojantrading.Service;
using Trojantrading.Util;

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

        public PdfBoardController(
            IPdfBoardRepository pdfBoardRepository,
            TrojantradingDbContext trojantradingDbContext,
            IShare share)
        {
            this.pdfBoardRepository = pdfBoardRepository;
            this.trojantradingDbContext = trojantradingDbContext;
            this.share = share;
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
        [Route("WritePdf")]
        public IActionResult WritePdf()
        {
            var model = new TestViewModel { DocTitle = "ABC", DocContent = "This is a test" };
            return new ViewAsPdf(model);
        }
        #endregion
    }

    public class TestViewModel {
        public string DocTitle { get; set; }
        public string DocContent { get; set; }
    }
}