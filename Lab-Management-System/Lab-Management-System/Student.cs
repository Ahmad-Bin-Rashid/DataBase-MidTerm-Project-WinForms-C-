using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Lab_Management_System
{
    public partial class Student : Form
    {
        public Student()
        {
            InitializeComponent();
        }
        
        string constr = "Data Source = AHMAD-HP; Initial Catalog = ProjectB; Integrated Security = True;";
        string emailPattern = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
        string regPattern = "/([0-9]{4})+(-)+([A-Z]{2,3})+(-)+([0-9]{2,3})/gm";

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            HomePage.cloPage.Show();
        }

        private void Student_Load(object sender, EventArgs e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            SqlConnection connection = new SqlConnection(constr);
            connection.Open();
            string query = "select * from Student;";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(constr);
            conn.Open();
            string query = "insert into Student (FirstName, LastName, Contact, Email, RegistrationNumber, Status) values (@firstName, @lastName, @contact, @email, @regNo, @status)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@firstName", firstNameTb.Text);
            cmd.Parameters.AddWithValue("@lastName", lastNameTb.Text);
            cmd.Parameters.AddWithValue("@contact", contactTb.Text);
            cmd.Parameters.AddWithValue("@email", emailTb.Text);
            cmd.Parameters.AddWithValue("@regNo", regNoTb.Text);
            cmd.Parameters.AddWithValue("@status", 5);
            int rows = cmd.ExecuteNonQuery();
            if (rows > 0)
            {
                MessageBox.Show("Inserted Successfully !!", "Success");
            }
            else
            {
                MessageBox.Show("Not Inserted Successfully !!", "Failure");

            }
            conn.Close();
            BindGridView();
        }
    }
}
