using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using VoNguyenMinhNhat_2280602222.model;

namespace VoNguyenMinhNhat_2280602222
{
    public partial class Form1 : Form
    {
        private readonly Model1 contextDB = new Model1();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            List<Lop> lops = contextDB.Lops.ToList();
            List<SV> dssv = contextDB.SVs.ToList();
            FillFacultyCombobox(lops);
            BindGrid(dssv);


        }
        private void FillFacultyCombobox(List<Lop> dslop)
        {
            cbLop.Items.Clear();
            cbLop.DataSource = dslop;
            cbLop.DisplayMember = "FacultyName";
            cbLop.ValueMember = "FacultyID";
        }
        private void BindGrid(List<SV> dssv)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in dssv)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.MaSV;
                dataGridView1.Rows[index].Cells[1].Value = item.TenSV;
                dataGridView1.Rows[index].Cells[2].Value = item.Lop.TenLop;
                dataGridView1.Rows[index].Cells[2].Value = item.NgaySinh.ToString("dd/MM/yyyy");


            }
        }
        private bool KiemTraRangBuoc()
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Không được để trống mã sinh viên");
                txtMaSV.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTenSV.Text))
            {
                MessageBox.Show("Không được để trống họ tên");
                txtTenSV.Focus();
                return false;
            }
            if (cbLop.SelectedIndex < 0)
            {
                MessageBox.Show("Không được để trống khoa");
                cbLop.Focus();
                return false;
            }
            return true;
        }
        private void ResetControl()
        {
            txtMaSV.Clear();
            txtTenSV.Clear();
            cbLop.SelectedIndex = -1;
            dataGridView1.ClearSelection();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!KiemTraRangBuoc()) return;

            string maSV = txtMaSV.Text;
            var timSV = contextDB.SVs.Find(maSV);
            if (timSV != null)
            {
                MessageBox.Show("Mã sinh viên đã tồn tại");
                return;
            }
            DateTime ngaySinh;
            if (!DateTime.TryParse(dtngaysinh.Text, out ngaySinh))
            {
                MessageBox.Show("Ngày sinh không hợp lệ.");
                return;
            }

            var student = new SV
            {
                MaSV = maSV, 
                TenSV = txtTenSV.Text,
                NgaySinh = ngaySinh, 
            };

            contextDB.SVs.Add(student);
            contextDB.SaveChanges();
            MessageBox.Show("Thêm sinh viên thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

            BindGrid(contextDB.SVs.ToList());
            ResetControl();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
