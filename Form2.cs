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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public int LuongCB { get; set; }


        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void btn_Yes_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Vui Lòng Nhập Đủ Thông Tin!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MaNV = textBox2.Text;
                TenNV = textBox1.Text;
                LuongCB = int.Parse(textBox3.Text);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public void ClearFields()
        {
            textBox2.Clear();
            textBox1.Clear();
            textBox3.Clear();
        }

        private void btn_Skip_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
