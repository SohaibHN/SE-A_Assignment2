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

namespace SE_A_Assignment2
{

    /// <summary>  
    ///  This class allows two rich text boxes to compare, uses google-diff-match-patchs https://code.google.com/archive/p/google-diff-match-patch/
    ///  uses version stored in code table to compare different versions of code, differences highlighted in green
    /// </summary>  
    public partial class VersionCompare : Form
    {
        ScintillaNET.Scintilla TextArea;
        ScintillaNET.Scintilla TextArea2;
        SqlConnection mySqlConnection;
        public String connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BugTrackerDB"].ConnectionString;
        public string bugid;
        public VersionCompare()
        {
            InitializeComponent();
            this.Text = "Bug Tracker - Compare Code";
            this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);

            TextArea = new ScintillaNET.Scintilla(); //CODEBOX 
            //RTB3.Controls.Add(TextArea);
            //TextArea.Text = contents;

            // BASIC CONFIG FOR CODEBOX
            TextArea.Dock = System.Windows.Forms.DockStyle.Fill;
            //TextArea.TextChanged += (this.OnTextChanged);

            // INITIAL VIEW CONFIG FOR CODEBOX
            TextArea.WrapMode = WrapMode.None;
            TextArea.IndentationGuides = IndentView.LookBoth;

            TextArea2 = new ScintillaNET.Scintilla(); //CODEBOX 
            RTB1.Controls.Add(TextArea2);
            //TextArea.Text = contents;

            // BASIC CONFIG FOR CODEBOX
            TextArea2.Dock = System.Windows.Forms.DockStyle.Fill;
            //TextArea.TextChanged += (this.OnTextChanged);

            // INITIAL VIEW CONFIG FOR CODEBOX
            TextArea2.WrapMode = WrapMode.None;
            TextArea2.IndentationGuides = IndentView.LookBoth;

            // STYLING FOR CODEBOX
            InitColors();
            InitSyntaxColoring();
            InitNumberMargin();

            // INIT HOTKEYS FOR CODEBOX
            InitHotkeys();

           // ComboBoxData();
        }

        // this is the diff object;
        diff_match_patch DIFF = new diff_match_patch();

        // these are the diffs
        List<Diff> diffs;

        // chunks for formatting the two RTBs:
        List<Chunk> chunklist1;
        List<Chunk> chunklist2;

        // two color lists:
        Color[] colors1 = new Color[3] { Color.LightGreen, Color.LightSalmon, Color.White };
        Color[] colors2 = new Color[3] { Color.LightSalmon, Color.LightGreen, Color.White };


        public struct Chunk
        {
            public int startpos;
            public int length;
            public Color BackColor;
        }

        List<Chunk> collectChunks(RichTextBox RTB)
        {
            RTB.Text = "";
            List<Chunk> chunkList = new List<Chunk>();
            foreach (Diff d in diffs)
            {
                if (RTB == RTB2 && d.operation == Operation.DELETE) continue;  // **
                if (RTB == RTB3 && d.operation == Operation.INSERT) continue;  // **

                Chunk ch = new Chunk();
                int length = RTB.TextLength;
                RTB.AppendText(d.text);
                ch.startpos = length;
                ch.length = d.text.Length;
                ch.BackColor = RTB == RTB3 ? colors1[(int)d.operation]
                                           : colors2[(int)d.operation];
                chunkList.Add(ch);
            }
            return chunkList;

        }

        void paintChunks(RichTextBox RTB, List<Chunk> theChunks)
        {
            foreach (Chunk ch in theChunks)
            {
                RTB.Select(ch.startpos, ch.length);
                RTB.SelectionBackColor = ch.BackColor;
            }

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void paintboxes()
        {
            diffs = DIFF.diff_main(RTB3.Text, RTB2.Text);
            DIFF.diff_cleanupSemanticLossless(diffs);    

            chunklist1 = collectChunks(RTB3);
            chunklist2 = collectChunks(RTB2);

            paintChunks(RTB3, chunklist1);
            paintChunks(RTB2, chunklist2);

            RTB3.SelectionLength = 0;
            RTB2.SelectionLength = 0;
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
                bugid = value;
            }
        }

        public void ComboBoxData()
        {
            DataSet ds = new DataSet();
            mySqlConnection = new SqlConnection(connectionString);
            SqlDataAdapter daVersion = new SqlDataAdapter();
            SqlCommand selversion = new SqlCommand("SELECT version FROM code_data WHERE FK_TICKET_ID=@ID ", mySqlConnection);
            selversion.Parameters.AddWithValue("@ID", bugid);
            daVersion.SelectCommand = selversion;
            daVersion.Fill(ds, "code_data");

            BindingSource bs = new BindingSource();
            bs.DataSource = ds.Tables["code_data"];
            Version1.DataSource = ds.Tables["code_data"]; //binds version dataset to combo box datasource
            Version1.DisplayMember = "version"; // This is text displayed
            Version1.ValueMember = "version"; // This is the value returned

            Version2.BindingContext = new BindingContext();
            Version2.DataSource = ds.Tables["code_data"]; //binds version dataset to combo box datasource
            Version2.DisplayMember = "version"; // This is text displayed
            Version2.ValueMember = "version"; // This is the value returned

        }

        private void VersionCompare_Load(object sender, EventArgs e)
        {
            ComboBoxData();
        }

        private void Version1_SelectedIndexChanged(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP(1) CODE FROM code_data WHERE [FK_TICKET_ID] = @BUGID AND [VERSION] = @Version ORDER BY ID DESC", mySqlConnection); // loads newest code if multiple exist

            cmd.Parameters.AddWithValue("@BUGID", _theValue);
            cmd.Parameters.AddWithValue("@Version", Version1.Text);


            mySqlConnection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read()) //populate text boxes
                {
                    TextArea.Text = reader["code"].ToString();
                    RTB3.Text = reader["code"].ToString();
                }

            }
            mySqlConnection.Close();
            paintboxes();

        }

        private void Version2_SelectedIndexChanged(object sender, EventArgs e)
        {
            mySqlConnection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT TOP(1) CODE FROM code_data WHERE [FK_TICKET_ID] = @BUGID AND [VERSION] = @Version ORDER BY ID DESC", mySqlConnection); // loads newest code if multiple exist

            cmd.Parameters.AddWithValue("@BUGID", _theValue);
            cmd.Parameters.AddWithValue("@Version", Version2.Text);


            mySqlConnection.Open();

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read()) //populate text boxes
                {
                    TextArea2.Text = reader["code"].ToString();
                    RTB2.Text = reader["code"].ToString();
                }

            }
            mySqlConnection.Close();
            paintboxes();
        }

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

            TextArea2.Styles[Style.Default].Font = "Consolas";


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
            TextArea2.Lexer = Lexer.Cpp;


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

            TextArea2.Styles[Style.LineNumber].BackColor = IntToColor(BACK_COLOR);
            TextArea2.Styles[Style.LineNumber].ForeColor = IntToColor(FORE_COLOR);
            TextArea2.Styles[Style.IndentGuide].ForeColor = IntToColor(FORE_COLOR);
            TextArea2.Styles[Style.IndentGuide].BackColor = IntToColor(BACK_COLOR);

            var nums = TextArea.Margins[NUMBER_MARGIN];
            nums.Width = 30;
            nums.Type = MarginType.Number;
            nums.Sensitive = true;
            nums.Mask = 0;
            var nums2 = TextArea.Margins[NUMBER_MARGIN];
            nums2.Width = 30;
            nums2.Type = MarginType.Number;
            nums2.Sensitive = true;
            nums2.Mask = 0;


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
