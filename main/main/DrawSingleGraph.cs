using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Data;
using ImageOfStock;
using System.Diagnostics;

namespace main
{
    enum GraphType{
        KLineGraph
    };

    class DrawSingleGraph
    {
        private Panel graphPanel;
        private string stockId;
        private GraphType type;

        private DrawKLine kLine;
        private DrawDetail detail;

        public DrawSingleGraph(Panel _graphPanel)
        {
            _graphPanel.Visible = false;
            graphPanel = _graphPanel;
            stockId = "0000001";

            kLine = new DrawKLine((ChartGraph)MainWindow.GetChildFromPanelByName(graphPanel, "KLineGraph"));
            detail = new DrawDetail((Panel)MainWindow.GetChildFromPanelByName(graphPanel, "DetailPanel"));
        }

        public void HideGraph()
        {
            DataReader.StopSpecialReader();
            DataLoader.StopSpecialLoader();
            graphPanel.Visible = false;
        }

        public void ShowGraph(string _stockId)
        {
            DataLoader.InvokeSpecialLoader(_stockId);
            DataReader.InvokeSpecialReader(_stockId);

            if (stockId != _stockId)
            {
                stockId = _stockId;
                Stopwatch sw = new Stopwatch();
                sw.Start();
                kLine.SetStockId(stockId);
                sw.Stop();
                long a = sw.ElapsedMilliseconds;
                detail.SetStockId(stockId);
            }
            graphPanel.Visible = true;
            switch (type)
            {
                case GraphType.KLineGraph:
                    kLine.ShowKLine();
                    break;
                default:
                    break;
            }
        }
    }
}
