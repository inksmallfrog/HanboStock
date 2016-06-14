//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using System.Threading;
//using Dao;
//using ImageOfStock;
//using System.Diagnostics;

//namespace StockMonitor.SingleGraph
//{
//    enum GraphType{
//        KLineGraph,
//        TimeLineGraph
//    };

//    class DrawSingleGraph
//    {
//        private Panel graphPanel;
//        private string stockId;
//        private GraphType type;

//        private DrawKLine kLine;
//        private DrawTimeLine timeLine;
//        private DrawDetail detail;

//        public DrawSingleGraph(Panel _graphPanel)
//        {
//            _graphPanel.Visible = false;
//            graphPanel = _graphPanel;
//            stockId = "0";

//            kLine = new DrawKLine((KLineGraph)MainWindow.GetChildFromPanelByName(graphPanel, "KLineGraph"));
//            timeLine = new DrawTimeLine((TimeLineGraph)MainWindow.GetChildFromPanelByName(graphPanel, "timeLineGraph"));
//            detail = new DrawDetail((Panel)MainWindow.GetChildFromPanelByName(graphPanel, "DetailPanel"));

//            kLine.Hide();
//            timeLine.Show();
//        }

//        public void HideGraph()
//        {
//            DataLoader.StopSingleLoader();
//            DataSaver.StopSingleSaver();
//            graphPanel.Visible = false;
//        }

//        public void ShowGraph(string _stockId)
//        {
//            Stopwatch sw = new Stopwatch();
//            sw.Start();
//            DataSaver.InvokeSingleSaver(_stockId);
//            sw.Stop();
//            long a = sw.ElapsedMilliseconds;
//            sw.Reset();
//            sw.Start();
//            DataLoader.InvokeSingleLoader(_stockId);
//            sw.Stop();
//            long b = sw.ElapsedMilliseconds;
//            sw.Reset();

//            sw.Start();
//            if (stockId != _stockId)
//            {
//                stockId = _stockId;
//                kLine.SetStockId(stockId);
//                timeLine.SetStockId(stockId);
//                detail.SetStockId(stockId);
//            }
//            sw.Stop();
//            long c = sw.ElapsedMilliseconds;
//            sw.Reset();

//            sw.Start();
//            graphPanel.Visible = true;
//            switch (type)
//            {
//                case GraphType.KLineGraph:
//                    kLine.Show();
//                    break;
//                case GraphType.TimeLineGraph:
//                    timeLine.Show();
//                    break;
//                default:
//                    break;
//            }
//            sw.Stop();
//            long d = sw.ElapsedMilliseconds;
//            sw.Reset();
//        }

//        public void ChangeKLinePanel(KLineGraph.IndicatorType type)
//        {
//            kLine.ChangePanel(type);
//        }
//    }
//}
