using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankSim.Application.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankPaymentController : ControllerBase
    {
        
        [HttpGet]
        public string Get()
        {
            return "Here it Is: #######";
        }


        [Authorize]
        [HttpPost]
        public string Post()
        {
            return "Post BankPayment Success";
        }
    }
}
