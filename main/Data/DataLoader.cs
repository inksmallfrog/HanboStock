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
    public class RoutineLoader
    {
        private Timer routineLoadTimer;
        private bool isRunning;
        //private Object objLock = new Object();

        public RoutineLoader()
        {
            routineLoadTimer = new Timer(new TimerCallback(ThreadCallBack), null, Timeout.Infinite, Config.GlobalConfig.ThreadConfig.RealtimeReadTime);
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
            else
            {
                ReadRealTimeFromDB();
            }
            if (WebConfig.ConnectMode)
            {
                if (GlobalData.StocksTable.Rows.Count > 0)
                {
                    DBHelper.CleanOldPerbidData(GlobalData.StocksTable.Select("stockId='1000001'")[0]["date"].ToString());
                }
            }
            routineLoadTimer.Change(Config.GlobalConfig.ThreadConfig.RealtimeReadTime, Config.GlobalConfig.ThreadConfig.RealtimeReadTime);
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
            routineLoadTimer.Change(Timeout.Infinite, Config.GlobalConfig.ThreadConfig.RealtimeReadTime);
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
                if (bufferTable.Rows.Count > 0)
                {
                    GlobalData.LastTradeTime = TimeSpan.Parse(bufferTable.Rows[0]["time"].ToString());
                    GlobalData.LastTradeDate = DateTime.Parse(bufferTable.Rows[0]["date"].ToString());
                }   
            }
            else
            {
                ReadRealTimeFromDB();
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
        private DataDefine.RealtimeRecord lastRecord;
        private bool isPerbidSync = false;
        private List<DataRow> perbidBuffer = new List<DataRow>();

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

        public DataTable Perbid
        {
            get
            {
                return perbidSync;
            }
        }
        private DataTable perbidSync;

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
                    isPerbidSync = false;
                    lastRecord.vol = 0;
                    perbidBuffer.Clear();
                    if(perbidSync != null)
                    {
                        perbidSync.Clear();
                    }
                    if(perminut != null)
                    {
                        perminut.Clear();
                    }
                }
            }
        }

        public SingleLoader()
        {
            singleLoadTimer = new Timer(new TimerCallback(ThreadCallBack), null, Timeout.Infinite, Config.GlobalConfig.ThreadConfig.RealtimeReadTime);
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
                LoadHistory();
                LoadPerbid();
                LoadCurrent();
                LoadPerminut();
                stockIdChanged = false;
            }
            singleLoadTimer.Change(Config.GlobalConfig.ThreadConfig.PerbidReadTime, Config.GlobalConfig.ThreadConfig.PerbidReadTime);
        }

        public void Stop()
        {
            isRunning = false;
            singleLoadTimer.Change(Timeout.Infinite, Config.GlobalConfig.ThreadConfig.RealtimeReadTime);
        }

        private void ThreadCallBack(Object threadLock)
        {
            if (!GlobalData.IsTradeTime() || !WebConfig.ConnectMode)
            {
                return;
            }
            LoadPerbid();
            LoadCurrent();
            LoadPerminut();
        }

        private void LoadCurrent()
        {
            if (WebConfig.ConnectMode)
            {
                DataAPIFactory.GetDataAPI(APIConfig.ApiType).GetStockRealTime(current, stockId);
                try
                {
                    GlobalData.LastTradeTime = TimeSpan.Parse(current["time"].ToString());
                    GlobalData.LastTradeDate = DateTime.Parse(current["date"].ToString());
                }
                catch (Exception e)
                {

                }
            }
            else
            {
                current = DBHelper.GetStockRealTime(stockId);
            }
            if(lastRecord.vol == 0)
            {
                lastRecord.vol = int.Parse(current["volume"].ToString());
                lastRecord.ask = decimal.Parse(current["ask1Price"].ToString());
                lastRecord.bid = decimal.Parse(current["bid1Price"].ToString());
            }
            else if(decimal.Parse(current["volume"].ToString()) != lastRecord.vol)
            {
                CalculatePerbid();
            }
        }

        private void CalculatePerbid()
        {
            DataRow newPerbid = perbidSync.NewRow();
            newPerbid["date"] = current["date"];
            newPerbid["time"] = current["time"];
            newPerbid["price"] = current["price"];
            newPerbid["vol"] = (decimal.Parse(current["volume"].ToString()) - lastRecord.vol);
            if(decimal.Parse(current["price"].ToString()) <= lastRecord.bid)
            {
                newPerbid["type"] = -1;
            }
            else if(decimal.Parse(current["price"].ToString()) >= lastRecord.ask)
            {
                newPerbid["type"] = 1;
            }
            else
            {
                newPerbid["type"] = 0;
            }
            if (!isPerbidSync)
            {
                perbidBuffer.Add(newPerbid);
            }
            else
            {
                perbidSync.Rows.Add(newPerbid);
            }
            
            lastRecord.vol = int.Parse(current["volume"].ToString());
            lastRecord.ask = decimal.Parse(current["ask1Price"].ToString());
            lastRecord.bid = decimal.Parse(current["bid1Price"].ToString());

            if (!isPerbidSync)
            {
                TimeSpan syncTime = TimeSpan.Parse(perbidSync.Rows[perbidSync.Rows.Count - 1]["time"].ToString());
                TimeSpan currentTime = TimeSpan.Parse(perbidBuffer[0]["time"].ToString());
                if (syncTime.Add(TimeSpan.Parse("00:00:03")) >= currentTime)
                {
                    foreach(DataRow row in perbidBuffer)
                    {
                        TimeSpan rowTime = TimeSpan.Parse(row["time"].ToString());
                        if (rowTime < syncTime.Add(TimeSpan.Parse("00:00:03"))) continue;
                        DataRow r = perbidSync.NewRow();
                        r["date"] = row["date"];
                        r["time"] = row["time"];
                        r["price"] = row["price"];
                        r["vol"] = row["vol"];
                        r["type"] = row["type"];
                        perbidSync.Rows.Add(r);
                    }
                    perbidBuffer.Clear();
                    isPerbidSync = true;
                }
            }
        }

        private void LoadHistory()
        {
            histories =  DBHelper.GetHistories(stockId);
        }

        private void LoadPerbid()
        {
            if (!isPerbidSync)
            {
                perbidSync = DBHelper.GetPerbid(stockId);
            }
        }

        private void LoadPerminut()
        {
            perminut.Clear();

            String lastPrice = "0";
            int vol = 0;

            TimeSpan startTime = TimeSpan.Parse("09:25:00");
            TimeSpan minut = TimeSpan.Parse("00:01:00");
            TimeSpan morningStart = TimeSpan.Parse("09:30:00");
            TimeSpan morningEnd = TimeSpan.Parse("11:30:00");
            TimeSpan noonStart = TimeSpan.Parse("13:00:00");
            TimeSpan noonEnd = TimeSpan.Parse("15:00:00");

            bool hasCaculate = false;

            foreach (DataRow row in perbidSync.Rows)
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

                    for (; startTime < rowTime - minut && startTime != morningEnd; startTime += minut)
                    {
                        minutRow = perminut.NewRow();

                        minutRow["stockId"] = stockId;
                        minutRow["date"] = row["date"];
                        minutRow["time"] = startTime.ToString();
                        minutRow["price"] = lastPrice;
                        minutRow["vol"] = 0;

                        perminut.Rows.Add(minutRow);
                    }
                    if(startTime == morningEnd && (rowTime == noonStart))
                    {
                        startTime = TimeSpan.Parse("13:00:00");
                        continue;
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
