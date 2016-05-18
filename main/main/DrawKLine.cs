using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Data;
using ImageOfStock;
using Data;

namespace main
{
    class DrawKLine
    {
        private ImageOfStock.ChartGraph kLineGraph;

        private int mainPanelID = -1;
        private int volumePanelID = -1;
        private int kdjPanelID = -1;
        private int macdPanelID = -1;

        private string lineType = "日线";

        private string stockId;

        public DrawKLine(ImageOfStock.ChartGraph _kLineGraph)
        {
            kLineGraph = _kLineGraph;
            CandleGraph();
            kLineGraph.Visible = false;
        }

        public void ShowKLine(){
            kLineGraph.Visible = true;
        }

        public void HideKLine()
        {
            kLineGraph.Visible = false;
        }

        public void SetStockId(string _stockId)
        {
            if (stockId != _stockId)
            {
                kLineGraph.ClearGraph();
                stockId = _stockId;
                BindData(DataReader.GetSpecialHistories(), DataReader.GetRealTimeList().Select("stockId='" + stockId + "'")[0]);
            }
        }

        private void CandleGraph()
        {
            this.kLineGraph.ResetNullGraph();
            this.kLineGraph.UseScrollAddSpeed = true;
            this.kLineGraph.SetXScaleField("date");
            this.kLineGraph.CanDragSeries = true;
            this.kLineGraph.SetSrollStep(10, 10);
            this.kLineGraph.ShowLeftScale = true;
            this.kLineGraph.ShowRightScale = false;
            
            this.kLineGraph.LeftPixSpace =60;
            this.kLineGraph.RightPixSpace = 0;
            //K线图+BS点
            mainPanelID = this.kLineGraph.AddChartPanel(60);
            string candleName = "K线图-日线";
            this.kLineGraph.AddCandle(candleName, "openP", "high", "low", "closeP", mainPanelID, true);
            this.kLineGraph.YMBuySellSignal(mainPanelID, candleName, "BUYEMA", "(CLOSE+HIGH+LOW)/3", "SELLEMA", "BUYEMA");
            this.kLineGraph.AddBollingerBands("MID", "UP", "DOWN", "closeP", 20, 2, mainPanelID);
            this.kLineGraph.SetYScaleField(mainPanelID, new string[] { "high", "low" });
            //成交量
            volumePanelID = this.kLineGraph.AddChartPanel(20);
            this.kLineGraph.AddHistogram("volume", "", candleName, volumePanelID);
            this.kLineGraph.SetHistogramStyle("volume", Color.Red, Color.SkyBlue, 1, false);
            this.kLineGraph.AddSimpleMovingAverage("VOL-MA1", "MA5", "volume", 5, volumePanelID);
            this.kLineGraph.SetTrendLineStyle("VOL-MA1", Color.White, Color.White, 1, DashStyle.Solid);
            this.kLineGraph.AddSimpleMovingAverage("VOL-MA2", "MA10", "volume", 10, volumePanelID);
            this.kLineGraph.SetTrendLineStyle("VOL-MA2", Color.Yellow, Color.Yellow, 1, DashStyle.Solid);
            this.kLineGraph.AddSimpleMovingAverage("VOL-MA3", "MA20", "volume", 20, volumePanelID);
            this.kLineGraph.SetTrendLineStyle("VOL-MA3", Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 255), 1, DashStyle.Solid);
            this.kLineGraph.SetTick(volumePanelID, 1);
            this.kLineGraph.SetDigit(volumePanelID, 0);
            //kdjPanelID = this.kLineGraph.AddChartPanel(20);
            //this.kLineGraph.AddStochasticOscillator("K", "D", "J", 9, "CLOSE", "HIGH", "LOW", kdjPanelID);
            macdPanelID = this.kLineGraph.AddChartPanel(20);
            this.kLineGraph.AddMacd("MACD", "DIFF", "DEA", "closeP", 26, 12, 9, macdPanelID);
        }

