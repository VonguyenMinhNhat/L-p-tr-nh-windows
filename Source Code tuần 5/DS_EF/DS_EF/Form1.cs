using DS_EF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace DS_EF
{
    public partial class Frm : Form
    {
        public Frm()
        {
            InitializeComponent();
        }
        private void frm_Load(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB context = new StudentContextDB();
                List<faculty> listfaculties = context.Faculties.ToList(); 
                List<student> liststudents = context.Students.ToList();
                FillFacultyCmb(listfaculties);
                BindGrid(liststudents);

            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFacultyCmb(List<faculty> listfaculties)
        {
            this.cmb_Khoa.DataSource = listfaculties;
            this.cmb_Khoa.DisplayMember = "FacultyName";
            this.cmb_Khoa.ValueMember = "FacultyID";
        }

        private void BindGrid(List<student> liststudents) {
            dgv_DSSV.Rows.Clear();
            foreach (var item in liststudents)
            {
                int index = dgv_DSSV.Rows.Add();
                dgv_DSSV.Rows[index].Cells[0].Value = item.StudentID;
                dgv_DSSV.Rows[index].Cells[1].Value = item.FullName;
                dgv_DSSV.Rows[index].Cells[2].Value = item.Faculty.FacultyName;
                dgv_DSSV.Rows[index].Cells[3].Value = item.AverageScore;
            }
            MessageBox.Show($"Có {liststudents.Count} sinh viên được hiển thị.");
        }

        private bool IsValidStudentID(string MSSV)
        {
            if (string.IsNullOrEmpty(MSSV) || MSSV.Length != 10)
            {
                MessageBox.Show("Mã Số Sinh Viên Phải Có 10 Ký Tự!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            return true;
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                
                using (StudentContextDB context = new StudentContextDB())
                {
                    
                    if (string.IsNullOrWhiteSpace(txt_MSSV.Text) || string.IsNullOrWhiteSpace(txt_HoTen.Text) ||
                        string.IsNullOrWhiteSpace(txt_DTB.Text))
                    {
                        MessageBox.Show("Vui Lòng Nhập Đầy Đủ Thông Tin", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; 
                    }

                    
                    string MSSV = txt_MSSV.Text;
                    if (IsValidStudentID(MSSV))
                    {
                        
                        if (int.TryParse(MSSV, out int studentID))
                        {
                            
                            bool studentExists = context.Students.Any(s => s.StudentID == studentID);
                            if (studentExists)
                            {
                                MessageBox.Show("Mã Số Sinh Viên đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return; 
                            }

                            
                            if (decimal.TryParse(txt_DTB.Text, out decimal averageScore))
                            {
                                
                                student newStudent = new student()
                                {
                                    StudentID = studentID, 
                                    FullName = txt_HoTen.Text,
                                    AverageScore = averageScore,
                                    FacultyID = (int)cmb_Khoa.SelectedValue,
                                };

                                
                                context.Students.Add(newStudent);
                                context.SaveChanges();

                                
                                List<student> liststudents = context.Students.Include(s => s.Faculty).ToList();
                                BindGrid(liststudents);

                                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("Điểm Trung Bình không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Mã Số Sinh Viên không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btn_Del_Click(object sender, EventArgs e)
        {
            try
            {
                using (StudentContextDB context = new StudentContextDB())
                {
                    
                    string MSSV = txt_MSSV.Text;

                    
                    if (IsValidStudentID(MSSV) && int.TryParse(MSSV, out int studentID))
                    {
                        
                        student studentToDelete = context.Students.FirstOrDefault(s => s.StudentID == studentID);

                        if (studentToDelete != null)
                        {
                            DialogResult dialogResult = MessageBox.Show("Bạn có muốn xóa sinh viên này không?", "Cảnh Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dialogResult == DialogResult.Yes)
                            {
                                context.Students.Remove(studentToDelete);
                                context.SaveChanges();


                                List<student> liststudents = context.Students.Include(s => s.Faculty).ToList();
                                BindGrid(liststudents);



                                MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên với MSSV này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mã số sinh viên không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                StudentContextDB context = new StudentContextDB();
                string MSSV = txt_MSSV.Text;
                if (IsValidStudentID(MSSV) && int.TryParse(MSSV, out int ID))
                {
                    student editStudent = context.Students.FirstOrDefault(s => s.StudentID == ID);
                    if (editStudent != null)
                    {
                        editStudent.FullName = txt_HoTen.Text;
                        if (decimal.TryParse(txt_DTB.Text, out decimal averageScore))
                        {
                            editStudent.AverageScore = averageScore;
                            editStudent.FacultyID = (int)cmb_Khoa.SelectedValue;
                            context.SaveChanges();


                            List<student> liststudents = context.Students.Include(s => s.Faculty).ToList();
                            BindGrid(liststudents);

                            MessageBox.Show("Cập nhật sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Điểm Trung Bình không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Mã số sinh viên xóa không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmb_Khoa_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
