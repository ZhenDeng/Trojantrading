using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Trojantrading.Models;
using Trojantrading.Repositories.Generic;
using Trojantrading.Utilities;

namespace Trojantrading.Repositories
{
    public interface IPdfBoardRepository
    {
        Task<List<PdfBoard>> GetPdfBoards();
        Task<ApiResponse> DeletePdfBoards(PdfBoard pdf);
    }
    public class PdfBoardRepository:IPdfBoardRepository
    {
        private readonly IRepositoryV2<PdfBoard> _pdfBoardRepository;

        public PdfBoardRepository(IRepositoryV2<PdfBoard> pdfBoardRepository)
        {
            _pdfBoardRepository = pdfBoardRepository;
        }

        public async Task<List<PdfBoard>> GetPdfBoards() {
            var pdfBoards = await  _pdfBoardRepository.Queryable.GetListAsync();
            return pdfBoards;
        }

        public async Task<ApiResponse> DeletePdfBoards(PdfBoard pdf)
        {
            try
            {
                _pdfBoardRepository.Delete(pdf);
                await _pdfBoardRepository.SaveChangesAsync();

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", pdf.Title);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                return new ApiResponse()
                {
                    Status = "success",
                    Message = "Successfully delete Pdf Boards file"
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
    }
}