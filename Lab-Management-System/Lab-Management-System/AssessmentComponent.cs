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
    public partial class AssessmentComponent : Form
    {
        public AssessmentComponent()
        {
            InitializeComponent();
            BindGridView();
            BindComboBox1();
            BindComboBox2();
        }

        string constr = "Data Source = AHMAD-HP; Initial Catalog = ProjectB; Integrated Security = True;";
        string assessmentCompId = null;
        string assessmentId = null;
        string rubricId = null;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
        }

        private void BindGridView()
        {
            SqlConnection connection = new SqlConnection(constr);
            connection.Open();
            string query = "select * from AssessmentComponent;";
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
            string query = "select * from Rubric;";
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

        private void BindComboBox2()
        {
            SqlConnection conn = new SqlConnection(constr);
            string query = "select * from Assessment;";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string name = reader.GetString(1);
                comboBox2.Items.Add(name);
            }
            conn.Close();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "Please Enter the Component Name !!");

            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void numericUpDown1_Leave(object sender, EventArgs e)
        {
            if(numericUpDown1.Value == 0)
            {
                numericUpDown1.Focus();
                errorProvider2.SetError(numericUpDown1, "Marks cannot be set as Zero !!");

            }
            else
            {
                errorProvider2.Clear();
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                comboBox1.Focus();
                errorProvider3.SetError(comboBox1, "Please Select the Rubric from Dropdown !!");
            }
            else
            {
                errorProvider3.Clear();
            }
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox2.Text))
            {
                comboBox2.Focus();
                errorProvider4.SetError(comboBox2, "Please Select the Assessment from Dropdown !!");
            }
            else
            {
                errorProvider4.Clear();
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            assessmentCompId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            rubricId = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            numericUpDown1.Value = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells[3].Value);
            assessmentId = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void ClearField()
        {
            textBox1.Clear();
            numericUpDown1.Value = 0;
            comboBox1.Text = "";
            comboBox2.Text = "";
        }

        private int GetRubricId(string details)
        {
            int id = 1;
            SqlConnection conn = new SqlConnection(constr);
            string query = $"select * from Rubric where Details = '{details}';";
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

        private int GetAssessmentId(string title)
        {
            int id = 1;
            SqlConnection conn = new SqlConnection(constr);
            string query = $"select * from Assessment where Title = '{title}';";
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

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                errorProvider1.SetError(textBox1, "Please Enter the Component Name !!");

            }
            else if (numericUpDown1.Value == 0)
            {
                numericUpDown1.Focus();
                errorProvider2.SetError(numericUpDown1, "Marks cannot be set as Zero !!");

            }
            else if (string.IsNullOrEmpty(comboBox1.Text))
            {
                comboBox1.Focus();
                errorProvider3.SetError(comboBox1, "Please Select the Rubric from Dropdown !!");
            }
            else if (string.IsNullOrEmpty(comboBox2.Text))
            {
                comboBox2.Focus();
                errorProvider4.SetError(comboBox2, "Please Select the Assessment from Dropdown !!");
            }
            else
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                string query = "insert into AssessmentComponent (Name, RubricId, TotalMarks, DateCreated, DateUpdated, AssessmentId) values (@name, @rubricId, @totalMarks, @dateCreated, @dateUpdated, @assessmentId)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", textBox1.Text);
                cmd.Parameters.AddWithValue("@totalMarks", Convert.ToInt32(numericUpDown1.Value));
                cmd.Parameters.AddWithValue("@rubricId", GetRubricId(comboBox1.Text));
                DateTime dt = DateTime.Now;
                cmd.Parameters.AddWithValue("@dateCreated", dt);
                cmd.Parameters.AddWithValue("@dateUpdated", dt);
                cmd.Parameters.AddWithValue("@assessmentId", GetAssessmentId(comboBox2.Text));

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
            if(assessmentCompId == null || assessmentId == null || rubricId == null)
            {
                MessageBox.Show("Please Double click the row from the table You want to Update !!", "Failure");
            }
            else
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox1.Focus();
                    errorProvider1.SetError(textBox1, "Please Enter the Component Name !!");

                }
                else if (numericUpDown1.Value == 0)
                {
                    numericUpDown1.Focus();
                    errorProvider2.SetError(numericUpDown1, "Marks cannot be set as Zero !!");

                }
                else if (string.IsNullOrEmpty(comboBox1.Text))
                {
                    comboBox1.Focus();
                    errorProvider3.SetError(comboBox1, "Please Select the Rubric from Dropdown !!");
                }
                else if (string.IsNullOrEmpty(comboBox2.Text))
                {
                    comboBox2.Focus();
                    errorProvider4.SetError(comboBox2, "Please Select the Assessment from Dropdown !!");
                }
                else
                {
                    SqlConnection conn = new SqlConnection(constr);
                    conn.Open();
                    string query = "update AssessmentComponent set Name = @name, RubricId = @rubricId, TotalMarks = @totalMarks, DateUpdated =  @dateUpdated, AssessmentId = @assessmentId where Id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", assessmentCompId);
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@rubricId", rubricId);
                    cmd.Parameters.AddWithValue("@totalMarks", Convert.ToInt32(numericUpDown1.Value));
                    DateTime dt = DateTime.Now;
                    cmd.Parameters.AddWithValue("@dateUpdated", dt);
                    cmd.Parameters.AddWithValue("@assessmentId", assessmentId);

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
            if (assessmentCompId == null)
            {
                MessageBox.Show("Please Double click the row from the table You want to Delete !!", "Failure");
            }
            else
            {
                if (string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox1.Focus();
                    errorProvider1.SetError(textBox1, "Please Enter the Component Name !!");

                }
                else if (numericUpDown1.Value == 0)
                {
                    numericUpDown1.Focus();
                    errorProvider2.SetError(numericUpDown1, "Marks cannot be set as Zero !!");

                }
                else if (string.IsNullOrEmpty(comboBox1.Text))
                {
                    comboBox1.Focus();
                    errorProvider3.SetError(comboBox1, "Please Select the Rubric from Dropdown !!");
                }
                else if (string.IsNullOrEmpty(comboBox2.Text))
                {
                    comboBox2.Focus();
                    errorProvider4.SetError(comboBox2, "Please Select the Assessment from Dropdown !!");
                }
                else
                {
                    SqlConnection conn = new SqlConnection(constr);
                    conn.Open();
                    string query = "delete from AssessmentComponent where Id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", assessmentCompId);
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
