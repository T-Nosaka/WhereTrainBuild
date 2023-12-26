namespace WhereTrainBuild.Dialog
{
    partial class BuildDialogTrainDialog
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
            this.SelectSetBtn = new System.Windows.Forms.Button();
            this.DelLineBtn = new System.Windows.Forms.Button();
            this.DelSetBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LineLbl = new System.Windows.Forms.Label();
            this.LineSetListBox = new System.Windows.Forms.ListBox();
            this.LineListBox = new System.Windows.Forms.ListBox();
            this.TrainListBox = new System.Windows.Forms.ListBox();
            this.EditBtn = new System.Windows.Forms.Button();
            this.AddBtn = new System.Windows.Forms.Button();
            this.DelBtn = new System.Windows.Forms.Button();
            this.EditLineBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.AddLineBtn = new System.Windows.Forms.Button();
            this.DiaSrcTxt = new System.Windows.Forms.TextBox();
            this.FileSelectBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.BuildBtn = new System.Windows.Forms.Button();
            this.DiaBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.TimeTableCmb = new System.Windows.Forms.ComboBox();
            this.TimeTableSetBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SelectSetBtn
            // 
            this.SelectSetBtn.Location = new System.Drawing.Point(274, 57);
            this.SelectSetBtn.Name = "SelectSetBtn";
            this.SelectSetBtn.Size = new System.Drawing.Size(75, 23);
            this.SelectSetBtn.TabIndex = 16;
            this.SelectSetBtn.Text = "選択";
            this.SelectSetBtn.UseVisualStyleBackColor = true;
            this.SelectSetBtn.Click += new System.EventHandler(this.SelectSetBtn_Click);
            // 
            // DelLineBtn
            // 
            this.DelLineBtn.Location = new System.Drawing.Point(274, 224);
            this.DelLineBtn.Name = "DelLineBtn";
            this.DelLineBtn.Size = new System.Drawing.Size(75, 23);
            this.DelLineBtn.TabIndex = 14;
            this.DelLineBtn.Text = "削除";
            this.DelLineBtn.UseVisualStyleBackColor = true;
            this.DelLineBtn.Click += new System.EventHandler(this.DelLineBtn_Click);
            // 
            // DelSetBtn
            // 
            this.DelSetBtn.Location = new System.Drawing.Point(274, 86);
            this.DelSetBtn.Name = "DelSetBtn";
            this.DelSetBtn.Size = new System.Drawing.Size(75, 23);
            this.DelSetBtn.TabIndex = 15;
            this.DelSetBtn.Text = "削除";
            this.DelSetBtn.UseVisualStyleBackColor = true;
            this.DelSetBtn.Click += new System.EventHandler(this.DelSetBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "時刻表種別";
            // 
            // LineLbl
            // 
            this.LineLbl.AutoSize = true;
            this.LineLbl.Location = new System.Drawing.Point(12, 119);
            this.LineLbl.Name = "LineLbl";
            this.LineLbl.Size = new System.Drawing.Size(29, 12);
            this.LineLbl.TabIndex = 13;
            this.LineLbl.Text = "方面";
            // 
            // LineSetListBox
            // 
            this.LineSetListBox.FormattingEnabled = true;
            this.LineSetListBox.ItemHeight = 12;
            this.LineSetListBox.Location = new System.Drawing.Point(12, 57);
            this.LineSetListBox.Name = "LineSetListBox";
            this.LineSetListBox.Size = new System.Drawing.Size(234, 52);
            this.LineSetListBox.TabIndex = 10;
            this.LineSetListBox.SelectedIndexChanged += new System.EventHandler(this.LineSetListBox_SelectedIndexChanged);
            // 
            // LineListBox
            // 
            this.LineListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LineListBox.FormattingEnabled = true;
            this.LineListBox.ItemHeight = 12;
            this.LineListBox.Location = new System.Drawing.Point(14, 134);
            this.LineListBox.Name = "LineListBox";
            this.LineListBox.Size = new System.Drawing.Size(232, 124);
            this.LineListBox.TabIndex = 11;
            this.LineListBox.SelectedIndexChanged += new System.EventHandler(this.LineListBox_SelectedIndexChanged);
            // 
            // TrainListBox
            // 
            this.TrainListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TrainListBox.FormattingEnabled = true;
            this.TrainListBox.ItemHeight = 12;
            this.TrainListBox.Location = new System.Drawing.Point(377, 28);
            this.TrainListBox.Name = "TrainListBox";
            this.TrainListBox.Size = new System.Drawing.Size(222, 184);
            this.TrainListBox.TabIndex = 17;
            // 
            // EditBtn
            // 
            this.EditBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EditBtn.Location = new System.Drawing.Point(622, 28);
            this.EditBtn.Name = "EditBtn";
            this.EditBtn.Size = new System.Drawing.Size(75, 23);
            this.EditBtn.TabIndex = 18;
            this.EditBtn.Text = "編集";
            this.EditBtn.UseVisualStyleBackColor = true;
            this.EditBtn.Click += new System.EventHandler(this.EditBtn_Click);
            // 
            // AddBtn
            // 
            this.AddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddBtn.Location = new System.Drawing.Point(622, 57);
            this.AddBtn.Name = "AddBtn";
            this.AddBtn.Size = new System.Drawing.Size(75, 23);
            this.AddBtn.TabIndex = 19;
            this.AddBtn.Text = "追加";
            this.AddBtn.UseVisualStyleBackColor = true;
            this.AddBtn.Click += new System.EventHandler(this.AddBtn_Click);
            // 
            // DelBtn
            // 
            this.DelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DelBtn.Location = new System.Drawing.Point(622, 92);
            this.DelBtn.Name = "DelBtn";
            this.DelBtn.Size = new System.Drawing.Size(75, 23);
            this.DelBtn.TabIndex = 20;
            this.DelBtn.Text = "削除";
            this.DelBtn.UseVisualStyleBackColor = true;
            this.DelBtn.Click += new System.EventHandler(this.DelBtn_Click);
            // 
            // EditLineBtn
            // 
            this.EditLineBtn.Location = new System.Drawing.Point(274, 134);
            this.EditLineBtn.Name = "EditLineBtn";
            this.EditLineBtn.Size = new System.Drawing.Size(75, 23);
            this.EditLineBtn.TabIndex = 21;
            this.EditLineBtn.Text = "編集";
            this.EditLineBtn.UseVisualStyleBackColor = true;
            this.EditLineBtn.Click += new System.EventHandler(this.EditLineBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(385, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 22;
            this.label2.Text = "電車";
            // 
            // AddLineBtn
            // 
            this.AddLineBtn.Location = new System.Drawing.Point(274, 163);
            this.AddLineBtn.Name = "AddLineBtn";
            this.AddLineBtn.Size = new System.Drawing.Size(75, 23);
            this.AddLineBtn.TabIndex = 23;
            this.AddLineBtn.Text = "追加";
            this.AddLineBtn.UseVisualStyleBackColor = true;
            this.AddLineBtn.Click += new System.EventHandler(this.AddLineBtn_Click);
            // 
            // DiaSrcTxt
            // 
            this.DiaSrcTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DiaSrcTxt.Location = new System.Drawing.Point(377, 253);
            this.DiaSrcTxt.Name = "DiaSrcTxt";
            this.DiaSrcTxt.Size = new System.Drawing.Size(222, 19);
            this.DiaSrcTxt.TabIndex = 24;
            this.DiaSrcTxt.TextChanged += new System.EventHandler(this.DiaSrcTxt_TextChanged);
            // 
            // FileSelectBtn
            // 
            this.FileSelectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.FileSelectBtn.Location = new System.Drawing.Point(605, 251);
            this.FileSelectBtn.Name = "FileSelectBtn";
            this.FileSelectBtn.Size = new System.Drawing.Size(28, 23);
            this.FileSelectBtn.TabIndex = 25;
            this.FileSelectBtn.Text = "...";
            this.FileSelectBtn.UseVisualStyleBackColor = true;
            this.FileSelectBtn.Click += new System.EventHandler(this.FileSelectBtn_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(385, 238);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 12);
            this.label3.TabIndex = 26;
            this.label3.Text = "動的構築スクリプト";
            // 
            // BuildBtn
            // 
            this.BuildBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BuildBtn.Enabled = false;
            this.BuildBtn.Location = new System.Drawing.Point(639, 251);
            this.BuildBtn.Name = "BuildBtn";
            this.BuildBtn.Size = new System.Drawing.Size(63, 23);
            this.BuildBtn.TabIndex = 27;
            this.BuildBtn.Text = "構築";
            this.BuildBtn.UseVisualStyleBackColor = true;
            this.BuildBtn.Click += new System.EventHandler(this.BuildBtn_Click);
            // 
            // DiaBtn
            // 
            this.DiaBtn.Location = new System.Drawing.Point(274, 192);
            this.DiaBtn.Name = "DiaBtn";
            this.DiaBtn.Size = new System.Drawing.Size(75, 23);
            this.DiaBtn.TabIndex = 23;
            this.DiaBtn.Text = "ダイアグラム";
            this.DiaBtn.UseVisualStyleBackColor = true;
            this.DiaBtn.Click += new System.EventHandler(this.DiaBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 12);
            this.label4.TabIndex = 28;
            this.label4.Text = "タイムテーブル種別";
            // 
            // TimeTableCmb
            // 
            this.TimeTableCmb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TimeTableCmb.FormattingEnabled = true;
            this.TimeTableCmb.Location = new System.Drawing.Point(125, 6);
            this.TimeTableCmb.Name = "TimeTableCmb";
            this.TimeTableCmb.Size = new System.Drawing.Size(121, 20);
            this.TimeTableCmb.TabIndex = 29;
            // 
            // TimeTableSetBtn
            // 
            this.TimeTableSetBtn.Location = new System.Drawing.Point(274, 6);
            this.TimeTableSetBtn.Name = "TimeTableSetBtn";
            this.TimeTableSetBtn.Size = new System.Drawing.Size(75, 23);
            this.TimeTableSetBtn.TabIndex = 30;
            this.TimeTableSetBtn.Text = "選択";
            this.TimeTableSetBtn.UseVisualStyleBackColor = true;
            this.TimeTableSetBtn.Click += new System.EventHandler(this.TimeTableSetBtn_Click);
            // 
            // BuildDialogTrainDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 293);
            this.Controls.Add(this.TimeTableSetBtn);
            this.Controls.Add(this.TimeTableCmb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.BuildBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.FileSelectBtn);
            this.Controls.Add(this.DiaSrcTxt);
            this.Controls.Add(this.DiaBtn);
            this.Controls.Add(this.AddLineBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.EditLineBtn);
            this.Controls.Add(this.DelBtn);
            this.Controls.Add(this.AddBtn);
            this.Controls.Add(this.EditBtn);
            this.Controls.Add(this.TrainListBox);
            this.Controls.Add(this.SelectSetBtn);
            this.Controls.Add(this.DelLineBtn);
            this.Controls.Add(this.DelSetBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LineLbl);
            this.Controls.Add(this.LineSetListBox);
            this.Controls.Add(this.LineListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(730, 256);
            this.Name = "BuildDialogTrainDialog";
            this.Text = "電車構築";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BuildDialogTrainDialog_FormClosed);
            this.Load += new System.EventHandler(this.BuildDialogTrainDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SelectSetBtn;
        private System.Windows.Forms.Button DelLineBtn;
        private System.Windows.Forms.Button DelSetBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LineLbl;
        private System.Windows.Forms.ListBox LineSetListBox;
        private System.Windows.Forms.ListBox LineListBox;
        private System.Windows.Forms.ListBox TrainListBox;
        private System.Windows.Forms.Button EditBtn;
        private System.Windows.Forms.Button AddBtn;
        private System.Windows.Forms.Button DelBtn;
        private System.Windows.Forms.Button EditLineBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button AddLineBtn;
        private System.Windows.Forms.TextBox DiaSrcTxt;
        private System.Windows.Forms.Button FileSelectBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BuildBtn;
        private System.Windows.Forms.Button DiaBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox TimeTableCmb;
        private System.Windows.Forms.Button TimeTableSetBtn;
    }
}