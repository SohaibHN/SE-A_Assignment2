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
using ScintillaNET;
using System.IO;
using System.Configuration;
using ScintillaNET.Demo.Utils;

namespace SE_A_Assignment2
{
    /// <summary>  
    ///  This class is the main page for the application, uses tab control instead of multiple different windows forms linking to whatever
    ///  Allows new bug to be submitted
    ///  Admins to manage user details
    ///  Changing password
    ///  Links to bugdetails/codedetails.cs to update ticket info
    /// </summary>  
    public partial class MainApp : Form
    {
        public String MainLoggedInUser;
        public String MainLoggedInCategory;
        public String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BugTrackerDB"].ConnectionString;
        ScintillaNET.Scintilla TextArea;
        SqlConnection mySqlConnection;

        public MainApp()
        {

            TextArea = new ScintillaNET.Scintilla();

            InitializeComponent();

            this.Text = "Bug Tracker - Home";
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            tabPage1.Text = @"View Bugs";
            tabPage2.Text = @"Add New Bug";
            tabPage4.Text = @"Change Password";
            tabPage5.Text = @"Admin";
            this.tabControl1.SelectedTab = tabPage1;

            MainLoggedInUser = loginform.LoggedInUser;
            MainLoggedInCategory = loginform.LoggedInCategory;
            
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged; // used whenever a tab is changed

        }

        private void tabControl1_SelectedIndexChanged(Object sender, EventArgs e) // used whenever a tab is changed
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage2"])//insert ticket tab
            {
                BugDataInital();
            }

            if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage5"])//admin tab
            {
                GridViewDataAdmin(); //populate admin grid based on user table
                dataGridViewAdmin.CurrentCell.Selected = false;
                dataGridViewAdmin.RowHeadersVisible = false;
                //grid visual features
            }

        }

        public int GridViewDataAdmin()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter daUsers = new SqlDataAdapter();
            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand selUsers = new SqlCommand("SELECT ID, Username, Category FROM USERS", mySqlConnection);
            daUsers.SelectCommand = selUsers;
            daUsers.Fill(ds, "users");

            BindingSource bsusers = new BindingSource(); 
            bsusers.DataSource = ds.Tables["users"];
            dataGridViewAdmin.DataSource = ds.Tables["users"]; //binds users dataset to admin grid datasource

