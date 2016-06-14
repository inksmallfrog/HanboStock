using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;

using Dao.DataAPI;
using Config.GlobalConfig;

namespace Dao
{
    //public class DataLoader
    //{
    //    private static RoutineLoader routineLoader = new RoutineLoader();
    //    private static SingleLoader singleLoader = new SingleLoader();

    //    public static void InvokeRoutingReader()
    //    {
    //        routineLoader.Start();
    //    }

    //    public static void LoadRealTimeList()
    //    {
    //        routineLoader.Load();
    //    }

    //    public static DataTable GetRealTimeList()
    //    {
    //        return routineLoader.RealTimeList;
    //    }

    //    public static void InvokeSingleLoader(string stockId)
    //    {
    //        singleLoader.StockId = stockId;
    //        singleLoader.Start();
    //    }

    //    public static void StopSingleLoader()
    //    {
    //        singleLoader.Stop();
    //    }

    //    public static DataTable GetSingleHistories()
    //    {
    //        return singleLoader.Histories;
    //    }

    //    public static DataTable GetSinglePerbid()
    //    {
    //        return singleLoader.Perbid;
    //    }

    //    public static DataTable GetSinglePerminut()
    //    {
    //        return singleLoader.Perminut;
    //    }

    //    public static void InvokeMultipleReader(List<string> stocksId)
    //    {
            
    //    }

    //    public static void StopMultipleReader()
    //    {

    //    }
    //}

    public class RoutineLoader
    {
        private Timer routineLoadTimer;
        private bool isRunning;
        //private Object objLock = new Object();

        public RoutineLoader()
        {
            routineLoadTimer = new Timer(new TimerCallback(ThreadCallBack), null, Timeout.Infinite, 2000);
            isRunning = false;
        }

        public void Start()
        {
            if (isRunning)
            {
                return;
            }
            isRunning = true;
            if (WebConfig.ConnectMode)
            {
                ReadRealTime();
            }

            if (!WebConfig.ConnectMode)
            {
                ReadRealTimeFromDB();
            }

            DBHelper.CleanOldPerbidData(GlobalData.StocksTable.Select("stockId='1000001'")[0]["date"].ToString());
            routineLoadTimer.Change(3000, 3000);
        }

        public void ReadNow()
        {
            if (WebConfig.ConnectMode)
            {
                ReadRealTime();
            }

            if (!WebConfig.ConnectMode)
            {
                ReadRealTimeFromDB();
            }
        }

        public void Stop()
        {
            isRunning = false;
            routineLoadTimer.Change(Timeout.Infinite, 2000);
        }

        public void Load()
        {
            ReadRealTime();
        }

        private void ThreadCallBack(Object threadLock)
        {
            if (!GlobalData.IsTradeTime() || !WebConfig.ConnectMode)
            {
                return;
            }
            ReadRealTime();
        }

        private void ReadRealTime()
        {
            DataTable bufferTable = DataDefine.GetNewStocksRealtimeTable();
            DataAPIFactory.GetDataAPI(APIConfig.ApiType).GetRealTimeTable(bufferTable, GlobalData.CurrentShowList);
            if (Config.GlobalConfig.WebConfig.ConnectMode)
            {
                GlobalData.StocksTable = bufferTable;
                GlobalData.LastTradeTime = TimeSpan.Parse(bufferTable.Rows[0]["time"].ToString());
                GlobalData.LastTradeDate = DateTime.Parse(bufferTable.Rows[0]["date"].ToString());
            }
            
        }

        private void ReadRealTimeFromDB()
        {
            GlobalData.StocksTable = DBHelper.GetRealtime();
            GlobalData.LastTradeTime = TimeSpan.Parse(GlobalData.StocksTable.Rows[0]["time"].ToString());
            GlobalData.LastTradeDate = DateTime.Parse(GlobalData.StocksTable.Rows[0]["date"].ToString());
        }
    }

    public class SingleLoader
    {
        private Timer singleLoadTimer;
        private bool isRunning;
        private string stockId;
        private bool stockIdChanged;
        private String lastMinutTime;

        private DataRow current;
        public DataRow Current
        {
            get
            {
                return current;
            }
        }

        private DataTable histories;
        public DataTable Histories
        {
            get
            {
                return histories;
            }
        }

        private DataTable perbid;
        public DataTable Perbid
        {
            get
            {
                return perbid;
            }
        }

        private DataTable perminut;
        public DataTable Perminut
        {
            get
            {
                return perminut;
            }
        }

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

