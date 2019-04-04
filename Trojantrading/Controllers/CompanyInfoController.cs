using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Trojantrading.Repositories;
using Trojantrading.Models;

namespace Trojantrading.Controllers
{
    [Route("[Controller]")]
    [Authorize(Roles = "Admin, Customer")]
    public class CompanyInfoController
    {
        private readonly CompanyInfoRepository _companyInfoRepository;

        public CompanyInfoController(CompanyInfoRepository companyInfoRepository)
        {
            this._companyInfoRepository = companyInfoRepository;
        }

        public IActionResult Get()
        {
            var companyInfo = _companyInfoRepository.Get(1);
            return null;
        } 


        public IActionResult Update()
        {
            var companyInfo = new CompanyInfo();
            _companyInfoRepository.Update(companyInfo);
            return null;
        }
    }
}