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
            // kLinePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.kLineGraph);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "kLinePanel";
            this.Size = new System.Drawing.Size(724, 720);
            this.ResumeLayout(false);

        }

        #endregion

        private KLineGraph kLineGraph;
    }
}
