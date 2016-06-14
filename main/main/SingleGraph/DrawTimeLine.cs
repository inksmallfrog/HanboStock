using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dao;
using ImageOfStock;

namespace StockMonitor.SingleGraph
{
    class DrawTimeLine : ISubSingleGraph
    {
        private TimeLineGraph graph = null;

        private String stockId;

        public DrawTimeLine(TimeLineGraph _graph){
            graph = _graph;
        }

        public void SetStockId(String _stockId)
        {
             if (stockId != _stockId)
            {
                stockId = _stockId;
                //graph.BindData(DataLoader.GetSinglePerminut(), DataLoader.GetRealTimeList().Select("stockId='" + stockId + "'")[0]);
            }
        }

        public void Show()
        {
            graph.Visible = true;
        }

        public void Hide()
        {
            graph.Visible = false;
        }
    }
}
