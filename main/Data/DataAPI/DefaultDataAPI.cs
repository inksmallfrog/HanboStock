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
using System.Diagnostics;

using Config.GlobalConfig;

namespace Dao.DataAPI
{
    public class DefaultDataAPI : IDataAPI
    {
        //网易接口
        private String realtimeUrl = "http://api.money.126.net/data/feed/";

        //？
        private String historyUrl = "http://web.ifzq.gtimg.cn/appstock/app/kline/kline?_var=kline_day&param=";

        //新浪接口
        private String perBidsUrl = "http://vip.stock.finance.sina.com.cn/quotes_service/view/CN_TransListV2.php?symbol=";

        void IDataAPI.GetRealTimeTable(DataTable resultTable, List<String> needList)
        {
            string urlContent = "";
            foreach (String stockId in needList)
            {
                urlContent += stockId.PadLeft(7, '0') + ",";
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
                if (WebConfig.ConnectMode)
                {
                    WebConfig.ConnectMode = false;
                    MessageBox.Show("无法连接网络，进入脱机运行模式");
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
                if (WebConfig.ConnectMode)
                {
                    WebConfig.ConnectMode = false;
                    MessageBox.Show("无法连接网络，进入脱机运行模式");
                }

                return;
            }

            foreach (KeyValuePair<string, JToken> kv in obj)
            {
                JToken currentObj = kv.Value;
                DataRow current = resultTable.NewRow();

                current["status"] = (currentObj["status"] == null || currentObj["status"].ToString() == "") ? (int)DataDefine.StockStatus.StockStatusLeave : int.Parse(currentObj["status"].ToString());
                if (int.Parse(current["status"].ToString()) == (int)DataDefine.StockStatus.StockStatusLeave
                    || int.Parse(current["status"].ToString()) == (int)DataDefine.StockStatus.StockStatusFundLeave)
                {
                    DBHelper.RemoveStockCode(currentObj["code"].ToString());
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
                
                resultTable.Rows.Add(current);    
            }
        }

        void IDataAPI.GetStockRealTime(DataRow current, String stockId)
        {
            string url = realtimeUrl + stockId.PadLeft(7, '0') + ",";

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
                if (WebConfig.ConnectMode)
                {
                    WebConfig.ConnectMode = false;
                    MessageBox.Show("无法连接网络，进入脱机运行模式");
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
                MessageBox.Show("网络繁忙，加载到无效的json实时数据！");
            }

            JToken currentObj = obj[stockId];

            current["status"] = (currentObj["status"] == null || currentObj["status"].ToString() == "") ? (int)DataDefine.StockStatus.StockStatusLeave : int.Parse(currentObj["status"].ToString());
            if (int.Parse(current["status"].ToString()) == (int)DataDefine.StockStatus.StockStatusLeave
                || int.Parse(current["status"].ToString()) == (int)DataDefine.StockStatus.StockStatusFundLeave)
            {
                DBHelper.RemoveStockCode(currentObj["code"].ToString());
                return;
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
            
        }
        void IDataAPI.GetHistoriesTable(System.Data.DataTable table, string stockId, int dirtaDay)
        {
            dirtaDay = (dirtaDay == -1) ? 10000 : dirtaDay;
            string code = (stockId[0] == '1') ? "sz" + stockId.Substring(1) : "sh" + stockId.Substring(1);
            string url = historyUrl + code + ",day,,," + dirtaDay + ",";

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

        void IDataAPI.GetPerbidTable(System.Data.DataTable table, string stockId, int count)
        {
            string url = perBidsUrl;
            url += (stockId[0] == '1') ? "sz" + stockId.Substring(1) : "sh" + stockId.Substring(1);
            if (count != -1)
            {
                url += "&num=" + count;
            }
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
            foreach (string jsCode in jsCodes)
            {
                if (jsCode.Contains("var") || jsCode == "\n") continue;

                string[] contents = ParseEachPerBid(jsCode);

                if (int.Parse(contents[1]) == 0) continue;

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
        private static string[] ParseEachPerBid(string jsCode)
        {
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

    public class SinaDataAPI : IDataAPI
    {
        //网易接口
        private String realtimeUrl = "http://hq.sinajs.cn/rn=spgy2&list=";

        //腾讯
        private String historyUrl = "http://web.ifzq.gtimg.cn/appstock/app/kline/kline?_var=kline_day&param=";

        //新浪接口
        private String perBidsUrl = "http://vip.stock.finance.sina.com.cn/quotes_service/view/CN_TransListV2.php?symbol=";

        void IDataAPI.GetRealTimeTable(DataTable resultTable, List<String> needList)
        {
            string urlContent = "";
            foreach (String stockId in needList)
            {
                urlContent += (stockId[0] == '1') ? "sz" + stockId.Substring(1) : "sh" + stockId.Substring(1);
                urlContent += ",";
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
                if (WebConfig.ConnectMode)
                {
                    WebConfig.ConnectMode = false;
                    MessageBox.Show("无法连接网络，进入脱机运行模式");
                }

                return;
            }

            System.IO.Stream s = response.GetResponseStream();
            StreamReader Reader = new StreamReader(s, Encoding.GetEncoding("GB2312"));

            string[] stocks = Reader.ReadToEnd().Replace("\n", "").Split(';');
            foreach (string stock in stocks)
            {
                if (stock == "") continue;
                DataRow current = resultTable.NewRow();
                ParseRealtime(stock, current);
                resultTable.Rows.Add(current);
            }
        }

        void IDataAPI.GetStockRealTime(DataRow current, String stockId)
        {
            string url = realtimeUrl + ((stockId[0] == '1') ? ("sz" + stockId.Substring(1)) : ("sh" + stockId));

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
                if (WebConfig.ConnectMode)
                {
                    WebConfig.ConnectMode = false;
                    MessageBox.Show("无法连接网络，进入脱机运行模式");
                }

                return;
            }

            System.IO.Stream s = response.GetResponseStream();
            StreamReader Reader = new StreamReader(s, Encoding.GetEncoding("GB2312"));

            string[] stocks = Reader.ReadToEnd().Replace("\n", "").Split(';');
            foreach (string stock in stocks)
            {
                if (stock == "") continue;
                ParseRealtime(stock, current);
            }

        }
        void ParseRealtime(string stock, DataRow current)
        {
            string stockId = stock.Split('=')[0];
            stockId = stockId.Replace("var hq_str_sz", "1");
            stockId = stockId.Replace("var hq_str_sh", "0");

            current["stockId"] = stockId;

            string data = stock.Split('=')[1];
            data = data.Replace("\"", "");
            string[] result = data.Split(',');

            if(result.Length < 2)
            {
                DBHelper.RemoveStockCode(stockId);
                return;
            }

            current["name"] = result[0];
            current["date"] = result[30];
            current["time"] = result[31];
            current["openP"] = result[1];
            current["closeP"] = result[2];
            current["price"] = result[3];
            current["high"] = result[4];
            current["low"] = result[5];
            current["volume"] = result[8];
            current["turnover"] = result[9];
            if(result[32] == "03")
            {
                current["status"] = 4;
            }
            else
            {
                current["status"] = 0;
            }

            for (int i = 1; i < 6; ++i)
            {
                current["bid" + i + "Price"] = result[11 + (i - 1) * 2];
                current["bid" + i + "Vol"] = result[10 + (i - 1) * 2];

                current["ask" + i + "Price"] = result[21 + (i - 1) * 2];
                current["ask" + i + "Vol"] = result[20 + (i - 1) * 2];
            }
        }

        void IDataAPI.GetHistoriesTable(System.Data.DataTable table, string stockId, int dirtaDay)
        {
            dirtaDay = (dirtaDay == -1) ? 10000 : dirtaDay;
            string code = (stockId[0] == '1') ? "sz" + stockId.Substring(1) : "sh" + stockId;
            string url = historyUrl + code + ",day,,," + dirtaDay + ",";

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

        void IDataAPI.GetPerbidTable(System.Data.DataTable table, string stockId, int count)
        {
            string url = perBidsUrl;
            url += (stockId[0] == '1') ? "sz" + stockId.Substring(1) : "sh" + stockId;
            if (count != -1)
            {
                url += "&num=" + count;
            }
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
            foreach (string jsCode in jsCodes)
            {
                if (jsCode.Contains("var") || jsCode == "\n") continue;

                string[] contents = ParseEachPerBid(jsCode);


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
        private static string[] ParseEachPerBid(string jsCode)
        {
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
