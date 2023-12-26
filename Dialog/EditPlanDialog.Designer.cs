namespace WhereTrainBuild.Dialog
{
    partial class EditPlanDialog
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
            this.AliveTimePicker = new System.Windows.Forms.DateTimePicker();
            this.AliveDayNum = new System.Windows.Forms.NumericUpDown();
            this.StartDayNum = new System.Windows.Forms.NumericUpDown();
            this.StartTimePicker = new System.Windows.Forms.DateTimePicker();
            this.PassChk = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.AliveDayNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDayNum)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelBtn
            // 
            this.CancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(117, 165);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 14;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // OkBtn
            // 
            this.OkBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OkBtn.Location = new System.Drawing.Point(25, 165);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 13;
            this.OkBtn.Text = "OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "到着";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "出発";
            // 
            // AliveTimePicker
            // 
            this.AliveTimePicker.CustomFormat = "HH:mm:ss";
            this.AliveTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.AliveTimePicker.Location = new System.Drawing.Point(65, 34);
            this.AliveTimePicker.Name = "AliveTimePicker";
            this.AliveTimePicker.ShowUpDown = true;
            this.AliveTimePicker.Size = new System.Drawing.Size(74, 19);
            this.AliveTimePicker.TabIndex = 16;
            // 
            // AliveDayNum
            // 
            this.AliveDayNum.Location = new System.Drawing.Point(25, 34);
            this.AliveDayNum.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.AliveDayNum.Name = "AliveDayNum";
            this.AliveDayNum.Size = new System.Drawing.Size(34, 19);
            this.AliveDayNum.TabIndex = 17;
            // 
            // StartDayNum
            // 
            this.StartDayNum.Location = new System.Drawing.Point(25, 91);
            this.StartDayNum.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StartDayNum.Name = "StartDayNum";
            this.StartDayNum.Size = new System.Drawing.Size(34, 19);
            this.StartDayNum.TabIndex = 19;
            // 
            // StartTimePicker
            // 
            this.StartTimePicker.CustomFormat = "HH:mm:ss";
            this.StartTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartTimePicker.Location = new System.Drawing.Point(65, 91);
            this.StartTimePicker.Name = "StartTimePicker";
            this.StartTimePicker.ShowUpDown = true;
            this.StartTimePicker.Size = new System.Drawing.Size(74, 19);
            this.StartTimePicker.TabIndex = 18;
            // 
            // PassChk
            // 
            this.PassChk.AutoSize = true;
            this.PassChk.Location = new System.Drawing.Point(25, 132);
            this.PassChk.Name = "PassChk";
            this.PassChk.Size = new System.Drawing.Size(48, 16);
            this.PassChk.TabIndex = 20;
            this.PassChk.Text = "通過";
            this.PassChk.UseVisualStyleBackColor = true;
            // 
            // EditPlanDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 200);
            this.Controls.Add(this.PassChk);
            this.Controls.Add(this.StartDayNum);
            this.Controls.Add(this.StartTimePicker);
            this.Controls.Add(this.AliveDayNum);
            this.Controls.Add(this.AliveTimePicker);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditPlanDialog";
            this.Text = "計画";
            this.Load += new System.EventHandler(this.EditPlanDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AliveDayNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StartDayNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker AliveTimePicker;
        private System.Windows.Forms.NumericUpDown AliveDayNum;
        private System.Windows.Forms.NumericUpDown StartDayNum;
        private System.Windows.Forms.DateTimePicker StartTimePicker;
        private System.Windows.Forms.CheckBox PassChk;
    }
}