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
    public partial class Form1 : Form
    {
        SqlConnection mySqlConnection;
        public Form1()
        {
            InitializeComponent();
            String[] myData = new String[100];
            mySqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Admin\source\repos\SE-A_Assignment2\SE-A_Assignment2\database\BugTracker.mdf;Integrated Security=True;Connect Timeout=30");
            String selectsmt = "SELECT * FROM tickets";

            SqlCommand mySqlCommand = new SqlCommand(selectsmt, mySqlConnection);

            mySqlConnection.Open();
            

        SqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            int i = 0;
            while (mySqlDataReader.Read())
            {
                Console.WriteLine(mySqlDataReader); //reads a line of the query result at a time
                myData[i++] = (String)mySqlDataReader["User"]; //store in an array too for use later
               
            }

            for (int j = 0; j < i; j++) //now iterate through our good old, bog standard array
            {
                Console.WriteLine("***" + myData[j] + "***");
                MessageBox.Show(myData[j], "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



        }
    }
}
