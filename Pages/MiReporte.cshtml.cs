using DevExpress.DataAccess.Json;
using EIKON.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DevExpress.XtraReports.UI;
using EIKON.UI.Models;
using DevExpress.ClipboardSource.SpreadsheetML;
//using EIKON.Data.Models;
namespace EIKON.UI.Pages
{
    public class MiReporteModel : PageModel
    {
        //public EikonDataServices apiServices;
        private readonly EikonDataServices _eikonService;

        //public List<MyDataModel> Data { get; private set; }

        public MiReporteModel(EikonDataServices eikonService)
        {
            _eikonService = eikonService;
        }
        public void OnGet()
        {
            // Crear una instancia del reporte
            //var report = new rrhel006();

            

            //var jsonDataSource = CreateDataSourceFromWeb();
            //report.DataSource = jsonDataSource;

            // Exportar el reporte a PDF o a otro formato
            ////using (var stream = new MemoryStream())
            ////{
            ////    report.ExportToPdf(stream);
            ////    File(stream.ToArray(), "application/pdf", "Puestos.pdf");
            ////}
        }
        //public  async Task<IEnumerable<rptPuestosEstatus>> CreateDataSourceFromWeb()
        //{
        //  var  data = await _eikonService.GetDataRrhel006("A");
        //    return  data;
           
        //}
    }
        //private readonly IHttpClientFactory _httpClientFactory;

        //public MiReporteModel(IHttpClientFactory httpClientFactory)
        //{
        //    _httpClientFactory = httpClientFactory;
        //}

        //public async Task<IActionResult> OnPostDownloadReportAsync()
        //{
        //    var client = _httpClientFactory.CreateClient();
        //    var response = await client.GetAsync("/api/Reportes/reporte-empresas");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var fileBytes = await response.Content.ReadAsByteArrayAsync();
        //        return File(fileBytes, "application/pdf", "report.pdf");
        //    }

        //    return Page(); // Maneja el error según sea necesario
        //}
    }

