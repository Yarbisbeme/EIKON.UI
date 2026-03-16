using DevExpress.DataAccess.Json;
//using EIKON.UI.PredefinedReports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
namespace EIKON.UI.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;
        public ReportesController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration.GetSection("AppApi").GetSection("ApiBaseUrl").Value;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("reporte-empresas")]
        public IActionResult GenerateReport()
        {
            // Crear una instancia del reporte
            // var report = new rptEmpresas();
            return null;
        }
        //    // Cargar los datos desde el archivo JSON
        //    var jsonDataSource = new JsonDataSource
        //    {
        //        JsonSource = new UriJsonSource(new Uri(_apiBaseUrl + "Trhem000"))
        //    };

        //    //report.DataSource = jsonDataSource;

        //    // Exportar el reporte a PDF o a otro formato
        //    //using (var stream = new MemoryStream())
        //    //{
        //    //    report.ExportToPdf(stream);
        //    //    return File(stream.ToArray(), "application/pdf", "Empresas.pdf");
        //    //}
        //}

    }
}
