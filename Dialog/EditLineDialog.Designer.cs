namespace WhereTrainBuild.Dialog
{
    partial class EditLineDialog
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
            this.StationList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StationLbl = new System.Windows.Forms.Label();
            this.AddBtn = new System.Windows.Forms.Button();
            this.BuildBtn = new System.Windows.Forms.Button();
            this.DisplayTxt = new System.Windows.Forms.TextBox();
            this.NameTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LineAddBtn = new System.Windows.Forms.Button();
            this.PathListBox = new System.Windows.Forms.ListBox();
            this.SetListLbl = new System.Windows.Forms.Label();
            this.SetList = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StationList
            // 
            this.StationList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StationList.FormattingEnabled = true;
            this.StationList.ItemHeight = 12;
            this.StationList.Location = new System.Drawing.Point(7, 18);
            this.StationList.Name = "StationList";
            this.StationList.Size = new System.Drawing.Size(322, 52);
            this.StationList.TabIndex = 0;
            this.StationList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StationList_MouseDown);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "駅";
            // 
            // StationLbl
            // 
            this.StationLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StationLbl.AutoSize = true;
            this.StationLbl.BackColor = System.Drawing.Color.White;
            this.StationLbl.Location = new System.Drawing.Point(55, 91);
            this.StationLbl.Name = "StationLbl";
            this.StationLbl.Size = new System.Drawing.Size(35, 12);
            this.StationLbl.TabIndex = 2;
            this.StationLbl.Text = "クリック";
            this.StationLbl.Click += new System.EventHandler(this.StationLbl_Click);
            // 
            // AddBtn
            // 
            this.AddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddBtn.Location = new System.Drawing.Point(254, 86);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(75, 23);
            this.AddBtn.TabIndex = 3;
            this.AddBtn.Text = "追加";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // BuildBtn
            // 
            this.BuildBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BuildBtn.Location = new System.Drawing.Point(254, 115);
            this.BuildBtn.Name = "BuildBtn";
            this.BuildBtn.Size = new System.Drawing.Size(75, 25);
            this.BuildBtn.TabIndex = 10;
            this.BuildBtn.Text = "構築";
            this.BuildBtn.UseVisualStyleBackColor = true;
            this.BuildBtn.Click += new System.EventHandler(this.BuildBtn_Click);
            // 
            // DisplayTxt
            // 
            this.DisplayTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DisplayTxt.Location = new System.Drawing.Point(32, 217);
            this.DisplayTxt.Name = "DisplayTxt";
            this.DisplayTxt.Size = new System.Drawing.Size(100, 19);
            this.DisplayTxt.TabIndex = 8;
            // 
            // NameTxt
            // 
            this.NameTxt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NameTxt.Location = new System.Drawing.Point(32, 180);
            this.NameTxt.Name = "NameTxt";
            this.NameTxt.Size = new System.Drawing.Size(100, 19);
            this.NameTxt.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 202);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "表示名称";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 165);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "名称";
            // 
            // LineAddBtn
            // 
            this.LineAddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LineAddBtn.Location = new System.Drawing.Point(41, 256);
            this.LineAddBtn.Name = "LineAddBtn";
            this.LineAddBtn.Size = new System.Drawing.Size(89, 23);
            this.LineAddBtn.TabIndex = 14;
            this.LineAddBtn.Text = "追加";
            this.LineAddBtn.UseVisualStyleBackColor = true;
            this.LineAddBtn.Click += new System.EventHandler(this.LineAddBtn_Click);
            // 
            // PathListBox
            // 
            this.PathListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PathListBox.FormattingEnabled = true;
            this.PathListBox.ItemHeight = 12;
            this.PathListBox.Location = new System.Drawing.Point(14, 12);
            this.PathListBox.Name = "PathListBox";
            this.PathListBox.Size = new System.Drawing.Size(200, 136);
            this.PathListBox.TabIndex = 13;
            // 
            // SetListLbl
            // 
            this.SetListLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SetListLbl.AutoSize = true;
            this.SetListLbl.Location = new System.Drawing.Point(227, 165);
            this.SetListLbl.Name = "SetListLbl";
            this.SetListLbl.Size = new System.Drawing.Size(65, 12);
            this.SetListLbl.TabIndex = 12;
            this.SetListLbl.Text = "時刻表種別";
            // 
            // SetList
            // 
            this.SetList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SetList.FormattingEnabled = true;
            this.SetList.Location = new System.Drawing.Point(224, 185);
            this.SetList.Name = "SetList";
            this.SetList.Size = new System.Drawing.Size(171, 46);
            this.SetList.TabIndex = 17;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.StationList);
            this.groupBox1.Controls.Add(this.StationLbl);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.AddBtn);
            this.groupBox1.Controls.Add(this.BuildBtn);
            this.groupBox1.Location = new System.Drawing.Point(219, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(336, 146);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "駅";
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(295, 256);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 19;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // EditLineDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 288);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.SetList);
            this.Controls.Add(this.LineAddBtn);
            this.Controls.Add(this.PathListBox);
            this.Controls.Add(this.SetListLbl);
            this.Controls.Add(this.DisplayTxt);
            this.Controls.Add(this.NameTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(578, 327);
            this.Name = "EditLineDialog";
            this.Text = "方面編集ダイアログ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditLineDialog_FormClosed);
            this.Load += new System.EventHandler(this.EditLineDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox StationList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label StationLbl;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button BuildBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button LineAddBtn;
        private System.Windows.Forms.ListBox PathListBox;
        private System.Windows.Forms.Label SetListLbl;
        public System.Windows.Forms.TextBox DisplayTxt;
        public System.Windows.Forms.TextBox NameTxt;
        private System.Windows.Forms.CheckedListBox SetList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button CancelBtn;
    }
}