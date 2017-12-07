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
    public partial class AddUser : Form
    {
        SqlConnection mySqlConnection;
        public String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BugTrackerDB"].ConnectionString;
        int i = 0;
        public AddUser()
        {
            InitializeComponent();
            this.Text = "Add New User - Bug Tracker";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //SqlConnection con = new SqlConnection("Data Source=NiluNilesh;Integrated Security=True");  
            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO users (username, passwordhash, category) VALUES (@username, @password, @category)", mySqlConnection);
            string myPassword = textBox2.Text;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(myPassword);
            bool validPassword = BCrypt.Net.BCrypt.Verify(myPassword, hashedPassword);

            cmd.Parameters.AddWithValue("@username", textBox1.Text);
            cmd.Parameters.AddWithValue("@password", hashedPassword);
            cmd.Parameters.AddWithValue("@category", textBox3.Text);
            mySqlConnection.Open();
            try
            {
                int i = cmd.ExecuteNonQuery();

                if (i != 0)
                {
                    MessageBox.Show("New User Added");
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("User exists, please choose another");
            }

            mySqlConnection.Close();



            if (validPassword) {  }
        }
    }
}
