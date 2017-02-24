namespace ImageOfStock
{
    partial class kLinePanel
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(kLinePanel));
            this.kLineGraph = new ImageOfStock.KLineGraph();
            this.macd = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // kLineGraph
            // 
            this.kLineGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kLineGraph.AxisSpace = 0;
            this.kLineGraph.CanDragSeries = false;
            this.kLineGraph.CrossOverIndex = 0;
            this.kLineGraph.FirstVisibleRecord = 0;
            this.kLineGraph.LastVisibleRecord = 0;
            this.kLineGraph.LeftPixSpace = 0;
            this.kLineGraph.Location = new System.Drawing.Point(0, 0);
            this.kLineGraph.Name = "kLineGraph";
            this.kLineGraph.ProcessBarValue = 0;
            this.kLineGraph.RightPixSpace = 0;
            this.kLineGraph.ScrollLeftStep = 1;
            this.kLineGraph.ScrollRightStep = 1;
            this.kLineGraph.ShowCrossHair = false;
            this.kLineGraph.ShowLeftScale = false;
            this.kLineGraph.ShowPercent = 0;
            this.kLineGraph.ShowRightScale = false;
            this.kLineGraph.Size = new System.Drawing.Size(724, 700);
            this.kLineGraph.TabIndex = 0;
            this.kLineGraph.Text = "x`";
            this.kLineGraph.TimekeyField = null;
            this.kLineGraph.UseScrollAddSpeed = false;
            // 
            // macd
            // 
            this.macd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.macd.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.macd.ForeColor = System.Drawing.Color.Red;
            this.macd.Location = new System.Drawing.Point(0, 697);
            this.macd.Name = "macd";
            this.macd.Size = new System.Drawing.Size(75, 23);
            this.macd.TabIndex = 1;
            this.macd.Text = "MACD";
            this.macd.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.macd.UseVisualStyleBackColor = true;
            this.macd.Click += new System.EventHandler(this.macd_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(81, 697);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "KDJ";
            this.button1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // kLinePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.macd);
            this.Controls.Add(this.kLineGraph);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "kLinePanel";
            this.Size = new System.Drawing.Size(724, 720);
            this.ResumeLayout(false);

        }

        #endregion

        private KLineGraph kLineGraph;
        private System.Windows.Forms.Button macd;
        private System.Windows.Forms.Button button1;
    }
}
