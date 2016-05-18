using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace Data
{
    public class DataLoader
    {
        private static SpecialLoader spLoader = new SpecialLoader();
        private static MultipleLoader mtLoader = new MultipleLoader();

        public static void InvokeSpecialLoader(string stockId)
        {
            spLoader.StockId = stockId;
            spLoader.Start();
        }

        public static void StopSpecialLoader()
        {
            spLoader.Stop();
        }

        public static void InvokeMultipleLoader(List<string> stocksId)
        {

        }

        public static void StopInvokeLoader()
        {

        }
    }

    public class SpecialLoader
    {
        private string stockId;
        private bool stockIdChanged;
        private Timer perBidsLoadTimer;

        public string StockId
        {
            set
            {
                if (value != stockId)
                {
                    stockId = value;
                    stockIdChanged = true;
                }
            }
        }

        public SpecialLoader()
        {
            perBidsLoadTimer = new Timer(new TimerCallback(ThreadCallBack), null, Timeout.Infinite, 2000);
            stockIdChanged = true;
        }

        public void Start()
        {
            if (!ProgramConfig.WebConfig.ConnectMode)
            {
                return;
            }
            if (stockIdChanged)
            {

                LoadHistory();

                LoadPerbid();

                stockIdChanged = false;
            }
            perBidsLoadTimer.Change(2000, 2000);
        }

        public void Stop()
        {
            perBidsLoadTimer.Change(Timeout.Infinite, 2000);
        }

        private void ThreadCallBack(Object threadLock)
        {
            if (!CommonData.IsTradeTime())
            {
                return;
            }
            LoadPerbid();
        }

        private void LoadHistory()
        {
            DataTable stockHistories = DataDefine.GetNewStocksHistoryTable();
           
            ProgramConfig.APIConfig.GetHistoriesTable(stockHistories, stockId);

            DBHelper.InsertIntoHistory(stockHistories);

        }

        private void LoadPerbid()
        {
            DataTable stockPerbid = DataDefine.GetNewStocksPerbidTable();

            string newestPeriBidsInDB = DBHelper.GetNewestPerbidTime(stockId);

            ProgramConfig.APIConfig.GetPerbidTable(stockPerbid, stockId, newestPeriBidsInDB);

            DBHelper.InsertIntoPerbid(stockPerbid);
        }
    }

    public class MultipleLoader
    {

    }
}
