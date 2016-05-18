using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Data
{
    public class CommonData
    {
        private static TimeSpan tradeMorningOpen;
        private static TimeSpan tradeMorningClose;
        private static TimeSpan tradeNoonOpen;
        private static TimeSpan tradeNoonClose;

        private static DataTable stocksCode = null;
        public static DataTable StocksCode
        {
            get
            {
                return stocksCode;
            }
        }

        public static void InitCommonData()
        {
            tradeMorningOpen = new TimeSpan(9, 30, 0);
            tradeMorningClose = new TimeSpan(11, 30, 0);
            tradeNoonOpen = new TimeSpan(13, 0, 0);
            tradeNoonClose = new TimeSpan(15, 0, 0);
            InitStocksCode();
        }

        private static void InitStocksCode(){
            if (stocksCode != null)
            {
                return;
            }
            stocksCode = DBHelper.GetStocksCode();
        }

        public static DataDefine.TradeTimeState TradeTimeState()
        {
            DateTime now = DateTime.Now;
            if (now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
            {
                return DataDefine.TradeTimeState.NotTradeDay;
            }
            if (now.TimeOfDay < tradeMorningOpen)
            {
                return DataDefine.TradeTimeState.BeforeMorning;
            }
            if (now.TimeOfDay >= tradeMorningOpen && now.TimeOfDay <= tradeMorningClose)
            {
                return DataDefine.TradeTimeState.Morning;
            }
            if (now.TimeOfDay > tradeMorningClose && now.TimeOfDay < tradeNoonOpen)
            {
                return DataDefine.TradeTimeState.BeforeNoon;
            }
            if (now.TimeOfDay >= tradeNoonOpen && now.TimeOfDay <= tradeNoonClose)
            {
                return DataDefine.TradeTimeState.Noon;
            }
           
            return DataDefine.TradeTimeState.AfterNoon;
        }

        public static bool IsTradeTime()
        {
            switch (TradeTimeState())
            {
                case DataDefine.TradeTimeState.AfterNoon:
                case DataDefine.TradeTimeState.BeforeMorning:
                case DataDefine.TradeTimeState.BeforeNoon:
                case DataDefine.TradeTimeState.NotTradeDay:
                    return false;
            }
            return true;
        }
    }
}
