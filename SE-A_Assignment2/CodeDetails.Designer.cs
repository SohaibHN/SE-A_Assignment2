namespace SE_A_Assignment2
{
    partial class CodeDetails
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeDetails));
            this.label1 = new System.Windows.Forms.Label();
            this.URL = new System.Windows.Forms.TextBox();
            this.BugID = new System.Windows.Forms.TextBox();
            this.BugAuthor = new System.Windows.Forms.TextBox();
            this.BugClass = new System.Windows.Forms.TextBox();
            this.BugMethods = new System.Windows.Forms.TextBox();
            this.BugLines = new System.Windows.Forms.TextBox();
            this.GenerateCode = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.PanelSearch = new System.Windows.Forms.Panel();
            this.BtnNextSearch = new System.Windows.Forms.Button();
            this.BtnPrevSearch = new System.Windows.Forms.Button();
            this.BtnClearSearch = new System.Windows.Forms.Button();
            this.TxtSearch = new System.Windows.Forms.TextBox();
            this.CodeBox = new System.Windows.Forms.Panel();
            this.SaveButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.BugVersion = new System.Windows.Forms.TextBox();
            this.PanelSearch.SuspendLayout();
            this.CodeBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Location = new System.Drawing.Point(1, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(600, 2);
            this.label1.TabIndex = 1002;
            // 
            // URL
            // 
            this.URL.Location = new System.Drawing.Point(76, 41);
            this.URL.Name = "URL";
            this.URL.Size = new System.Drawing.Size(153, 20);
            this.URL.TabIndex = 0;
            // 
            // BugID
            // 
            this.BugID.Location = new System.Drawing.Point(416, 41);
            this.BugID.Name = "BugID";
            this.BugID.ReadOnly = true;
            this.BugID.Size = new System.Drawing.Size(153, 20);
            this.BugID.TabIndex = 4;
            // 
            // BugAuthor
            // 
            this.BugAuthor.Location = new System.Drawing.Point(416, 67);
            this.BugAuthor.Name = "BugAuthor";
            this.BugAuthor.Size = new System.Drawing.Size(153, 20);
            this.BugAuthor.TabIndex = 5;
            // 
            // BugClass
            // 
            this.BugClass.Location = new System.Drawing.Point(76, 98);
            this.BugClass.Name = "BugClass";
            this.BugClass.Size = new System.Drawing.Size(153, 20);
            this.BugClass.TabIndex = 2;
            // 
            // BugMethods
            // 
            this.BugMethods.Location = new System.Drawing.Point(76, 124);
            this.BugMethods.Name = "BugMethods";
            this.BugMethods.Size = new System.Drawing.Size(153, 20);
            this.BugMethods.TabIndex = 3;
            // 
            // BugLines
            // 
            this.BugLines.Location = new System.Drawing.Point(416, 95);
            this.BugLines.Name = "BugLines";
            this.BugLines.Size = new System.Drawing.Size(153, 20);
            this.BugLines.TabIndex = 6;
            // 
            // GenerateCode
            // 
            this.GenerateCode.Location = new System.Drawing.Point(170, 64);
            this.GenerateCode.Name = "GenerateCode";
            this.GenerateCode.Size = new System.Drawing.Size(59, 23);
            this.GenerateCode.TabIndex = 1;
            this.GenerateCode.Text = "Generate";
            this.GenerateCode.UseVisualStyleBackColor = true;
            this.GenerateCode.Click += new System.EventHandler(this.GenerateCode_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1010;
            this.label2.Text = "URL";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 1011;
            this.label3.Text = "Methods:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(365, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 1012;
            this.label4.Text = "Author:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 1013;
            this.label5.Text = "Class:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(365, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 1014;
            this.label6.Text = "Bug ID:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(365, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 1015;
            this.label7.Text = "Lines:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 159);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(328, 13);
            this.label8.TabIndex = 1017;
            this.label8.Text = "Please generate the code from the URL or paste into the box below:";
            // 
            // PanelSearch
            // 
            this.PanelSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelSearch.BackColor = System.Drawing.Color.White;
            this.PanelSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelSearch.Controls.Add(this.BtnNextSearch);
            this.PanelSearch.Controls.Add(this.BtnPrevSearch);
            this.PanelSearch.Controls.Add(this.BtnClearSearch);
            this.PanelSearch.Controls.Add(this.TxtSearch);
            this.PanelSearch.Location = new System.Drawing.Point(285, 3);
            this.PanelSearch.Name = "PanelSearch";
            this.PanelSearch.Size = new System.Drawing.Size(292, 40);
            this.PanelSearch.TabIndex = 1018;
            this.PanelSearch.Visible = false;
            // 
            // BtnNextSearch
            // 
            this.BtnNextSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnNextSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnNextSearch.ForeColor = System.Drawing.Color.White;
            this.BtnNextSearch.Image = ((System.Drawing.Image)(resources.GetObject("BtnNextSearch.Image")));
            this.BtnNextSearch.Location = new System.Drawing.Point(233, 4);
            this.BtnNextSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnNextSearch.Name = "BtnNextSearch";
            this.BtnNextSearch.Size = new System.Drawing.Size(25, 30);
            this.BtnNextSearch.TabIndex = 9;
            this.BtnNextSearch.Tag = "Find next (Enter)";
            this.BtnNextSearch.UseVisualStyleBackColor = true;
            this.BtnNextSearch.Click += new System.EventHandler(this.BtnNextSearch_Click);
            // 
            // BtnPrevSearch
            // 
            this.BtnPrevSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnPrevSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPrevSearch.ForeColor = System.Drawing.Color.White;
            this.BtnPrevSearch.Image = ((System.Drawing.Image)(resources.GetObject("BtnPrevSearch.Image")));
            this.BtnPrevSearch.Location = new System.Drawing.Point(205, 4);
            this.BtnPrevSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnPrevSearch.Name = "BtnPrevSearch";
            this.BtnPrevSearch.Size = new System.Drawing.Size(25, 30);
            this.BtnPrevSearch.TabIndex = 8;
            this.BtnPrevSearch.Tag = "Find previous (Shift+Enter)";
            this.BtnPrevSearch.UseVisualStyleBackColor = true;
            this.BtnPrevSearch.Click += new System.EventHandler(this.BtnPrevSearch_Click);
            // 
            // BtnClearSearch
            // 
            this.BtnClearSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClearSearch.ForeColor = System.Drawing.Color.White;
            this.BtnClearSearch.Image = ((System.Drawing.Image)(resources.GetObject("BtnClearSearch.Image")));
            this.BtnClearSearch.Location = new System.Drawing.Point(261, 4);
            this.BtnClearSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnClearSearch.Name = "BtnClearSearch";
            this.BtnClearSearch.Size = new System.Drawing.Size(25, 30);
            this.BtnClearSearch.TabIndex = 7;
            this.BtnClearSearch.Tag = "Close (Esc)";
            this.BtnClearSearch.UseVisualStyleBackColor = true;
            this.BtnClearSearch.Click += new System.EventHandler(this.BtnClearSearch_Click);
            // 
            // TxtSearch
            // 
            this.TxtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtSearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TxtSearch.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtSearch.Location = new System.Drawing.Point(10, 6);
            this.TxtSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TxtSearch.Name = "TxtSearch";
            this.TxtSearch.Size = new System.Drawing.Size(189, 25);
            this.TxtSearch.TabIndex = 6;
            // 
            // CodeBox
            // 
            this.CodeBox.Controls.Add(this.PanelSearch);
            this.CodeBox.Location = new System.Drawing.Point(12, 181);
            this.CodeBox.Name = "CodeBox";
            this.CodeBox.Size = new System.Drawing.Size(580, 276);
            this.CodeBox.TabIndex = 8;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(525, 463);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(59, 23);
            this.SaveButton.TabIndex = 9;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(365, 127);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 13);
            this.label9.TabIndex = 1022;
            this.label9.Text = "Version:";
            // 
            // BugVersion
            // 
            this.BugVersion.Location = new System.Drawing.Point(416, 124);
            this.BugVersion.Name = "BugVersion";
            this.BugVersion.Size = new System.Drawing.Size(153, 20);
            this.BugVersion.TabIndex = 7;
            // 
            // CodeDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 492);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.BugVersion);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.GenerateCode);
            this.Controls.Add(this.BugLines);
            this.Controls.Add(this.BugMethods);
            this.Controls.Add(this.BugClass);
            this.Controls.Add(this.BugAuthor);
            this.Controls.Add(this.BugID);
            this.Controls.Add(this.URL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CodeBox);
            this.Name = "CodeDetails";
            this.Text = "CodeDetails";
            this.Load += new System.EventHandler(this.CodeDetails_Load);
            this.PanelSearch.ResumeLayout(false);
            this.PanelSearch.PerformLayout();
            this.CodeBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox URL;
        private System.Windows.Forms.TextBox BugID;
        private System.Windows.Forms.TextBox BugAuthor;
        private System.Windows.Forms.TextBox BugClass;
        private System.Windows.Forms.TextBox BugMethods;
        private System.Windows.Forms.TextBox BugLines;
        private System.Windows.Forms.Button GenerateCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel PanelSearch;
        private System.Windows.Forms.Button BtnNextSearch;
        private System.Windows.Forms.Button BtnPrevSearch;
        private System.Windows.Forms.Button BtnClearSearch;
        private System.Windows.Forms.TextBox TxtSearch;
        private System.Windows.Forms.Panel CodeBox;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox BugVersion;
    }
}