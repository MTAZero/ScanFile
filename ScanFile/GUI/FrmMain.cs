using ScanFile.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;

namespace ScanFile.GUI
{
    public partial class FrmMain : Form
    {
        private ScanFileContext db = Helper.db;

        private GlobalKeyboardHook gkh;


        #region Hàm khởi tạo
        public FrmMain()
        {
            InitializeComponent();
            Helper.Reload();

            gkh = new GlobalKeyboardHook();
            gkh.KeyboardPressed += gkh_KeyPress;

        }
        #endregion


        #region LoadForm

        private void LoadDgvFile()
        {
            int i = 0;
            dgvFile.DataSource = db.FILEEXEs
                                 .OrderByDescending(p=>p.SOLANSUDUNG)
                                 .ToList()
                                 .Select(p => new
                                 {
                                     ID = p.ID,
                                     STT = ++i,
                                     Ten = p.TEN,
                                     DuongDan = p.DUONGDAN,
                                     SoLanMo = p.SOLANSUDUNG
                                 })
                                 .ToList();
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            LoadDgvFile();
        }

        private void panel1_Enter(object sender, EventArgs e)
        {
            notifyIcon.Visible = true;
            this.Hide();
        }
        #endregion

        #region Sự kiện click menu
        private void exitToolTrip_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ScanToolStrip_Click(object sender, EventArgs e)
        {
            notifyIcon.Visible = false;
            this.Show();
        }

        private void viewToolTrip_Click(object sender, EventArgs e)
        {
            FrmThongKe form = new FrmThongKe();
            form.Show();
        }
        #endregion

        #region  Sự kiện
        private void btnAn_Click(object sender, EventArgs e)
        {

            notifyIcon.Visible = true;
            this.Hide();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có chắc chắn load lại database?\nTất cả lịch sử mở của bạn sẽ bị xóa",
                            "Thông báo",
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question);

            if (rs == DialogResult.Cancel) return;

            try
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.ShowDialog();

                string path = fd.SelectedPath;

                DirectoryInfo di = new DirectoryInfo(path);

                int i = 0;
                var listFile = di.GetFiles("*.exe", SearchOption.AllDirectories)
                                     .Where(p => !p.Attributes.HasFlag(FileAttributes.Hidden))
                                     .Select(p => new
                                     {
                                         STT = ++i,
                                         Ten = p.Name,
                                         DuongDan = p.FullName,
                                         SoLanMo = 0
                                     })
                                     .ToList();

                /// Lưu cơ sở dữ liệu
                db.FILEEXEs.RemoveRange(db.FILEEXEs);
                foreach (var item in listFile)
                {
                    FILEEXE a = new FILEEXE();
                    a.TEN = item.Ten.Substring(0, item.Ten.Length - 4);
                    a.DUONGDAN = item.DuongDan;
                    a.SOLANSUDUNG = 0;
                    db.FILEEXEs.Add(a);
                }
                db.SaveChanges();

                /// thông báo và load lại datagridview
                MessageBox.Show("Load thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadDgvFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load thất bại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult rs = MessageBox.Show("Bạn có chắc chắn xóa file này trong cơ sở dữ liệu",
                                              "Thông báo",
                                              MessageBoxButtons.OKCancel,
                                              MessageBoxIcon.Warning);

            if (rs == DialogResult.Cancel) return;

            try
            {
                int id = (int)dgvFile.SelectedRows[0].Cells["ID"].Value;
                FILEEXE file = db.FILEEXEs.Where(p => p.ID == id).FirstOrDefault();

                try
                {
                    db.FILEEXEs.Remove(file);
                    db.SaveChanges();

                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDgvFile();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xóa thất bại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch
            {
                MessageBox.Show("Chưa có file nào được chọn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion

        #region Mở file click icon và hotkey
        #endregion

        #region Sự kiện keyboardHook
        void gkh_KeyPress(object sender, GlobalKeyboardHookEventArgs e)
        {
            

            
            if (e.KeyboardData.VirtualCode != (GlobalKeyboardHook.VkF7))
                return;


            gkh.KeyboardPressed -= gkh_KeyPress;
            if (e.KeyboardState == GlobalKeyboardHook.KeyboardState.KeyDown)
            {
                FrmOpenFile formOpenFile = new FrmOpenFile();
                formOpenFile.ShowDialog();

                e.Handled = false;
                
            }

            gkh.KeyboardPressed += gkh_KeyPress;
        }


        #endregion

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            gkh.KeyboardPressed -= gkh_KeyPress;
            FrmOpenFile formOpenFile = new FrmOpenFile();
            formOpenFile.ShowDialog();
            gkh.KeyboardPressed += gkh_KeyPress;
        }

    }
}