        public void BindData(DataTable histories, DataRow current){
            this.kLineGraph.SetTitle(mainPanelID, current["name"] + "(" + current["stockId"].ToString().Substring(1) + ")");
            this.kLineGraph.SetTitle(kdjPanelID, "KDJ(9,3,3)");
            this.kLineGraph.SetTitle(volumePanelID, "VOL(5,10,20)");
            this.kLineGraph.SetTitle(macdPanelID, "MACD(12,26,9)");

            if (histories.Rows.Count == 0 
                || histories.Rows[histories.Rows.Count - 1]["date"] != current["date"])
            {
                if (double.Parse(current["price"].ToString()) != 0.0)
                {
                    DataRow realtimeRow = histories.NewRow();
                    realtimeRow["date"] = current["date"];
                    realtimeRow["closeP"] = current["price"];
                    realtimeRow["openP"] = current["openP"];
                    realtimeRow["high"] = current["high"];
                    realtimeRow["low"] = current["low"];
                    realtimeRow["volume"] = current["volume"];
                    histories.Rows.Add(realtimeRow);
                }
            }

            /*switch (lineType)
            {
                case "5分钟":
                case "15分钟":
                            case "60分钟":
                    this.kLineGraph.SetIntervalType(mainPanelID, ChartGraph.IntervalType.Minute);
                this.kLineGraph.SetIntervalType(volumePanelID, ChartGraph.IntervalType.Minute);
                    this.kLineGraph.SetIntervalType(kdjPanelID, ChartGraph.IntervalType.Minute);
                    break;
                case "日线":
                    this.kLineGraph.SetIntervalType(mainPanelID, ChartGraph.IntervalType.Day);
                    this.kLineGraph.SetIntervalType(volumePanelID, ChartGraph.IntervalType.Day);
                    this.kLineGraph.SetIntervalType(kdjPanelID, ChartGraph.IntervalType.Day);
                    break;
                case "周线":
                    this.kLineGraph.SetIntervalType(mainPanelID, ChartGraph.IntervalType.Week);
                    this.kLineGraph.SetIntervalType(volumePanelID, ChartGraph.IntervalType.Week);
                    this.kLineGraph.SetIntervalType(kdjPanelID, ChartGraph.IntervalType.Week);
                    break;
                case "月线":
                    this.kLineGraph.SetIntervalType(mainPanelID, ChartGraph.IntervalType.Month);
                    this.kLineGraph.SetIntervalType(volumePanelID, ChartGraph.IntervalType.Month);
                    this.kLineGraph.SetIntervalType(kdjPanelID, ChartGraph.IntervalType.Month);
                    break;
            }
            this.kLineGraph.RefreshGraph();
            for (int i = 0; i < histories.Rows.Count; ++i)
            {
                string timeKey = histories.Rows[i]["date"].ToString();
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
                this.kLineGraph.SetValue("OPEN", histories.Rows[i]["openP"], dt);
                this.kLineGraph.SetValue("HIGH", histories.Rows[i]["high"], dt);
                this.kLineGraph.SetValue("LOW", histories.Rows[i]["low"], dt);
                this.kLineGraph.SetValue("CLOSE", histories.Rows[i]["closeP"], dt);
                this.kLineGraph.SetValue("VOL", histories.Rows[i]["volume"], dt);

                double ymValue = (Convert.ToDouble(histories.Rows[i]["closeP"].ToString()) + Convert.ToDouble(histories.Rows[i]["high"].ToString()) + Convert.ToDouble(histories.Rows[i]["openP"].ToString())) / 3;
                this.kLineGraph.SetValue("(CLOSE+HIGH+LOW)/3", ymValue, dt);

                //kLineGraph.ParentForm.BeginInvoke(new UpdateProcessBarDelegate(UpdateProcessBar), new int[] { histories.Count - 1, i });
            }*/
            this.kLineGraph.BindData(DataReader.GetSpecialHistories());
            this.kLineGraph.RefreshGraph();
            this.kLineGraph.Enabled = true;
        }
    }
}
