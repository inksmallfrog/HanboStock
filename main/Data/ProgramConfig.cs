using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Net;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Data;
using System.Diagnostics;


namespace Data
{
    namespace ProgramConfig
    {
        //=====================================数据库配置=====================================
        public class DBConfig{
            private static string dbName = "StockDB";
            public static string DBName
            {
                get
                {
                    return dbName;
                }
            }

            private static string dbPath = System.Environment.CurrentDirectory;
            public static string DBPath
            {
                get
                {
                    return dbPath;
                }
            }

            private static string initStocksListFile = "stockcode.dat";
            public static string InitStocksListFile
            {
                get
                {
                    return initStocksListFile;
                }
            }
        }


        //=====================================数据配置====================================
        class StockConfig
        {
            private static int oldestYear = 2010;
            public static int OldestYear
            {
                get
                {
                    return oldestYear;
                }
            }
        }
        
        //=====================================线程配置====================================
        public class ThreadConfig
        {
            private static int routingReadSize = 960;
            public static int RoutingReadSize
            {
                get
                {
                    return routingReadSize;
                }
            }
        }

        //=====================================网络配置====================================
        public class WebConfig
        {
            private static bool connectMode = true;
            public static bool ConnectMode
            {
                get
                {
                    return connectMode;
                }
                set
                {
                    connectMode = value;
                }
            }
        }


        //=====================================API配置=====================================
        public class APIConfig{
            private static string realtimeUrl = "http://api.money.126.net/data/feed/";
            public static void ReadRealTime(DataTable table, int start, int size, Object objLock)
            {
                string urlContent = "";
                for (int i = 0; i < size && i + start < CommonData.StocksCode.Rows.Count; ++i)
                {
                    urlContent += CommonData.StocksCode.Rows[i + start]["stockId"].ToString() + ",";
                }
                string url = realtimeUrl + urlContent;

                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url); ;
                System.Net.HttpWebResponse response = null;
                try
                {
                    response = (System.Net.HttpWebResponse)request.GetResponse();
                }
                catch (Exception ex)
                {
                    //网络连接失败
                }
                if (response == null)
                {
                    lock(objLock){
                        if (WebConfig.ConnectMode)
                        {
                            WebConfig.ConnectMode = false;
                            MessageBox.Show("无法连接网络，进入脱机运行模式");
                        }
                    }

                    return;
                }
                System.IO.Stream s = response.GetResponseStream();
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                String json = Reader.ReadToEnd();
                //去除WEB调用格式
                json = json.Replace("_ntes_quote_callback(", "");
                json = json.Replace(");", "");

                JObject obj = null;
                try
                {
                    obj = JObject.Parse(json);
                }
                catch
                {
                    //数据读取失败，可能是API繁忙
                    MessageBox.Show("网络繁忙，加载到无效的json实时数据！" + start);
                }

                foreach (KeyValuePair<string, JToken> kv in obj)
                {
                    JToken currentObj = kv.Value;
                    DataRow current = table.NewRow();

                    current["status"] = (currentObj["status"] == null || currentObj["status"].ToString() == "") ? (int)DataDefine.StockStatus.StockStatusLeave : int.Parse(currentObj["status"].ToString());
                    if (int.Parse(current["status"].ToString()) == (int)DataDefine.StockStatus.StockStatusLeave 
                        || int.Parse(current["status"].ToString()) == (int)DataDefine.StockStatus.StockStatusFundLeave)
                    {
                        continue;
                    }
                    string[] dateTime = currentObj["update"].ToString().Split(' ');
                    current["stockId"] = currentObj["code"].ToString();
                    current["name"] = currentObj["name"].ToString();
                    current["date"] = dateTime[0];
                    current["time"] = dateTime[1];
                    current["openP"] = double.Parse(currentObj["open"].ToString());
                    current["closeP"] = double.Parse(currentObj["yestclose"].ToString());
                    current["price"] = double.Parse(currentObj["price"].ToString());
                    current["high"] = double.Parse(currentObj["high"].ToString());
                    current["low"] = double.Parse(currentObj["low"].ToString());
                    current["volume"] = decimal.Parse(currentObj["volume"].ToString());
                    current["turnover"] = decimal.Parse(currentObj["turnover"].ToString());
                    for (int i = 1; i < 6; ++i)
                    {
                        current["ask" + i + "Price"] = double.Parse(currentObj["ask" + i].ToString());
                        current["ask" + i + "Vol"] = int.Parse(currentObj["askvol" + i].ToString());
                      
                        current["bid" + i + "Price"] = double.Parse(currentObj["bid" + i].ToString());
                        current["bid" + i + "Vol"] = int.Parse(currentObj["bidvol" + i].ToString());
                    }
                    lock(objLock){
                        table.Rows.Add(current);
                    }
                    
                }
            }

