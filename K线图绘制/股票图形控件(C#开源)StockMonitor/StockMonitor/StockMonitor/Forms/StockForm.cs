using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using ImageOfStock;
using System.Drawing.Drawing2D;

namespace StockMonitor.Forms
{
    public partial class StockForm : BaseForm
    {
        public StockForm()
        {
            this.TopLevel = false;
            this.Show();
            InitializeComponent();
            CandleGraph();
         }


        private List<HistoryStockData> data;
        private CurrentStockData cuData;

        public List<HistoryStockData> Data { set { data = value; } }
        public CurrentStockData CuData { set { cuData = value; } }
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void open()
        {
            CandleGraph();
            this.chartGraph1.ProcessBarValue = 0;
            Thread refreshData = new Thread(new ParameterizedThreadStart(UpdateDataToGraph));
            refreshData.IsBackground = true;
            refreshData.Start(data);
        }

        public void CandleGraph()
        {
            this.chartGraph1.ResetNullGraph();
            this.chartGraph1.UseScrollAddSpeed = true;
            this.chartGraph1.SetXScaleField("日期");
            this.chartGraph1.CanDragSeries = true;
            this.chartGraph1.SetSrollStep(10, 10);
            this.chartGraph1.ShowLeftScale = true;
            this.chartGraph1.ShowRightScale = true;
            this.chartGraph1.LeftPixSpace = 85;
            this.chartGraph1.RightPixSpace = 85;
            //K线图+BS点
            mainPanelID = this.chartGraph1.AddChartPanel(40);
            string candleName = "K线图-1";
            this.chartGraph1.AddCandle(candleName, "OPEN", "HIGH", "LOW", "CLOSE", mainPanelID, true);
            this.chartGraph1.YMBuySellSignal(mainPanelID, candleName, "BUYEMA", "(CLOSE+HIGH+LOW)/3", "SELLEMA", "BUYEMA");
            this.chartGraph1.AddBollingerBands("MID", "UP", "DOWN", "CLOSE", 20, 2, mainPanelID);
            this.chartGraph1.SetYScaleField(mainPanelID, new string[] { "HIGH","LOW"});
            //成交量
            volumePanelID = this.chartGraph1.AddChartPanel(20);
            this.chartGraph1.AddHistogram("VOL", "", candleName, volumePanelID);
            this.chartGraph1.SetHistogramStyle("VOL", Color.Red, Color.SkyBlue, 1, false);
            this.chartGraph1.AddSimpleMovingAverage("VOL-MA1", "MA5", "VOL", 5, volumePanelID);
            this.chartGraph1.SetTrendLineStyle("VOL-MA1", Color.White, Color.White, 1, DashStyle.Solid);
            this.chartGraph1.AddSimpleMovingAverage("VOL-MA2", "MA10", "VOL",10, volumePanelID);
            this.chartGraph1.SetTrendLineStyle("VOL-MA2", Color.Yellow, Color.Yellow, 1, DashStyle.Solid);
            this.chartGraph1.AddSimpleMovingAverage("VOL-MA3", "MA20", "VOL", 20, volumePanelID);
            this.chartGraph1.SetTrendLineStyle("VOL-MA3", Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 255), 1, DashStyle.Solid);
            this.chartGraph1.SetTick(volumePanelID, 1);
            this.chartGraph1.SetDigit(volumePanelID, 0);
            kdjPanelID = this.chartGraph1.AddChartPanel(20);
            this.chartGraph1.AddStochasticOscillator("K", "D", "J", 9, "CLOSE", "HIGH", "LOW", kdjPanelID);
            macdPanelID = this.chartGraph1.AddChartPanel(20);
            this.chartGraph1.AddMacd("MACD", "DIFF", "DEA", "CLOSE", 26, 12, 9, macdPanelID);
        }

        int mainPanelID = -1;
        int volumePanelID = -1;
        int kdjPanelID = -1;
        int macdPanelID = -1;

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomForm_Load(object sender, EventArgs e)
        {
            this.Text = "STOCK MONITOR";
            this.Location = new Point(0, 0);
            this.Size = new Size(Screen.GetWorkingArea(this).Width, Screen.GetWorkingArea(this).Height);
            CandleGraph();
        }

        /// <summary>
        /// 更新进度条
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateProcessBar(object obj)
        {
            int[] values = obj as int[];
            int total = values[0];
            int current = values[1];
            int processValue = Convert.ToInt32((double)current / (double)total * 100);
            if (processValue > this.chartGraph1.ProcessBarValue)
            {
                this.chartGraph1.ProcessBarValue = processValue;
            }
            if (current == total - 2)
            {
                this.chartGraph1.ProcessBarValue = 100;
                this.chartGraph1.RefreshGraph();
            }
        }

        private delegate void UpdateProcessBarDelegate(object obj);

        /// <summary>
        /// 更新数据到图像
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateDataToGraph(object obj)
        {
            this.chartGraph1.SetTitle(mainPanelID, cuData.name + "(" + cuData.code + ")");
            this.chartGraph1.SetTitle(kdjPanelID, "KDJ(9,3,3)");
            this.chartGraph1.SetTitle(volumePanelID, "VOL(5,10,20)");
            this.chartGraph1.SetTitle(macdPanelID, "MACD(12,26,9)");
            string lineType = "日线";
            switch (lineType)
            {
                case "5分钟":
                case "15分钟":
                case "30分钟":
                case "60分钟":
                    this.chartGraph1.SetIntervalType(mainPanelID, ChartGraph.IntervalType.Minute);
                    this.chartGraph1.SetIntervalType(volumePanelID, ChartGraph.IntervalType.Minute);
                    this.chartGraph1.SetIntervalType(kdjPanelID, ChartGraph.IntervalType.Minute);
                    break;
                case "日线":
                    this.chartGraph1.SetIntervalType(mainPanelID, ChartGraph.IntervalType.Day);
                    this.chartGraph1.SetIntervalType(volumePanelID, ChartGraph.IntervalType.Day);
                    this.chartGraph1.SetIntervalType(kdjPanelID, ChartGraph.IntervalType.Day);
                    break;
                case "周线":
                    this.chartGraph1.SetIntervalType(mainPanelID, ChartGraph.IntervalType.Week);
                    this.chartGraph1.SetIntervalType(volumePanelID, ChartGraph.IntervalType.Week);
                    this.chartGraph1.SetIntervalType(kdjPanelID, ChartGraph.IntervalType.Week);
                    break;
                case "月线":
                    this.chartGraph1.SetIntervalType(mainPanelID, ChartGraph.IntervalType.Month);
                    this.chartGraph1.SetIntervalType(volumePanelID, ChartGraph.IntervalType.Month);
                    this.chartGraph1.SetIntervalType(kdjPanelID, ChartGraph.IntervalType.Month);
                    break;
            }
            this.chartGraph1.RefreshGraph();
            for (int i = data.Count - 1; i >= 2; i--)
            {
                HistoryStockData[] stcs = new HistoryStockData[data.Count];
                data.CopyTo(stcs);
                string timeKey = stcs[i].date;
                int year = 1970;
                int month = 1;
                int day = 1;
                int hour = 0;
                int minute = 0;
                switch (lineType)
                {
                    case "5分钟":
                    case "15分钟":
                    case "30分钟":
                    case "60分钟":
                        month = Convert.ToInt32(timeKey.Substring(0, 1));
                        day = Convert.ToInt32(timeKey.Substring(1, 2));
                        hour = Convert.ToInt32(timeKey.Substring(3, 2));
                        minute = Convert.ToInt32(timeKey.Substring(5, 2));
                        break;
                    case "日线":
                    case "周线":
                        year = Convert.ToInt32(timeKey.Substring(0, 4));
                        month = Convert.ToInt32(timeKey.Substring(5, 2));
                        day = Convert.ToInt32(timeKey.Substring(8, 2));
                        break;
                    case "月线":
                        year = Convert.ToInt32(timeKey.Substring(0, 4));
                        month = Convert.ToInt32(timeKey.Substring(4, 2));
                        break;
                }
                DateTime dt = new DateTime(year, month, day, hour, minute, 0);
                this.chartGraph1.SetValue("OPEN", stcs[i].openPrice, dt);
                this.chartGraph1.SetValue("HIGH", stcs[i].highestPrice, dt);
                this.chartGraph1.SetValue("LOW", stcs[i].lowestPrice, dt);
                this.chartGraph1.SetValue("CLOSE", stcs[i].closePrice, dt);
                this.chartGraph1.SetValue("VOL", stcs[i].volume, dt);

                double ymValue = (Convert.ToDouble(stcs[i].lowestPrice) + Convert.ToDouble(stcs[i].highestPrice) + Convert.ToDouble(stcs[i].openPrice)) / 3;
                this.chartGraph1.SetValue("(CLOSE+HIGH+LOW)/3", ymValue, dt);

                BeginInvoke(new UpdateProcessBarDelegate(UpdateProcessBar), new int[] { data.Count, data.Count - i });
            } 
            this.chartGraph1.Enabled = true;
        }
     
    }
}