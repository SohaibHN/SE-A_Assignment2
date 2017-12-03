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
        public AddUser()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //SqlConnection con = new SqlConnection("Data Source=NiluNilesh;Integrated Security=True");  
            mySqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\source\repos\SE-A_Assignment2\SE-A_Assignment2\database\BugTracker.mdf;Integrated Security=True;Connect Timeout=30");
            SqlCommand cmd = new SqlCommand("INSERT INTO users (username, passwordhash, category) VALUES (@username, @password, @category)", mySqlConnection);
            string myPassword = textBox2.Text;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(myPassword);
            bool validPassword = BCrypt.Net.BCrypt.Verify(myPassword, hashedPassword);

            cmd.Parameters.AddWithValue("@username", textBox1.Text);
            cmd.Parameters.AddWithValue("@password", hashedPassword);
            cmd.Parameters.AddWithValue("@category", textBox3.Text);
            mySqlConnection.Open();
            int i = cmd.ExecuteNonQuery();

            mySqlConnection.Close();

            if (i != 0)
            {
                MessageBox.Show(i + "Data Saved");
            }

            MessageBox.Show(myPassword);
            MessageBox.Show(hashedPassword);
            if (validPassword) { MessageBox.Show("works"); }
        }
    }
}
