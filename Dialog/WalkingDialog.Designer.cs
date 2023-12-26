namespace WhereTrainBuild.Dialog
{
    partial class WalkingDialog
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
            this.StopBtn = new System.Windows.Forms.Button();
            this.CountLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // StopBtn
            // 
            this.StopBtn.Location = new System.Drawing.Point(29, 56);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(102, 27);
            this.StopBtn.TabIndex = 0;
            this.StopBtn.Text = "停止";
            this.StopBtn.UseVisualStyleBackColor = true;
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // CountLbl
            // 
            this.CountLbl.AutoSize = true;
            this.CountLbl.Location = new System.Drawing.Point(22, 17);
            this.CountLbl.Name = "CountLbl";
            this.CountLbl.Size = new System.Drawing.Size(11, 12);
            this.CountLbl.TabIndex = 1;
            this.CountLbl.Text = "0";
            // 
            // WalkingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(159, 95);
            this.Controls.Add(this.CountLbl);
            this.Controls.Add(this.StopBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "WalkingDialog";
            this.Text = "線路解析";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WalkingDialog_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StopBtn;
        public System.Windows.Forms.Label CountLbl;
    }
}