using Microsoft.AspNetCore.Mvc;
using Trojantrading.Repositories;
using Trojantrading.Util;
using Trojantrading.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;

namespace Trojantrading.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class PdfBoardController:Controller
    {
        private readonly IPdfBoardRepository _pdfBoardRepository;

        public PdfBoardController(PdfBoardRepository pdfBoardRepository)
        {
            this._pdfBoardRepository = pdfBoardRepository;
        }

        [HttpGet("GetPdfBoards")]
        [NoCache]
        [ProducesResponseType(typeof(List<PdfBoard>), 200)]
        public IActionResult GetPdfBoards()
        {
            return Ok(_pdfBoardRepository.GetPdfBoards());
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
                                    type=="agent"?"agent":"wholesaler");

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            uploadFile.CopyToAsync(stream);
                        }

                        return Ok(new ApiResponse
                        {
                            Status = "success",
                            Message = "Pdf file uploaded."
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
    }
}