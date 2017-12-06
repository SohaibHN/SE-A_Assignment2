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

namespace SE_A_Assignment2
{
    public partial class loginform : Form
    {
        SqlConnection mySqlConnection;
        public static String LoggedInUser;
        public static String LoggedInCategory;
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
            int maxId;

            mySqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\source\repos\SE-A_Assignment2\SE-A_Assignment2\database\BugTracker.mdf;Integrated Security=True;Connect Timeout=30");

            SqlCommand cmd = new SqlCommand("select username, passwordhash, category from users where username=@username", mySqlConnection);
            cmd.Parameters.AddWithValue("@username", textBox1.Text);
            mySqlConnection.Open();

            if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Login Details can't be empty");
            }
            else
            {

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        if (BCrypt.Net.BCrypt.Verify(textBox2.Text, reader.GetString(1)))
                        {
                            //login
                            LoggedInUser = textBox1.Text;
                            LoggedInCategory = reader.GetString(2);
                            DialogResult = System.Windows.Forms.DialogResult.OK;
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Username or Password");
                        }

                    }
                }
                mySqlConnection.Close();
            }
        }

        private void New_User_Click(object sender, EventArgs e)
        {
            AddUser RegisterForm = new AddUser();
            //this.Visible = false;
            RegisterForm.Show();
        }
    }
}