            //历史数据获取接口
            private static string historyUrl = "http://web.ifzq.gtimg.cn/appstock/app/kline/kline?_var=kline_day&param=";
            public static void GetHistoriesTable(DataTable table, string stockId)
            {
                string code = (stockId[0] == '1')? "sz" + stockId.Substring(1) : "sh" + stockId.Substring(1);
                string url = historyUrl + code + ",day,,,10000,";
                
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url); ;
                System.Net.HttpWebResponse response = null;
                try
                {
                    response = (System.Net.HttpWebResponse)request.GetResponse();
                }
                catch (Exception ex)
                {
                    //访问的时间不存在
                    return;
                }

                System.IO.Stream s = response.GetResponseStream();
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                String json = Reader.ReadToEnd();
                json = json.Replace("kline_day=", "");
                JObject obj = null;
                try
                {
                    obj = JObject.Parse(json);
                }
                catch (Exception ex)
                {
                    //数据读取失败，可能是API繁忙
                    MessageBox.Show("网络繁忙，加载到无效的json历史数据！" + ex.Message);
                    return;
                }

                JToken histories = obj["data"][code]["day"];
                foreach (JToken history in histories)
                {
                    DataRow stockHistory = table.NewRow();
                    stockHistory["stockId"] = stockId;
                    stockHistory["date"] = history[0];
                    stockHistory["openP"] = double.Parse(history[1].ToString());
                    stockHistory["closeP"] = double.Parse(history[2].ToString());
                    stockHistory["high"] = double.Parse(history[3].ToString());
                    stockHistory["low"] = double.Parse(history[4].ToString());
                    stockHistory["volume"] = decimal.Parse(history[5].ToString());

                    table.Rows.Add(stockHistory);
                }
            }

            //分笔数据接口
            private static string perBidsUrl = "http://vip.stock.finance.sina.com.cn/quotes_service/view/CN_TransListV2.php?num=5000&symbol=";
            public static void GetPerbidTable(DataTable table, string stockId, string startTime)
            {
                string url = perBidsUrl;
                url += (stockId[0] == '1')? "sz" + stockId.Substring(1) : "sh" + stockId.Substring(1);
                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url); ;
                System.Net.HttpWebResponse response = null;
                try
                {
                    response = (System.Net.HttpWebResponse)request.GetResponse();
                }
                catch (Exception ex)
                {
                    //访问的时间不存在
                    return;
                }
                System.IO.Stream s = response.GetResponseStream();
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                string[] jsCodes = Reader.ReadToEnd().Split(';');
                string date = DateTime.Now.ToShortDateString();
                foreach(string jsCode in jsCodes){
                    if(jsCode.Contains("var") || jsCode == "\n") continue;

                    string[] contents = ParseEachPerBid(jsCode);
                    
                    if (contents[2].Equals(startTime))
                    {
                        return;
                    }
                    DataRow stockPerbid = table.NewRow();
                    stockPerbid["stockId"] = stockId;
                    stockPerbid["date"] = date;
                    stockPerbid["time"] = contents[0];
                    stockPerbid["price"] = double.Parse(contents[2]);
                    stockPerbid["vol"] = int.Parse(contents[1]);
                    stockPerbid["type"] = int.Parse(contents[3]);
                    table.Rows.Add(stockPerbid);
                }
            }
            private static string[] ParseEachPerBid(string jsCode){
                string[] contents = new string[4];
                //" trade_item_list[0] = new Array('11:30:00', '200', '10.340', 'UP')";
                string data = jsCode.Substring(jsCode.IndexOf('\''));
                data = data.Substring(0, data.Length - 1);
                contents = data.Replace("'", "").Replace(" ", "").Split(',');
                if (contents[3].Equals("UP"))
                {
                    contents[3] = "1";
                }
                else if (contents[3].Equals("DOWN"))
                {
                    contents[3] = "-1";
                }
                else
                {
                    contents[3] = "0";
                }
                return contents;
            }
        }
    }
}
