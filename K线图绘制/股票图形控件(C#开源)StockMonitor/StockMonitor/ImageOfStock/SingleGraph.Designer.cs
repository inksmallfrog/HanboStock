namespace ImageOfStock
{
    partial class SingleGraph
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
            this.timeLineGraph = new ImageOfStock.TimeLineGraph();
            this.detailPanel = new ImageOfStock.DetailPanel();
            this.kLinePanel = new ImageOfStock.kLinePanel();
            this.SuspendLayout();
            // 
            // timeLineGraph
            // 
            this.timeLineGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeLineGraph.BackColor = System.Drawing.Color.Black;
            this.timeLineGraph.DrawTime = true;
            this.timeLineGraph.IsDrawTitle = false;
            this.timeLineGraph.LineFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.timeLineGraph.LineWidth = 1;
            this.timeLineGraph.Location = new System.Drawing.Point(0, 0);
            this.timeLineGraph.MouseHeight = 10;
            this.timeLineGraph.MouseWidth = 10;
            this.timeLineGraph.Name = "timeLineGraph";
            this.timeLineGraph.NodeLastCharX = "";
            this.timeLineGraph.NodeLastCharY = "";
            this.timeLineGraph.PointsNumber = ((long)(240));
            this.timeLineGraph.Size = new System.Drawing.Size(724, 720);
            this.timeLineGraph.SplitColorX = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.timeLineGraph.SplitColorY = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.timeLineGraph.SplitWidthX = 0;
            this.timeLineGraph.SplitWidthY = 0;
            this.timeLineGraph.TabIndex = 2;
            this.timeLineGraph.Text = "timeLineGraph1";
            this.timeLineGraph.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.timeLineGraph.TitleHeight = 0;
            this.timeLineGraph.Visible = false;
            this.timeLineGraph.XDescription = "时间";
            this.timeLineGraph.XMaxValue = ((long)(300));
            this.timeLineGraph.XPointsNumber = ((long)(26));
            this.timeLineGraph.XValue = "时间";
            this.timeLineGraph.XYFontX = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.timeLineGraph.XYFontY = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.timeLineGraph.XYLineColorX = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.timeLineGraph.XYLineColorY = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.timeLineGraph.XYLineMaxX = 976;
            this.timeLineGraph.XYLineMaxY = 700;
            this.timeLineGraph.XYLineWidthX = 2;
            this.timeLineGraph.XYLineWidthY = 1;
            this.timeLineGraph.YDescription = "值";
            this.timeLineGraph.YMaxValue = ((long)(9000));
            this.timeLineGraph.YPriceLines = 16;
            this.timeLineGraph.YValue = "值";
            this.timeLineGraph.YVolLines = 10;
            // 
            // detailPanel
            // 
            this.detailPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.detailPanel.BackColor = System.Drawing.Color.Black;
            this.detailPanel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.detailPanel.ForeColor = System.Drawing.Color.LightGray;
            this.detailPanel.Location = new System.Drawing.Point(724, 0);
            this.detailPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.detailPanel.Name = "detailPanel";
            this.detailPanel.Size = new System.Drawing.Size(300, 720);
            this.detailPanel.TabIndex = 1;
            // 
            // kLinePanel
            // 
            this.kLinePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kLinePanel.BackColor = System.Drawing.Color.Black;
            this.kLinePanel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kLinePanel.Location = new System.Drawing.Point(0, 0);
            this.kLinePanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.kLinePanel.Name = "kLinePanel";
            this.kLinePanel.Size = new System.Drawing.Size(724, 720);
            this.kLinePanel.TabIndex = 0;
            // 
            // SingleGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.timeLineGraph);
            this.Controls.Add(this.detailPanel);
            this.Controls.Add(this.kLinePanel);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SingleGraph";
            this.Size = new System.Drawing.Size(1024, 720);
            this.ResumeLayout(false);

        }

        #endregion

        private kLinePanel kLinePanel;
        private DetailPanel detailPanel;
        private TimeLineGraph timeLineGraph;
    }
}
