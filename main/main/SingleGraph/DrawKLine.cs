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
using Dao;

namespace StockMonitor.SingleGraph
{
    class DrawKLine : ISubSingleGraph
    {
        private KLineGraph kLineGraph;

        private String stockId;

        public DrawKLine(KLineGraph _kLineGraph)
        {
            kLineGraph = _kLineGraph;
            kLineGraph.CandleGraph();
            kLineGraph.Visible = false;
        }

        public void Show(){
            kLineGraph.Visible = true;
        }

        public void Hide()
        {
            kLineGraph.Visible = false;
        }

        public void SetStockId(String _stockId)
        {
            if (stockId != _stockId)
            {
                kLineGraph.ClearGraph();
                stockId = _stockId;
                //BindData(DataLoader.GetSingleHistories(), GlobalData.StocksTable.Select("stockId='" + stockId + "'")[0]);
            }
        }

        public void BindData(DataTable histories, DataRow current){
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
                    realtimeRow["volume"] = (Int64.Parse(current["volume"].ToString()) / 100);
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
            //this.kLineGraph.BindData(DataLoader.GetSingleHistories());
            this.kLineGraph.RefreshGraph();
            this.kLineGraph.Enabled = true;
        }

        public void ChangePanel(KLineGraph.IndicatorType type)
        {
            kLineGraph.ChoseIndicator(type);
        }
    }
}
