using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data;

namespace Data
{
    public class DataReader
    {
        private static RoutineReader routineReader = new RoutineReader();
        private static SpecialReader specialReader = new SpecialReader();

        public static void InvokeRoutingReader()
        {
            routineReader.Start();
        }

        public static DataTable GetRealTimeList()
        {
            return routineReader.RealTimeList;
        }

        public static void InvokeSpecialReader(string stockId)
        {
            specialReader.StockId = stockId;
            specialReader.Start();
        }

        public static void StopSpecialReader()
        {
            specialReader.Stop();
        }

        public static DataTable GetSpecialHistories()
        {
            return specialReader.Histories;
        }

        public static DataTable GetSpecialPerbid()
        {
            return specialReader.Perbid;
        }

        public static void InvokeMultipleReader(List<string> stocksId)
        {
            
        }

        public static void StopMultipleReader()
        {

        }
    }

    class RoutineReader
    {
        private Timer routineReadTimer;
        private int threadCount;
        private bool isRunning;
        private ManualResetEvent[] manualEvents;
        private SubReader[] subReaders;

        private DataTable realTimeTable = DataDefine.GetNewStocksRealtimeTable();
        public DataTable RealTimeList
        {
            get
            {
                return realTimeTable;
            }
            set
            {
                realTimeTable = value;
            }
        }

        private DataTable realTimeTableBuffer = null;
        
        public RoutineReader()
        {
            routineReadTimer = new Timer(new TimerCallback(ThreadCallBack), null, Timeout.Infinite, 2000);
            threadCount = (int)Math.Ceiling(CommonData.StocksCode.Rows.Count * 1.0 / ProgramConfig.ThreadConfig.RoutingReadSize);
            isRunning = false;
            manualEvents = new ManualResetEvent[threadCount];
            subReaders = new SubReader[threadCount];
            realTimeTableBuffer = DataDefine.GetNewStocksRealtimeTable();

            for (int i = 0; i < threadCount; ++i)
            {
                manualEvents[i] = new ManualResetEvent(false);
                subReaders[i] = new SubReader(i * ProgramConfig.ThreadConfig.RoutingReadSize, realTimeTableBuffer);
            }
        }

        public void Start()
        {
            if (isRunning)
            {
                return;
            }
            isRunning = true;
            ReadRealTime();
            if (!ProgramConfig.WebConfig.ConnectMode)
            {
                ReadRealTimeFromDB();
            }
            routineReadTimer.Change(2000, 2000);
        }

        public void Stop()
        {
            isRunning = false;
            routineReadTimer.Change(Timeout.Infinite, 2000);
        }

        private void ThreadCallBack(Object threadLock)
        {
            if (!CommonData.IsTradeTime() || !ProgramConfig.WebConfig.ConnectMode)
            {
                return;
            }
            ReadRealTime();
        }

        private void ReadRealTime()
        {
            realTimeTableBuffer.Clear();
            for (int i = 0; i < threadCount; ++i)
            {
                manualEvents[i].Reset();
                ThreadPool.QueueUserWorkItem(subReaders[i].ThreadCallBack, manualEvents[i]);
            }
            ManualResetEvent.WaitAll(manualEvents);
            realTimeTable = realTimeTableBuffer.Copy();
            realTimeTable.DefaultView.Sort = "stockId ASC";
            realTimeTable = realTimeTable.DefaultView.ToTable();
        }

        private void ReadRealTimeFromDB(){
            realTimeTable = DBHelper.GetRealtime();
        }

        private class SubReader
        {
            int startPos;
            DataTable realTime;
            static Object ol = new Object();

            public SubReader(int _startPos, DataTable _realTime)
            {
                startPos = _startPos;
                realTime = _realTime;
            }

            public void ThreadCallBack(object manualEvent)
            {
                ProgramConfig.APIConfig.ReadRealTime(realTime, startPos, ProgramConfig.ThreadConfig.RoutingReadSize, ol);
                ((ManualResetEvent)manualEvent).Set();
            }
        }
    }

    public class SpecialReader
    {
        private Timer specialReadTimer;
        private bool isRunning;
        private string stockId;
        private bool stockIdChanged;

        private DataTable histories;
        public DataTable Histories
        {
            get
            {
                return histories;
            }
        }

        private DataTable perBids;
        public DataTable Perbid
        {
            get
            {
                return perBids;
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

        public SpecialReader()
        {
            specialReadTimer = new Timer(new TimerCallback(ThreadCallBack), null, Timeout.Infinite, 2000);
            isRunning = false;
            stockIdChanged = true;
            stockId = "-1";
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
                ReadHistory();
                ReadPerbid();
                stockIdChanged = false;
            }
            specialReadTimer.Change(2000, 2000);
        }

        public void Stop()
        {
            isRunning = false;
            specialReadTimer.Change(Timeout.Infinite, 2000);
        }

        private void ThreadCallBack(Object threadLock)
        {
            if (!CommonData.IsTradeTime())
            {
                return;
            }
            ReadPerbid();
        }

        private void ReadHistory()
        {
            histories =  DBHelper.GetHistories(stockId);
        }

        private void ReadPerbid()
        {
            perBids = DBHelper.GetPerbid(stockId);
        }
    }

    public class MultipleReader
    {

    }
}
