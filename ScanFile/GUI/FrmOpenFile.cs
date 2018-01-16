using ScanFile.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScanFile.GUI
{
    public partial class FrmOpenFile : Form
    {
        private ScanFileContext db = Helper.db;

        #region Hàm khởi tạo
        public FrmOpenFile()
        {
            InitializeComponent();
            Helper.Reload();
        }
        #endregion

        #region LoadForm

        private void LoadInitControl()
        {
            //cbxFile.DataSource = db.FILEEXEs.OrderByDescending(p => p.SOLANSUDUNG).Select(p => new { ID = p.ID, Ten = p.TEN }).ToList();
            //cbxFile.DisplayMember = "Ten";
            //cbxFile.ValueMember = "ID";
        }
        private void FrmOpenFile_Load(object sender, EventArgs e)
        {
            LoadInitControl();

            AutoCompleteStringCollection allowedTypes = new AutoCompleteStringCollection();
            var listString = db.FILEEXEs.OrderByDescending(p => p.SOLANSUDUNG).Select(p => p.TEN).ToArray();

            allowedTypes.AddRange(listString);
            txtTenFile.AutoCompleteCustomSource = allowedTypes;
            txtTenFile.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtTenFile.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }
        #endregion

        #region Sự kiện
        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMoFile_Click(object sender, EventArgs e)
        {
            FILEEXE file = db.FILEEXEs.Where(p => p.TEN == txtTenFile.Text).FirstOrDefault();
            if (file == null)
            {
                MessageBox.Show("Không tìm thấy file", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process.Start(file.DUONGDAN);
                file.SOLANSUDUNG++;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mở file thất bại\n"+ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }
        #endregion

        #region Hàm chức năng
        private void OpenFile()
        {
            FILEEXE file = db.FILEEXEs.Where(p => p.TEN == txtTenFile.Text).FirstOrDefault();
            if (file == null)
            {
                MessageBox.Show("Không tìm thấy file", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                Process.Start(file.DUONGDAN);
                file.SOLANSUDUNG++;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mở file thất bại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void FrmOpenFile_Enter(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void FrmOpenFile_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OpenFile();
            }
        }
    }
}
