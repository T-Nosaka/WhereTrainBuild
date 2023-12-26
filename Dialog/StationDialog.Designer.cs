namespace WhereTrainBuild.Dialog
{
    partial class StationDialog
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
            this.NameTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.VisibleChk = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.OkBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.LatLbl = new System.Windows.Forms.Label();
            this.LngLbl = new System.Windows.Forms.Label();
            this.CdNum = new System.Windows.Forms.NumericUpDown();
            this.CdLbl = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.GCdNum = new System.Windows.Forms.NumericUpDown();
            this.MaxBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.CdNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GCdNum)).BeginInit();
            this.SuspendLayout();
            // 
            // NameTxt
            // 
            this.NameTxt.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NameTxt.Location = new System.Drawing.Point(94, 12);
            this.NameTxt.Name = "NameTxt";
            this.NameTxt.Size = new System.Drawing.Size(138, 19);
            this.NameTxt.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // VisibleChk
            // 
            this.VisibleChk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VisibleChk.AutoSize = true;
            this.VisibleChk.Location = new System.Drawing.Point(173, 100);
            this.VisibleChk.Name = "VisibleChk";
            this.VisibleChk.Size = new System.Drawing.Size(59, 16);
            this.VisibleChk.TabIndex = 2;
            this.VisibleChk.Text = "Visible";
            this.VisibleChk.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(133, 154);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.OkBtn.Location = new System.Drawing.Point(34, 154);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 11;
            this.OkBtn.Text = "Ok";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "緯度";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "経度";
            // 
            // LatLbl
            // 
            this.LatLbl.AutoSize = true;
            this.LatLbl.Location = new System.Drawing.Point(52, 100);
            this.LatLbl.Name = "LatLbl";
            this.LatLbl.Size = new System.Drawing.Size(19, 12);
            this.LatLbl.TabIndex = 13;
            this.LatLbl.Text = "0.0";
            // 
            // LngLbl
            // 
            this.LngLbl.AutoSize = true;
            this.LngLbl.Location = new System.Drawing.Point(52, 129);
            this.LngLbl.Name = "LngLbl";
            this.LngLbl.Size = new System.Drawing.Size(19, 12);
            this.LngLbl.TabIndex = 13;
            this.LngLbl.Text = "0.0";
            // 
            // CdNum
            // 
            this.CdNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CdNum.Location = new System.Drawing.Point(94, 44);
            this.CdNum.Maximum = new decimal(new int[] {
            1316134911,
            2328,
            0,
            0});
            this.CdNum.Minimum = new decimal(new int[] {
            1316134911,
            2328,
            0,
            -2147483648});
            this.CdNum.Name = "CdNum";
            this.CdNum.Size = new System.Drawing.Size(114, 19);
            this.CdNum.TabIndex = 14;
            // 
            // CdLbl
            // 
            this.CdLbl.AutoSize = true;
            this.CdLbl.Location = new System.Drawing.Point(12, 46);
            this.CdLbl.Name = "CdLbl";
            this.CdLbl.Size = new System.Drawing.Size(44, 12);
            this.CdLbl.TabIndex = 13;
            this.CdLbl.Text = "駅コード";
            this.CdLbl.Click += new System.EventHandler(this.CdLbl_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 71);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "駅グループ";
            // 
            // GCdNum
            // 
            this.GCdNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GCdNum.Location = new System.Drawing.Point(94, 69);
            this.GCdNum.Maximum = new decimal(new int[] {
            1316134911,
            2328,
            0,
            0});
            this.GCdNum.Minimum = new decimal(new int[] {
            1316134911,
            2328,
            0,
            -2147483648});
            this.GCdNum.Name = "GCdNum";
            this.GCdNum.Size = new System.Drawing.Size(114, 19);
            this.GCdNum.TabIndex = 14;
            // 
            // MaxBtn
            // 
            this.MaxBtn.Location = new System.Drawing.Point(214, 46);
            this.MaxBtn.Name = "MaxBtn";
            this.MaxBtn.Size = new System.Drawing.Size(27, 23);
            this.MaxBtn.TabIndex = 15;
            this.MaxBtn.Text = "△";
            this.MaxBtn.UseVisualStyleBackColor = true;
            this.MaxBtn.Click += new System.EventHandler(this.MaxBtn_Click);
            // 
            // StationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 189);
            this.Controls.Add(this.MaxBtn);
            this.Controls.Add(this.GCdNum);
            this.Controls.Add(this.CdNum);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LngLbl);
            this.Controls.Add(this.LatLbl);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.CdLbl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.VisibleChk);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NameTxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(217, 228);
            this.Name = "StationDialog";
            this.Text = "駅";
            this.Load += new System.EventHandler(this.StationDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CdNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GCdNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.TextBox NameTxt;
        private System.Windows.Forms.CheckBox VisibleChk;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LatLbl;
        private System.Windows.Forms.Label LngLbl;
        private System.Windows.Forms.NumericUpDown CdNum;
        private System.Windows.Forms.Label CdLbl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown GCdNum;
        private System.Windows.Forms.Button MaxBtn;
    }
}