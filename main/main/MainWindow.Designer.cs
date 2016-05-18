using System.Drawing;

namespace main
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
            this.KLineGraph = new ImageOfStock.ChartGraph();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.SingleGraphPanel = new System.Windows.Forms.Panel();
            this.DetailPanel = new System.Windows.Forms.Panel();
            this.StockTitlePanel = new System.Windows.Forms.Panel();
            this.StockTitle = new System.Windows.Forms.Label();
            this.OtherPanel = new System.Windows.Forms.Panel();
            this.DetailInfoPanel = new System.Windows.Forms.Panel();
            this.Outter = new System.Windows.Forms.Label();
            this.OutterLabel = new System.Windows.Forms.Label();
            this.Inner = new System.Windows.Forms.Label();
            this.InnerLabel = new System.Windows.Forms.Label();
            this.VolumeRatio = new System.Windows.Forms.Label();
            this.VolumeRatioLabel = new System.Windows.Forms.Label();
            this.Volume = new System.Windows.Forms.Label();
            this.VolumeLabel = new System.Windows.Forms.Label();
            this.Low = new System.Windows.Forms.Label();
            this.LowLabel = new System.Windows.Forms.Label();
            this.Percent = new System.Windows.Forms.Label();
            this.PercentLabel = new System.Windows.Forms.Label();
            this.High = new System.Windows.Forms.Label();
            this.HighLabel = new System.Windows.Forms.Label();
            this.UpDown = new System.Windows.Forms.Label();
            this.UpDownLabel = new System.Windows.Forms.Label();
            this.Open = new System.Windows.Forms.Label();
            this.OpenLabel = new System.Windows.Forms.Label();
            this.Price = new System.Windows.Forms.Label();
            this.PriceLabel = new System.Windows.Forms.Label();
            this.BidPanel = new System.Windows.Forms.Panel();
            this.Bid5Vol = new System.Windows.Forms.Label();
            this.Bid5Price = new System.Windows.Forms.Label();
            this.Bid5Label = new System.Windows.Forms.Label();
            this.Bid4Vol = new System.Windows.Forms.Label();
            this.Bid4Price = new System.Windows.Forms.Label();
            this.Bid4Label = new System.Windows.Forms.Label();
            this.Bid3Vol = new System.Windows.Forms.Label();
            this.Bid3Price = new System.Windows.Forms.Label();
            this.Bid3Panel = new System.Windows.Forms.Label();
            this.Bid2Vol = new System.Windows.Forms.Label();
            this.Bid2Price = new System.Windows.Forms.Label();
            this.Bid2Label = new System.Windows.Forms.Label();
            this.Bid1Vol = new System.Windows.Forms.Label();
            this.Bid1Price = new System.Windows.Forms.Label();
            this.Bid1Label = new System.Windows.Forms.Label();
            this.ABDirtaPanel = new System.Windows.Forms.Panel();
            this.ABD = new System.Windows.Forms.Label();
            this.ABDLabel = new System.Windows.Forms.Label();
            this.ABS = new System.Windows.Forms.Label();
            this.ABSLabel = new System.Windows.Forms.Label();
            this.AskPanel = new System.Windows.Forms.Panel();
            this.Ask5Vol = new System.Windows.Forms.Label();
            this.Ask5Price = new System.Windows.Forms.Label();
            this.Ask5Label = new System.Windows.Forms.Label();
            this.Ask4Vol = new System.Windows.Forms.Label();
            this.Ask4Price = new System.Windows.Forms.Label();
            this.Ask4Label = new System.Windows.Forms.Label();
            this.Ask3Vol = new System.Windows.Forms.Label();
            this.Ask3Price = new System.Windows.Forms.Label();
            this.Ask3Label = new System.Windows.Forms.Label();
            this.Ask2Vol = new System.Windows.Forms.Label();
            this.Ask2Price = new System.Windows.Forms.Label();
            this.As2Label = new System.Windows.Forms.Label();
            this.Ask1Vol = new System.Windows.Forms.Label();
            this.Ask1Price = new System.Windows.Forms.Label();
            this.Ask1Label = new System.Windows.Forms.Label();
            this.perbidList = new main.DoubleBufferListView();
            this.StockList = new main.DoubleBufferListView();
            this.menuStrip1.SuspendLayout();
            this.KLineGraph.SuspendLayout();
            this.SingleGraphPanel.SuspendLayout();
            this.DetailPanel.SuspendLayout();
            this.StockTitlePanel.SuspendLayout();
            this.OtherPanel.SuspendLayout();
            this.DetailInfoPanel.SuspendLayout();
            this.BidPanel.SuspendLayout();
            this.ABDirtaPanel.SuspendLayout();
            this.AskPanel.SuspendLayout();
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
            this.WindowCloseButton.Location = new System.Drawing.Point(933, 0);
            this.WindowCloseButton.Name = "WindowCloseButton";
            this.WindowCloseButton.Size = new System.Drawing.Size(75, 23);
            this.WindowCloseButton.TabIndex = 2;
            this.WindowCloseButton.Text = "Close";
            this.WindowCloseButton.UseVisualStyleBackColor = true;
            this.WindowCloseButton.Click += new System.EventHandler(this.WindowCloseButton_Click);
            // 
            // stockFreshTimer
            // 
            this.stockFreshTimer.Interval = 10000;
            this.stockFreshTimer.Tick += new System.EventHandler(this.stockFreshTimer_Tick);
            // 
            // KLineGraph
            // 
            this.KLineGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.KLineGraph.AxisSpace = 0;
            this.KLineGraph.CanDragSeries = false;
            this.KLineGraph.Controls.Add(this.ButtonPanel);
            this.KLineGraph.CrossOverIndex = 0;
            this.KLineGraph.FirstVisibleRecord = 0;
            this.KLineGraph.LastVisibleRecord = 0;
            this.KLineGraph.LeftPixSpace = 0;
            this.KLineGraph.Location = new System.Drawing.Point(0, 0);
            this.KLineGraph.Name = "KLineGraph";
            this.KLineGraph.ProcessBarValue = 0;
            this.KLineGraph.RightPixSpace = 0;
            this.KLineGraph.ScrollLeftStep = 1;
            this.KLineGraph.ScrollRightStep = 1;
            this.KLineGraph.ShowCrossHair = false;
            this.KLineGraph.ShowLeftScale = false;
            this.KLineGraph.ShowPercent = 0;
            this.KLineGraph.ShowRightScale = false;
            this.KLineGraph.Size = new System.Drawing.Size(764, 692);
            this.KLineGraph.TabIndex = 3;
            this.KLineGraph.Text = "chartGraph1";
            this.KLineGraph.TimekeyField = null;
            this.KLineGraph.UseScrollAddSpeed = false;
            this.KLineGraph.Visible = false;
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonPanel.Location = new System.Drawing.Point(0, 692);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Size = new System.Drawing.Size(1024, 20);
            this.ButtonPanel.TabIndex = 1;
            // 
            // SingleGraphPanel
            // 
            this.SingleGraphPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SingleGraphPanel.Controls.Add(this.KLineGraph);
            this.SingleGraphPanel.Controls.Add(this.DetailPanel);
            this.SingleGraphPanel.Location = new System.Drawing.Point(0, 28);
            this.SingleGraphPanel.Name = "SingleGraphPanel";
            this.SingleGraphPanel.Size = new System.Drawing.Size(1024, 712);
            this.SingleGraphPanel.TabIndex = 4;
            this.SingleGraphPanel.Visible = false;
            // 
            // DetailPanel
            // 
            this.DetailPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DetailPanel.BackColor = System.Drawing.Color.Transparent;
            this.DetailPanel.Controls.Add(this.StockTitlePanel);
            this.DetailPanel.Controls.Add(this.OtherPanel);
            this.DetailPanel.Controls.Add(this.DetailInfoPanel);
            this.DetailPanel.Controls.Add(this.BidPanel);
            this.DetailPanel.Controls.Add(this.ABDirtaPanel);
            this.DetailPanel.Controls.Add(this.AskPanel);
            this.DetailPanel.Location = new System.Drawing.Point(764, 0);
            this.DetailPanel.Name = "DetailPanel";
            this.DetailPanel.Size = new System.Drawing.Size(260, 692);
            this.DetailPanel.TabIndex = 0;
            // 
            // StockTitlePanel
            // 
            this.StockTitlePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StockTitlePanel.Controls.Add(this.StockTitle);
            this.StockTitlePanel.Location = new System.Drawing.Point(0, 0);
            this.StockTitlePanel.Name = "StockTitlePanel";
            this.StockTitlePanel.Size = new System.Drawing.Size(260, 30);
            this.StockTitlePanel.TabIndex = 0;
            // 
            // StockTitle
            // 
            this.StockTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StockTitle.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StockTitle.ForeColor = System.Drawing.Color.Yellow;
            this.StockTitle.Location = new System.Drawing.Point(0, 0);
            this.StockTitle.Name = "StockTitle";
            this.StockTitle.Size = new System.Drawing.Size(260, 30);
            this.StockTitle.TabIndex = 0;
            this.StockTitle.Text = "label1";
            this.StockTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OtherPanel
            // 
            this.OtherPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OtherPanel.Controls.Add(this.perbidList);
            this.OtherPanel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OtherPanel.Location = new System.Drawing.Point(0, 364);
            this.OtherPanel.Name = "OtherPanel";
            this.OtherPanel.Size = new System.Drawing.Size(260, 328);
            this.OtherPanel.TabIndex = 3;
            // 
            // DetailInfoPanel
            // 
            this.DetailInfoPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DetailInfoPanel.Controls.Add(this.Outter);
            this.DetailInfoPanel.Controls.Add(this.OutterLabel);
            this.DetailInfoPanel.Controls.Add(this.Inner);
            this.DetailInfoPanel.Controls.Add(this.InnerLabel);
            this.DetailInfoPanel.Controls.Add(this.VolumeRatio);
            this.DetailInfoPanel.Controls.Add(this.VolumeRatioLabel);
            this.DetailInfoPanel.Controls.Add(this.Volume);
            this.DetailInfoPanel.Controls.Add(this.VolumeLabel);
            this.DetailInfoPanel.Controls.Add(this.Low);
            this.DetailInfoPanel.Controls.Add(this.LowLabel);
            this.DetailInfoPanel.Controls.Add(this.Percent);
            this.DetailInfoPanel.Controls.Add(this.PercentLabel);
            this.DetailInfoPanel.Controls.Add(this.High);
            this.DetailInfoPanel.Controls.Add(this.HighLabel);
            this.DetailInfoPanel.Controls.Add(this.UpDown);
            this.DetailInfoPanel.Controls.Add(this.UpDownLabel);
            this.DetailInfoPanel.Controls.Add(this.Open);
            this.DetailInfoPanel.Controls.Add(this.OpenLabel);
            this.DetailInfoPanel.Controls.Add(this.Price);
            this.DetailInfoPanel.Controls.Add(this.PriceLabel);
            this.DetailInfoPanel.Location = new System.Drawing.Point(0, 260);
            this.DetailInfoPanel.Name = "DetailInfoPanel";
            this.DetailInfoPanel.Size = new System.Drawing.Size(260, 104);
            this.DetailInfoPanel.TabIndex = 2;
            // 
            // Outter
            // 
            this.Outter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Outter.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Outter.ForeColor = System.Drawing.Color.Red;
            this.Outter.Location = new System.Drawing.Point(52, 81);
            this.Outter.Name = "Outter";
            this.Outter.Size = new System.Drawing.Size(78, 20);
            this.Outter.TabIndex = 19;
            this.Outter.Text = "label1";
            this.Outter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OutterLabel
            // 
            this.OutterLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OutterLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.OutterLabel.Location = new System.Drawing.Point(0, 81);
            this.OutterLabel.Name = "OutterLabel";
            this.OutterLabel.Size = new System.Drawing.Size(52, 20);
            this.OutterLabel.TabIndex = 18;
            this.OutterLabel.Text = "外盘";
            this.OutterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Inner
            // 
            this.Inner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Inner.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Inner.ForeColor = System.Drawing.Color.LawnGreen;
            this.Inner.Location = new System.Drawing.Point(182, 81);
            this.Inner.Name = "Inner";
            this.Inner.Size = new System.Drawing.Size(78, 20);
            this.Inner.TabIndex = 17;
            this.Inner.Text = "label1";
            this.Inner.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // InnerLabel
            // 
            this.InnerLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InnerLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.InnerLabel.Location = new System.Drawing.Point(130, 81);
            this.InnerLabel.Name = "InnerLabel";
            this.InnerLabel.Size = new System.Drawing.Size(52, 20);
            this.InnerLabel.TabIndex = 16;
            this.InnerLabel.Text = "内盘";
            this.InnerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VolumeRatio
            // 
            this.VolumeRatio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.VolumeRatio.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.VolumeRatio.Location = new System.Drawing.Point(182, 61);
            this.VolumeRatio.Name = "VolumeRatio";
            this.VolumeRatio.Size = new System.Drawing.Size(78, 20);
            this.VolumeRatio.TabIndex = 15;
            this.VolumeRatio.Text = "label1";
            this.VolumeRatio.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // VolumeRatioLabel
            // 
            this.VolumeRatioLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.VolumeRatioLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.VolumeRatioLabel.Location = new System.Drawing.Point(130, 61);
            this.VolumeRatioLabel.Name = "VolumeRatioLabel";
            this.VolumeRatioLabel.Size = new System.Drawing.Size(52, 20);
            this.VolumeRatioLabel.TabIndex = 14;
            this.VolumeRatioLabel.Text = "量比";
            this.VolumeRatioLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Volume
            // 
            this.Volume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Volume.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Volume.ForeColor = System.Drawing.Color.Yellow;
            this.Volume.Location = new System.Drawing.Point(52, 61);
            this.Volume.Name = "Volume";
            this.Volume.Size = new System.Drawing.Size(78, 20);
            this.Volume.TabIndex = 13;
            this.Volume.Text = "label1";
            this.Volume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // VolumeLabel
            // 
            this.VolumeLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.VolumeLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.VolumeLabel.Location = new System.Drawing.Point(0, 61);
            this.VolumeLabel.Name = "VolumeLabel";
            this.VolumeLabel.Size = new System.Drawing.Size(52, 20);
            this.VolumeLabel.TabIndex = 12;
            this.VolumeLabel.Text = "总量";
            this.VolumeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Low
            // 
            this.Low.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Low.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Low.Location = new System.Drawing.Point(182, 41);
            this.Low.Name = "Low";
            this.Low.Size = new System.Drawing.Size(78, 20);
            this.Low.TabIndex = 11;
            this.Low.Text = "label1";
            this.Low.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LowLabel
            // 
            this.LowLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LowLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.LowLabel.Location = new System.Drawing.Point(130, 41);
            this.LowLabel.Name = "LowLabel";
            this.LowLabel.Size = new System.Drawing.Size(52, 20);
            this.LowLabel.TabIndex = 10;
            this.LowLabel.Text = "最低";
            this.LowLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Percent
            // 
            this.Percent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Percent.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Percent.Location = new System.Drawing.Point(52, 41);
            this.Percent.Name = "Percent";
            this.Percent.Size = new System.Drawing.Size(78, 20);
            this.Percent.TabIndex = 9;
            this.Percent.Text = "label1";
            this.Percent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PercentLabel
            // 
            this.PercentLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PercentLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.PercentLabel.Location = new System.Drawing.Point(0, 41);
            this.PercentLabel.Name = "PercentLabel";
            this.PercentLabel.Size = new System.Drawing.Size(52, 20);
            this.PercentLabel.TabIndex = 8;
            this.PercentLabel.Text = "涨幅";
            this.PercentLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // High
            // 
            this.High.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.High.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.High.Location = new System.Drawing.Point(182, 21);
            this.High.Name = "High";
            this.High.Size = new System.Drawing.Size(78, 20);
            this.High.TabIndex = 7;
            this.High.Text = "label1";
            this.High.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // HighLabel
            // 
            this.HighLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HighLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.HighLabel.Location = new System.Drawing.Point(130, 21);
            this.HighLabel.Name = "HighLabel";
            this.HighLabel.Size = new System.Drawing.Size(52, 20);
            this.HighLabel.TabIndex = 6;
            this.HighLabel.Text = "最高";
            this.HighLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // UpDown
            // 
            this.UpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UpDown.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UpDown.Location = new System.Drawing.Point(52, 21);
            this.UpDown.Name = "UpDown";
            this.UpDown.Size = new System.Drawing.Size(78, 20);
            this.UpDown.TabIndex = 5;
            this.UpDown.Text = "label1";
            this.UpDown.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UpDownLabel
            // 
            this.UpDownLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UpDownLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.UpDownLabel.Location = new System.Drawing.Point(0, 21);
            this.UpDownLabel.Name = "UpDownLabel";
            this.UpDownLabel.Size = new System.Drawing.Size(52, 20);
            this.UpDownLabel.TabIndex = 4;
            this.UpDownLabel.Text = "涨跌";
            this.UpDownLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Open
            // 
            this.Open.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Open.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Open.Location = new System.Drawing.Point(182, 1);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(78, 20);
            this.Open.TabIndex = 3;
            this.Open.Text = "label1";
            this.Open.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // OpenLabel
            // 
            this.OpenLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpenLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.OpenLabel.Location = new System.Drawing.Point(130, 1);
            this.OpenLabel.Name = "OpenLabel";
            this.OpenLabel.Size = new System.Drawing.Size(52, 20);
            this.OpenLabel.TabIndex = 2;
            this.OpenLabel.Text = "今开";
            this.OpenLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Price
            // 
            this.Price.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Price.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Price.Location = new System.Drawing.Point(52, 1);
            this.Price.Name = "Price";
            this.Price.Size = new System.Drawing.Size(78, 20);
            this.Price.TabIndex = 1;
            this.Price.Text = "label1";
            this.Price.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PriceLabel
            // 
            this.PriceLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.PriceLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.PriceLabel.Location = new System.Drawing.Point(0, 1);
            this.PriceLabel.Name = "PriceLabel";
            this.PriceLabel.Size = new System.Drawing.Size(52, 20);
            this.PriceLabel.TabIndex = 0;
            this.PriceLabel.Text = "现价";
            this.PriceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BidPanel
            // 
            this.BidPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BidPanel.Controls.Add(this.Bid5Vol);
            this.BidPanel.Controls.Add(this.Bid5Price);
            this.BidPanel.Controls.Add(this.Bid5Label);
            this.BidPanel.Controls.Add(this.Bid4Vol);
            this.BidPanel.Controls.Add(this.Bid4Price);
            this.BidPanel.Controls.Add(this.Bid4Label);
            this.BidPanel.Controls.Add(this.Bid3Vol);
            this.BidPanel.Controls.Add(this.Bid3Price);
            this.BidPanel.Controls.Add(this.Bid3Panel);
            this.BidPanel.Controls.Add(this.Bid2Vol);
            this.BidPanel.Controls.Add(this.Bid2Price);
            this.BidPanel.Controls.Add(this.Bid2Label);
            this.BidPanel.Controls.Add(this.Bid1Vol);
            this.BidPanel.Controls.Add(this.Bid1Price);
            this.BidPanel.Controls.Add(this.Bid1Label);
            this.BidPanel.Location = new System.Drawing.Point(0, 156);
            this.BidPanel.Name = "BidPanel";
            this.BidPanel.Size = new System.Drawing.Size(260, 104);
            this.BidPanel.TabIndex = 1;
            // 
            // Bid5Vol
            // 
            this.Bid5Vol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Bid5Vol.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid5Vol.ForeColor = System.Drawing.Color.Yellow;
            this.Bid5Vol.Location = new System.Drawing.Point(130, 81);
            this.Bid5Vol.Name = "Bid5Vol";
            this.Bid5Vol.Size = new System.Drawing.Size(130, 20);
            this.Bid5Vol.TabIndex = 14;
            this.Bid5Vol.Text = "label1";
            this.Bid5Vol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Bid5Price
            // 
            this.Bid5Price.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid5Price.Location = new System.Drawing.Point(52, 81);
            this.Bid5Price.Name = "Bid5Price";
            this.Bid5Price.Size = new System.Drawing.Size(78, 20);
            this.Bid5Price.TabIndex = 13;
            this.Bid5Price.Text = "label1";
            this.Bid5Price.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Bid5Label
            // 
            this.Bid5Label.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid5Label.ForeColor = System.Drawing.Color.Gainsboro;
            this.Bid5Label.Location = new System.Drawing.Point(0, 81);
            this.Bid5Label.Name = "Bid5Label";
            this.Bid5Label.Size = new System.Drawing.Size(52, 20);
            this.Bid5Label.TabIndex = 12;
            this.Bid5Label.Text = "买五";
            this.Bid5Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Bid4Vol
            // 
            this.Bid4Vol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Bid4Vol.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid4Vol.ForeColor = System.Drawing.Color.Yellow;
            this.Bid4Vol.Location = new System.Drawing.Point(130, 61);
            this.Bid4Vol.Name = "Bid4Vol";
            this.Bid4Vol.Size = new System.Drawing.Size(130, 20);
            this.Bid4Vol.TabIndex = 11;
            this.Bid4Vol.Text = "label1";
            this.Bid4Vol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Bid4Price
            // 
            this.Bid4Price.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid4Price.Location = new System.Drawing.Point(52, 61);
            this.Bid4Price.Name = "Bid4Price";
            this.Bid4Price.Size = new System.Drawing.Size(78, 20);
            this.Bid4Price.TabIndex = 10;
            this.Bid4Price.Text = "label1";
            this.Bid4Price.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Bid4Label
            // 
            this.Bid4Label.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid4Label.ForeColor = System.Drawing.Color.Gainsboro;
            this.Bid4Label.Location = new System.Drawing.Point(0, 61);
            this.Bid4Label.Name = "Bid4Label";
            this.Bid4Label.Size = new System.Drawing.Size(52, 20);
            this.Bid4Label.TabIndex = 9;
            this.Bid4Label.Text = "买四";
            this.Bid4Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Bid3Vol
            // 
            this.Bid3Vol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Bid3Vol.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid3Vol.ForeColor = System.Drawing.Color.Yellow;
            this.Bid3Vol.Location = new System.Drawing.Point(130, 41);
            this.Bid3Vol.Name = "Bid3Vol";
            this.Bid3Vol.Size = new System.Drawing.Size(130, 20);
            this.Bid3Vol.TabIndex = 8;
            this.Bid3Vol.Text = "label1";
            this.Bid3Vol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Bid3Price
            // 
            this.Bid3Price.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid3Price.Location = new System.Drawing.Point(52, 41);
            this.Bid3Price.Name = "Bid3Price";
            this.Bid3Price.Size = new System.Drawing.Size(78, 20);
            this.Bid3Price.TabIndex = 7;
            this.Bid3Price.Text = "label1";
            this.Bid3Price.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Bid3Panel
            // 
            this.Bid3Panel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid3Panel.ForeColor = System.Drawing.Color.Gainsboro;
            this.Bid3Panel.Location = new System.Drawing.Point(0, 41);
            this.Bid3Panel.Name = "Bid3Panel";
            this.Bid3Panel.Size = new System.Drawing.Size(52, 20);
            this.Bid3Panel.TabIndex = 6;
            this.Bid3Panel.Text = "买三";
            this.Bid3Panel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Bid2Vol
            // 
            this.Bid2Vol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Bid2Vol.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid2Vol.ForeColor = System.Drawing.Color.Yellow;
            this.Bid2Vol.Location = new System.Drawing.Point(130, 21);
            this.Bid2Vol.Name = "Bid2Vol";
            this.Bid2Vol.Size = new System.Drawing.Size(130, 20);
            this.Bid2Vol.TabIndex = 5;
            this.Bid2Vol.Text = "label1";
            this.Bid2Vol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Bid2Price
            // 
            this.Bid2Price.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid2Price.Location = new System.Drawing.Point(52, 21);
            this.Bid2Price.Name = "Bid2Price";
            this.Bid2Price.Size = new System.Drawing.Size(78, 20);
            this.Bid2Price.TabIndex = 4;
            this.Bid2Price.Text = "label1";
            this.Bid2Price.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Bid2Label
            // 
            this.Bid2Label.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid2Label.ForeColor = System.Drawing.Color.Gainsboro;
            this.Bid2Label.Location = new System.Drawing.Point(0, 21);
            this.Bid2Label.Name = "Bid2Label";
            this.Bid2Label.Size = new System.Drawing.Size(52, 20);
            this.Bid2Label.TabIndex = 3;
            this.Bid2Label.Text = "买二";
            this.Bid2Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Bid1Vol
            // 
            this.Bid1Vol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Bid1Vol.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid1Vol.ForeColor = System.Drawing.Color.Yellow;
            this.Bid1Vol.Location = new System.Drawing.Point(130, 1);
            this.Bid1Vol.Name = "Bid1Vol";
            this.Bid1Vol.Size = new System.Drawing.Size(130, 20);
            this.Bid1Vol.TabIndex = 2;
            this.Bid1Vol.Text = "label1";
            this.Bid1Vol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Bid1Price
            // 
            this.Bid1Price.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid1Price.Location = new System.Drawing.Point(52, 1);
            this.Bid1Price.Name = "Bid1Price";
            this.Bid1Price.Size = new System.Drawing.Size(78, 20);
            this.Bid1Price.TabIndex = 1;
            this.Bid1Price.Text = "label1";
            this.Bid1Price.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Bid1Label
            // 
            this.Bid1Label.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Bid1Label.ForeColor = System.Drawing.Color.Gainsboro;
            this.Bid1Label.Location = new System.Drawing.Point(0, 1);
            this.Bid1Label.Name = "Bid1Label";
            this.Bid1Label.Size = new System.Drawing.Size(52, 20);
            this.Bid1Label.TabIndex = 0;
            this.Bid1Label.Text = "买一";
            this.Bid1Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ABDirtaPanel
            // 
            this.ABDirtaPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ABDirtaPanel.Controls.Add(this.ABD);
            this.ABDirtaPanel.Controls.Add(this.ABDLabel);
            this.ABDirtaPanel.Controls.Add(this.ABS);
            this.ABDirtaPanel.Controls.Add(this.ABSLabel);
            this.ABDirtaPanel.Location = new System.Drawing.Point(0, 30);
            this.ABDirtaPanel.Name = "ABDirtaPanel";
            this.ABDirtaPanel.Size = new System.Drawing.Size(260, 22);
            this.ABDirtaPanel.TabIndex = 0;
            // 
            // ABD
            // 
            this.ABD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ABD.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ABD.Location = new System.Drawing.Point(180, 1);
            this.ABD.Name = "ABD";
            this.ABD.Size = new System.Drawing.Size(80, 18);
            this.ABD.TabIndex = 3;
            this.ABD.Text = "label1";
            this.ABD.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ABDLabel
            // 
            this.ABDLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ABDLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ABDLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.ABDLabel.Location = new System.Drawing.Point(130, 1);
            this.ABDLabel.Name = "ABDLabel";
            this.ABDLabel.Size = new System.Drawing.Size(50, 18);
            this.ABDLabel.TabIndex = 2;
            this.ABDLabel.Text = "委差";
            this.ABDLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ABS
            // 
            this.ABS.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ABS.Location = new System.Drawing.Point(50, 1);
            this.ABS.Name = "ABS";
            this.ABS.Size = new System.Drawing.Size(80, 18);
            this.ABS.TabIndex = 1;
            this.ABS.Text = "label1";
            this.ABS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ABSLabel
            // 
            this.ABSLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ABSLabel.ForeColor = System.Drawing.Color.Gainsboro;
            this.ABSLabel.Location = new System.Drawing.Point(0, 1);
            this.ABSLabel.Name = "ABSLabel";
            this.ABSLabel.Size = new System.Drawing.Size(50, 18);
            this.ABSLabel.TabIndex = 0;
            this.ABSLabel.Text = "委比";
            this.ABSLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AskPanel
            // 
            this.AskPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AskPanel.Controls.Add(this.Ask5Vol);
            this.AskPanel.Controls.Add(this.Ask5Price);
            this.AskPanel.Controls.Add(this.Ask5Label);
            this.AskPanel.Controls.Add(this.Ask4Vol);
            this.AskPanel.Controls.Add(this.Ask4Price);
            this.AskPanel.Controls.Add(this.Ask4Label);
            this.AskPanel.Controls.Add(this.Ask3Vol);
            this.AskPanel.Controls.Add(this.Ask3Price);
            this.AskPanel.Controls.Add(this.Ask3Label);
            this.AskPanel.Controls.Add(this.Ask2Vol);
            this.AskPanel.Controls.Add(this.Ask2Price);
            this.AskPanel.Controls.Add(this.As2Label);
            this.AskPanel.Controls.Add(this.Ask1Vol);
            this.AskPanel.Controls.Add(this.Ask1Price);
            this.AskPanel.Controls.Add(this.Ask1Label);
            this.AskPanel.Location = new System.Drawing.Point(0, 52);
            this.AskPanel.Name = "AskPanel";
            this.AskPanel.Size = new System.Drawing.Size(260, 104);
            this.AskPanel.TabIndex = 0;
            // 
            // Ask5Vol
            // 
            this.Ask5Vol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Ask5Vol.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask5Vol.ForeColor = System.Drawing.Color.Yellow;
            this.Ask5Vol.Location = new System.Drawing.Point(130, 1);
            this.Ask5Vol.Name = "Ask5Vol";
            this.Ask5Vol.Size = new System.Drawing.Size(130, 20);
            this.Ask5Vol.TabIndex = 14;
            this.Ask5Vol.Text = "label1";
            this.Ask5Vol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Ask5Price
            // 
            this.Ask5Price.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask5Price.Location = new System.Drawing.Point(52, 1);
            this.Ask5Price.Name = "Ask5Price";
            this.Ask5Price.Size = new System.Drawing.Size(78, 20);
            this.Ask5Price.TabIndex = 13;
            this.Ask5Price.Text = "label1";
            this.Ask5Price.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Ask5Label
            // 
            this.Ask5Label.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask5Label.ForeColor = System.Drawing.Color.Gainsboro;
            this.Ask5Label.Location = new System.Drawing.Point(0, 1);
            this.Ask5Label.Name = "Ask5Label";
            this.Ask5Label.Size = new System.Drawing.Size(52, 20);
            this.Ask5Label.TabIndex = 12;
            this.Ask5Label.Text = "卖五";
            this.Ask5Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Ask4Vol
            // 
            this.Ask4Vol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Ask4Vol.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask4Vol.ForeColor = System.Drawing.Color.Yellow;
            this.Ask4Vol.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Ask4Vol.Location = new System.Drawing.Point(130, 21);
            this.Ask4Vol.Name = "Ask4Vol";
            this.Ask4Vol.Size = new System.Drawing.Size(130, 20);
            this.Ask4Vol.TabIndex = 11;
            this.Ask4Vol.Text = "label1";
            this.Ask4Vol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Ask4Price
            // 
            this.Ask4Price.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask4Price.Location = new System.Drawing.Point(52, 21);
            this.Ask4Price.Name = "Ask4Price";
            this.Ask4Price.Size = new System.Drawing.Size(78, 20);
            this.Ask4Price.TabIndex = 10;
            this.Ask4Price.Text = "label1";
            this.Ask4Price.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Ask4Label
            // 
            this.Ask4Label.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask4Label.ForeColor = System.Drawing.Color.Gainsboro;
            this.Ask4Label.Location = new System.Drawing.Point(0, 21);
            this.Ask4Label.Name = "Ask4Label";
            this.Ask4Label.Size = new System.Drawing.Size(52, 20);
            this.Ask4Label.TabIndex = 9;
            this.Ask4Label.Text = "卖四";
            this.Ask4Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Ask3Vol
            // 
            this.Ask3Vol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Ask3Vol.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask3Vol.ForeColor = System.Drawing.Color.Yellow;
            this.Ask3Vol.Location = new System.Drawing.Point(130, 41);
            this.Ask3Vol.Name = "Ask3Vol";
            this.Ask3Vol.Size = new System.Drawing.Size(130, 20);
            this.Ask3Vol.TabIndex = 8;
            this.Ask3Vol.Text = "label1";
            this.Ask3Vol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Ask3Price
            // 
            this.Ask3Price.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask3Price.Location = new System.Drawing.Point(52, 41);
            this.Ask3Price.Name = "Ask3Price";
            this.Ask3Price.Size = new System.Drawing.Size(78, 20);
            this.Ask3Price.TabIndex = 7;
            this.Ask3Price.Text = "label1";
            this.Ask3Price.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Ask3Label
            // 
            this.Ask3Label.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask3Label.ForeColor = System.Drawing.Color.Gainsboro;
            this.Ask3Label.Location = new System.Drawing.Point(0, 41);
            this.Ask3Label.Name = "Ask3Label";
            this.Ask3Label.Size = new System.Drawing.Size(52, 20);
            this.Ask3Label.TabIndex = 6;
            this.Ask3Label.Text = "卖三";
            this.Ask3Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Ask2Vol
            // 
            this.Ask2Vol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Ask2Vol.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask2Vol.ForeColor = System.Drawing.Color.Yellow;
            this.Ask2Vol.Location = new System.Drawing.Point(130, 61);
            this.Ask2Vol.Name = "Ask2Vol";
            this.Ask2Vol.Size = new System.Drawing.Size(130, 20);
            this.Ask2Vol.TabIndex = 5;
            this.Ask2Vol.Text = "label1";
            this.Ask2Vol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Ask2Price
            // 
            this.Ask2Price.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask2Price.Location = new System.Drawing.Point(52, 61);
            this.Ask2Price.Name = "Ask2Price";
            this.Ask2Price.Size = new System.Drawing.Size(78, 20);
            this.Ask2Price.TabIndex = 4;
            this.Ask2Price.Text = "label1";
            this.Ask2Price.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // As2Label
            // 
            this.As2Label.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.As2Label.ForeColor = System.Drawing.Color.Gainsboro;
            this.As2Label.Location = new System.Drawing.Point(0, 61);
            this.As2Label.Name = "As2Label";
            this.As2Label.Size = new System.Drawing.Size(52, 20);
            this.As2Label.TabIndex = 3;
            this.As2Label.Text = "卖二";
            this.As2Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Ask1Vol
            // 
            this.Ask1Vol.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Ask1Vol.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask1Vol.ForeColor = System.Drawing.Color.Yellow;
            this.Ask1Vol.Location = new System.Drawing.Point(130, 81);
            this.Ask1Vol.Name = "Ask1Vol";
            this.Ask1Vol.Size = new System.Drawing.Size(130, 20);
            this.Ask1Vol.TabIndex = 2;
            this.Ask1Vol.Text = "label1";
            this.Ask1Vol.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Ask1Price
            // 
            this.Ask1Price.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask1Price.Location = new System.Drawing.Point(52, 81);
            this.Ask1Price.Name = "Ask1Price";
            this.Ask1Price.Size = new System.Drawing.Size(78, 20);
            this.Ask1Price.TabIndex = 1;
            this.Ask1Price.Text = "label2";
            this.Ask1Price.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Ask1Label
            // 
            this.Ask1Label.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Ask1Label.ForeColor = System.Drawing.Color.Gainsboro;
            this.Ask1Label.Location = new System.Drawing.Point(0, 81);
            this.Ask1Label.Name = "Ask1Label";
            this.Ask1Label.Size = new System.Drawing.Size(52, 20);
            this.Ask1Label.TabIndex = 0;
            this.Ask1Label.Text = "卖一";
            this.Ask1Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // perbidList
            // 
            this.perbidList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.perbidList.BackColor = System.Drawing.Color.Black;
            this.perbidList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.perbidList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.perbidList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.perbidList.LabelWrap = false;
            this.perbidList.Location = new System.Drawing.Point(1, 1);
            this.perbidList.MultiSelect = false;
            this.perbidList.Name = "perbidList";
            this.perbidList.Scrollable = false;
            this.perbidList.Size = new System.Drawing.Size(259, 326);
            this.perbidList.TabIndex = 0;
            this.perbidList.UseCompatibleStateImageBehavior = false;
            this.perbidList.View = System.Windows.Forms.View.Details;
            // 
            // StockList
            // 
            this.StockList.AllowColumnReorder = true;
            this.StockList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StockList.AutoArrange = false;
            this.StockList.BackColor = System.Drawing.Color.Black;
            this.StockList.BackgroundImageTiled = true;
            this.StockList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.StockList.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.StockList.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.StockList.FullRowSelect = true;
            this.StockList.HideSelection = false;
            this.StockList.LabelWrap = false;
            this.StockList.Location = new System.Drawing.Point(0, 28);
            this.StockList.MultiSelect = false;
            this.StockList.Name = "StockList";
            this.StockList.OwnerDraw = true;
            this.StockList.Scrollable = false;
            this.StockList.ShowGroups = false;
            this.StockList.Size = new System.Drawing.Size(1024, 740);
            this.StockList.TabIndex = 0;
            this.StockList.UseCompatibleStateImageBehavior = false;
            this.StockList.View = System.Windows.Forms.View.Details;
            this.StockList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.show_List_MouseDoubleClick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.ControlBox = false;
            this.Controls.Add(this.SingleGraphPanel);
            this.Controls.Add(this.WindowCloseButton);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.StockList);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "韩波炒股";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.KLineGraph.ResumeLayout(false);
            this.SingleGraphPanel.ResumeLayout(false);
            this.DetailPanel.ResumeLayout(false);
            this.StockTitlePanel.ResumeLayout(false);
            this.OtherPanel.ResumeLayout(false);
            this.DetailInfoPanel.ResumeLayout(false);
            this.BidPanel.ResumeLayout(false);
            this.ABDirtaPanel.ResumeLayout(false);
            this.AskPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DoubleBufferListView StockList;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 首页ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 公式ToolStripMenuItem;
        private System.Windows.Forms.Button WindowCloseButton;
        private System.Windows.Forms.Timer stockFreshTimer;
        private ImageOfStock.ChartGraph KLineGraph;
        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.Panel SingleGraphPanel;
        private System.Windows.Forms.Panel DetailPanel;
        private System.Windows.Forms.Panel OtherPanel;
        private System.Windows.Forms.Panel DetailInfoPanel;
        private System.Windows.Forms.Panel BidPanel;
        private System.Windows.Forms.Panel ABDirtaPanel;
        private System.Windows.Forms.Label ABD;
        private System.Windows.Forms.Label ABDLabel;
        private System.Windows.Forms.Label ABS;
        private System.Windows.Forms.Label ABSLabel;
        private System.Windows.Forms.Panel AskPanel;
        private System.Windows.Forms.Label Ask1Price;
        private System.Windows.Forms.Label Ask1Label;
        private System.Windows.Forms.Panel StockTitlePanel;
        private System.Windows.Forms.Label StockTitle;
        private System.Windows.Forms.Label Bid3Panel;
        private System.Windows.Forms.Label Bid2Vol;
        private System.Windows.Forms.Label Bid2Price;
        private System.Windows.Forms.Label Bid2Label;
        private System.Windows.Forms.Label Bid1Vol;
        private System.Windows.Forms.Label Bid1Price;
        private System.Windows.Forms.Label Bid1Label;
        private System.Windows.Forms.Label Ask5Vol;
        private System.Windows.Forms.Label Ask5Price;
        private System.Windows.Forms.Label Ask5Label;
        private System.Windows.Forms.Label Ask4Vol;
        private System.Windows.Forms.Label Ask4Price;
        private System.Windows.Forms.Label Ask4Label;
        private System.Windows.Forms.Label Ask3Vol;
        private System.Windows.Forms.Label Ask3Price;
        private System.Windows.Forms.Label Ask3Label;
        private System.Windows.Forms.Label Ask2Vol;
        private System.Windows.Forms.Label Ask2Price;
        private System.Windows.Forms.Label As2Label;
        private System.Windows.Forms.Label Ask1Vol;
        private System.Windows.Forms.Label LowLabel;
        private System.Windows.Forms.Label Percent;
        private System.Windows.Forms.Label PercentLabel;
        private System.Windows.Forms.Label High;
        private System.Windows.Forms.Label HighLabel;
        private System.Windows.Forms.Label UpDown;
        private System.Windows.Forms.Label UpDownLabel;
        private System.Windows.Forms.Label Open;
        private System.Windows.Forms.Label OpenLabel;
        private System.Windows.Forms.Label Price;
        private System.Windows.Forms.Label PriceLabel;
        private System.Windows.Forms.Label Bid5Vol;
        private System.Windows.Forms.Label Bid5Price;
        private System.Windows.Forms.Label Bid5Label;
        private System.Windows.Forms.Label Bid4Vol;
        private System.Windows.Forms.Label Bid4Price;
        private System.Windows.Forms.Label Bid4Label;
        private System.Windows.Forms.Label Bid3Vol;
        private System.Windows.Forms.Label Bid3Price;
        private System.Windows.Forms.Label VolumeLabel;
        private System.Windows.Forms.Label Low;
        private System.Windows.Forms.Label VolumeRatioLabel;
        private System.Windows.Forms.Label Volume;
        private System.Windows.Forms.Label Outter;
        private System.Windows.Forms.Label OutterLabel;
        private System.Windows.Forms.Label Inner;
        private System.Windows.Forms.Label InnerLabel;
        private System.Windows.Forms.Label VolumeRatio;
        private DoubleBufferListView perbidList;
    }
}

