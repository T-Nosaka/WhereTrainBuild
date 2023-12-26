namespace WhereTrainBuild.Dialog
{
    partial class FactoryProDialog
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
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.OverTimePicker = new System.Windows.Forms.DateTimePicker();
            this.NameTxt = new System.Windows.Forms.TextBox();
            this.UniqIDTxt = new System.Windows.Forms.TextBox();
            this.UrlTxt = new System.Windows.Forms.TextBox();
            this.LeftBottomLbl = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.RightUpLbl = new System.Windows.Forms.Label();
            this.CalcAreaBtn = new System.Windows.Forms.Button();
            this.RssKeywordTxt = new System.Windows.Forms.TextBox();
            this.RssTxt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.RssGrp = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.AreaGrp = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ColorLbl = new System.Windows.Forms.Label();
            this.RssGrp.SuspendLayout();
            this.AreaGrp.SuspendLayout();
            this.SuspendLayout();
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(224, 288);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 9;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OkBtn.Location = new System.Drawing.Point(70, 288);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 8;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "名前";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "ID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(27, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "URL";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(205, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "日またぎ時刻";
            // 
            // OverTimePicker
            // 
            this.OverTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OverTimePicker.CustomFormat = "HH:mm";
            this.OverTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.OverTimePicker.Location = new System.Drawing.Point(280, 46);
            this.OverTimePicker.Name = "OverTimePicker";
            this.OverTimePicker.ShowUpDown = true;
            this.OverTimePicker.Size = new System.Drawing.Size(66, 19);
            this.OverTimePicker.TabIndex = 11;
            // 
            // NameTxt
            // 
            this.NameTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameTxt.Location = new System.Drawing.Point(88, 21);
            this.NameTxt.Name = "NameTxt";
            this.NameTxt.Size = new System.Drawing.Size(100, 19);
            this.NameTxt.TabIndex = 13;
            // 
            // UniqIDTxt
            // 
            this.UniqIDTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UniqIDTxt.Location = new System.Drawing.Point(88, 46);
            this.UniqIDTxt.Name = "UniqIDTxt";
            this.UniqIDTxt.Size = new System.Drawing.Size(100, 19);
            this.UniqIDTxt.TabIndex = 13;
            // 
            // UrlTxt
            // 
            this.UrlTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UrlTxt.Location = new System.Drawing.Point(88, 71);
            this.UrlTxt.Name = "UrlTxt";
            this.UrlTxt.Size = new System.Drawing.Size(259, 19);
            this.UrlTxt.TabIndex = 13;
            // 
            // LeftBottomLbl
            // 
            this.LeftBottomLbl.AutoSize = true;
            this.LeftBottomLbl.BackColor = System.Drawing.Color.White;
            this.LeftBottomLbl.ForeColor = System.Drawing.Color.Black;
            this.LeftBottomLbl.Location = new System.Drawing.Point(88, 18);
            this.LeftBottomLbl.Name = "LeftBottomLbl";
            this.LeftBottomLbl.Size = new System.Drawing.Size(19, 12);
            this.LeftBottomLbl.TabIndex = 14;
            this.LeftBottomLbl.Text = "0.0";
            this.LeftBottomLbl.Click += new System.EventHandler(this.LeftBottomLbl_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "左下";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "右上";
            // 
            // RightUpLbl
            // 
            this.RightUpLbl.AutoSize = true;
            this.RightUpLbl.BackColor = System.Drawing.Color.White;
            this.RightUpLbl.ForeColor = System.Drawing.Color.Black;
            this.RightUpLbl.Location = new System.Drawing.Point(88, 36);
            this.RightUpLbl.Name = "RightUpLbl";
            this.RightUpLbl.Size = new System.Drawing.Size(19, 12);
            this.RightUpLbl.TabIndex = 14;
            this.RightUpLbl.Text = "0.0";
            this.RightUpLbl.Click += new System.EventHandler(this.RightUpLbl_Click);
            // 
            // CalcAreaBtn
            // 
            this.CalcAreaBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CalcAreaBtn.Location = new System.Drawing.Point(269, 18);
            this.CalcAreaBtn.Name = "CalcAreaBtn";
            this.CalcAreaBtn.Size = new System.Drawing.Size(64, 30);
            this.CalcAreaBtn.TabIndex = 15;
            this.CalcAreaBtn.Text = "エリア取得";
            this.CalcAreaBtn.UseVisualStyleBackColor = true;
            this.CalcAreaBtn.Click += new System.EventHandler(this.CalcAreaBtn_Click);
            // 
            // RssKeywordTxt
            // 
            this.RssKeywordTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RssKeywordTxt.Location = new System.Drawing.Point(8, 76);
            this.RssKeywordTxt.Name = "RssKeywordTxt";
            this.RssKeywordTxt.Size = new System.Drawing.Size(315, 19);
            this.RssKeywordTxt.TabIndex = 13;
            // 
            // RssTxt
            // 
            this.RssTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RssTxt.Location = new System.Drawing.Point(8, 35);
            this.RssTxt.Name = "RssTxt";
            this.RssTxt.Size = new System.Drawing.Size(315, 19);
            this.RssTxt.TabIndex = 16;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "Keyword";
            // 
            // RssGrp
            // 
            this.RssGrp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RssGrp.Controls.Add(this.label5);
            this.RssGrp.Controls.Add(this.label8);
            this.RssGrp.Controls.Add(this.RssTxt);
            this.RssGrp.Controls.Add(this.RssKeywordTxt);
            this.RssGrp.Location = new System.Drawing.Point(24, 96);
            this.RssGrp.Name = "RssGrp";
            this.RssGrp.Size = new System.Drawing.Size(338, 109);
            this.RssGrp.TabIndex = 18;
            this.RssGrp.TabStop = false;
            this.RssGrp.Text = "RSS";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "URL";
            // 
            // AreaGrp
            // 
            this.AreaGrp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AreaGrp.Controls.Add(this.label6);
            this.AreaGrp.Controls.Add(this.LeftBottomLbl);
            this.AreaGrp.Controls.Add(this.CalcAreaBtn);
            this.AreaGrp.Controls.Add(this.RightUpLbl);
            this.AreaGrp.Controls.Add(this.label7);
            this.AreaGrp.Location = new System.Drawing.Point(23, 211);
            this.AreaGrp.Name = "AreaGrp";
            this.AreaGrp.Size = new System.Drawing.Size(339, 65);
            this.AreaGrp.TabIndex = 19;
            this.AreaGrp.TabStop = false;
            this.AreaGrp.Text = "範囲";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(205, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 20;
            this.label9.Text = "基本色";
            // 
            // ColorLbl
            // 
            this.ColorLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ColorLbl.BackColor = System.Drawing.Color.White;
            this.ColorLbl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ColorLbl.Location = new System.Drawing.Point(280, 21);
            this.ColorLbl.Name = "ColorLbl";
            this.ColorLbl.Size = new System.Drawing.Size(66, 19);
            this.ColorLbl.TabIndex = 21;
            this.ColorLbl.Click += new System.EventHandler(this.ColorLbl_Click);
            // 
            // FactoryProDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 323);
            this.Controls.Add(this.ColorLbl);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.AreaGrp);
            this.Controls.Add(this.RssGrp);
            this.Controls.Add(this.UrlTxt);
            this.Controls.Add(this.UniqIDTxt);
            this.Controls.Add(this.NameTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.OverTimePicker);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(390, 362);
            this.Name = "FactoryProDialog";
            this.Text = "ファクトリ";
            this.Load += new System.EventHandler(this.FactoryProDialog_Load);
            this.RssGrp.ResumeLayout(false);
            this.RssGrp.PerformLayout();
            this.AreaGrp.ResumeLayout(false);
            this.AreaGrp.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker OverTimePicker;
        private System.Windows.Forms.TextBox NameTxt;
        private System.Windows.Forms.TextBox UniqIDTxt;
        private System.Windows.Forms.TextBox UrlTxt;
        private System.Windows.Forms.Label LeftBottomLbl;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label RightUpLbl;
        private System.Windows.Forms.Button CalcAreaBtn;
        private System.Windows.Forms.TextBox RssKeywordTxt;
        private System.Windows.Forms.TextBox RssTxt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox RssGrp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox AreaGrp;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label ColorLbl;
    }
}