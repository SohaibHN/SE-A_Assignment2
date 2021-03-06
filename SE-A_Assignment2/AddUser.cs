﻿using System;
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
    /// <summary>  
    ///  Allows user to register
    ///  Inserts into user table
    /// </summary>  
    public partial class AddUser : Form
    {
        SqlConnection mySqlConnection;
        public String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BugTrackerDB"].ConnectionString;
        int i = 0;
        public AddUser()
        {
            InitializeComponent();
            this.Text = "Bug Tracker - Register";
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(Username.Text) || string.IsNullOrEmpty(Password.Text) || string.IsNullOrEmpty(Category.Text)) // values required
            {

            }
            else
            {
                string OrigPassword = Password.Text;
                string User = Username.Text;
                string Cate = Category.Text;
                register(User, OrigPassword, Cate); //runs method with user fields
            }
        }

        // Multiple parameters.
        /// <param name="username">Used to indicate username to create account</param>
        /// <param name="password">Authenticate against username</param>
        /// <param name="Category">Type/privliges of user</param>
        public bool register(String Username, String Password, String Category)
        {
            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO users (username, passwordhash, category) VALUES (@username, @password, @category)", mySqlConnection);
            string myPassword = this.Password.Text;

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(myPassword);
            bool validPassword = BCrypt.Net.BCrypt.Verify(myPassword, hashedPassword); //hashes password with bcrypt

            cmd.Parameters.AddWithValue("@username", Username);
            cmd.Parameters.AddWithValue("@password", hashedPassword);
            cmd.Parameters.AddWithValue("@category", Category);
            mySqlConnection.Open();
            try
            {
                int i = cmd.ExecuteNonQuery();

                if (i != 0)
                {
                    MessageBox.Show("New User Added");
                    this.Close();
                    return true;
                }
            }
            catch
            {
                MessageBox.Show("Username exists, please choose another");
                return false;
            }
            
            mySqlConnection.Close();
            return false;
        }

    }
}