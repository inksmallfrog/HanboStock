namespace ImageOfStock
{
    partial class SimpleTimeLineGraph
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
            this.updown = new System.Windows.Forms.Label();
            this.price = new System.Windows.Forms.Label();
            this.name = new System.Windows.Forms.Label();
            this.timeLineGraph.SuspendLayout();
            this.SuspendLayout();
            // 
            // timeLineGraph
            // 
            this.timeLineGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeLineGraph.BackColor = System.Drawing.Color.Black;
            this.timeLineGraph.Controls.Add(this.updown);
            this.timeLineGraph.Controls.Add(this.price);
            this.timeLineGraph.Controls.Add(this.name);
            this.timeLineGraph.DrawTime = false;
            this.timeLineGraph.IsDrawTitle = true;
            this.timeLineGraph.LineFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.timeLineGraph.LineWidth = 1;
            this.timeLineGraph.Location = new System.Drawing.Point(1, 0);
            this.timeLineGraph.Margin = new System.Windows.Forms.Padding(0);
            this.timeLineGraph.MouseHeight = 10;
            this.timeLineGraph.MouseWidth = 10;
            this.timeLineGraph.Name = "timeLineGraph";
            this.timeLineGraph.NodeLastCharX = "";
            this.timeLineGraph.NodeLastCharY = "";
            this.timeLineGraph.PointsNumber = ((long)(240));
            this.timeLineGraph.Size = new System.Drawing.Size(340, 252);
            this.timeLineGraph.SplitColorX = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.timeLineGraph.SplitColorY = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.timeLineGraph.SplitWidthX = 0;
            this.timeLineGraph.SplitWidthY = 0;
            this.timeLineGraph.TabIndex = 0;
            this.timeLineGraph.Text = "timeLineGraph1";
            this.timeLineGraph.TitleHeight = 30;
            this.timeLineGraph.XDescription = "时间";
            this.timeLineGraph.XMaxValue = ((long)(300));
            this.timeLineGraph.XPointsNumber = ((long)(12));
            this.timeLineGraph.XValue = "时间";
            this.timeLineGraph.XYFontX = new System.Drawing.Font("Microsoft YaHei Mono", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLineGraph.XYFontY = new System.Drawing.Font("Microsoft YaHei Mono", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeLineGraph.XYLineColorX = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.timeLineGraph.XYLineColorY = System.Drawing.Color.FromArgb(((int)(((byte)(176)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.timeLineGraph.XYLineMaxX = 250;
            this.timeLineGraph.XYLineMaxY = 222;
            this.timeLineGraph.XYLineWidthX = 2;
            this.timeLineGraph.XYLineWidthY = 1;
            this.timeLineGraph.YDescription = "值";
            this.timeLineGraph.YMaxValue = ((long)(9000));
            this.timeLineGraph.YPriceLines = 8;
            this.timeLineGraph.YValue = "值";
            this.timeLineGraph.YVolLines = 4;
            // 
            // updown
            // 
            this.updown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.updown.AutoSize = true;
            this.updown.Location = new System.Drawing.Point(22945, 0);
            this.updown.Margin = new System.Windows.Forms.Padding(0);
            this.updown.Name = "updown";
            this.updown.Size = new System.Drawing.Size(50, 20);
            this.updown.TabIndex = 2;
            this.updown.Text = "label2";
            // 
            // price
            // 
            this.price.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.price.AutoSize = true;
            this.price.Location = new System.Drawing.Point(11077, 0);
            this.price.Margin = new System.Windows.Forms.Padding(0);
            this.price.Name = "price";
            this.price.Size = new System.Drawing.Size(50, 20);
            this.price.TabIndex = 1;
            this.price.Text = "label1";
            // 
            // name
            // 
            this.name.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.name.AutoSize = true;
            this.name.BackColor = System.Drawing.Color.Transparent;
            this.name.Location = new System.Drawing.Point(180, 11728);
            this.name.Margin = new System.Windows.Forms.Padding(0);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(50, 20);
            this.name.TabIndex = 0;
            this.name.Text = "label1";
            // 
            // SimpleTimeLineGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.timeLineGraph);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = System.Drawing.Color.White;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SimpleTimeLineGraph";
            this.Size = new System.Drawing.Size(340, 250);
            this.timeLineGraph.ResumeLayout(false);
            this.timeLineGraph.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label name;
        private System.Windows.Forms.Label price;
        private System.Windows.Forms.Label updown;
        private TimeLineGraph timeLineGraph;

    }
}
