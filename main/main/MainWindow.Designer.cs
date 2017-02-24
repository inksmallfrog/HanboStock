using System.Drawing;

namespace StockMonitor
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.首页ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.公式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.WindowCloseButton = new System.Windows.Forms.Button();
            this.stockFreshTimer = new System.Windows.Forms.Timer(this.components);
            this.singleGraph = new ImageOfStock.SingleGraph();
            this.multiList = new ImageOfStock.MultiList();
            this.stocksList = new ImageOfStock.StocksList();
            this.stockSearch1 = new ImageOfStock.StockSearch();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Black;
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.首页ToolStripMenuItem,
            this.公式ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1024, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 首页ToolStripMenuItem
            // 
            this.首页ToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            this.首页ToolStripMenuItem.Name = "首页ToolStripMenuItem";
            this.首页ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.首页ToolStripMenuItem.Text = "首页";
            this.首页ToolStripMenuItem.Click += new System.EventHandler(this.首页ToolStripMenuItem_Click);
            // 
            // 公式ToolStripMenuItem
            // 
            this.公式ToolStripMenuItem.ForeColor = System.Drawing.Color.Red;
            this.公式ToolStripMenuItem.Name = "公式ToolStripMenuItem";
            this.公式ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.公式ToolStripMenuItem.Text = "公式";
            this.公式ToolStripMenuItem.Click += new System.EventHandler(this.公式ToolStripMenuItem_Click);
            // 
            // WindowCloseButton
            // 
            this.WindowCloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WindowCloseButton.BackColor = System.Drawing.Color.White;
            this.WindowCloseButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("WindowCloseButton.BackgroundImage")));
            this.WindowCloseButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.WindowCloseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WindowCloseButton.Location = new System.Drawing.Point(993, 0);
            this.WindowCloseButton.Name = "WindowCloseButton";
            this.WindowCloseButton.Size = new System.Drawing.Size(31, 28);
            this.WindowCloseButton.TabIndex = 2;
            this.WindowCloseButton.UseVisualStyleBackColor = false;
            this.WindowCloseButton.Click += new System.EventHandler(this.WindowCloseButton_Click);
            // 
            // stockFreshTimer
            // 
            this.stockFreshTimer.Enabled = true;
            this.stockFreshTimer.Interval = 3000;
            this.stockFreshTimer.Tick += new System.EventHandler(this.stockFreshTimer_Tick);
            // 
            // singleGraph
            // 
            this.singleGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.singleGraph.BackColor = System.Drawing.Color.Black;
            this.singleGraph.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.singleGraph.Location = new System.Drawing.Point(0, 28);
            this.singleGraph.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.singleGraph.Name = "singleGraph";
            this.singleGraph.Size = new System.Drawing.Size(1024, 720);
            this.singleGraph.TabIndex = 5;
            this.singleGraph.Visible = false;
            // 
            // multiList
            // 
            this.multiList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.multiList.BackColor = System.Drawing.Color.Black;
            this.multiList.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.multiList.ForeColor = System.Drawing.Color.White;
            this.multiList.Location = new System.Drawing.Point(0, 28);
            this.multiList.Margin = new System.Windows.Forms.Padding(0);
            this.multiList.Name = "multiList";
            this.multiList.Size = new System.Drawing.Size(1024, 729);
            this.multiList.TabIndex = 4;
            this.multiList.Visible = false;
            // 
            // stocksList
            // 
            this.stocksList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.stocksList.AutoArrange = false;
            this.stocksList.BackColor = System.Drawing.Color.Black;
            this.stocksList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.stocksList.FullRowSelect = true;
            this.stocksList.Header = null;
            this.stocksList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.stocksList.HeaderWidth = null;
            this.stocksList.HideSelection = false;
            this.stocksList.LabelWrap = false;
            this.stocksList.ListMaxSize = 27;
            this.stocksList.Location = new System.Drawing.Point(0, 28);
            this.stocksList.MultiSelect = false;
            this.stocksList.Name = "stocksList";
            this.stocksList.OwnerDraw = true;
            this.stocksList.PauseStateId = 4;
            this.stocksList.Scrollable = false;
            this.stocksList.ShowGroups = false;
            this.stocksList.Size = new System.Drawing.Size(1024, 764);
            this.stocksList.TabIndex = 3;
            this.stocksList.UseCompatibleStateImageBehavior = false;
            this.stocksList.View = System.Windows.Forms.View.Details;
            // 
            // stockSearch1
            // 
            this.stockSearch1.Location = new System.Drawing.Point(774, 457);
            this.stockSearch1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stockSearch1.Name = "stockSearch1";
            this.stockSearch1.Size = new System.Drawing.Size(250, 300);
            this.stockSearch1.TabIndex = 6;
            this.stockSearch1.Visible = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1024, 756);
            this.ControlBox = false;
            this.Controls.Add(this.stockSearch1);
            this.Controls.Add(this.singleGraph);
            this.Controls.Add(this.multiList);
            this.Controls.Add(this.stocksList);
            this.Controls.Add(this.WindowCloseButton);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "韩波炒股";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 首页ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 公式ToolStripMenuItem;
        private System.Windows.Forms.Button WindowCloseButton;
        private System.Windows.Forms.Timer stockFreshTimer;
        private ImageOfStock.StocksList stocksList;
        private ImageOfStock.MultiList multiList;
        private ImageOfStock.SingleGraph singleGraph;
        private ImageOfStock.StockSearch stockSearch1;
    }
}

