using ScanFile.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ScanFile.GUI
{
    public partial class FrmThongKe : Form
    {
        private ScanFileContext db = Helper.db;

        #region Hàm khởi tạo
        public FrmThongKe()
        {
            InitializeComponent();
            Helper.Reload();
        }

        #endregion

        #region LoadForm
        private void LoadChartThongKe()
        {
            int i = 0;
            chartDoThi.DataSource = db.FILEEXEs.ToList()
                                    .OrderByDescending(p=>p.SOLANSUDUNG)
                                    .Take(10)
                                    .Where(p=>p.SOLANSUDUNG>0)
                                    .Select(p => new
                                    {
                                        STT = ++i,
                                        Ten = p.TEN,
                                        SoLanMo = p.SOLANSUDUNG
                                    })
                                    .ToList();

            chartDoThi.Series[0].XValueMember = "Ten";
            chartDoThi.Series[0].XValueType = ChartValueType.String;
            chartDoThi.Series[0].YValueMembers = "SoLanMo";
            chartDoThi.Series[0].YValueType = ChartValueType.Int32;
        }

        private void FrmThongKe_Load(object sender, EventArgs e)
        {
            LoadChartThongKe();
        }
        #endregion

        private void btnAn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
