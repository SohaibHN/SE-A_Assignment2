using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SE_A_Assignment2
{
    public partial class MainApp : Form
    {
        public String MainLoggedInUser;
        public String MainLoggedInCategory;

        public MainApp()
        {
            InitializeComponent();
            this.Text = "Bug Tracker";

            MainLoggedInUser = loginform.LoggedInUser;
            MainLoggedInCategory = loginform.LoggedInCategory;

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void MainApp_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'bugTrackerDataSet.tickets' table. You can move, or remove it, as needed.
            this.ticketsTableAdapter.Fill(this.bugTrackerDataSet.tickets);
            if (MainLoggedInCategory != "Admin") { ManageUsers.Visible = false; }
            
        }

        private void ViewBugs_Click(object sender, EventArgs e)
        {

        }
    }
}
