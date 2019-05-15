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
        string GetMimeType(string file);
        byte[] CopyFileToMemory(string fileUrl, bool isLocalFile = false);
        void UpdateFileDownloadCookie();
    }
    public class PdfBoardRepository:IPdfBoardRepository
    {
        private readonly TrojantradingDbContext trojantradingDbContext;
        private readonly IHttpContextAccessor httpAccessor;

        public PdfBoardRepository(TrojantradingDbContext trojantradingDbContext, IHttpContextAccessor httpAccessor)
        {
            this.trojantradingDbContext = trojantradingDbContext;
            this.httpAccessor = httpAccessor;
        }

        public List<PdfBoard> GetPdfBoards() {
            var pdfBoards = trojantradingDbContext.PdfBoards.ToList();
            return pdfBoards;
        }

        public string GetMimeType(string file)
        {
            string extension = Path.GetExtension(file).ToLowerInvariant();
            switch (extension)
            {
                case ".txt": return "text/plain";
                case ".pdf": return "application/pdf";
                case ".doc": return "application/vnd.ms-word";
                case ".docx": return "application/vnd.ms-word";
                case ".xls": return "application/vnd.ms-excel";
                case ".png": return "image/png";
                case ".jpg": return "image/jpeg";
                case ".jpeg": return "image/jpeg";
                case ".gif": return "image/gif";
                case ".csv": return "text/csv";
                default: return "";
            }
        }

        public byte[] CopyFileToMemory(string fileUrl, bool isLocalFile = false)
        {
            // Create A byte Array To Hold File Data
            try
            {
                byte[] fileBytes = null;

                // Error Check, We Must Have a URL
                if (string.IsNullOrEmpty(fileUrl))
                    return fileBytes;

                if (isLocalFile)
                    fileBytes = File.ReadAllBytes(fileUrl);
                else
                    fileBytes = new WebClient().DownloadData(fileUrl);


                // Return The Data
                return fileBytes;
            }
            catch { return null; }
        }

        public void UpdateFileDownloadCookie()
        {
            httpAccessor.HttpContext.Response.Cookies.Append("filedownload", "end", new CookieOptions()
            {
                Path = "/"
            });
        }
    }
}