        public SingleLoader()
        {
            singleLoadTimer = new Timer(new TimerCallback(ThreadCallBack), null, Timeout.Infinite, 2000);
            isRunning = false;
            stockIdChanged = true;
            stockId = "-1";
            lastMinutTime = "09:30";
            current = DataDefine.GetNewStocksRealtimeTable().NewRow();
            perminut = DataDefine.GetNewStocksPerminutTable();
        }

        public void Start()
        {
            if (isRunning)
            {
                return;
            }
            isRunning = true;
            if (stockIdChanged)
            {
                perminut.Clear();
                LoadCurrent();
                LoadHistory();
                LoadPerbid();
                LoadPerminut();
                stockIdChanged = false;
            }
            singleLoadTimer.Change(6000, 6000);
        }

        public void Stop()
        {
            isRunning = false;
            singleLoadTimer.Change(Timeout.Infinite, 2000);
        }

        private void ThreadCallBack(Object threadLock)
        {
            if (!GlobalData.IsTradeTime() || !WebConfig.ConnectMode)
            {
                return;
            }
            LoadCurrent();
            LoadPerbid();
            LoadPerminut();
        }

        private void LoadCurrent()
        {
            if (WebConfig.ConnectMode)
            {
                DataAPIFactory.GetDataAPI(APIConfig.ApiType).GetStockRealTime(current, stockId);
                GlobalData.LastTradeTime = TimeSpan.Parse(current["time"].ToString());
                GlobalData.LastTradeDate = DateTime.Parse(current["date"].ToString());
            }
            else
            {
                current = DBHelper.GetStockRealTime(stockId);
            }
        }

        private void LoadHistory()
        {
            histories =  DBHelper.GetHistories(stockId);
        }

        private void LoadPerbid()
        {
            perbid = DBHelper.GetPerbid(stockId);
        }

        private void LoadPerminut()
        {
            String lastPrice = "0";
            int vol = 0;

            TimeSpan startTime = TimeSpan.Parse("09:25:00");
            TimeSpan minut = TimeSpan.Parse("00:01:00");
            TimeSpan morningStart = TimeSpan.Parse("09:30:00");
            TimeSpan morningEnd = TimeSpan.Parse("11:30:00");
            TimeSpan noonStart = TimeSpan.Parse("13:00:00");
            TimeSpan noonEnd = TimeSpan.Parse("15:00:00");

            bool hasCaculate = false;

            foreach (DataRow row in perbid.Rows)
            {
                hasCaculate = false;
                TimeSpan rowTime = TimeSpan.Parse(row["time"].ToString().Substring(0, 5) + ":00");
                if (rowTime <= morningStart || rowTime == morningEnd || rowTime == noonEnd)
                {
                    lastPrice = row["price"].ToString();
                    vol += Int32.Parse(row["vol"].ToString());
                    hasCaculate = true;
                }
                
                if (!rowTime.ToString().Substring(0, 5).Equals(startTime.ToString().Substring(0, 5)) && rowTime > morningStart)
                {
                    DataRow minutRow = null;

                    if (startTime < morningStart)
                    {
                        startTime = morningStart;

                        minutRow = perminut.NewRow();

                        minutRow["stockId"] = stockId;
                        minutRow["date"] = row["date"];
                        minutRow["time"] = startTime.ToString();
                        minutRow["price"] = row["price"];
                        minutRow["vol"] = 0;

                        perminut.Rows.Add(minutRow);
                    }

                    for (; startTime < rowTime - minut; startTime += minut)
                    {
                        minutRow = perminut.NewRow();

                        minutRow["stockId"] = stockId;
                        minutRow["date"] = row["date"];
                        minutRow["time"] = startTime.ToString();
                        minutRow["price"] = lastPrice;
                        minutRow["vol"] = 0;

                        perminut.Rows.Add(minutRow);
                    }

                    minutRow = perminut.NewRow();

                    minutRow["stockId"] = stockId;
                    minutRow["date"] = row["date"];
                    minutRow["time"] = startTime.ToString();
                    minutRow["price"] = lastPrice;
                    minutRow["vol"] = vol;

                    perminut.Rows.Add(minutRow);
                    vol = 0;
                    startTime += minut;

                    if (rowTime == morningEnd)
                    {
                        startTime = TimeSpan.Parse("13:00:00");
                        continue;
                    }
                }
                if (!hasCaculate)
                {
                    lastPrice = row["price"].ToString();
                    vol += Int32.Parse(row["vol"].ToString());
                }
            }
        }
    }
}
