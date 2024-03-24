using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_Management_System
{
    public partial class HomePage : Form
    {
        public HomePage()
        {
            InitializeComponent();
        }

        public static Student studentPage = new Student();
        public static StudentAttendance studentAttendancePage = new StudentAttendance();
        public static StudentResult studentResultPage = new StudentResult();
        public static Clo cloPage = new Clo();
        public static Rubric rubricPage = new Rubric();
        public static RubricLevel rubricLevelPage = new RubricLevel();
        public static Assessment assessmentPage = new Assessment();
        public static AssessmentComponent assessmentComponentPage = new AssessmentComponent();

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            studentPage.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            studentAttendancePage.Show();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            cloPage.Show();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rubricPage.Show();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            rubricLevelPage.Show();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            assessmentPage.Show();
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            assessmentComponentPage.Show();
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            studentResultPage.Show();
        }
    }
}
