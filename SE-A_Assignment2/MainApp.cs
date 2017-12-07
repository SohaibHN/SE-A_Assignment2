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
using System.IO;
using System.Configuration;

namespace SE_A_Assignment2
{
    public partial class MainApp : Form
    {
        public String MainLoggedInUser;
        public String MainLoggedInCategory;
        public String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BugTrackerDB"].ConnectionString;
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
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage2"])//your specific tabname
            {
                int maxId;
                mySqlConnection = new SqlConnection(connectionString);
                using (SqlCommand dataCommand =
                        new SqlCommand("Select IDENT_CURRENT('tickets')", mySqlConnection))
                {
                    //Select IDENT_CURRENT('tickets')
                    mySqlConnection.Open();
                    maxId = Convert.ToInt32(dataCommand.ExecuteScalar());
                }

                mySqlConnection.Close();
                BugID.Text = maxId.ToString();
                BugDeadlineDate.Text = DateTime.Now.ToString("yyyy-MM-dd");


            }
        }



        private void MainApp_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'bugTrackerDataSet.tickets' table. You can move, or remove it, as needed.
            this.ticketsTableAdapter.Fill(this.bugTrackerDataSet.tickets);
            // TODO: This line of code loads data into the 'bugTrackerDataSet1.tickets' table. You can move, or remove it, as needed.
            //this.ticketsTableAdapter1.Fill(this.bugTrackerDataSet1.tickets);
            if (MainLoggedInCategory != "Admin") { ManageUsers.Visible = false; tabControl1.TabPages.Remove(tabPage5); }

        }

        private void ViewBugs_Click(object sender, EventArgs e)
        {

            // int rowindex = dataGridView1.CurrentCell.RowIndex;
            // int columnindex = dataGridView1.CurrentCell.ColumnIndex;
            //string str = dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString();
            string str = null;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                str = row.Cells[0].Value.ToString(); // gets bug ID and uses it for the bug details form
            }

            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM code_data WHERE [FK_Ticket_ID] = @BUGID", mySqlConnection);

            cmd.Parameters.AddWithValue("@BUGID", str);
            mySqlConnection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    BugDetails BugDetails = new BugDetails();
                    BugDetails.TheValue = str;
                    BugDetails.Show();
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("There is no extra details, would you like to add some?", "Code Details", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //do something
                        CodeDetails CodeDetails = new CodeDetails();
                        CodeDetails.TheValue = str;
                        CodeDetails.Show();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
            }
        }

        private void ManageUsers_Click(object sender, EventArgs e)
        {

        }

        private void Logout_Click(object sender, EventArgs e)
        {
            Application.Restart();
            this.Visible = false;
            Environment.Exit(0);
        }

        private void ChangePass_Click(object sender, EventArgs e)
        {
            bool verifyeduser = false;
            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("select username, passwordhash, category from users where username=@username", mySqlConnection);
            cmd.Parameters.AddWithValue("@username", MainLoggedInUser);
            mySqlConnection.Open();

            if (string.IsNullOrWhiteSpace(NewPass1.Text) || string.IsNullOrEmpty(NewPass2.Text))
            {
                MessageBox.Show("New Details can't be empty");
            }
            else
            {

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        if (BCrypt.Net.BCrypt.Verify(CurrentPass.Text, reader.GetString(1)))
                        {
                            verifyeduser = true;
                            MessageBox.Show(reader.GetString(1));
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Password");
                        }

                    }
                    mySqlConnection.Close();

                    if (verifyeduser)
                    {

                        if (NewPass1.Text == NewPass2.Text)
                        {


                            SqlCommand cmd2 = new SqlCommand("UPDATE users SET passwordhash =@password WHERE username = @usernamesearch", mySqlConnection);
                            string myPassword = NewPass1.Text;


                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(myPassword);
                            MessageBox.Show(hashedPassword);
                            bool validPassword = BCrypt.Net.BCrypt.Verify(myPassword, hashedPassword);

                            cmd2.Parameters.AddWithValue("@usernamesearch", MainLoggedInUser);
                            cmd2.Parameters.AddWithValue("@password", hashedPassword);
                            if (verifyeduser)
                            {
                                mySqlConnection.Open();
                                try
                                {
                                    int i = cmd2.ExecuteNonQuery();

                                    if (i != 0)
                                    {
                                        MessageBox.Show("Password Updated");
                                        
                                    }
                                }
                                catch (SqlException f)
                                {
                                    MessageBox.Show(f.Message);
                                }

                                mySqlConnection.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("New Passwords do not Match");
                        }
                    }
                }
            }
        }

        private void SaveBug_Click(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(connectionString);
           // SqlCommand cmd = new SqlCommand("INSERT INTO tickets (user, description, reproductionsteps, project, status, severity, datelogged, deadline) VALUES (@username, @description, @reproductionsteps, @project, @status, @severity, @datelogged, @deadline)", mySqlConnection);
            SqlCommand cmd = new SqlCommand("INSERT INTO tickets ([user], description, reproductionsteps, project, status, severity, datelogged, deadline) VALUES (@username, @description, @reproductionsteps, @project, @status, @severity, @datelogged, @deadline)", mySqlConnection);
            String Severity;
            if (radioButton1.Checked)
            {
                Severity = radioButton1.Text;
            }
            else if (radioButton2.Checked)
            {
                Severity = radioButton2.Text;
            }
            else if (radioButton3.Checked)
            {
                Severity = radioButton3.Text;
            }
            else
            {
                MessageBox.Show("Select a Severity Value");
                return;
                
            }



            cmd.Parameters.AddWithValue("@username", MainLoggedInUser);
            cmd.Parameters.AddWithValue("@description", BugDesc.Text);
            cmd.Parameters.AddWithValue("@reproductionsteps", BugSteps.Text);
            cmd.Parameters.AddWithValue("@project", BugProject.Text);
            cmd.Parameters.AddWithValue("@status", "Broken");
            cmd.Parameters.AddWithValue("@severity", Severity);
            cmd.Parameters.AddWithValue("@datelogged", DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@deadline", BugDeadlineDate.Text);



            mySqlConnection.Open();
            try
            {
                int i = cmd.ExecuteNonQuery();

                if (i != 0)
                {
                    MessageBox.Show("New Bug reported");
                    this.ticketsTableAdapter.Fill(this.bugTrackerDataSet.tickets);
                    dataGridView1.Update();
                    dataGridView1.Refresh();

                }
            }
            catch (SqlException f)
            {
                MessageBox.Show(f.Message);
            }

            mySqlConnection.Close();
        }

    }
}
