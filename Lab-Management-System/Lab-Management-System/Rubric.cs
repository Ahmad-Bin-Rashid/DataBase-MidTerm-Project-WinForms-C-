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

namespace Lab_Management_System
{
    public partial class Rubric : Form
    {
        public Rubric()
        {
            InitializeComponent();
        }

        string constr = "Data Source = AHMAD-HP; Initial Catalog = ProjectB; Integrated Security = True;";
        string RubricId = null;

        private void Rubric_Load(object sender, EventArgs e)
        {
            BindGridView();
            BindComboBox();
        }

        private void BindGridView()
        {
            SqlConnection connection = new SqlConnection(constr);
            connection.Open();
            string query = "select Rubric.Id, Rubric.Details As 'Rubric-Details', Clo.Name As 'Clo-Name' from Rubric inner join Clo on Rubric.CloId = Clo.Id;";
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
            string query = "select * from Clo;";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(1);
                comboBox1.Items.Add(name);
            }
            conn.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
        }


        private string GetCloName(int id)
        {
            string name = null;
            SqlConnection conn = new SqlConnection(constr);
            string query = $"select * from Clo where Id = {id};";
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

        private int GetCloId(string name)
        {
            int id = 1;
            SqlConnection conn = new SqlConnection(constr);
            string query = $"select * from Clo where Name = '{name}';";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                id = reader.GetInt32(0);
            }
            conn.Close();
            return id;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "Please fill the TextBox !!");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "Please fill the TextBox !!");
            }
            else if (string.IsNullOrEmpty(comboBox1.SelectedItem.ToString()))
            {
                comboBox1.Focus();
                errorProvider2.SetError(comboBox1, "Please Select any Clo Name !!");

            }
            else
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                string query = "insert into Rubric (Details, CloId) values (@details, @cloId)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@details", textBox1.Text);
                cmd.Parameters.AddWithValue("@cloId", GetCloId(comboBox1.Text));

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

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            RubricId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            comboBox1.SelectedItem = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void ClearField()
        {
            textBox1.Clear();
            comboBox1.Text = "";
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                comboBox1.Focus();
                errorProvider2.SetError(comboBox1, "Please Select any Clo Name !!");

            }
            else
            {
                errorProvider2.Clear();
            }
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (RubricId == null)
            {
                MessageBox.Show("Please Double click the row from the table You want to Update !!", "Failure");
            }
            else
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox1.Focus();
                    errorProvider1.SetError(textBox1, "Please fill the TextBox !!");
                }
                else if (string.IsNullOrEmpty(comboBox1.SelectedItem.ToString()))
                {
                    comboBox1.Focus();
                    errorProvider2.SetError(comboBox1, "Please Select any Clo Name !!");

                }
                else
                {
                    SqlConnection conn = new SqlConnection(constr);
                    conn.Open();
                    string query = "update Rubric set Details = @details, CloId =  @cloId where Id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", RubricId);
                    cmd.Parameters.AddWithValue("@details", textBox1.Text);
                    cmd.Parameters.AddWithValue("@cloId", GetCloId(comboBox1.Text));

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

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (RubricId == null)
            {
                MessageBox.Show("Please Double click the row from the table You want to Delete !!", "Failure");
            }
            else
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox1.Focus();
                    errorProvider1.SetError(textBox1, "Please fill the TextBox !!");
                }
                else if (string.IsNullOrEmpty(comboBox1.SelectedItem.ToString()))
                {
                    comboBox1.Focus();
                    errorProvider2.SetError(comboBox1, "Please Select any Clo Name !!");

                }
                else
                {
                    SqlConnection conn = new SqlConnection(constr);
                    conn.Open();
                    string query = "delete from Rubric where Id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", RubricId);
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
}
