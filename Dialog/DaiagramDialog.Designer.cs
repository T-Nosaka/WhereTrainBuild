namespace WhereTrainBuild.Dialog
{
    partial class DaiagramDialog
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
            this.DiaPanel = new WhereTrainBuild.Dialog.DaiagramDialog.PanelEx();
            this.SuspendLayout();
            // 
            // DiaPanel
            // 
            this.DiaPanel.Location = new System.Drawing.Point(0, 0);
            this.DiaPanel.Margin = new System.Windows.Forms.Padding(4);
            this.DiaPanel.Name = "DiaPanel";
            this.DiaPanel.Size = new System.Drawing.Size(696, 303);
            this.DiaPanel.TabIndex = 0;
            // 
            // DaiagramDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(721, 390);
            this.Controls.Add(this.DiaPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DaiagramDialog";
            this.Text = "DaiagramDialog";
            this.Load += new System.EventHandler(this.DaiagramDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private PanelEx DiaPanel;
    }
}