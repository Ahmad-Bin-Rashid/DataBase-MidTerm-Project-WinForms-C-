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
        string regNoPattern = "^[\\d]{4}[\\-]{1}[A-Z]{2,4}[\\-]{1}[\\d]{1,3}$";
        string studentId = null;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            
        }

        private void Student_Load(object sender, EventArgs e)
        {
            BindGridView();
            BindComboBox();
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

        private void BindComboBox()
        {
            SqlConnection conn = new SqlConnection(constr);
            string query = "select * from Lookup where Category = 'STUDENT_STATUS';";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                string name = reader.GetString(1);
                comboBox1.Items.Add(name);
            }
            comboBox1.SelectedItem = comboBox1.Items[0].ToString();
            conn.Close();
        }

        private int GetComboBoxValue()
        {
            int value = 5;
            string name = comboBox1.SelectedItem.ToString();
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

        private string SetComboBoxValue(string value)
        {
            string name = "Active";
            SqlConnection conn = new SqlConnection(constr);
            string query = $"select * from Lookup where LookupId = '{value}';";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                name = reader.GetString(1);
            }
            conn.Close();
            return name;
        }


        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            studentId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            firstNameTb.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            lastNameTb.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            contactTb.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            emailTb.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            regNoTb.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();

            string value = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            comboBox1.SelectedValue = SetComboBoxValue(value);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(firstNameTb.Text) || (!Regex.IsMatch(firstNameTb.Text[0].ToString(), "[A-Z]")))
            {
                firstNameTb.Focus();
                firstNameErrorProvider.SetError(firstNameTb, "First Letter of Name should be Capital.");
            }
            else if (string.IsNullOrEmpty(lastNameTb.Text) || (!Regex.IsMatch(lastNameTb.Text[0].ToString(), "[A-Z]")))
            {
                lastNameTb.Focus();
                lastNameErrorProvider.SetError(lastNameTb, "First Letter of Name should be Capital.");
            }
            else if (string.IsNullOrEmpty(regNoTb.Text) || !Regex.IsMatch(regNoTb.Text, regNoPattern))
            {
                regNoTb.Focus();
                regNoErrorProvider.SetError(regNoTb, "Follow the Format like: 2023-CS-10");
            }
            else if (string.IsNullOrEmpty(contactTb.Text) || !Regex.IsMatch(contactTb.Text, "[0-9]"))
            {
                contactTb.Focus();
                contactErrorProvider.SetError(contactTb, "Only Numbers allowed.");

            }
            else if (string.IsNullOrEmpty(emailTb.Text) || !Regex.IsMatch(emailTb.Text, emailPattern))
            {
                emailTb.Focus();
                emailErrorProvider.SetError(emailTb, "Invalid Email Format");
            }
            else
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
                cmd.Parameters.AddWithValue("@status", GetComboBoxValue());
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


        private void button2_Click(object sender, EventArgs e)
        {
            if (studentId == null)
            {
                MessageBox.Show("Please Double click the row from the table You want to Update !!", "Failure");
            }
            else
            {
                if (string.IsNullOrEmpty(firstNameTb.Text) || (!Regex.IsMatch(firstNameTb.Text[0].ToString(), "[A-Z]")))
                {
                    firstNameTb.Focus();
                    firstNameErrorProvider.SetError(firstNameTb, "First Letter of Name should be Capital.");
                }
                else if (string.IsNullOrEmpty(lastNameTb.Text) || (!Regex.IsMatch(lastNameTb.Text[0].ToString(), "[A-Z]")))
                {
                    lastNameTb.Focus();
                    lastNameErrorProvider.SetError(lastNameTb, "First Letter of Name should be Capital.");
                }
                else if (string.IsNullOrEmpty(regNoTb.Text) || !Regex.IsMatch(regNoTb.Text, regNoPattern))
                {
                    regNoTb.Focus();
                    regNoErrorProvider.SetError(regNoTb, "Follow the Format like: 2023-CS-10");
                }
                else if (string.IsNullOrEmpty(contactTb.Text) || !Regex.IsMatch(contactTb.Text, "[0-9]"))
                {
                    contactTb.Focus();
                    contactErrorProvider.SetError(contactTb, "Only Numbers allowed.");

                }
                else if (string.IsNullOrEmpty(emailTb.Text) || !Regex.IsMatch(emailTb.Text, emailPattern))
                {
                    emailTb.Focus();
                    emailErrorProvider.SetError(emailTb, "Invalid Email Format");
                }
                else
                {
                    SqlConnection conn = new SqlConnection(constr);
                    conn.Open();
                    string query = "update Student set FirstName = @firstName, LastName = @lastName, Contact = @contact, Email = @email, RegistrationNumber = @regNo, Status = @status where Id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", studentId);
                    cmd.Parameters.AddWithValue("@firstName", firstNameTb.Text);
                    cmd.Parameters.AddWithValue("@lastName", lastNameTb.Text);
                    cmd.Parameters.AddWithValue("@contact", contactTb.Text);
                    cmd.Parameters.AddWithValue("@email", emailTb.Text);
                    cmd.Parameters.AddWithValue("@regNo", regNoTb.Text);
                    cmd.Parameters.AddWithValue("@status", GetComboBoxValue());
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Updated Successfully !!", "Success");
                    }
                    else
                    {
                        MessageBox.Show("Not Updated Successfully !!", "Failure");

                    }
                    ClearField();
                    conn.Close();
                    BindGridView();

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (studentId == null)
            {
                MessageBox.Show("Please Double click the row from the table You want to Delete !!", "Failure");
            }
            else
            {
                if (string.IsNullOrEmpty(firstNameTb.Text) || (!Regex.IsMatch(firstNameTb.Text[0].ToString(), "[A-Z]")))
                {
                    firstNameTb.Focus();
                    firstNameErrorProvider.SetError(firstNameTb, "First Letter of Name should be Capital.");
                }
                else if (string.IsNullOrEmpty(lastNameTb.Text) || (!Regex.IsMatch(lastNameTb.Text[0].ToString(), "[A-Z]")))
                {
                    lastNameTb.Focus();
                    lastNameErrorProvider.SetError(lastNameTb, "First Letter of Name should be Capital.");
                }
                else if (string.IsNullOrEmpty(regNoTb.Text) || !Regex.IsMatch(regNoTb.Text, regNoPattern))
                {
                    regNoTb.Focus();
                    regNoErrorProvider.SetError(regNoTb, "Follow the Format like: 2023-CS-10");
                }
                else if (string.IsNullOrEmpty(contactTb.Text) || !Regex.IsMatch(contactTb.Text, "[0-9]"))
                {
                    contactTb.Focus();
                    contactErrorProvider.SetError(contactTb, "Only Numbers allowed.");

                }
                else if (string.IsNullOrEmpty(emailTb.Text) || !Regex.IsMatch(emailTb.Text, emailPattern))
                {
                    emailTb.Focus();
                    emailErrorProvider.SetError(emailTb, "Invalid Email Format");
                }
                else
                {
                    SqlConnection conn = new SqlConnection(constr);
                    conn.Open();
                    string query = "delete from Student where Id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", studentId);
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


        private void firstNameTb_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(firstNameTb.Text) || (!Regex.IsMatch(firstNameTb.Text[0].ToString(), "[A-Z]")))
            {
                firstNameTb.Focus();
                firstNameErrorProvider.SetError(firstNameTb, "First Letter of Name should be Capital.");
            }
            else
            {
                firstNameErrorProvider.Clear();
            }
        }

        private void lastNameTb_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(lastNameTb.Text) || (!Regex.IsMatch(lastNameTb.Text[0].ToString(), "[A-Z]")))
            {
                lastNameTb.Focus();
                lastNameErrorProvider.SetError(lastNameTb, "First Letter of Name should be Capital.");
            }
            else
            {
                lastNameErrorProvider.Clear();
            }
        }

        private void regNoTb_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(regNoTb.Text) || !Regex.IsMatch(regNoTb.Text, regNoPattern))
            {
                regNoTb.Focus();
                regNoErrorProvider.SetError(regNoTb, "Follow the Format like: 2023-CS-10");
            }
            else
            {
                regNoErrorProvider.Clear();
            }
        }

        private void contactTb_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(contactTb.Text) || !Regex.IsMatch(contactTb.Text, "^[0-9]{9,13}$"))
            {
                contactTb.Focus();
                contactErrorProvider.SetError(contactTb, "Only Numbers allowed.Range: 9 - 13 Digits");

            }
            else
            {
                contactErrorProvider.Clear();
            }
        }

        private void emailTb_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(emailTb.Text) || !Regex.IsMatch(emailTb.Text, emailPattern))
            {
                emailTb.Focus();
                emailErrorProvider.SetError(emailTb, "Invalid Email Format");
            }
            else
            {
                emailErrorProvider.Clear();
            }
        }
    
        private void ClearField()
        {
            firstNameTb.Clear();
            lastNameTb.Clear();
            regNoTb.Clear();
            contactTb.Clear();
            emailTb.Clear();
        }
    
    }
}
