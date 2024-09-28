using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace baitaptuan5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnADD_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows.Add(frm.MaNV, frm.TenNV, frm.LuongCB);
            }
        }

        private void btnfix_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // Kiểm tra có mục nào được chọn không
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Cập nhật thông tin từ các TextBox
                Form2 frm = new Form2();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    selectedRow.Cells["Column1"].Value = frm.MaNV;
                    selectedRow.Cells["Column2"].Value = frm.TenNV;
                    selectedRow.Cells["Column3"].Value = frm.LuongCB;

                    frm.ClearFields();
                }
            }
            else { MessageBox.Show("Vui lòng chọn một nhân viên để sửa."); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn mục để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
