using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using EIKON.UI.Interfaces;
using DevExpress.Data.Utils;
using DevExpress.Xpo;
namespace EIKON.UI.PredefinedReports
{
    public partial class rrhel006 : DevExpress.XtraReports.UI.XtraReport
    {
        public rrhel006()
        {
            InitializeComponent();
        }
        public void LoadData(IEnumerable<RptPuestosEstatus> data)
        {
            
            this.DataSource = data;
        }
    }
}
