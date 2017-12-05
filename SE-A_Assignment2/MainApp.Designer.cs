namespace SE_A_Assignment2
{
    partial class MainApp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.ManageUsers = new System.Windows.Forms.Button();
            this.ViewBugs = new System.Windows.Forms.Button();
            this.Line1 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.projectDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.severityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateLoggedDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deadlineDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ticketsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bugTrackerDataSet = new SE_A_Assignment2.BugTrackerDataSet();
            this.ticketsTableAdapter = new SE_A_Assignment2.BugTrackerDataSetTableAdapters.ticketsTableAdapter();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bugTrackerDataSet)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Image = global::SE_A_Assignment2.Properties.Resources.test;
            this.button1.Location = new System.Drawing.Point(2, 164);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 116);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ManageUsers
            // 
            this.ManageUsers.Location = new System.Drawing.Point(2, 132);
            this.ManageUsers.Name = "ManageUsers";
            this.ManageUsers.Size = new System.Drawing.Size(93, 26);
            this.ManageUsers.TabIndex = 2;
            this.ManageUsers.Text = "Manage Users";
            this.ManageUsers.UseVisualStyleBackColor = true;
            this.ManageUsers.Click += new System.EventHandler(this.ManageUsers_Click);
            // 
            // ViewBugs
            // 
            this.ViewBugs.Location = new System.Drawing.Point(367, 447);
            this.ViewBugs.Name = "ViewBugs";
            this.ViewBugs.Size = new System.Drawing.Size(93, 26);
            this.ViewBugs.TabIndex = 3;
            this.ViewBugs.Text = "View All Bugs";
            this.ViewBugs.UseVisualStyleBackColor = true;
            this.ViewBugs.Click += new System.EventHandler(this.ViewBugs_Click);
            // 
            // Line1
            // 
            this.Line1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Line1.Location = new System.Drawing.Point(6, 36);
            this.Line1.Name = "Line1";
            this.Line1.Size = new System.Drawing.Size(800, 3);
            this.Line1.TabIndex = 1000;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(95, -55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(2, 550);
            this.label1.TabIndex = 1001;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.userDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.projectDataGridViewTextBoxColumn,
            this.statusDataGridViewTextBoxColumn,
            this.severityDataGridViewTextBoxColumn,
            this.dateLoggedDataGridViewTextBoxColumn,
            this.deadlineDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.ticketsBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(103, 28);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(626, 413);
            this.dataGridView1.TabIndex = 1002;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            // 
            // userDataGridViewTextBoxColumn
            // 
            this.userDataGridViewTextBoxColumn.DataPropertyName = "User";
            this.userDataGridViewTextBoxColumn.HeaderText = "User";
            this.userDataGridViewTextBoxColumn.Name = "userDataGridViewTextBoxColumn";
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            // 
            // projectDataGridViewTextBoxColumn
            // 
            this.projectDataGridViewTextBoxColumn.DataPropertyName = "Project";
            this.projectDataGridViewTextBoxColumn.HeaderText = "Project";
            this.projectDataGridViewTextBoxColumn.Name = "projectDataGridViewTextBoxColumn";
            // 
            // statusDataGridViewTextBoxColumn
            // 
            this.statusDataGridViewTextBoxColumn.DataPropertyName = "Status";
            this.statusDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusDataGridViewTextBoxColumn.Name = "statusDataGridViewTextBoxColumn";
            // 
            // severityDataGridViewTextBoxColumn
            // 
            this.severityDataGridViewTextBoxColumn.DataPropertyName = "Severity";
            this.severityDataGridViewTextBoxColumn.HeaderText = "Severity";
            this.severityDataGridViewTextBoxColumn.Name = "severityDataGridViewTextBoxColumn";
            // 
            // dateLoggedDataGridViewTextBoxColumn
            // 
            this.dateLoggedDataGridViewTextBoxColumn.DataPropertyName = "Date Logged";
            this.dateLoggedDataGridViewTextBoxColumn.HeaderText = "Date Logged";
            this.dateLoggedDataGridViewTextBoxColumn.Name = "dateLoggedDataGridViewTextBoxColumn";
            // 
            // deadlineDataGridViewTextBoxColumn
            // 
            this.deadlineDataGridViewTextBoxColumn.DataPropertyName = "Deadline";
            this.deadlineDataGridViewTextBoxColumn.HeaderText = "Deadline";
            this.deadlineDataGridViewTextBoxColumn.Name = "deadlineDataGridViewTextBoxColumn";
            // 
            // ticketsBindingSource
            // 
            this.ticketsBindingSource.DataMember = "tickets";
            this.ticketsBindingSource.DataSource = this.bugTrackerDataSet;
            // 
            // bugTrackerDataSet
            // 
            this.bugTrackerDataSet.DataSetName = "BugTrackerDataSet";
            this.bugTrackerDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ticketsTableAdapter
            // 
            this.ticketsTableAdapter.ClearBeforeFill = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(751, 508);
            this.tabControl1.TabIndex = 1003;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ManageUsers);
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.ViewBugs);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(743, 482);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Controls.Add(this.Line1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(743, 482);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 54);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(721, 387);
            this.textBox1.TabIndex = 1001;
            // 
            // MainApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 501);
            this.Controls.Add(this.tabControl1);
            this.Name = "MainApp";
            this.Text = "MainApp";
            this.Load += new System.EventHandler(this.MainApp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ticketsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bugTrackerDataSet)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button ManageUsers;
        private System.Windows.Forms.Button ViewBugs;
        private System.Windows.Forms.Label Line1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private BugTrackerDataSet bugTrackerDataSet;
        private System.Windows.Forms.BindingSource ticketsBindingSource;
        private BugTrackerDataSetTableAdapters.ticketsTableAdapter ticketsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn userDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn projectDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn severityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateLoggedDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn deadlineDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBox1;
    }
}