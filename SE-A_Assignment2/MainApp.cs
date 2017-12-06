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
using ScintillaNET;

namespace SE_A_Assignment2
{
    public partial class MainApp : Form
    {
        public String MainLoggedInUser;
        public String MainLoggedInCategory;
        SqlConnection mySqlConnection;
        public MainApp()
        {
            InitializeComponent();
            this.Text = "Bug Tracker";
            tabPage1.Text = @"View Bugs";
            tabPage2.Text = @"Add New Bug";
            this.tabControl1.SelectedTab = tabPage1;

            MainLoggedInUser = loginform.LoggedInUser;
            MainLoggedInCategory = loginform.LoggedInCategory;


            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
        }

        private void tabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {

            MessageBox.Show("You are in the TabControl.SelectedIndexChanged event.");

            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage2"])//your specific tabname
            {
                mySqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\source\repos\SE-A_Assignment2\SE-A_Assignment2\database\BugTracker.mdf;Integrated Security=True;Connect Timeout=30");
                //SqlCommand cmd = new SqlCommand(" Select IDENT_CURRENT('tickets')", mySqlConnection);
                int maxId;
                using (SqlCommand dataCommand =
                        new SqlCommand("Select IDENT_CURRENT('tickets')", mySqlConnection))
                {
                    //Select IDENT_CURRENT('tickets')
                    mySqlConnection.Open();
                    maxId = Convert.ToInt32(dataCommand.ExecuteScalar());
                }

                mySqlConnection.Close();

                BugID.Text = maxId.ToString();


            }
        }


        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void MainApp_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'bugTrackerDataSet1.tickets' table. You can move, or remove it, as needed.
            this.ticketsTableAdapter1.Fill(this.bugTrackerDataSet1.tickets);
            if (MainLoggedInCategory != "Admin") { ManageUsers.Visible = false; tabControl1.TabPages.Remove(tabPage5); }
            
        }

        private void ViewBugs_Click(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\source\repos\SE-A_Assignment2\SE-A_Assignment2\database\BugTracker.mdf;Integrated Security=True;Connect Timeout=30");
            //SqlCommand cmd = new SqlCommand(" Select IDENT_CURRENT('tickets')", mySqlConnection);
            int maxId;
            using (SqlCommand dataCommand =
                    new SqlCommand("Select IDENT_CURRENT('tickets')", mySqlConnection))
            {
                //Select IDENT_CURRENT('tickets')
                mySqlConnection.Open();
                maxId = Convert.ToInt32(dataCommand.ExecuteScalar());
            }

            //MessageBox.Show(maxId.ToString());

            mySqlConnection.Close();

            int rowindex = dataGridView1.CurrentCell.RowIndex;
            int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            string str = dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString();


            MessageBox.Show(str);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ManageUsers_Click(object sender, EventArgs e)
        {

        }

        private void Logout_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }
    }
}