            int count = ds.Tables["users"].Rows.Count;
            return count;

        }

        private void BugDataInital()
        {
            int maxId;
            mySqlConnection = new SqlConnection(connectionString);
            using (SqlCommand dataCommand =
                    new SqlCommand("Select IDENT_CURRENT('tickets')", mySqlConnection))
            {
              mySqlConnection.Open();
              maxId = 1 + Convert.ToInt32(dataCommand.ExecuteScalar()); //displays "newest" ticket ID value for visual purpose only

            }

            mySqlConnection.Close();
            BugID.Text = maxId.ToString(); //displays "newest" ticket ID value for user
            BugDeadlineDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"); //todays date + 1 = default deadline
            Severity.SelectedIndex = 1; //defaults to Medium

            // Custom formats for date picker
            this.BugDeadlineDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.BugDeadlineDate.CustomFormat = " dd/MM/yyyy ";
            this.BugDeadlineDate.Value = DateTime.Now;

            // Sets default for drop down to unassigned instead of first user in table
            BugAssigned.SelectedItem = null;
            BugAssigned.Text = "";
            BugAssigned.SelectedText = "Unassigned"; //defaults to unassigned


            // BASIC CONFIG FOR CODE BOX
            TextArea.Dock = System.Windows.Forms.DockStyle.Fill;
            CodeBox.Controls.Add(TextArea);

            // INITIAL VIEW CONFIG FOR CODE BOX
            TextArea.WrapMode = WrapMode.None;
            TextArea.IndentationGuides = IndentView.LookBoth;

            // STYLING FOR CODE BOX
            InitColors();
            InitSyntaxColoring();
            InitNumberMargin();

            // INIT HOTKEYS FOR CODE BOX
            InitHotkeys();


        }

        private void MainApp_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'bugTrackerDataSetMain.tickets' table. You can move, or remove it, as needed.
            //this.ticketsTableAdapter2.Fill(this.bugTrackerDataSetMain.tickets);
            // 
            if (MainLoggedInCategory != "Admin") { tabControl1.TabPages.Remove(tabPage5); } //removes admin tab if not admin
            tabControl1.TabPages.Remove(tabPage3);
            GridViewData(); //loads ticket data into grid
            dataGridView1.Columns[3].Visible = false; //hides column because its too long and displayed in text box instead
            dataGridView1.RowHeadersVisible = false; //removes top area
            dataGridView1.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView1_RowPrePaint); //painting method to colour rows

            AssignedFilter.Text = "All Tickets"; //defaults filter to all tickets
            dataGridView1.CurrentCell.Selected = false;
            ClearTextBoxes(); //clears text boxes

        }

        public int GridViewData()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter daTickets = new SqlDataAdapter();
            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand selTickets = new SqlCommand("SELECT * FROM tickets", mySqlConnection);
            daTickets.SelectCommand = selTickets;
            daTickets.Fill(ds, "tickets");

            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables["tickets"];
            dataGridView1.DataSource = ds.Tables["tickets"]; //binds user dataset into grids datasource

            SqlDataAdapter daUsers = new SqlDataAdapter();
            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand selUsers = new SqlCommand("SELECT username FROM USERS WHERE category!=@category ", mySqlConnection);
            selUsers.Parameters.AddWithValue("@category", "Tester");
            daUsers.SelectCommand = selUsers;
            daUsers.Fill(ds, "users");

            BindingSource bs2 = new BindingSource();
            bs2.DataSource = ds.Tables["users"];
            BugAssigned.DataSource = ds.Tables["users"]; //binds user dataset to combo box datasource
            BugAssigned.DisplayMember = "username"; // This is text displayed
            BugAssigned.ValueMember = "username"; // This is the value returned

            int count = ds.Tables["tickets"].Rows.Count;
            return count; //used for unit tests to see if data is pulled

        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {


            if (e.RowIndex < 0 )
                return;
            //Compare value and change color
            DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[5];
            DateTime date = DateTime.Now.Date;
            DateTime deadline = (DateTime)dataGridView1.Rows[e.RowIndex].Cells[9].Value;
            String value = cell.Value == null ? string.Empty : cell.Value.ToString();
            if (deadline < date && value.Equals("Fixed") != true && value.Equals("On Hold") != true)
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Tomato; //expired date
            }
            else if (value.Equals("Broken") == true)
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Orange; //broken
            }
            else if (value.Equals("Fixed") == true)
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Green; //fixed
            }
            else
            {
                dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue; // on hold
            }
        }

        private void ViewBugs_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedCells.Count > 0)
            {
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
                    if (reader.HasRows) //if there is any rows in code data relating to ticket, full bug detail window opens.
                    {
                        BugDetails BugDetails = new BugDetails();
                        BugDetails.TheValue = str; //uses bug id in bug details class
                        BugDetails.FormClosed += BugDetails_FormClosed;
                        BugDetails.Show();
                    }
                    else //user is required to enter code details to view page
                    {
                        DialogResult dialogResult = MessageBox.Show("There is no extra details, would you like to add some?", "Code Details", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            //do something
                            CodeDetails CodeDetails = new CodeDetails();
                            CodeDetails.TheValue = str; // uses bug id in code details class
                            CodeDetails.Show();
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            return;
                        }
                    }
                }
            }
        }

        void BugDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            GridViewData();
            // on closing of bug details grid data refreshes to reflect any updates
        }

        private void ManageUsers_Click(object sender, EventArgs e)
        {

        }

        private void Logout_Click(object sender, EventArgs e) //logout
        {
            Application.Restart();
            this.Visible = false;
            Environment.Exit(0);
        }

        private void ChangePass_Click(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(connectionString); //sql connection
            SqlCommand cmd = new SqlCommand("select username, passwordhash, category from users where username=@username", mySqlConnection);
            cmd.Parameters.AddWithValue("@username", MainLoggedInUser); // adds username from the login variable set previously
            mySqlConnection.Open();

            if (string.IsNullOrWhiteSpace(NewPass1.Text) || string.IsNullOrEmpty(NewPass2.Text)) // values required
            {
                MessageBox.Show("New Details can't be empty");
            }
            else
            {

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        if (BCrypt.Net.BCrypt.Verify(CurrentPass.Text, reader.GetString(1))) //verify password hash with bycrypt
                        {
                            // if user has authenticated, update password method is ran
                            UpdatePassword(MainLoggedInUser, NewPass1.Text, NewPass2.Text);
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Password");
                        }

                    }
                    mySqlConnection.Close();

                }
            }
        }

        // Multiple parameters.
        /// <param name="username">Used to indicate username to change password against</param>
        /// <param name="password">Authenticate against username later on</param>
        /// <param name="password2">Checks against password one if its the same</param>
        public bool UpdatePassword(String username, String password1, String password2)
        {
            if (password1 == password2) //checks same password entered
            {

                mySqlConnection = new SqlConnection(connectionString); //sql connection
                SqlCommand cmd2 = new SqlCommand("UPDATE users SET passwordhash =@password WHERE username = @usernamesearch", mySqlConnection);
                string myPassword = password1;


                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(myPassword);
                bool validPassword = BCrypt.Net.BCrypt.Verify(myPassword, hashedPassword); // sets password to hash with bcrypt

                cmd2.Parameters.AddWithValue("@usernamesearch", username);
                cmd2.Parameters.AddWithValue("@password", hashedPassword);
                mySqlConnection.Open();
                try
                {
                    int i = cmd2.ExecuteNonQuery();

                    if (i != 0)
                    {
                        MessageBox.Show("Password Updated");
                        GridViewDataAdmin(); 
                        ClearTextBoxes();
                        return true;

                    }
                }
                catch (SqlException f)
                {
                    MessageBox.Show(f.Message);
                }

                mySqlConnection.Close();

            }
            else
            {
                MessageBox.Show("New Passwords do not Match");
                
            }
            return false;

        }

        public int InsertTicketData()
        {
            mySqlConnection = new SqlConnection(connectionString); //sql connection
            // SqlCommand cmd = new SqlCommand("INSERT INTO tickets (user, description, reproductionsteps, project, status, severity, datelogged, deadline) VALUES (@username, @description, @reproductionsteps, @project, @status, @severity, @datelogged, @deadline)", mySqlConnection);
            SqlCommand cmd = new SqlCommand("INSERT INTO tickets ([user], description, reproductionsteps, project, status, severity, datelogged, deadline, assigned) OUTPUT INSERTED.ID VALUES (@username, @description, @reproductionsteps, @project, @status, @severity, @datelogged, @deadline, @assigned)", mySqlConnection);
            //String Severity;
            int newId = 0;
            cmd.Parameters.AddWithValue("@username", MainLoggedInUser);
            cmd.Parameters.AddWithValue("@description", BugDesc.Text);
            cmd.Parameters.AddWithValue("@reproductionsteps", BugSteps.Text);
            cmd.Parameters.AddWithValue("@project", BugProject.Text);
            cmd.Parameters.AddWithValue("@status", "Broken");
            cmd.Parameters.AddWithValue("@severity", Severity.Text);
            cmd.Parameters.AddWithValue("@datelogged", DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@deadline", BugDeadlineDate.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@assigned", BugAssigned.Text);

            mySqlConnection.Open();
            try
            {
                //int i = cmd.ExecuteNonQuery();
                newId = (Int32)cmd.ExecuteScalar(); // GETS BUGID THAT IS JUST INSERTED USED FOR INSERTING INTO CODE TABLE
                if (newId != 0)
                {
                    MessageBox.Show("New Bug reported");

                }
            }
            catch (SqlException f)
            {
                MessageBox.Show(f.Message);
            }

            mySqlConnection.Close();
            return newId; //returns bug ID to insert code into the code table if needed
        }

        // Single parameter.
        /// <param name="TicketID">takes ID from previous method to use as foreign key in code table.</param>
        private void InsertCodeData(int TicketID) //takes ID from previous method to use as foreign key
        {
            mySqlConnection = new SqlConnection(connectionString); //sql connection
            SqlCommand cmd = new SqlCommand("INSERT INTO code_data (FK_Ticket_ID, [Code], [Version], [Class], [Methods], [Lines], [URL], [Author], [Date]) VALUES (@BUGID, @Code, @Version, @Class, @Methods, @Lines, @Source, @Author, @Date)", mySqlConnection);

            cmd.Parameters.AddWithValue("@Code", TextArea.Text);
            cmd.Parameters.AddWithValue("@Version", BugVersion.Text);
            cmd.Parameters.AddWithValue("@Methods", BugMethods.Text);
            cmd.Parameters.AddWithValue("@Class", BugClass.Text);
            cmd.Parameters.AddWithValue("@Lines", BugLines.Text);
            cmd.Parameters.AddWithValue("@Source", URL.Text);
            cmd.Parameters.AddWithValue("@Author", BugAuthor.Text);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@BUGID", TicketID);

            mySqlConnection.Open();
            try
            {
                int i = cmd.ExecuteNonQuery();

                if (i != 0)
                {
                    //this.Close();
                }
            }
            catch (SqlException f)
            {
                MessageBox.Show(f.Message);
            }

            mySqlConnection.Close();
        }

        private void SaveBug_Click(object sender, EventArgs e)
        {
            int BUGID;
            if (!string.IsNullOrWhiteSpace(BugDesc.Text) && !string.IsNullOrWhiteSpace(BugSteps.Text) && !string.IsNullOrWhiteSpace(BugProject.Text))
            {

                if (!string.IsNullOrWhiteSpace(TextArea.Text))
                {
                    // INSERT INTO CODE TABLE
                    if (!string.IsNullOrWhiteSpace(BugLines.Text) && !string.IsNullOrWhiteSpace(BugVersion.Text) && !string.IsNullOrWhiteSpace(BugMethods.Text) && !string.IsNullOrWhiteSpace(BugClass.Text) && !string.IsNullOrWhiteSpace(URL.Text))
                    {
                        BUGID = InsertTicketData();
                        InsertCodeData(BUGID);
                    }
                    else
                    {
                        MessageBox.Show("All code fields require a value");
                    }
                }
                else
                {
                    InsertTicketData();
                }
            }
            else
            {
                MessageBox.Show("Ticket data cannot be left blank");
            }
            GridViewData();
            dataGridView1.Refresh();
            BugDataInital();
        }

        private void SearchData_TextChanged(object sender, EventArgs e) // filter options for ticket data grid on typing
        {
             (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("[Description] LIKE '%{0}%' OR [Severity] LIKE '%{0}%' OR [Project] LIKE '%{0}%' OR [Assigned] LIKE '%{0}%'", SearchData.Text);
            // filters through datasource with sql like statements
        }

        private void AssignedFilter_SelectedIndexChanged(object sender, EventArgs e)// filter options for ticket data grid on drop down
        {
            String filter;
            if (AssignedFilter.Text == "Me") //custom assignments for values
            {
                filter = MainLoggedInUser;
            }
            else if (AssignedFilter.Text == "Unassigned")
            {
                filter = "Unassigned";
            }
            else
            {
                filter = "";
            }
             (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("[Assigned] LIKE '%{0}%' ", filter);
            // filters through datasource with sql like statements

        }
        /*
       private void FilterSeverity_SelectedIndexChanged(object sender, EventArgs e)
       {
          String filter;
           if (FilterSeverity.Text == "All Tickets")
           {
               filter = "";
           }
           else
           {
               filter = FilterSeverity.Text;
           }

           (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("[Severity] LIKE '%{0}%' ", filter);
       }

   */
        #region codebox
        private void InitColors()
        {
            TextArea.SetSelectionBackColor(true, IntToColor(0x114D9C));
        }

        private void InitHotkeys()
        {
            // register the hotkeys with the form
            HotKeyManager.AddHotKey(this, OpenSearch, Keys.F, true);
            HotKeyManager.AddHotKey(this, OpenFindDialog, Keys.F, true, false, true);
            HotKeyManager.AddHotKey(this, OpenReplaceDialog, Keys.R, true);
            HotKeyManager.AddHotKey(this, OpenReplaceDialog, Keys.H, true);
            HotKeyManager.AddHotKey(this, Uppercase, Keys.U, true);
            HotKeyManager.AddHotKey(this, Lowercase, Keys.L, true);
            HotKeyManager.AddHotKey(this, ZoomIn, Keys.Oemplus, true);
            HotKeyManager.AddHotKey(this, ZoomOut, Keys.OemMinus, true);
            HotKeyManager.AddHotKey(this, ZoomDefault, Keys.D0, true);
            HotKeyManager.AddHotKey(this, CloseSearch, Keys.Escape);

            // remove conflicting hotkeys from scintilla
            TextArea.ClearCmdKey(Keys.Control | Keys.F);
            TextArea.ClearCmdKey(Keys.Control | Keys.R);
            TextArea.ClearCmdKey(Keys.Control | Keys.H);
            TextArea.ClearCmdKey(Keys.Control | Keys.L);
            TextArea.ClearCmdKey(Keys.Control | Keys.U);
        }

        private void InitSyntaxColoring()
        {

            // Configure the default style
            TextArea.StyleResetDefault();
            TextArea.Styles[Style.Default].Font = "Consolas";
            TextArea.Styles[Style.Default].Size = 10;
            TextArea.Styles[Style.Default].BackColor = IntToColor(0x212121);
            TextArea.Styles[Style.Default].ForeColor = IntToColor(0xFFFFFF);
            TextArea.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            TextArea.Styles[Style.Cpp.Identifier].ForeColor = IntToColor(0xD0DAE2);
            TextArea.Styles[Style.Cpp.Comment].ForeColor = IntToColor(0xBD758B);
            TextArea.Styles[Style.Cpp.CommentLine].ForeColor = IntToColor(0x40BF57);
            TextArea.Styles[Style.Cpp.CommentDoc].ForeColor = IntToColor(0x2FAE35);
            TextArea.Styles[Style.Cpp.Number].ForeColor = IntToColor(0xffa700);
            TextArea.Styles[Style.Cpp.String].ForeColor = IntToColor(0xffa700);
            TextArea.Styles[Style.Cpp.Character].ForeColor = IntToColor(0xE95454);
            TextArea.Styles[Style.Cpp.Preprocessor].ForeColor = IntToColor(0x8AAFEE);
            TextArea.Styles[Style.Cpp.Operator].ForeColor = IntToColor(0xE0E0E0);
            TextArea.Styles[Style.Cpp.Regex].ForeColor = IntToColor(0xff00ff);
            TextArea.Styles[Style.Cpp.CommentLineDoc].ForeColor = IntToColor(0x77A7DB);
            TextArea.Styles[Style.Cpp.Word].ForeColor = IntToColor(0x48A8EE);
            TextArea.Styles[Style.Cpp.Word2].ForeColor = IntToColor(0xF98906);
            TextArea.Styles[Style.Cpp.CommentDocKeyword].ForeColor = IntToColor(0xB3D991);
            TextArea.Styles[Style.Cpp.CommentDocKeywordError].ForeColor = IntToColor(0xFF0000);
            TextArea.Styles[Style.Cpp.GlobalClass].ForeColor = IntToColor(0x48A8EE);

            TextArea.Lexer = Lexer.Cpp;


            TextArea.SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while class extends implements import interface new case do while else if for in switch throw get set function var try catch finally while with default break continue delete return each const namespace package include use is as instanceof typeof author copy default deprecated eventType example exampleText exception haxe inheritDoc internal link mtasc mxmlc param private return see serial serialData serialField since throws usage version langversion playerversion productversion dynamic private public partial static intrinsic internal native override protected AS3 final super this arguments null Infinity NaN undefined true false abstract as base bool break by byte case catch char checked class const continue decimal default delegate do double descending explicit event extern else enum false finally fixed float for foreach from goto group if implicit in int interface internal into is lock long new null namespace object operator out override orderby params private protected public readonly ref return switch struct sbyte sealed short sizeof stackalloc static string select this throw true try typeof uint ulong unchecked unsafe ushort using var virtual volatile void while where yield");
            TextArea.SetKeywords(1, "bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void Null ArgumentError arguments Array Boolean Class Date DefinitionError Error EvalError Function int Math Namespace Number Object RangeError ReferenceError RegExp SecurityError String SyntaxError TypeError uint XML XMLList Boolean Byte Char DateTime Decimal Double Int16 Int32 Int64 IntPtr SByte Single UInt16 UInt32 UInt64 UIntPtr Void Path File ScintillaNET");

        }

        #region Numbers, Bookmarks, Code Folding

        /// <summary>
        /// the background color of the text area
        /// </summary>
        private const int BACK_COLOR = 0x2A211C;

        /// <summary>
        /// default text color of the text area
        /// </summary>
        private const int FORE_COLOR = 0xB7B7B7;

        /// <summary>
        /// change this to whatever margin you want the line numbers to show in
        /// </summary>
        private const int NUMBER_MARGIN = 1;

        /// <summary>
        /// change this to whatever margin you want the bookmarks/breakpoints to show in
        /// </summary>
        private const int BOOKMARK_MARGIN = 2;
        private const int BOOKMARK_MARKER = 2;

        private void InitNumberMargin()
        {

            TextArea.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
            TextArea.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
            TextArea.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

            var nums = TextArea.Margins[NUMBER_MARGIN];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;

        }

        #endregion

        #region Uppercase / Lowercase

        private void Lowercase()
        {

            // save the selection
            int start = TextArea.SelectionStart;
            int end = TextArea.SelectionEnd;

            // modify the selected text
            TextArea.ReplaceSelection(TextArea.GetTextRange(start, end - start).ToLower());

            // preserve the original selection
            TextArea.SetSelection(start, end);
        }

        private void Uppercase()
        {

            // save the selection
            int start = TextArea.SelectionStart;
            int end = TextArea.SelectionEnd;

            // modify the selected text
            TextArea.ReplaceSelection(TextArea.GetTextRange(start, end - start).ToUpper());

            // preserve the original selection
            TextArea.SetSelection(start, end);
        }

        #endregion

        #region Indent / Outdent

        private void Indent()
        {
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to indent,
            // although the indentation function exists. Pressing TAB with the editor focused confirms this.
            GenerateKeystrokes("{TAB}");
        }

        private void Outdent()
        {
            // we use this hack to send "Shift+Tab" to scintilla, since there is no known API to outdent,
            // although the indentation function exists. Pressing Shift+Tab with the editor focused confirms this.
            GenerateKeystrokes("+{TAB}");
        }

        private void GenerateKeystrokes(string keys)
        {
            HotKeyManager.Enable = false;
            TextArea.Focus();
            SendKeys.Send(keys);
            HotKeyManager.Enable = true;
        }

        #endregion

        #region Zoom

        private void ZoomIn()
        {
            TextArea.ZoomIn();
        }

        private void ZoomOut()
        {
            TextArea.ZoomOut();
        }

        private void ZoomDefault()
        {
            TextArea.Zoom = 0;
        }


        #endregion

        #region Quick Search Bar

        bool SearchIsOpen = false;

        private void OpenSearch()
        {

            SearchManager.SearchBox = TxtSearch;
            SearchManager.TextArea = TextArea;

            if (!SearchIsOpen)
            {
                SearchIsOpen = true;
                InvokeIfNeeded(delegate () {
                    PanelSearch.Visible = true;
                    TxtSearch.Text = SearchManager.LastSearch;
                    TxtSearch.Focus();
                    TxtSearch.SelectAll();
                });
            }
            else
            {
                InvokeIfNeeded(delegate () {
                    TxtSearch.Focus();
                    TxtSearch.SelectAll();
                });
            }
        }
        private void CloseSearch()
        {
            if (SearchIsOpen)
            {
                SearchIsOpen = false;
                InvokeIfNeeded(delegate () {
                    PanelSearch.Visible = false;
                    //CurBrowser.GetBrowser().StopFinding(true);
                });
            }
        }

        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            CloseSearch();
        }

        private void BtnPrevSearch_Click(object sender, EventArgs e)
        {
            SearchManager.Find(false, false);
        }
        private void BtnNextSearch_Click(object sender, EventArgs e)
        {
            SearchManager.Find(true, false);
        }
        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            SearchManager.Find(true, true);
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (HotKeyManager.IsHotkey(e, Keys.Enter))
            {
                SearchManager.Find(true, false);
            }
            if (HotKeyManager.IsHotkey(e, Keys.Enter, true) || HotKeyManager.IsHotkey(e, Keys.Enter, false, true))
            {
                SearchManager.Find(false, false);
            }
        }


        #endregion

        #region Find & Replace Dialog

        private void OpenFindDialog()
        {

        }
        private void OpenReplaceDialog()
        {


        }

        #endregion

        #region Utils

        public static Color IntToColor(int rgb)
        {
            return Color.FromArgb(255, (byte)(rgb >> 16), (byte)(rgb >> 8), (byte)rgb);
        }

        public void InvokeIfNeeded(Action action)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(action);
            }
            else
            {
                action.Invoke();
            }
        }

        #endregion

        #endregion

        private void GenerateCode_Click(object sender, EventArgs e)
        {
            string contents;
            TextArea.Clear();
            if (!string.IsNullOrWhiteSpace(URL.Text)) // checks if url is entered
            {
                using (var wc1 = new System.Net.WebClient())
                    try // checks for valid urls/404 errors etc and gives suitable error message to user
                    {
                        contents = wc1.DownloadString(URL.Text);
                        TextArea.Text = contents;
                    }
                    catch (System.Net.WebException w)
                    {
                        MessageBox.Show(w.Message);
                        TextArea.Clear();
                    }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e) //updates text boxes depending on what is selected in datagrid for tickets
        {
            mySqlConnection = new SqlConnection(connectionString);
            if (dataGridView1.SelectedCells.Count > 0)
            {

                string str = null;
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    str = row.Cells[0].Value.ToString(); // gets bug ID and uses it for sql
                }


                SqlCommand cmd = new SqlCommand("SELECT * FROM tickets WHERE [ID] = @BUGID", mySqlConnection);

                cmd.Parameters.AddWithValue("@BUGID", str);

                mySqlConnection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) //populates text boxes
                    {
                        TicketCreated.Text = reader["user"].ToString();
                        TicketDesc.Text = reader["description"].ToString();
                       // DeadlineDate.Value = Convert.ToDateTime(reader["deadline"]);
                        TicketRepo.Text = reader["ReproductionSteps"].ToString();
                       // Status.Text = reader["status"].ToString();
                        TicketProject.Text = reader["project"].ToString();
                        TicketAssigned.Text = reader["assigned"].ToString();
                        TicketSeverity.Text = reader["severity"].ToString();

                    }

                }
                mySqlConnection.Close();
            }
            
        }

        private void ClearTextBoxes() //code function to clear all text boxes in form
        {
            Action<Control.ControlCollection> func = null;

            func = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                        (control as TextBox).Clear();
                    else
                        func(control.Controls);
            };

            func(Controls);
        }

        private void ClearAllButton_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            BugDataInital();
        }

        private void UpdateUser_Click(object sender, EventArgs e)
        {

            mySqlConnection = new SqlConnection(connectionString); //sql connection

            if (string.IsNullOrWhiteSpace(ManageCategory.Text)) // values required
            {
                MessageBox.Show("Fields cannot be left blank");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(ManagePassword1.Text) || string.IsNullOrEmpty(ManagePassword2.Text)) // only updates category if no passwords entered
                {
                    mySqlConnection = new SqlConnection(connectionString); //sql connection
                    SqlCommand cmd = new SqlCommand("UPDATE users SET category =@category WHERE username = @usernamesearch", mySqlConnection);

                    cmd.Parameters.AddWithValue("@category", ManageCategory.Text);
                    cmd.Parameters.AddWithValue("@usernamesearch", ManageUsername.Text);

                    mySqlConnection.Open();
                    try
                    {
                        int i = cmd.ExecuteNonQuery();

                        if (i != 0)
                        {
                            MessageBox.Show("Category Updated");
                            ClearTextBoxes();
                            GridViewDataAdmin();
                        }
                    }
                    catch (SqlException f)
                    {
                        MessageBox.Show(f.Message);
                    }

                    mySqlConnection.Close();

                }
                else
                {

                    UpdatePassword(ManageUsername.Text, ManagePassword1.Text, ManagePassword2.Text); //password update method is ran with username and two new passwords
                }

            }

        }

        private void dataGridViewAdmin_SelectionChanged(object sender, EventArgs e) //populates text boxes dependign on what row is selected in admin grid
        {
            mySqlConnection = new SqlConnection(connectionString);
            if (dataGridViewAdmin.SelectedCells.Count > 0)
            {

                string str = null;
                foreach (DataGridViewRow row in dataGridViewAdmin.SelectedRows)
                {
                    str = row.Cells[0].Value.ToString(); // gets bug ID and uses it for the bug details form
                }


                SqlCommand cmd = new SqlCommand("SELECT * FROM USERS WHERE [ID] = @ID", mySqlConnection);

                cmd.Parameters.AddWithValue("@ID", str);

                mySqlConnection.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ManageUsername.Text = reader["username"].ToString();
                        ManageCategory.Text = reader["category"].ToString();
                    }

                }
                mySqlConnection.Close();
            }
        }
    }
}
