using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net;
using Newtonsoft.Json.Linq;

namespace StockLoader
{
    class Program
    {
        static List<String> stocksCode = new List<String>();
        static int dataForEachThread = 1024;
        static int threadCount;

        static void Main(string[] args)
        {
            LoadStocksCode();
            LoadData();
        }

        static void LoadStocksCode()
        {
            StreamReader sr = new StreamReader(".\stockcode.dat", Encoding.Default);
            String line;
            while ((line = sr.ReadLine()) != null){
                stocksCode.Add(line);
            }
        }

        static void LoadData()
        {
            threadCount = (int)Math.Ceiling(stocksCode.Count / (dataForEachThread * 1.0));
            for(int i = 0; i < threadCount; ++i){
                ThreadPool.QueueUserWorkItem(new WaitCallback(Loader), i * 1024);
            }
        }

        static private void Loader(object data_start){
            while(true){
                String urlContent = "";
                for(int current = (int)data_start; current < (int)data_start + 1024 && current < stocksCode.Count; ++current){
                    urlContent += stocksCode[current] + ",";
                }
                String url = "http://api.money.126.net/data/feed/" + urlContent + "money.api";

                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url);;
                    
                System.Net.HttpWebResponse response;
                try
                {
                    response = (System.Net.HttpWebResponse)request.GetResponse();
                }
                catch (Exception ex)
                {
                    
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
            }
        }
    }
}
