using EIKON.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EIKON.UI.Controllers
{
    //    [Route("api/[controller]")]
    //    [ApiController]
    //    public class ReportDataController : ControllerBase
    //    {
    //    }
    //}

    [ApiController]
    [Route("api/[controller]")]
    public class ReportDataController : ControllerBase
    {
        [HttpGet]
        [Route("data")]
        public IActionResult GetData()
        {
            var data = new List<Trhem000old>
        {
            new Trhem000old { EmCodigo = "01", EmDescri = "Eikon 01", Direcc = "San Geronimo", Telefono="809 320-1880", EmRnc="230-1234567" },
            new Trhem000old { EmCodigo = "02", EmDescri = "Eikon 02", Direcc = "San Geronimo C Oeste", Telefono="809 320-1881", EmRnc="130-1234567" },
        };

            return Ok(data);
        }
    }
}