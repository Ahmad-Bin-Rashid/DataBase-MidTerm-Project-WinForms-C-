using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_Management_System
{
    public partial class Clo : Form
    {
        public Clo()
        {
            InitializeComponent();
        }

        string constr = "Data Source = AHMAD-HP; Initial Catalog = ProjectB; Integrated Security = True;";
        string CloId = null;

        private void Clo_Load(object sender, EventArgs e)
        {
            BindGridView();
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
        }


        private void BindGridView()
        {
            SqlConnection connection = new SqlConnection(constr);
            connection.Open();
            string query = "select * from Clo;";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "Should be Filled...");
            }
            else
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                string query = "insert into Clo (Name, DateCreated, DateUpdated) values (@name, @dateCreated, @dateUpdated)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", textBox1.Text);
                DateTime dt = DateTime.Now;
                cmd.Parameters.AddWithValue("@dateCreated", dt);
                cmd.Parameters.AddWithValue("@dateUpdated", dt);

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

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (CloId == null)
            {
                MessageBox.Show("Double Click the Row from the table you want to Update !!");
            }
            else
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox1.Focus();
                    errorProvider1.SetError(textBox1, "Should be Filled...");
                }
                else
                {
                    SqlConnection conn = new SqlConnection(constr);
                    conn.Open();
                    string query = "update Clo set Name = @name, DateUpdated =  @dateUpdated where Id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", CloId);
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    DateTime dt = DateTime.Now;
                    cmd.Parameters.AddWithValue("@dateUpdated", dt);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Updated Successfully !!", "Success");
                    }
                    else
                    {
                        MessageBox.Show("Not Updated Successfully !!", "Failure");

                    }
                    conn.Close();
                    BindGridView();
                }
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (CloId == null)
            {
                MessageBox.Show("Double Click the Row from the table you want to Delete !!");
            }
            else
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox1.Focus();
                    errorProvider1.SetError(textBox1, "Should be Filled...");
                }
                else
                {
                    SqlConnection conn = new SqlConnection(constr);
                    conn.Open();
                    string query = "delete from Clo where Id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", CloId);
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        MessageBox.Show("Deleted Successfully !!", "Success");
                    }
                    else
                    {
                        MessageBox.Show("Not Deleted Successfully !!", "Failure");

                    }
                    conn.Close();
                    BindGridView();
                }
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "Should be Filled...");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            CloId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
        }
    }
}
