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
    public partial class StudentResult : Form
    {
        public StudentResult()
        {
            InitializeComponent();
        }

        string constr = "Data Source = AHMAD-HP; Initial Catalog = ProjectB; Integrated Security = True;";
        string componentMarks = null;

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
        }

        private void StudentResult_Load(object sender, EventArgs e)
        {
            BindGridView();
            BindComboBox1();
            BindComboBox2();
            BindComboBox3();
        }


        private void BindGridView()
        {
            SqlConnection connection = new SqlConnection(constr);
            connection.Open();
            string query = "select SR.StudentId, SR.AssessmentComponentId, S.RegistrationNumber, AC.Name, AC.TotalMarks, SR.RubricMeasurementId, SR.EvaluationDate from StudentResult as SR inner join Student as S on SR.StudentId = S.Id inner join AssessmentComponent as AC on SR.AssessmentComponentId = AC.Id;";
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
            string query = "select * from AssessmentComponent;";
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

        private void BindComboBox3()
        {
            comboBox3.Items.Add("Exceptional (4)");
            comboBox3.Items.Add("Good (3)");
            comboBox3.Items.Add("Fair (2)");
            comboBox3.Items.Add("Unsatisfactory (1)");
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                comboBox1.Focus();
                errorProvider1.SetError(comboBox1, "Please Select the Student !!");

            }
            else
            {
                errorProvider1.Clear();
            }
        }

        private void comboBox2_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox2.Text))
            {
                comboBox2.Focus();
                errorProvider2.SetError(comboBox2, "Please Select the Assessment Component !!");

            }
            else
            {
                errorProvider2.Clear();
            }
        }

        private void comboBox3_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(comboBox3.Text))
            {
                comboBox3.Focus();
                errorProvider3.SetError(comboBox3, "Please Select the Rubric Level !!");

            }
            else
            {
                errorProvider3.Clear();
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            componentMarks = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            comboBox3.Text = SetRubricLevel(dataGridView1.SelectedRows[0].Cells[5].Value.ToString());
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[6].Value);
        }

        private string SetRubricLevel(string value)
        {
            if (value== "4")
            {
                return "Exceptional (4)";
            }
            else if (value == "3")
            {
                return "Good (3)";
            }
            else if (value == "2")
            {
                return "Fair (2)";
            }
            else if (value == "1")
            {
                return "Unsatisfactory (1)";
            }
            else
            {
                return "";
            }
        }

        private int GetRubricLevel(string value)
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

        private void EvaluateBtn_Click(object sender, EventArgs e)
        {
            if (componentMarks == null)
            {
                MessageBox.Show("Please Double Click the rows from table you want to Evaluate !!");
            }
            else
            {
                int obtainedLevel = GetRubricLevel(comboBox3.Text);
                string obtainedMarks = Convert.ToString((obtainedLevel * Convert.ToDouble(componentMarks) / 4));
                textBox1.Text = obtainedMarks;

            }
        }

        private void ClearField()
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            comboBox3.Text = "";
        }

        private int GetStudentId(string regNo)
        {
            int id = 1;
            SqlConnection conn = new SqlConnection(constr);
            string query = $"select * from Student where RegistrationNumber = '{regNo}';";
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

        private int GetComponentId(string Name)
        {
            int id = 1;
            SqlConnection conn = new SqlConnection(constr);
            string query = $"select * from AssessmentComponent where Name = '{Name}';";
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
            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                comboBox1.Focus();
                errorProvider1.SetError(comboBox1, "Please Select the Student !!");

            }
            else if (string.IsNullOrEmpty(comboBox2.Text))
            {
                comboBox2.Focus();
                errorProvider2.SetError(comboBox2, "Please Select the Assessment Component !!");

            }
            else if (string.IsNullOrEmpty(comboBox3.Text))
            {
                comboBox3.Focus();
                errorProvider3.SetError(comboBox3, "Please Select the Rubric Level !!");

            }
            else
            {
                SqlConnection conn = new SqlConnection(constr);
                conn.Open();
                string query = "insert into StudentResult (StudentId, AssessmentComponentId, RubricMeasurementId, EvaluationDate) values (@studentId, @assessmentComponentId, @rubricMeasurementId, @evaluationDate)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@studentId", GetStudentId(comboBox1.Text));
                cmd.Parameters.AddWithValue("@assessmentComponentId", GetComponentId(comboBox2.Text));
                cmd.Parameters.AddWithValue("@rubricMeasurementId", GetRubricLevel(comboBox3.Text));
                cmd.Parameters.AddWithValue("@evaluationDate", dateTimePicker1.Value);
                
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
    }
}
