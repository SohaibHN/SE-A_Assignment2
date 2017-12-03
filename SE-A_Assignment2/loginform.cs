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
        public loginform()
        {
            InitializeComponent();
            String[] myData = new String[100];

        }

        private void button1_Click(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\source\repos\SE-A_Assignment2\SE-A_Assignment2\database\BugTracker.mdf;Integrated Security=True;Connect Timeout=30");
            SqlCommand cmd = new SqlCommand("select username, passwordhash from users where username=@username", mySqlConnection);
            cmd.Parameters.AddWithValue("@username", textBox1.Text);
            mySqlConnection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {

                    if (BCrypt.Net.BCrypt.Verify(textBox2.Text, reader.GetString(1)))
                    {
                        //login
                        MessageBox.Show("Works");
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Please enter Correct Username and Password");
                    }


                    

                }
            }
            mySqlConnection.Close();
            /*
            //cmd.Parameters.AddWithValue("@password", textBox2.Text);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            

            DataTable dt = new DataTable();
            sda.Fill(dt);
            mySqlConnection.Open();
            int i = cmd.ExecuteNonQuery();
            mySqlConnection.Close();

            if (dt.Rows.Count > 0)
            {
                MainApp settingsForm = new MainApp();
                //this.Visible = false;
                //settingsForm.Show();
                //this.Close();
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }

            else
            {

                MessageBox.Show("Please enter Correct Username and Password");
            }
            */
        }
    }
}
