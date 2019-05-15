using Microsoft.AspNetCore.Mvc;
using Trojantrading.Repositories;
using Trojantrading.Util;
using Trojantrading.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Trojantrading.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class PdfBoardController:Controller
    {
        private readonly IPdfBoardRepository pdfBoardRepository;
        private readonly TrojantradingDbContext trojantradingDbContext;
        private readonly IHttpContextAccessor httpAccessor;

        public PdfBoardController(
            IPdfBoardRepository pdfBoardRepository, 
            TrojantradingDbContext trojantradingDbContext,
            IHttpContextAccessor httpAccessor)
        {
            this.pdfBoardRepository = pdfBoardRepository;
            this.trojantradingDbContext = trojantradingDbContext;
            this.httpAccessor = httpAccessor;
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
                                    type.ToUpperInvariant()+ " PRICE LIST" + Path.GetExtension(uploadFile.FileName).ToLowerInvariant());

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            uploadFile.CopyToAsync(stream);
                        }
                        PdfBoard pdfBoard = new PdfBoard() {
                            Title = type.ToUpperInvariant() + " PRICE LIST" + Path.GetExtension(uploadFile.FileName).ToLowerInvariant(),
                            Path = path
                        };

                        if (trojantradingDbContext.PdfBoards.Where(pdf => pdf.Path == pdfBoard.Path).Count() == 0) {
                            trojantradingDbContext.PdfBoards.Add(pdfBoard);
                            trojantradingDbContext.SaveChanges();
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
                return BadRequest(new ApiResponse
                {
                    Status = "fail",
                    Message = ex.Message
                });
            }
        }
        #endregion

        [HttpGet("DownloadPdf")]
        [NoCache]
        public FileResult DownloadPdf(string file, string fileName = "", bool displaySaveAs = false)
        {
            if (string.IsNullOrEmpty(file)) return null;

            file = HttpUtility.UrlDecode(file).Trim();
            fileName = HttpUtility.UrlDecode(fileName).Trim();

            Uri uriResult;
            byte[] fileBytes = null;
            bool isLocalFile = false;

            bool isUrl = Uri.TryCreate(file, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (isUrl)
            {
                Uri fileUri = new Uri(file);

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = Path.GetFileName(fileUri.LocalPath);
                }
                else
                {
                    string fileExtension = Path.GetExtension(fileName);
                    if (string.IsNullOrEmpty(fileExtension))
                    {
                        fileName += Path.GetExtension(Path.GetFileName(fileUri.LocalPath));
                    }
                }
            }
            else            // file is on server
            {
                isLocalFile = true;
                // get full path
                file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);

                // check file name and extension
                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = Path.GetFileName(file);
                }
                else
                {
                    string fileExtension = Path.GetExtension(fileName);
                    if (string.IsNullOrEmpty(fileExtension))
                    {
                        fileName += Path.GetExtension(Path.GetFileName(file));
                    }
                }
            }
            fileBytes = pdfBoardRepository.CopyFileToMemory(file, isLocalFile);
            if (fileBytes == null) return null;     // file does not exist
            if (displaySaveAs || file.EndsWith(".eml"))
            {
                // force to display SaveAs dialog box
                httpAccessor.HttpContext.Response.Headers.Add("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
            }
            else
            {
                // file will open in browser 
                httpAccessor.HttpContext.Response.Headers.Add("Content-Disposition", "inline; filename=\"" + fileName + "\"");
            }

            pdfBoardRepository.UpdateFileDownloadCookie();
            return File(fileBytes, "application/" + Path.GetExtension(Path.GetFileName(file)).Replace(".", ""));
        }

    }
}