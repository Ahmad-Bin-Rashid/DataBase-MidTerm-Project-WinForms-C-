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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab_Management_System
{
    public partial class Assessment : Form
    {
        public Assessment()
        {
            InitializeComponent();
            BindGridView();
        }

        string constr = "Data Source = AHMAD-HP; Initial Catalog = ProjectB; Integrated Security = True;";
        string assessmentId = null;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
        }

        private void BindGridView()
        {
            SqlConnection connection = new SqlConnection(constr);
            connection.Open();
            string query = "select * from Assessment;";
            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            connection.Close();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "Please write the Title !!");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void numericUpDown1_Leave(object sender, EventArgs e)
        {
            if (numericUpDown1.Value == 0)
            {
                numericUpDown1.Focus();
                errorProvider2.SetError(numericUpDown1, "Total Marks cannot be Zero !!");
            }
            else
            {
                errorProvider2.Clear();
            }
        }

        private void numericUpDown2_Leave(object sender, EventArgs e)
        {
            if (numericUpDown2.Value == 0)
            {
                numericUpDown2.Focus();
                errorProvider3.SetError(numericUpDown2, "Total Marks cannot be Zero !!");
            }
            else
            {
                errorProvider3.Clear();
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            assessmentId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            numericUpDown1.Value = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[3].Value);
            numericUpDown2.Value = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[4].Value);
        }

        private void ClearField()
        {
            textBox1.Clear();
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "Please write the Title !!");
            }
            else if (numericUpDown1.Value == 0)
            {
                numericUpDown1.Focus();
                errorProvider2.SetError(numericUpDown1, "Total Marks cannot be Zero !!");
            }
            else if (numericUpDown2.Value == 0)
            {
                numericUpDown2.Focus();
                errorProvider3.SetError(numericUpDown2, "Total Marks cannot be Zero !!");
            }
            else
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                string query = "insert into Assessment (Title, DateCreated, TotalMarks, TotalWeightage) values (@title, @dateCreated, @totalMarks, @totalWeightage)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@title", textBox1.Text);
                DateTime dt = DateTime.Now;
                cmd.Parameters.AddWithValue("@dateCreated", dt);
                cmd.Parameters.AddWithValue("@totalMarks", Convert.ToInt32(numericUpDown1.Value));
                cmd.Parameters.AddWithValue("@totalWeightage", Convert.ToInt32(numericUpDown2.Value));

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

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (assessmentId == null)
            {
                MessageBox.Show("Please Double click the row from the table You want to Update !!", "Failure");
            }
            else
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox1.Focus();
                    errorProvider1.SetError(textBox1, "Please write the Title !!");
                }
                else if (numericUpDown1.Value == 0)
                {
                    numericUpDown1.Focus();
                    errorProvider2.SetError(numericUpDown1, "Total Marks cannot be Zero !!");
                }
                else if (numericUpDown2.Value == 0)
                {
                    numericUpDown2.Focus();
                    errorProvider3.SetError(numericUpDown2, "Total Marks cannot be Zero !!");
                }
                else
                {
                    SqlConnection conn = new SqlConnection(constr);
                    conn.Open();
                    string query = "update Assessment set Title = @title, TotalMarks = @totalMarks, TotalWeightage =  @totalWeightage where Id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", assessmentId);
                    cmd.Parameters.AddWithValue("@title", textBox1.Text);
                    cmd.Parameters.AddWithValue("@totalMarks", Convert.ToInt32(numericUpDown1.Value));
                    cmd.Parameters.AddWithValue("@totalWeightage", Convert.ToInt32(numericUpDown2.Value));

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
            if (assessmentId == null)
            {
                MessageBox.Show("Please Double click the row from the table You want to Delete !!", "Failure");
            }
            else
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox1.Focus();
                    errorProvider1.SetError(textBox1, "Please write the Title !!");
                }
                else if (numericUpDown1.Value == 0)
                {
                    numericUpDown1.Focus();
                    errorProvider2.SetError(numericUpDown1, "Total Marks cannot be Zero !!");
                }
                else if (numericUpDown2.Value == 0)
                {
                    numericUpDown2.Focus();
                    errorProvider3.SetError(numericUpDown2, "Total Marks cannot be Zero !!");
                }
                else
                {
                    SqlConnection conn = new SqlConnection(constr);
                    conn.Open();
                    string query = "delete from Assessment where Id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", assessmentId);
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
