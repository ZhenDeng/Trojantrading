using System;
using System.Collections.Generic;
using System.Linq;
using Trojantrading.Models;

namespace Trojantrading.Repositories
{
    public interface IPdfBoardRepository
    {
        List<PdfBoard> GetPdfBoards();
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
    }
}