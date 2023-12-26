namespace WhereTrainBuild
{
    partial class MapForm
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
            this.ReqLbl = new System.Windows.Forms.Label();
            this.InfoLbl = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mapFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MapURLMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.factoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PropertyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BuildTrainMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearTrainMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ZoomBar = new System.Windows.Forms.TrackBar();
            this.ScheduleTimer = new System.Windows.Forms.Timer(this.components);
            this.MapViewCnt = new WhereTrainBuild.MapUtil.View.MapViewControl();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomBar)).BeginInit();
            this.SuspendLayout();
            // 
            // ReqLbl
            // 
            this.ReqLbl.AutoSize = true;
            this.ReqLbl.BackColor = System.Drawing.Color.Transparent;
            this.ReqLbl.Location = new System.Drawing.Point(25, 66);
            this.ReqLbl.Name = "ReqLbl";
            this.ReqLbl.Size = new System.Drawing.Size(11, 12);
            this.ReqLbl.TabIndex = 3;
            this.ReqLbl.Text = "+";
            // 
            // InfoLbl
            // 
            this.InfoLbl.AutoSize = true;
            this.InfoLbl.BackColor = System.Drawing.Color.Transparent;
            this.InfoLbl.Location = new System.Drawing.Point(25, 38);
            this.InfoLbl.Name = "InfoLbl";
            this.InfoLbl.Size = new System.Drawing.Size(19, 12);
            this.InfoLbl.TabIndex = 2;
            this.InfoLbl.Text = "0,0";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.factoryToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(616, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewMenuItem,
            this.LoadMenuItem,
            this.SaveMenuItem,
            this.toolStripMenuItem1,
            this.toolStripSeparator3,
            this.mapFolderToolStripMenuItem,
            this.MapURLMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.fileToolStripMenuItem.Text = "ファイル";
            // 
            // NewMenuItem
            // 
            this.NewMenuItem.Name = "NewMenuItem";
            this.NewMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.NewMenuItem.Size = new System.Drawing.Size(180, 22);
            this.NewMenuItem.Text = "新規";
            this.NewMenuItem.Click += new System.EventHandler(this.NewMenuItem_Click);
            // 
            // LoadMenuItem
            // 
            this.LoadMenuItem.Name = "LoadMenuItem";
            this.LoadMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.LoadMenuItem.Size = new System.Drawing.Size(180, 22);
            this.LoadMenuItem.Text = "開く";
            this.LoadMenuItem.Click += new System.EventHandler(this.Load_Click);
            // 
            // SaveMenuItem
            // 
            this.SaveMenuItem.Name = "SaveMenuItem";
            this.SaveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.SaveMenuItem.Size = new System.Drawing.Size(180, 22);
            this.SaveMenuItem.Text = "保存";
            this.SaveMenuItem.Click += new System.EventHandler(this.Save_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // mapFolderToolStripMenuItem
            // 
            this.mapFolderToolStripMenuItem.Name = "mapFolderToolStripMenuItem";
            this.mapFolderToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.mapFolderToolStripMenuItem.Text = "地図保存位置";
            this.mapFolderToolStripMenuItem.Click += new System.EventHandler(this.mapFolderToolStripMenuItem_Click);
            // 
            // MapURLMenuItem
            // 
            this.MapURLMenuItem.Name = "MapURLMenuItem";
            this.MapURLMenuItem.Size = new System.Drawing.Size(180, 22);
            this.MapURLMenuItem.Text = "地図URL";
            this.MapURLMenuItem.Click += new System.EventHandler(this.MapURLMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exitToolStripMenuItem.Text = "終了";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // factoryToolStripMenuItem
            // 
            this.factoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PropertyMenuItem,
            this.BuildTrainMenuItem,
            this.ClearTrainMenuItem});
            this.factoryToolStripMenuItem.Name = "factoryToolStripMenuItem";
            this.factoryToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.factoryToolStripMenuItem.Text = "組織";
            // 
            // PropertyMenuItem
            // 
            this.PropertyMenuItem.Name = "PropertyMenuItem";
            this.PropertyMenuItem.Size = new System.Drawing.Size(122, 22);
            this.PropertyMenuItem.Text = "プロパティ";
            this.PropertyMenuItem.Click += new System.EventHandler(this.PropertyMenuItem_Click);
            // 
            // BuildTrainMenuItem
            // 
            this.BuildTrainMenuItem.Name = "BuildTrainMenuItem";
            this.BuildTrainMenuItem.Size = new System.Drawing.Size(122, 22);
            this.BuildTrainMenuItem.Text = "電車構築";
            this.BuildTrainMenuItem.Click += new System.EventHandler(this.BuildTrainMenuItem_Click);
            // 
            // ClearTrainMenuItem
            // 
            this.ClearTrainMenuItem.Name = "ClearTrainMenuItem";
            this.ClearTrainMenuItem.Size = new System.Drawing.Size(122, 22);
            this.ClearTrainMenuItem.Text = "電車破棄";
            this.ClearTrainMenuItem.Click += new System.EventHandler(this.ClearTrainMenuItem_Click);
            // 
            // ZoomBar
            // 
            this.ZoomBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ZoomBar.Location = new System.Drawing.Point(559, 38);
            this.ZoomBar.Maximum = 1999;
            this.ZoomBar.Minimum = 1;
            this.ZoomBar.Name = "ZoomBar";
            this.ZoomBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.ZoomBar.Size = new System.Drawing.Size(45, 379);
            this.ZoomBar.TabIndex = 0;
            this.ZoomBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.ZoomBar.Value = 1000;
            this.ZoomBar.Scroll += new System.EventHandler(this.ZoomBar_Scroll);
            // 
            // ScheduleTimer
            // 
            this.ScheduleTimer.Enabled = true;
            this.ScheduleTimer.Interval = 1000;
            this.ScheduleTimer.Tick += new System.EventHandler(this.ScheduleTimer_Tick);
            // 
            // MapViewCnt
            // 
            this.MapViewCnt.ChangeZoomResetCache = true;
            this.MapViewCnt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MapViewCnt.Location = new System.Drawing.Point(0, 0);
            this.MapViewCnt.MapScale = 0;
            this.MapViewCnt.Name = "MapViewCnt";
            this.MapViewCnt.Size = new System.Drawing.Size(616, 429);
            this.MapViewCnt.TabIndex = 0;
            this.MapViewCnt.Timeout = 5000;
            this.MapViewCnt.OnMercatorDownPoint += new WhereTrainBuild.MapUtil.View.MapViewControl.OnMercatorMouseDownDelegate(this.MapViewCnt_OnMercatorDownPoint);
            this.MapViewCnt.OnMercatorMousePoint += new WhereTrainBuild.MapUtil.View.MapViewControl.OnMercatorMousePointDelegate(this.MapViewCnt_OnMercatorMousePoint);
            this.MapViewCnt.OnMercatorUpPoint += new WhereTrainBuild.MapUtil.View.MapViewControl.OnMercatorMouseUpDelegate(this.MapViewCnt_OnMercatorUpPoint);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.toolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem1.Text = "上書き保存";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // MapForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 429);
            this.Controls.Add(this.ZoomBar);
            this.Controls.Add(this.ReqLbl);
            this.Controls.Add(this.InfoLbl);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.MapViewCnt);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MapForm";
            this.Text = "WhereTrainBuild";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MapForm_FormClosed);
            this.Load += new System.EventHandler(this.MapForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MapForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MapForm_DragEnter);
            this.Resize += new System.EventHandler(this.MapForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected WhereTrainBuild.MapUtil.View.MapViewControl MapViewCnt;
        protected System.Windows.Forms.TrackBar ZoomBar;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label InfoLbl;
        private System.Windows.Forms.Label ReqLbl;
        private System.Windows.Forms.Timer ScheduleTimer;
        private System.Windows.Forms.ToolStripMenuItem factoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClearTrainMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NewMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem PropertyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem BuildTrainMenuItem;
        private System.Windows.Forms.ToolStripMenuItem MapURLMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}