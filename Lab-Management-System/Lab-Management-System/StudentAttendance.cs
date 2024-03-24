using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Lab_Management_System
{
    public partial class StudentAttendance : Form
    {
        public StudentAttendance()
        {
            InitializeComponent();
            
        }

        string constr = "Data Source = AHMAD-HP; Initial Catalog = ProjectB; Integrated Security = True;";

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
        }

        private void BindGridView()
        {
            SqlConnection connection = new SqlConnection(constr);
            connection.Open();
            string query = "select SA.AttendanceId, SA.StudentId, S.RegistrationNumber, CA.AttendanceDate, SA.AttendanceStatus from StudentAttendance as SA inner join Student as S on SA.StudentId = S.Id inner join ClassAttendance as CA on SA.AttendanceId = CA.Id;";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
        }

        private void BindComboBox1()
        {
            SqlConnection conn = new SqlConnection(constr);
            string query = "select * from Student where Status = 5;";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(5);
                comboBox1.Items.Add(name);
            }
            conn.Close();
        }

        private void BindComboBox2()
        {
            SqlConnection conn = new SqlConnection(constr);
            string query = "select * from Lookup where Category = 'ATTENDANCE_STATUS';";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(1);
                comboBox2.Items.Add(name);
            }
            comboBox2.SelectedItem = comboBox2.Items[0].ToString();
            conn.Close();
        }

        private void StudentAttendance_Load(object sender, EventArgs e)
        {
            BindGridView();
            BindComboBox1();
            BindComboBox2();
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                comboBox1.Focus();
                errorProvider1.SetError(comboBox1, "Please Select Student !!");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void ClearField()
        {
            comboBox1.Text = "";
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int attendanceid = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            int studentid = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            comboBox1.Text = GetStudentRegNo(studentid);
            dateTimePicker1.Value = GetAttendanceDate(attendanceid);

        }

        private void InsertDate()
        {
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();
            string query = "insert into ClassAttendance (AttendanceDate) values (@date)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@date", dateTimePicker1.Value);
            int rows = cmd.ExecuteNonQuery();
            conn.Close();
        }

        private DateTime GetAttendanceDate(int id)
        {
            DateTime value = DateTime.Now;
            SqlConnection conn = new SqlConnection(constr);
            string query = $"select * from ClassAttendance where Id = {id};";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                value = reader.GetDateTime(1);
            }
            conn.Close();
            return value;
        }

        private int GetAttendanceId()
        {
            int value = 1;
            SqlConnection conn = new SqlConnection(constr);
            string query = $"select * from ClassAttendance where AttendanceDate = '{dateTimePicker1.Value}';";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                value = reader.GetInt32(0);
            }
            conn.Close();
            return value;
        }

        private string GetStudentRegNo(int id)
        {
            string value = null;
            SqlConnection conn = new SqlConnection(constr);
            string query = $"select * from Student where Id = {id};";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                value = reader.GetString(5);
            }
            conn.Close();
            return value;
        }

        private int GetStudentId()
        {
            int value = 1;
            string regNo = comboBox1.SelectedItem.ToString();
            SqlConnection conn = new SqlConnection(constr);
            string query = $"select * from Student where RegistrationNumber = '{regNo}';";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                value = reader.GetInt32(0);
            }
            conn.Close();
            return value;
        }

        private int GetStatusValue()
        {
            int value = 1;
            string name = comboBox2.SelectedItem.ToString();
            SqlConnection conn = new SqlConnection(constr);
            string query = $"select * from Lookup where Name = '{name}';";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                value = reader.GetInt32(0);
            }
            conn.Close();
            return value;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                comboBox1.Focus();
                errorProvider1.SetError(comboBox1, "Please Select Student !!");
            }
            else
            {
                InsertDate();
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                string query = "insert into StudentAttendance (AttendanceId, StudentId, AttendanceStatus) values (@attendanceId, @studentId, @status)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@attendanceId", GetAttendanceId());
                cmd.Parameters.AddWithValue("@studentId", GetStudentId());
                cmd.Parameters.AddWithValue("@status", GetStatusValue());
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Inserted Successfully !!", "Success");
                }
                else
                {
                    MessageBox.Show("Not Inserted Successfully !!", "Failure");

                }
                ClearField();
                conn.Close();
                BindGridView();
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                comboBox1.Focus();
                errorProvider1.SetError(comboBox1, "Please Select Student !!");
            }
            else
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                string query = "delete from StudentAttendance where AttendanceId = @attendanceid and StudentId = @studentId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@attendanceid", GetAttendanceId());
                cmd.Parameters.AddWithValue("@studentId", GetStudentId());
                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    MessageBox.Show("Deleted Successfully !!", "Success");
                }
                else
                {
                    MessageBox.Show("Not Deleted Successfully !!", "Failure");

                }
                ClearField();
                conn.Close();
                BindGridView();
            }
        }
    }
}
