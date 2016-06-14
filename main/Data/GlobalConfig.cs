using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;

using Dao.DataAPI;

namespace Config
{
    namespace GlobalConfig
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
            private static int oldestYear = 1990;
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
            private static DataAPIFactory.APIType apiType = DataAPIFactory.APIType.DefaultAPI;
            public static DataAPIFactory.APIType ApiType
            {
                get
                {
                    return apiType;
                }
                set
                {
                    apiType = value;
                }
            }
        }
    }
}
