using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Trojantrading.Models;

namespace Trojantrading.Repositories
{
    public interface IPdfBoardRepository
    {
        List<PdfBoard> GetPdfBoards();
        ApiResponse DeletePdfBoards(PdfBoard pdf);
    }
    public class PdfBoardRepository:IPdfBoardRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;

        public PdfBoardRepository(TrojantradingDbContext trojantradingDbContext)
        {
            this.trojantradingDbContext = trojantradingDbContext;
        }

        public List<PdfBoard> GetPdfBoards() {
            var pdfBoards = trojantradingDbContext.PdfBoards.ToList();
            return pdfBoards;
        }

        public ApiResponse DeletePdfBoards(PdfBoard pdf)
        {
            try
            {
                trojantradingDbContext.PdfBoards.Remove(pdf);
                trojantradingDbContext.SaveChanges();

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