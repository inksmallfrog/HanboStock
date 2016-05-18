using System;
using System.Collections.Generic;
using System.Windows.Forms;
using StockMonitor.Forms;

namespace StockMonitor
{
    public class DrawK
    {
        private string code;
        private Panel k_panel;

        public DrawK(string code, Panel k_panel,int type)
        {
            this.code = code;
            this.k_panel = k_panel;
            k_panel.Visible = true;
            a.Width = k_panel.Width;
            a.Height = k_panel.Height;
            k_panel.Controls.Clear();            //绘制前清空组件
            k_panel.Controls.Add(a);             //添加K线绘制组件
            switch (type)
            {
                case 1: a.Data = Data.getHistoryDataFromFile(code,100,".\\data"); break;
                case 2: a.Data = Data.getHistoryDataFormDatabase(code, 100); break;
                case 3: a.Data = Data.getHistoryData(code, 100); break;
                default: a.Data = Data.getHistoryDataFormDatabase(code, 100); break;
            }
            //a.Data = Data.getHistoryDataFormDatabase(code, 100);
            ////List<HistoryStockData> b = Data.getHistoryData(code, 100);
            //List<HistoryStockData> c = Data.getHistoryDataFormDatabase(code, 100);
            //int count = 0;
            //foreach (HistoryStockData i in b)
            //{
            //    HistoryStockData[] k = new HistoryStockData[c.Count];
            //    c.CopyTo(k);
            //    if (k[count].date!=i.date|| k[count].highestPrice != i.highestPrice || k[count].lowestPrice != i.lowestPrice
            //        || k[count].openPrice!= i.openPrice || k[count].closePrice != i.closePrice || k[count].volume != i.volume )
            //    {
            //        MessageBox.Show("艹");
            //    }

            //    count++;
            //}

            a.CuData = Data.getCurrentData(code); 
            a.open();
        }

        private StockForm a = new StockForm();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>


        static void Main()
        {
        
        }

    }
}