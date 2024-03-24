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
using System.Xml.Linq;

namespace Lab_Management_System
{
    public partial class RubricLevel : Form
    {
        public RubricLevel()
        {
            InitializeComponent();
            
        }

        string constr = "Data Source = AHMAD-HP; Initial Catalog = ProjectB; Integrated Security = True;";
        string rubricLevelId = null;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
        }

        private void BindGridView()
        {
            SqlConnection connection = new SqlConnection(constr);
            connection.Open();
            string query = "select RubricLevel.Id, Rubric.Details As 'Rubric-Details', RubricLevel.Details As 'Level-Details', RubricLevel.MeasurementLevel from RubricLevel inner join Rubric on RubricLevel.RubricId = Rubric.Id;";
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
            comboBox2.Items.Add("Exceptional (4)");
            comboBox2.Items.Add("Good (3)");
            comboBox2.Items.Add("Fair (2)");
            comboBox2.Items.Add("Unsatisfactory (1)");
        }

        private void ClearField()
        {
            comboBox1.Text = "";
            textBox1.Clear();
            comboBox2.Text = "";
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                comboBox1.Focus();
                errorProvider1.SetError(comboBox1, "Please Select Rubric !!");
            }
            else if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                errorProvider2.SetError(textBox1, "Please Fill the TextBox !!");
            }
            else if (string.IsNullOrEmpty(comboBox2.Text))
            {
                comboBox2.Focus();
                errorProvider3.SetError(comboBox2, "Please Select Measurement Level !!");
            }
            else
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                string query = "insert into RubricLevel (RubricId, Details, MeasurementLevel) values (@rubricId, @details, @measurementLevel)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@rubricId", GetRubricId(comboBox1.Text));
                cmd.Parameters.AddWithValue("@details", textBox1.Text);
                cmd.Parameters.AddWithValue("@measurementLevel", GetMeasurementLevel(comboBox2.Text));

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

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                comboBox1.Focus();
                errorProvider1.SetError(comboBox1, "Please Select Rubric !!");
            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Focus();
                errorProvider2.SetError(textBox1, "Please Fill the TextBox !!");
            }
            else
            {
                errorProvider2.Clear();
            }
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox2.Text))
            {
                comboBox2.Focus();
                errorProvider3.SetError(comboBox2, "Please Select Measurement Level !!");
            }
            else
            {
                errorProvider3.Clear();
            }
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

        private int GetMeasurementLevel(string value)
        {
            if (value == "Exceptional (4)")
            {
                return 4;
            }
            else if (value == "Good (3)")
            {
                return 3;
            }
            else if (value == "Fair (2)")
            {
                return 2;
            }
            else if (value == "Unsatisfactory (1)")
            {
                return 1;
            }
            else
            {
                return 4;
            }
        }

        private string SetMeasurementLevel(string number)
        {
            if (number == "4")
            {
                return "Exceptional (4)";
            }
            else if (number == "3")
            {
                return "Good (3)";
            }
            else if (number == "2")
            {
                return "Fair (2)";
            }
            else if (number == "1")
            {
                return "Unsatisfactory (1)";
            }
            else
            {
                return "";
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            rubricLevelId = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            comboBox2.Text = SetMeasurementLevel(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());

        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (rubricLevelId == null)
            {
                MessageBox.Show("Please Double click the row from the table You want to Update !!", "Failure");
            }
            else
            {
                if (string.IsNullOrEmpty(comboBox1.Text))
                {
                    comboBox1.Focus();
                    errorProvider1.SetError(comboBox1, "Please Select Rubric !!");
                }
                else if (string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox1.Focus();
                    errorProvider2.SetError(textBox1, "Please Fill the TextBox !!");
                }
                else if (string.IsNullOrEmpty(comboBox2.Text))
                {
                    comboBox2.Focus();
                    errorProvider3.SetError(comboBox2, "Please Select Measurement Level !!");
                }
                else
                {
                    SqlConnection conn = new SqlConnection(constr);
                    conn.Open();
                    string query = "update RubricLevel set RubricId = @rubricId, Details = @details, MeasurementLevel =  @measureLevel where Id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", rubricLevelId);
                    cmd.Parameters.AddWithValue("@rubricId", GetRubricId(comboBox1.Text));
                    cmd.Parameters.AddWithValue("@details", textBox1.Text);
                    cmd.Parameters.AddWithValue("@measureLevel", GetMeasurementLevel(comboBox2.Text));

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
            if (rubricLevelId == null)
            {
                MessageBox.Show("Please Double click the row from the table You want to Delete !!", "Failure");
            }
            else
            {
                if (string.IsNullOrEmpty(comboBox1.Text))
                {
                    comboBox1.Focus();
                    errorProvider1.SetError(comboBox1, "Please Select Rubric !!");
                }
                else if (string.IsNullOrEmpty(textBox1.Text))
                {
                    textBox1.Focus();
                    errorProvider2.SetError(textBox1, "Please Fill the TextBox !!");
                }
                else if (string.IsNullOrEmpty(comboBox2.Text))
                {
                    comboBox2.Focus();
                    errorProvider3.SetError(comboBox2, "Please Select Measurement Level !!");
                }
                else
                {
                    SqlConnection conn = new SqlConnection(constr);
                    conn.Open();
                    string query = "delete from RubricLevel where Id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", rubricLevelId);
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

        private void RubricLevel_Load(object sender, EventArgs e)
        {
            BindGridView();
            BindComboBox();
            BindComboBox2();
        }
    }
}
