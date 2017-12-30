using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SE_A_Assignment2
{
    public partial class loginform : Form
    {
        SqlConnection mySqlConnection;
        public static String LoggedInUser;
        public static String LoggedInCategory;
        public String connectionString = ConfigurationManager.ConnectionStrings["BugTrackerDB"].ConnectionString;
       // String connectionString = (@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\source\repos\SE-A_Assignment2\SE-A_Assignment2\database\BugTracker.mdf;Integrated Security=True;Connect Timeout=30");
        public loginform()
        {
            InitializeComponent();
            this.Text = "Login - Bug Tracker";

            CodeDetails CodeDetails = new CodeDetails();
            //this.Visible = false;
            //CodeDetails.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {

            String username = textBox1.Text;
            String password = textBox2.Text;
            Login(username, password);

        }

        public bool Login(string username, string password)
        {
            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("select username, passwordhash, category from users where username=@username", mySqlConnection);
            cmd.Parameters.AddWithValue("@username", username);
            mySqlConnection.Open();

            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Login Details can't be empty");
            }
            else
            {

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        if (BCrypt.Net.BCrypt.Verify(password, reader.GetString(1)))
                        {
                            //login
                            LoggedInUser = username;
                            LoggedInCategory = reader.GetString(2);
                            return true;
                            DialogResult = System.Windows.Forms.DialogResult.OK;
                            
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Password");
                            return false;

                        }

                    }
                    if (!reader.HasRows) { MessageBox.Show("Username does not exist"); }
                }
                mySqlConnection.Close();
                return false;

            }
            return false;

        }

        private void New_User_Click(object sender, EventArgs e)
        {
            AddUser RegisterForm = new AddUser();
            //this.Visible = false;
            RegisterForm.Show();
        }
    }
}
