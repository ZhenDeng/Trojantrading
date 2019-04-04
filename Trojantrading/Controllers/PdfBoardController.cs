using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trojantrading.Repositories;

namespace Trojantrading.Controllers
{
    [Route("[Controller]")]
    [Authorize(Roles="Admin, User")]
    public class PdfBoardController:Controller
    {
        private readonly IPdfBoardRepository _pdfBoardRepository;

        public PdfBoardController(PdfBoardRepository pdfBoardRepository)
        {
            this._pdfBoardRepository = pdfBoardRepository;
        }
        

        public IActionResult getAll()
        {
            return null;
        }
    }
}