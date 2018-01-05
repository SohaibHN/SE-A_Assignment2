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
    /// <summary>  
    ///  basic login form with links to AddUser.cs 
    ///  password uses bcrypt for encryption
    /// </summary>  
    public partial class loginform : Form
    {
        SqlConnection mySqlConnection;
        public static String LoggedInUser; //used in main app
        public static String LoggedInCategory; //used in main app
        public String connectionString = ConfigurationManager.ConnectionStrings["BugTrackerDB"].ConnectionString;
        public loginform()
        {
            InitializeComponent();
            this.Text = "Bug Tracker - Login";

            CodeDetails CodeDetails = new CodeDetails();
            //this.Visible = false;
            //CodeDetails.Show();
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            String username = textBox1.Text;
            String password = textBox2.Text;
            Login(username, password); //login method with values added

        }

        // Multiple parameters.
        /// <param name="username">Used to indicate username to attempt login with</param>
        /// <param name="password">Authenticate against username.</param>
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

                        if (BCrypt.Net.BCrypt.Verify(password, reader.GetString(1))) //checks salt stored in db with user entered pass
                        {
                            //login
                            LoggedInUser = username;
                            LoggedInCategory = reader.GetString(2);                    
                            DialogResult = System.Windows.Forms.DialogResult.OK; //used to run main app class 
                            return true;

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
