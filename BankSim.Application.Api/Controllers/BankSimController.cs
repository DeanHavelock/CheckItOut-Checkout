using Microsoft.AspNetCore.Mvc;

namespace BankSim.Application.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BankSimController : ControllerBase
    {
        [HttpGet]
        public string Index()
        {
            return "A Client Auth Protected Bank Sim Api";
        }
    }
}
