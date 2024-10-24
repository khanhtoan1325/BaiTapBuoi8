using BaiTapBuoi8.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapBuoi8
{
    public partial class Form1 : Form
    {
        private int currentIndex = 0;
        private List<Student> studentsList;

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void LoadData()
        {
            using (var context = new Model1())
            {
                studentsList = context.Students.ToList();
                dataGridView1.DataSource = studentsList;

                if (studentsList.Count > 0)
                {
                    currentIndex = 0;
                    DisplayCurrentStudent();
                }
            }
        }
        private void DisplayCurrentStudent()
        {
            if (studentsList != null && studentsList.Count > 0)
            {
                var currentStudent = studentsList[currentIndex];
                textBoxFullName.Text = currentStudent.FullName;
                textBoxAge.Text = currentStudent.Age.ToString();
                comboBoxMajor.SelectedItem = currentStudent.Major;

                // Chọn dòng tương ứng trên DataGridView
                dataGridView1.ClearSelection();
                if (currentIndex >= 0 && currentIndex < dataGridView1.Rows.Count)
                {
                    dataGridView1.Rows[currentIndex].Selected = true;
                }
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBoxFullName.Text) &&
            !string.IsNullOrEmpty(textBoxAge.Text) &&
            comboBoxMajor.SelectedItem != null)
            {
                using (var context = new Model1())
                {
                    var student = new Student
                    {
                        FullName = textBoxFullName.Text,
                        Age = int.Parse(textBoxAge.Text),
                        Major = comboBoxMajor.SelectedItem.ToString()
                    };
                    context.Students.Add(student);
                    context.SaveChanges();
                    LoadData();

                    // Hiển thị thông báo thành công
                    MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                using (var context = new Model1())
                {
                    int studentId = (int)dataGridView1.CurrentRow.Cells[0].Value;
                    var student = context.Students.Find(studentId);
                    if (student != null)
                    {
                        // Xác nhận trước khi xóa
                        DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            context.Students.Remove(student);
                            context.SaveChanges();
                            LoadData();

                            // Hiển thị thông báo thành công
                            MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                using (var context = new Model1())
                {
                    int studentId = (int)dataGridView1.CurrentRow.Cells[0].Value;
                    var student = context.Students.Find(studentId);
                    if (student != null)
                    {
                        student.FullName = textBoxFullName.Text;
                        student.Age = int.Parse(textBoxAge.Text);
                        student.Major = comboBoxMajor.SelectedItem.ToString();
                        context.SaveChanges();
                        LoadData();

                        // Hiển thị thông báo thành công
                        MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sinh viên để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (studentsList != null && studentsList.Count > 0)
            {
                currentIndex++;
                if (currentIndex >= studentsList.Count)
                {
                    currentIndex = 0; // Quay về đầu danh sách nếu vượt quá cuối
                }
                DisplayCurrentStudent();
            }
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (studentsList != null && studentsList.Count > 0)
            {
                currentIndex--;
                if (currentIndex < 0)
                {
                    currentIndex = studentsList.Count - 1; // Quay về cuối danh sách nếu vượt quá đầu
                }
                DisplayCurrentStudent();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Đóng ứng dụng
                this.Close();
            }
        }
    }
}
