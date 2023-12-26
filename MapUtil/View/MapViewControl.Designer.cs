namespace WhereTrainBuild.MapUtil.View
{
    partial class MapViewControl
    {
        /// <summary> 
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MapViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "MapViewControl";
            this.Size = new System.Drawing.Size(492, 409);
            this.MouseLeave += new System.EventHandler(this.MapViewControl_MouseLeave);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MapViewControl_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MapViewControl_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MapViewControl_MouseDown);
            this.Resize += new System.EventHandler(this.MapViewControl_Resize);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MapViewControl_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
