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
    public class HeadInformationController:Controller
    {
        private readonly IHeadInformationRepository _headInformationRepository;

        public HeadInformationController(HeadInformationRepository headInformationRepository)
        {
            this._headInformationRepository = headInformationRepository;
        }

        [Route("/getall")]
        public IActionResult GetAll()
        {
            var headInformations = _headInformationRepository;
            return null;
        }

        [Route("/delete")]
        public IActionResult Delete(int id)
        {
            _headInformationRepository.Delete(id);
            return null;
        }

        [Route("/update")]
        public IActionResult Update(HeadInformation headInformation)
        {
            _headInformationRepository.Update(headInformation);
            return null;
        }

        [Route("/edit")]
        public IActionResult Edit(int id)
        {
            var headInformation = _headInformationRepository.Get(id);
            return Ok(headInformation);
        }

    }
}