using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Dao
{
    public class DataDefine
    {
        public enum StockStatus
        {
            StockStatusNormal = 0,
            StockStatusLeave = 1,
            StockStatusFundLeave = 2,
            StockStatusPause = 4,
            StockStatusFund = 8
        }

        public enum TradeTimeState
        {
            NotTradeDay = 0,
            BeforeMorning = 1,
            Morning = 2,
            BeforeNoon = 3,
            Noon = 4,
            AfterNoon = 5
        }

        private static DataTable stocksCodeSchema = new DataTable("stocksCodeSchema");
        private static DataTable stocksHistorySchema = new DataTable("stocksHistorySchema");
        private static DataTable stocksPerbidSchema = new DataTable("stocksPerbidSchema");
        private static DataTable stocksPerminutSchema = new DataTable("stocksPerminutSchema");
        private static DataTable stocksRealtimeSchema = new DataTable("stocksRealtimeSchema");
        private static DataTable formulaSchema = new DataTable("formulaSchema");

        public static void InitDataDefine(){
            stocksCodeSchema.Columns.Add("stockId");
            stocksCodeSchema.Columns.Add("name");
            stocksCodeSchema.Columns.Add("code");
            stocksCodeSchema.Columns.Add("type");
            stocksCodeSchema.Columns.Add("subType1");
            stocksCodeSchema.Columns.Add("subType2");
            stocksCodeSchema.Columns.Add("abbr");
            
            stocksHistorySchema.Columns.Add("stockId");
            stocksHistorySchema.Columns.Add("date");
            stocksHistorySchema.Columns.Add("openP");
            stocksHistorySchema.Columns.Add("closeP");
            stocksHistorySchema.Columns.Add("high");
            stocksHistorySchema.Columns.Add("low");
            stocksHistorySchema.Columns.Add("volume");

            stocksPerbidSchema.Columns.Add("stockId");
            stocksPerbidSchema.Columns.Add("date");
            stocksPerbidSchema.Columns.Add("time");
            stocksPerbidSchema.Columns.Add("price");
            stocksPerbidSchema.Columns.Add("vol");
            stocksPerbidSchema.Columns.Add("type");

            stocksPerminutSchema.Columns.Add("stockId");
            stocksPerminutSchema.Columns.Add("date");
            stocksPerminutSchema.Columns.Add("time");
            stocksPerminutSchema.Columns.Add("price");
            stocksPerminutSchema.Columns.Add("vol");

            stocksRealtimeSchema.Columns.Add("stockId");
            stocksRealtimeSchema.Columns.Add("name");
            stocksRealtimeSchema.Columns.Add("date");
            stocksRealtimeSchema.Columns.Add("time");
            stocksRealtimeSchema.Columns.Add("openP");
            stocksRealtimeSchema.Columns.Add("closeP");
            stocksRealtimeSchema.Columns.Add("price");
            stocksRealtimeSchema.Columns.Add("high");
            stocksRealtimeSchema.Columns.Add("low");
            stocksRealtimeSchema.Columns.Add("volume");
            stocksRealtimeSchema.Columns.Add("turnover");
            stocksRealtimeSchema.Columns.Add("status");
            for(int i = 1; i < 6; ++i){
                stocksRealtimeSchema.Columns.Add("ask" + i + "Price");
                stocksRealtimeSchema.Columns.Add("ask" + i + "Vol");

                stocksRealtimeSchema.Columns.Add("bid" + i + "Price");
                stocksRealtimeSchema.Columns.Add("bid" + i + "Vol");
            }

            formulaSchema.Columns.Add("name");
            formulaSchema.Columns.Add("formula");
        }

        public static DataTable GetNewStocksCodeTable(){ return stocksCodeSchema.Clone(); }
        public static DataTable GetNewStocksHistoryTable() { return stocksHistorySchema.Clone(); }
        public static DataTable GetNewStocksPerbidTable() { return stocksPerbidSchema.Clone(); }
        public static DataTable GetNewStocksPerminutTable() { return stocksPerminutSchema.Clone(); }
        public static DataTable GetNewStocksRealtimeTable() { return stocksRealtimeSchema.Clone(); }
        public static DataTable GetNewFormulaTable() { return formulaSchema.Clone(); }


        public struct RealtimeRecord
        {
            public int vol;
            public decimal bid;
            public decimal ask;
        }
        /*public struct StockCode
        {
            public string stockId;
            public string name;
            public string type;
            public string subtype1;
            public string subtype2;
            public string abbr;
        }

        public struct StockHistory
        {
            public string stockId;
            public string date;
            public double open;
            public double close;
            public double high;
            public double low;
            public int  volume;
        }

        public struct StockPerbid
        {
            public string stockId;
            public string date;
            public string time;
            public double price;
            public double updown;
            public int vol;
            public int type;
        }

        public class StockCurrent
        {
            public string stockId;          //股票ID
            public string name;             //股票名字
            public string date;             //日期
            public string time;             //时间
            public double open;             //今日开盘价
            public double close;            //昨日收盘价
            public double price;            //当前价
            public double high;             //今日最高价
            public double low;              //今日最低价
            public decimal volume;              //成交的股票数
            public decimal turnover;        //成交额
            public StockStatus status;

            public double[] askPrice = new double[5];
            public int[] askVol = new int[5];

            public double[] bidPrice = new double[5];
            public int[] bidVol = new int[5];
        }*/
    }
}
