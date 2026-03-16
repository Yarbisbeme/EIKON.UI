//using EIKON.Data.DTOs;
//using EIKON.Data.Models;
using DevExpress.XtraReports.UI;
using EIKON.UI.Interfaces;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace EIKON.UI.PredefinedReports
{
    public partial class Rptrrhel006a : DevExpress.XtraReports.UI.XtraReport
    {
        public Rptrrhel006a()
        {
            InitializeComponent();
        }
        public void LoadData(IEnumerable<Rrhel006aDto> data)
        {
            this.DataSource = data;
        }
    }
}
