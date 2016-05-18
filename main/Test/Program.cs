using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;
using Data;



namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DBHelper.Init();
            CommonData.InitCommonData();

            DataReader.InvokeRoutingReader();

            Stopwatch sw = new Stopwatch();
            sw.Start();
            DataLoader.InvokeSpecialLoader("0600009");
            DataReader.InvokeSpecialReader("0600009");
            sw.Stop();

            System.Console.WriteLine(sw.ElapsedMilliseconds);
            List<DataDefine.StockCurrent> sc = CommonData.RealTimeList;

        }
    }
}
