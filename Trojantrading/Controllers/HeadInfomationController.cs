using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trojantrading.Models;
using Trojantrading.Repositories;
using Trojantrading.Util;

namespace Trojantrading.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class HeadInfomationController : Controller
    {
        private readonly IHeaderInfomationRepository headerInfomationRepository;

        public HeadInfomationController(IHeaderInfomationRepository headerInfomationRepository)
        {
            this.headerInfomationRepository = headerInfomationRepository;
        }

        [HttpGet("GetHeadInformation")]
        [NoCache]
        [ProducesResponseType(typeof(HeadInformation), 200)]
        public IActionResult GetHeadInformation()
        {
            var headInformation = headerInfomationRepository.GetHeadInformation();
            return Ok(headInformation);
        }

        [HttpPost("AddHeader")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public IActionResult AddHeader([FromBody]HeadInformation headInformation)
        {
            var result = headerInfomationRepository.AddHeader(headInformation);
            return Ok(result);
        }

        [HttpPost("UpdateHeadInfomation")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public IActionResult UpdateHeadInfomation([FromBody]HeadInformation headInformation)
        {
            var result = headerInfomationRepository.UpdateHeadInfomation(headInformation);
            return Ok(result);
        }

        [HttpPost("DeleteHeadInfomation")]
        [NoCache]
        [ProducesResponseType(typeof(ApiResponse), 200)]
        public IActionResult DeleteHeadInfomation([FromBody]HeadInformation headInformation)
        {
            var result = headerInfomationRepository.DeleteHeadInfomation(headInformation);
            return Ok(result);
        }
    }
}