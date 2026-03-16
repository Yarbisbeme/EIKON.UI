using DevExpress.XtraReports.UI;

namespace EIKON.UI.PredefinedReports
{
    public static class ReportsFactory
    {
        public static Dictionary<string, Func<XtraReport>> Reports = new Dictionary<string, Func<XtraReport>>()
        {
            ["TestReport"] = () => new TestReport(),
            ["rptEmpresas"] = () => new rptEmpresas(),
            ["Report32"] = () => new Report32(),
            ["Report34"] = () => new Report34(),
            ["Reporte33"] = () => new Reporte33(),
             ["rrhel006"] = () => new rrhel006()
            // ["Rptrrhel006a"] = () => new Rptrrhel006a()
        };
    }
}