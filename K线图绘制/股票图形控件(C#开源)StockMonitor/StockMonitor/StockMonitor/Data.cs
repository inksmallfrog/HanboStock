using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Data;
using Newtonsoft.Json.Linq;
using MySql.Data.MySqlClient;
using MySql.Data.Types;

namespace StockMonitor
{
    public struct HistoryStockData{
        public string date;          //日期
        public double openPrice;     //开盘价
        public double closePrice;    //收盘价
        public double highestPrice;  //最高价
        public double lowestPrice;   //最低价
        public int volume;           //交易额 
    }

    public struct CurrentStockData
    {
        public string name;             //股票名字
        public string code;             //股票代码
        public string date;             //日期
        public string time;             //时间
        public double openningPrice;    //今日开盘价
        public double closingPrice;     //昨日收盘价
        public double currentPrice;     //当前价
        public double hightestPrice;    //今日最高价
        public double lowestPrice;      //今日最低价
        public double competitivePrice; //买一报价
        public double auctionPrice;     //卖一报价
        public int totalNumber;         //成交的股票数
        public double turnover;         //成交额
        public double increase;         //增长（可以是负的）
        public int buyOne;              //下面就不说了
        public double buyOnePrice;
        public int buyTwo;
        public double buyTwoPrice;
        public int buyThree;
        public double buyThreePrice;
        public int buyFour;
        public double buyFourPrice;
        public int buyFive;
        public double buyFivePrice;
        public int sellOne;
        public double sellOnePrice;
        public int sellTwo;
        public double sellTwoPrice;
        public int sellThree;
        public double sellThreePrice;
        public int sellFour;
        public double sellFourPrice;
        public int sellFive;
        public double sellFivePrice;
    }


    class Data
    {
        // 判断name是否为空来判断是否获取成功
        public static CurrentStockData getCurrentData(string stockCode){
            var data = new CurrentStockData();
            stockCode = stockCode.ToLower();
            string url = "http://apis.baidu.com/apistore/stockservice/stock?stockid=" + stockCode + "&list=1";

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("apikey", "50d2a55d48e3d9d157d0df783cc01ce2");

            System.Net.HttpWebResponse response;
            try
            {
                response = (System.Net.HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                data.name = "";
                return new CurrentStockData();
            }
            
            System.IO.Stream s = response.GetResponseStream();

            StreamReader Reader = new StreamReader(s, Encoding.UTF8);

            
            var obj = JObject.Parse(Reader.ReadToEnd());

            if (obj["retData"]["stockinfo"][0]["name"].ToString() != "")
            {
                try
                {
                    obj = JObject.Parse((obj["retData"]["stockinfo"][0]).ToString());
                    data.name = obj["name"].ToString();
                    data.code = obj["code"].ToString();
                    data.date = obj["date"].ToString();
                    data.time = obj["time"].ToString();
                    data.openningPrice = double.Parse(obj["OpenningPrice"].ToString());
                    data.closingPrice = double.Parse(obj["closingPrice"].ToString());
                    data.currentPrice = double.Parse(obj["currentPrice"].ToString());
                    data.hightestPrice = double.Parse(obj["hPrice"].ToString());
                    data.lowestPrice = double.Parse(obj["lPrice"].ToString());
                    data.competitivePrice = double.Parse(obj["competitivePrice"].ToString());
                    data.auctionPrice = double.Parse(obj["auctionPrice"].ToString());
                    data.totalNumber = int.Parse(obj["totalNumber"].ToString());
                    data.turnover = int.Parse(obj["turnover"].ToString());
                    data.increase = double.Parse(obj["increase"].ToString());
                    data.buyOne = int.Parse(obj["buyOne"].ToString());
                    data.buyOnePrice = double.Parse(obj["buyOnePrice"].ToString());
                    data.buyTwo = int.Parse(obj["buyTwo"].ToString());
                    data.buyTwoPrice = double.Parse(obj["buyTwoPrice"].ToString());
                    data.buyThree = int.Parse(obj["buyThree"].ToString());
                    data.buyThreePrice = double.Parse(obj["buyThreePrice"].ToString());
                    data.buyFour = int.Parse(obj["buyFour"].ToString());
                    data.buyFourPrice = double.Parse(obj["buyFourPrice"].ToString());
                    data.buyFive = int.Parse(obj["buyFive"].ToString());
                    data.buyFivePrice = double.Parse(obj["buyFivePrice"].ToString());

                    data.sellOne = int.Parse(obj["sellOne"].ToString());
                    data.sellOnePrice = double.Parse(obj["sellOnePrice"].ToString());
                    data.sellTwo = int.Parse(obj["sellTwo"].ToString());
                    data.sellTwoPrice = double.Parse(obj["sellTwoPrice"].ToString());
                    data.sellThree = int.Parse(obj["sellThree"].ToString());
                    data.sellThreePrice = double.Parse(obj["sellThreePrice"].ToString());
                    data.sellFour = int.Parse(obj["sellFour"].ToString());
                    data.sellFourPrice = double.Parse(obj["sellFourPrice"].ToString());
                    data.sellFive = int.Parse(obj["sellFive"].ToString());
                    data.sellFivePrice = double.Parse(obj["sellFivePrice"].ToString());
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Exception");
                    return new CurrentStockData();
                }
                
            }

            return data;
        }

        // 判断list长度是否为0来判断是否获取成功
        public static List<HistoryStockData> getHistoryData(string stockCode, int dayNumber)
        {
            stockCode = stockCode.ToLower();
            
            List<HistoryStockData> data = new List<HistoryStockData>();

            string url = "http://table.finance.yahoo.com/table.csv?s=" + changeCode(stockCode);

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("apikey", "50d2a55d48e3d9d157d0df783cc01ce2");

            System.Net.HttpWebResponse response;
            try
            {
                response = (System.Net.HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                return new List<HistoryStockData>();
            }

            StreamReader Reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            Reader.ReadLine();

            string[] s; HistoryStockData stockData;
            for (int i = 0; i < dayNumber; i++)
            {
                s = Reader.ReadLine().Split(',');

                stockData = new HistoryStockData();
                stockData.date = s[0];
                stockData.openPrice = double.Parse(s[1]);
                stockData.closePrice = double.Parse(s[2]);
                stockData.highestPrice = double.Parse(s[3]);
                stockData.lowestPrice = double.Parse(s[4]);
                stockData.volume = int.Parse(s[5]);

                data.Add(stockData);
            }
            return data;
        }

        // 判断list长度是否为0来判断是否获取成功,path是数据文件夹路径，如 "C:\data" 严格按照windows资源管理器来
        public static List<HistoryStockData> getHistoryDataAndSaveToFile(string stockCode, int dayNumber, string path)
        {
            stockCode = stockCode.ToLower();

            List<HistoryStockData> data = new List<HistoryStockData>();

            string url = "http://table.finance.yahoo.com/table.csv?s=" + changeCode(stockCode);

            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Headers.Add("apikey", "50d2a55d48e3d9d157d0df783cc01ce2");

            System.Net.HttpWebResponse response;
            try
            {
                response = (System.Net.HttpWebResponse)request.GetResponse();
            }
            catch (Exception ex)
            {
                return new List<HistoryStockData>();
            }

            StreamReader Reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);

            Reader.ReadLine();

            string[] s; HistoryStockData stockData;
            for (int i = 0; i < dayNumber; i++)
            {
                s = Reader.ReadLine().Split(',');

                stockData = new HistoryStockData();
                stockData.date = s[0];
                stockData.openPrice = double.Parse(s[1]);
                stockData.closePrice = double.Parse(s[3]);
                stockData.highestPrice = double.Parse(s[4]);
                stockData.lowestPrice = double.Parse(s[2]);
                stockData.volume = int.Parse(s[5]);

                data.Add(stockData);
            }

            reserveToFile(stockCode, data, path);

            return data;
        }

        public static void reserveToFile(string code, List<HistoryStockData> data, string path)
        {
            using (StreamWriter sw = new StreamWriter(path+String.Format("\\{0}.csv", code)))
            {
                for (int i = 0; i < data.Count; i++)
                {
                    sw.Write(data[i].date.ToString() + ",");
                    sw.Write(data[i].openPrice.ToString() + ",");
                    sw.Write(data[i].closePrice.ToString() + ",");
                    sw.Write(data[i].highestPrice.ToString() + ",");
                    sw.Write(data[i].lowestPrice.ToString() + ",");
                    sw.Write(data[i].volume.ToString() + "\n");
                }

                sw.Flush();
                sw.Close();
            }
        }

        public static void reserveToDatabase(string code, List<HistoryStockData> data)
        {
            string mysql_str = "server=localhost; User ID=root; Password=123; database=stock_data";
            MySqlConnection con = new MySqlConnection(mysql_str);
            con.Open();

            MySqlCommand com; string sql;

            sql = String.Format("create table if not exists {0}(date varchar(20) not null,open float(7,3) not null, close float(7,3) not null,high float(7,3) not null, low float(7,3) not null, volume int not null, primary key (date))", code);
            com = new MySqlCommand(sql, con);
            com.ExecuteNonQuery();

            for (int i = 0; i < data.Count; i++)
            {
                sql = String.Format("insert into {0}(date, open, close, high, low, volume) select '{1}' as date, {2} as open, {3} as close, {4} as high, {5} as low, {6} as volume where not exists (select date from {7} where date='{8}')", code, data[i].date, data[i].openPrice, data[i].closePrice, data[i].highestPrice, data[i].lowestPrice, data[i].volume, code, data[i].date);
                com = new MySqlCommand(sql, con);
                com.ExecuteNonQuery();
            }
            con.Close();
        }

        // 从数据库获取的是旧数据，不一定是最新的,如果数据库的内容少于请求，返回所有数据
        // 判断list长度是否为0来判断是否获取成功
        public static List<HistoryStockData> getHistoryDataFormDatabase(string stockCode, int dayNumber){
            string mysql_str = "server=localhost; User ID=root; Password=123; database=stock_data";
            MySqlConnection con = new MySqlConnection(mysql_str);
            con.Open();

            string sql;
            sql = String.Format("select * from {0}",stockCode);
            MySqlDataAdapter da = new MySqlDataAdapter(sql, con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            DataTable dt = ds.Tables[0];

            List<HistoryStockData> data = new List<HistoryStockData>();
            HistoryStockData stockData;
            if (dt.Rows.Count < dayNumber) dayNumber = dt.Rows.Count;

            for (int i = dayNumber-1; i >= 0; i--)
            {
                DataRow dr = dt.Rows[i];

                stockData = new HistoryStockData();
                stockData.date = dr[0].ToString();
                stockData.openPrice = double.Parse(dr[1].ToString());
                stockData.closePrice = double.Parse(dr[2].ToString());
                stockData.highestPrice = double.Parse(dr[3].ToString());
                stockData.lowestPrice = double.Parse(dr[4].ToString());
                stockData.volume = int.Parse(dr[5].ToString());

                data.Add(stockData);
            }

                con.Close();

            return data;
        }

        // 从数据库获取的是旧数据，不一定是最新的,如果数据库的内容少于请求，返回所有数据
        // 判断list长度是否为0来判断是否获取成功, path是数据文件夹路径，如 "C:\data" 用反斜杠
        public static List<HistoryStockData> getHistoryDataFromFile(string stockCode, int dayNumber,string path)
        {
            List<HistoryStockData> data = new List<HistoryStockData>();
            using (StreamReader sr = new StreamReader(path + String.Format("\\{0}.csv", stockCode), Encoding.UTF8))
            {
                string sh; int i = 0;
                
                string[] s; HistoryStockData stockData;
                while (i < dayNumber && (sh = sr.ReadLine()) != null)
                {
                    s = sh.Split(',');

                    stockData = new HistoryStockData();
                    stockData.date = s[0];
                    stockData.openPrice = double.Parse(s[1]);
                    stockData.closePrice = double.Parse(s[3]);
                    stockData.highestPrice = double.Parse(s[4]);
                    stockData.lowestPrice = double.Parse(s[2]);
                    stockData.volume = int.Parse(s[5]);

                    data.Add(stockData);
                    i++;
                }
            }
            return data;
        }

        public static string changeCode(string code)
        {
            string s = "";
            if (code.Contains("sz"))
            {
                s = code.Substring(2, code.Length - 2) + ".sz";
            }
            if (code.Contains("sh"))
            {
                s = code.Substring(2, code.Length - 2) + ".ss";
            }
            return s;
        }
    }
}
