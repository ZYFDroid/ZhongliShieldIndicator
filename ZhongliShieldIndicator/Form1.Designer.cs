namespace ZhongliShieldIndicator
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.rgnProgress = new System.Windows.Forms.Label();
            this.renderTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.钟离位置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.位置1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.位置2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.位置3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.位置4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重置小部件位置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkPlaySound = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rgnProgress
            // 
            this.rgnProgress.Location = new System.Drawing.Point(57, 18);
            this.rgnProgress.Name = "rgnProgress";
            this.rgnProgress.Size = new System.Drawing.Size(208, 10);
            this.rgnProgress.TabIndex = 0;
            this.rgnProgress.Visible = false;
            // 
            // renderTimer
            // 
            this.renderTimer.Interval = 1;
            this.renderTimer.Tick += new System.EventHandler(this.renderTimer_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Text = "钟离护盾指示器";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.钟离位置ToolStripMenuItem,
            this.chkPlaySound,
            this.重置小部件位置ToolStripMenuItem,
            this.toolStripSeparator1,
            this.退出ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 120);
            // 
            // 钟离位置ToolStripMenuItem
            // 
            this.钟离位置ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.位置1ToolStripMenuItem,
            this.位置2ToolStripMenuItem,
            this.位置3ToolStripMenuItem,
            this.位置4ToolStripMenuItem});
            this.钟离位置ToolStripMenuItem.Name = "钟离位置ToolStripMenuItem";
            this.钟离位置ToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.钟离位置ToolStripMenuItem.Text = "钟离所在队伍位置";
            // 
            // 位置1ToolStripMenuItem
            // 
            this.位置1ToolStripMenuItem.Name = "位置1ToolStripMenuItem";
            this.位置1ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.位置1ToolStripMenuItem.Text = "位置1";
            this.位置1ToolStripMenuItem.Click += new System.EventHandler(this.位置1ToolStripMenuItem_Click);
            // 
            // 位置2ToolStripMenuItem
            // 
            this.位置2ToolStripMenuItem.Name = "位置2ToolStripMenuItem";
            this.位置2ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.位置2ToolStripMenuItem.Text = "位置2";
            this.位置2ToolStripMenuItem.Click += new System.EventHandler(this.位置1ToolStripMenuItem_Click);
            // 
            // 位置3ToolStripMenuItem
            // 
            this.位置3ToolStripMenuItem.Name = "位置3ToolStripMenuItem";
            this.位置3ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.位置3ToolStripMenuItem.Text = "位置3";
            this.位置3ToolStripMenuItem.Click += new System.EventHandler(this.位置1ToolStripMenuItem_Click);
            // 
            // 位置4ToolStripMenuItem
            // 
            this.位置4ToolStripMenuItem.Name = "位置4ToolStripMenuItem";
            this.位置4ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.位置4ToolStripMenuItem.Text = "位置4";
            this.位置4ToolStripMenuItem.Click += new System.EventHandler(this.位置1ToolStripMenuItem_Click);
            // 
            // 重置小部件位置ToolStripMenuItem
            // 
            this.重置小部件位置ToolStripMenuItem.Name = "重置小部件位置ToolStripMenuItem";
            this.重置小部件位置ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.重置小部件位置ToolStripMenuItem.Text = "重置小部件位置";
            this.重置小部件位置ToolStripMenuItem.Click += new System.EventHandler(this.重置小部件位置ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // chkPlaySound
            // 
            this.chkPlaySound.CheckOnClick = true;
            this.chkPlaySound.Name = "chkPlaySound";
            this.chkPlaySound.Size = new System.Drawing.Size(180, 22);
            this.chkPlaySound.Text = "播放音效";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Blue;
            this.BackgroundImage = global::ZhongliShieldIndicator.Properties.Resources.bar_bg_glow;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(278, 46);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.rgnProgress);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "ZhongliShieldIndicator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label rgnProgress;
        private System.Windows.Forms.Timer renderTimer;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 钟离位置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 位置1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 位置2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 位置3ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 位置4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重置小部件位置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chkPlaySound;
    }
}

