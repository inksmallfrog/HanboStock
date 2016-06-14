using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using System.Net;
using System.IO;
using System.Diagnostics;

using Dao.DataAPI;
using Config.GlobalConfig;

namespace Dao
{
    /*public class DataSaver
    {
        private static SingleSaver sgSaver = new SingleSaver();
        private static MultipleSaver mtSaver = new MultipleSaver();

        public static void InvokeSingleSaver(string stockId)
        {
            sgSaver.StockId = stockId;
            sgSaver.Start();
        }

        public static void StopSingleSaver()
        {
            sgSaver.Stop();
        }

        public static void InvokeMultipleSaver(List<string> stocksId)
        {

        }

        public static void StopInvokeLoader()
        {

        }
    }*/

    public class DataSaver
    {
        public static void SaveHistory(Object stockId)
        {
            if (!Config.GlobalConfig.WebConfig.ConnectMode)
            {
                return;
            }
            DataTable stockHistories = DataDefine.GetNewStocksHistoryTable();
            string newestHistoryInDB = DBHelper.GetNewestHistoryDate(stockId.ToString());
            int dirtaDay = -1;

            if (!newestHistoryInDB.Equals(""))
            {
                DateTime lastHistoryDate = DateTime.Parse(newestHistoryInDB);

                if (lastHistoryDate.AddDays(1) >= GlobalData.LastTradeDate)
                {
                    return;
                }
                dirtaDay = (int)(GlobalData.LastTradeDate - lastHistoryDate).TotalDays;
            }
            
            DataAPIFactory.GetDataAPI(APIConfig.ApiType).GetHistoriesTable(stockHistories, stockId.ToString(), dirtaDay);
            DBHelper.InsertIntoHistory(stockHistories);
        }

        public static void SavePerbid(Object stockId)
        {

            DataTable stockPerbid = DataDefine.GetNewStocksPerbidTable();

            string newestPeriBidsInDB = DBHelper.GetNewestPerbidTime(stockId.ToString());

            TimeSpan newestTimeInDB = TimeSpan.Parse(newestPeriBidsInDB);
            if (newestTimeInDB < TimeSpan.Parse("15:00:00"))
            {
                int readCount = (int)(TimeSpan.Parse(GlobalData.LastTradeTime.ToString()) - newestTimeInDB).TotalSeconds / 2;
                if (readCount > 5000)
                {
                    readCount = -1;
                }
                DataAPIFactory.GetDataAPI(APIConfig.ApiType).GetPerbidTable(stockPerbid, stockId.ToString(), readCount);

                DBHelper.InsertIntoPerbid(stockPerbid);

            }
        }
    }
}
