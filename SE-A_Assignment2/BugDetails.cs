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
using ScintillaNET.Demo.Utils;
using LibGit2Sharp;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace SE_A_Assignment2
{
    /// <summary>  
    ///  This class displays all bug/ticket data in one window, allows updating the ticket and displays audit information of said ticket
    ///  On closing will return to main app and refresh ticket data on doing so.
    ///  Links to VersionCompare.cs with compare version button
    /// </summary>  
    public partial class BugDetails : Form
    {
        ScintillaNET.Scintilla TextArea;
        SqlConnection mySqlConnection;
        public String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BugTrackerDB"].ConnectionString;
        private string _repoSource;
        private UsernamePasswordCredentials _credentials;
        private DirectoryInfo _localFolder;
        private string username;
        private string password;
        private string sourceurl;
        private string VersionCheck;
        private string OriginalCode;
        private string codeid;
        public bool checker;

        public BugDetails()
        {
            InitializeComponent();
            this.Text = "Bug Tracker - Ticket Details";
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            TextArea = new ScintillaNET.Scintilla();
            CodeBox.Controls.Add(TextArea);
            //TextArea.Text = contents;

            // BASIC CONFIG
            TextArea.Dock = System.Windows.Forms.DockStyle.Fill;
            //TextArea.TextChanged += (this.OnTextChanged);

            // INITIAL VIEW CONFIG
            TextArea.WrapMode = WrapMode.None;
            TextArea.IndentationGuides = IndentView.LookBoth;

            // STYLING
            InitColors();
            InitSyntaxColoring();
            InitNumberMargin();

            // INIT HOTKEYS
            InitHotkeys();

            // Custom formats for date picker
            this.DeadlineDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DeadlineDate.CustomFormat = " dd/MM/yyyy ";


        }

        public void GridViewData()
        {
            DataSet ds = new DataSet();
            SqlDataAdapter daAudit = new SqlDataAdapter();
            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand selAudit = new SqlCommand("SELECT * FROM auditlog WHERE FK_BUGID = @BUGID", mySqlConnection);
            selAudit.Parameters.AddWithValue("@BUGID", BugIDTest.Text);
            daAudit.SelectCommand = selAudit;
            daAudit.Fill(ds, "auditlog");

            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables["auditlog"];
            dataGridView1.DataSource = ds.Tables["auditlog"];


            SqlDataAdapter daUsers = new SqlDataAdapter();
            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand selUsers = new SqlCommand("SELECT username FROM USERS WHERE category!=@category ", mySqlConnection);
            selUsers.Parameters.AddWithValue("@category", "Tester");
            daUsers.SelectCommand = selUsers;
            daUsers.Fill(ds, "users");

            BindingSource bs2 = new BindingSource();
            bs2.DataSource = ds.Tables["users"];
            BugAssigned.DataSource = ds.Tables["users"];
            BugAssigned.DisplayMember = "username"; // This is text displayed
            BugAssigned.ValueMember = "username"; // This is the value returned
        }

        private void BugDetails_Load(object sender, EventArgs e)
        {
            GridViewData(); //runs grid view to get audit data for ticket
            LoadTicketData(); //loads ticket data
            LoadCodeData(); //loads code data relating to this ticket
            //BugSeverity.SelectedIndex = 1;
            //Status.SelectedIndex = 0;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.RowHeadersVisible = false;
            //grid visual features
        }

        private void LoadTicketData() //load ticket info to populate text boxes
        {
            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT * FROM tickets WHERE [ID] = @BUGID", mySqlConnection); 

            cmd.Parameters.AddWithValue("@BUGID", _theValue);

            mySqlConnection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    BugCreatedBy.Text = reader["user"].ToString();
                    BugDesc.Text = reader["description"].ToString();
                    DeadlineDate.Value = Convert.ToDateTime(reader["deadline"]);
                    DateCreated.Text = reader["datelogged"].ToString();
                    Status.Text = reader["status"].ToString();
                    BugProject.Text = reader["project"].ToString();
                    BugAssigned.Text = reader["assigned"].ToString();
                    BugSeverity.Text = reader["severity"].ToString();
                }

            }
            mySqlConnection.Close();
        }

        private void LoadCodeData() //loads code data to populate fields
        {
            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP(1) * FROM code_data WHERE [FK_TICKET_ID] = @BUGID ORDER BY ID DESC", mySqlConnection); // loads newest code if multiple exist

            cmd.Parameters.AddWithValue("@BUGID", _theValue);


            mySqlConnection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read()) //populate text boxes
                {
                    TextArea.Text = reader["code"].ToString();
                    BugVersion.Text = reader["version"].ToString();
                    BugMethods.Text = reader["methods"].ToString();
                    BugClass.Text = reader["class"].ToString();
                    BugSource.Text = reader["URL"].ToString();
                    BugLines.Text = reader["Lines"].ToString();
                    CodeAuthor.Text = reader["Author"].ToString();
                    codeid = reader["Id"].ToString();
                }

            }
            mySqlConnection.Close();

            VersionCheck = BugVersion.Text;
            OriginalCode = TextArea.Text;
        }


        private string _theValue; // Ticket ID value from main app that will be used in this class
        public string TheValue
        {
            get
            {
                return _theValue; // Ticket ID value from main app 
            }
            set
            {
                _theValue = value;
                BugIDTest.Text = value; // Assign BUG ID to textbox

            }
        }

       

        private void UpdateButton_Click(object sender, EventArgs e)
        {

            if (TextArea.Text != OriginalCode && VersionCheck == BugVersion.Text)
            {
                MessageBox.Show("Code has changed but the version recorded is the same");
                return;
            }
            // if code has changed but version hasn't, inform user


            if (TextArea.Text == OriginalCode && VersionCheck != BugVersion.Text)
            {
                MessageBox.Show("Version has changed but the code is the same");
                return;
            }
            // if code has changed but version hasn't, inform user

            UpdateCodeData();
            UpdateTicketData();
           
        }

        public void UpdateCodeData()
        {

            mySqlConnection = new SqlConnection(connectionString);
            // SqlCommand cmd = new SqlCommand("INSERT INTO tickets (user, description, reproductionsteps, project, status, severity, datelogged, deadline) VALUES (@username, @description, @reproductionsteps, @project, @status, @severity, @datelogged, @deadline)", mySqlConnection);
            //SqlCommand cmd = new SqlCommand("UPDATE code_data SET [Code] = @Code, [Version] = @Version, [Class] = @Class, [Methods] = @Methods, [Lines] = @Lines, [URL] = @Source, [Date] = @Date WHERE [FK_Ticket_ID]=@BUGID", mySqlConnection);
            SqlCommand cmd = new SqlCommand("INSERT INTO code_data (FK_Ticket_ID, [Code], [Version], [Class], [Methods], [Lines], [URL], [Author], [Date]) VALUES (@BUGID, @Code, @Version, @Class, @Methods, @Lines, @Source, @Author, @Date)", mySqlConnection);


            if (TextArea.Text == OriginalCode && VersionCheck == BugVersion.Text)
            {
                cmd = new SqlCommand("UPDATE code_data SET [Class] = @Class, [Methods] = @Methods, [Lines] = @Lines, [URL] = @Source, [Author] = @Author WHERE [ID]=@CODEID", mySqlConnection);
                // if code & version is the same, an update is fine not another row
            }

            // UPDATE DATA IN CODE TABLE
            cmd.Parameters.AddWithValue("@Code", TextArea.Text);
            cmd.Parameters.AddWithValue("@Version", BugVersion.Text);
            cmd.Parameters.AddWithValue("@Methods", BugMethods.Text);
            cmd.Parameters.AddWithValue("@Class", BugClass.Text);
            cmd.Parameters.AddWithValue("@Lines", BugLines.Text);
            cmd.Parameters.AddWithValue("@Source", BugSource.Text);
            cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@BUGID", BugIDTest.Text);
            cmd.Parameters.AddWithValue("@CODEID", codeid);
            cmd.Parameters.AddWithValue("@Author", CodeAuthor.Text);


            mySqlConnection.Open();
            try
            {
                int i = cmd.ExecuteNonQuery();

                if (i != 0)
                {
                    //MessageBox.Show("Bug has been updated");
                    //this.Close();
                }
            }
            catch (SqlException f)
            {
                MessageBox.Show(f.Message);
            }

            mySqlConnection.Close();
        }

        public void UpdateTicketData()
        {
            // UPDATE DATA IN TICKETS TABLE

            mySqlConnection = new SqlConnection(connectionString);
            // cmd = new SqlCommand("INSERT INTO tickets ([user], description, project, status, severity, datelogged, deadline) VALUES (@username, @description, @reproductionsteps, @project, @status, @severity, @datelogged, @deadline)", mySqlConnection);
            SqlCommand cmd = new SqlCommand("UPDATE tickets SET [description] = @description, [project] = @project, [status] = @status, [assigned] = @assigned, [severity] = @severity, [deadline] = @deadline WHERE [ID]=@BUGID", mySqlConnection);


            cmd.Parameters.AddWithValue("@description", BugDesc.Text);
            cmd.Parameters.AddWithValue("@project", BugProject.Text);
            cmd.Parameters.AddWithValue("@status", Status.Text);
            cmd.Parameters.AddWithValue("@severity", BugSeverity.Text);
            //cmd.Parameters.AddWithValue("@deadline", DeadlineDate.Text);
            cmd.Parameters.AddWithValue("@deadline", DeadlineDate.Value.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@assigned", BugAssigned.Text);
            cmd.Parameters.AddWithValue("@BUGID", BugIDTest.Text);


            mySqlConnection.Open();
            try
            {
                int i = cmd.ExecuteNonQuery();

                if (i != 0)
                {
                    MessageBox.Show("Ticket info has been updated");
                    // mainapp.GridViewData();
                    this.Close(); //on close gridviewdata is ran to update the grid
                }
            }
            catch (SqlException f)
            {
                MessageBox.Show(f.Message);
            }

            mySqlConnection.Close();
        }

        private void Compare_Click(object sender, EventArgs e)
        {
            VersionCompare VersionCompare = new VersionCompare();
            VersionCompare.TheValue = BugIDTest.Text; // uses bug id in code details class
            VersionCompare.Show();
        }


        #region CommitFromApp

        private void Commit_Click(object sender, EventArgs e)
        {
            username = "";
            // auth code in username
            // need to be removed whenever committing to github
            password = string.Empty;
            String gitRepoUrl = "https://github.com/SohaibHN/Test.git";
            String localFolder = "C:\\Users\\Admin\\source\\repos\\Test";
            string path = Directory.GetCurrentDirectory();
            localFolder = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\Test"));

            if (!string.IsNullOrWhiteSpace(TextArea.Text))
            {
                var folder = new DirectoryInfo(localFolder);
                GitDeploy2(username, password, gitRepoUrl, localFolder);
                CommitAllChanges();
                if (checker)
                {
                    PushCommits("origin", "master"); //specifies commit info
                }
            }
        }

        public void GitDeploy2(string username, string password, string gitRepoUrl, string localFolder)
        {
            var folder = new DirectoryInfo(localFolder);
            gitRepoUrl = "https://github.com/SohaibHN/Test.git";

            if (!folder.Exists)
            {
                throw new Exception(string.Format("Source folder '{0}' does not exist.", _localFolder));
            }

            _localFolder = folder;


            _credentials = new UsernamePasswordCredentials
            {
                Username = username,
                Password = password
            };

            _repoSource = gitRepoUrl;
        }

        /// <summary>
        /// Commits all changes.
        /// </summary>
        /// <param name="message">Commit message.</param>
        /// <exception cref="System.Exception"></exception>
        /// 
        public void CommitAllChanges()
        {
            checker = true;
            using (var repo = new Repository(_localFolder.FullName))
            {
                Uri uri;
                if (Uri.TryCreate(BugSource.Text, UriKind.RelativeOrAbsolute, out uri)) { MessageBox.Show(uri.ToString()); }
                else
                {
                    MessageBox.Show("Not a Valid URL");
                    checker = false;
                    return;

                }

                //String filename = "123.txt"; // used during unit tests
                //TextArea.Text = "1231"; // used during unit tests
                string filename = System.IO.Path.GetFileName(uri.LocalPath); // gets file name from URL source in ticket info
                File.WriteAllText(Path.Combine(repo.Info.WorkingDirectory, filename), TextArea.Text);
                // Write content to file system


                // Stage the file
                repo.Stage(filename);
               // repo.LibGit2Sharp.Commands.Stage(filename);

                string CommitMessage = Interaction.InputBox("Enter Commit Message", "Enter Commit Message", "Commit Message Here", 50, 50);
                // visual basic commit message box popping up for user.

                if (CommitMessage.Length == 0) { MessageBox.Show("Commit Cancelled"); checker = false;  return; }

                // Create the committer's signature and commit
                Signature author = new Signature("Sohaib", "@SohaibHN", DateTime.Now); //default user with commit access is me
                Signature committer = author;

                // Commit to the repository
                try
                {
                    Commit commit = repo.Commit(CommitMessage, author, committer);
                    checker = true;
                }
                catch (Exception c)
                {
                    MessageBox.Show(c.Message);
                    checker = false;
                }

            }
        }
        /// <summary>
        /// Pushes all commits.
        /// </summary>
        /// <param name="remoteName">Name of the remote server.</param>
        /// <param name="branchName">Name of the remote branch.</param>
        /// <exception cref="System.Exception"></exception>
        public bool PushCommits(string remoteName, string branchName)
        {
            using (var repo = new Repository(_localFolder.FullName))
            {
                var remote = repo.Network.Remotes.FirstOrDefault(r => r.Name == remoteName);
                if (remote == null)
                {
                    repo.Network.Remotes.Add(remoteName, _repoSource);
                    remote = repo.Network.Remotes.FirstOrDefault(r => r.Name == remoteName);
                }

                var options = new PushOptions
                {
                    CredentialsProvider = (url, usernameFromUrl, types) => _credentials
                };

                PushOptions po = new PushOptions();

                po.CredentialsProvider = (_url, usernameFromUrl, _cred) => _credentials;

                try 
                {
                    repo.Network.Push(remote, @"refs/heads/master", options);
                    MessageBox.Show("Commit Succesfully Pushed"); 
                    return true;
                }
                catch (Exception p)
                {
                    MessageBox.Show(p.Message);
                    return false;
                }
            }
        }
        #endregion

        #region code box

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
    }
}
