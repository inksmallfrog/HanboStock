using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

using Config.GlobalConfig;

namespace Dao
{
    public class DBHelper
    {
        //使用连建池帮助连接
        private static string conStr = "server=localhost;" +
                                      "Initial Catalog=" + DBConfig.DBName + ";" +
                                      "integrated security=SSPI";
        private static string conAsMasterStr = "Server=localhost;Integrated security=SSPI;database=master";

        //数据表访问锁
        private static Object historyLock = new Object();
        private static Object perBidsLock = new Object();


        //=============================================================================
        //初始化数据库
        public static void Init()
        {
            using(SqlConnection conn = new SqlConnection(conStr)){
                if (!IsExistsDB())
                {
                    CreateDB();
                    try
                    {
                        conn.Open();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("无法打开数据库，请联系开发人员，信息:" + ex.Message);
                        DropDB();
                        Application.Exit();
                    }
                    CreateTables(conn);
                    LoadStocksCode(conn);
                }
            }
        }

        private static bool IsExistsDB()
        {
            string sql = "SELECT * FROM sys.databases WHERE name='" + DBConfig.DBName + "'";
            return (ExecuteSqlGetSingle(sql, true) != null);
        }

        private static void CreateDB()
        {
            string sql = String.Format("CREATE DATABASE {0} ON PRIMARY " +
                                       "(NAME = {0}_DB, " +
                                       "FILENAME = '{1}\\{0}DB.mdf', " +
                                       "SIZE = 50MB, MAXSIZE = 200MB, FILEGROWTH = 10%) " +
                                       "LOG ON (NAME = {0}_Log, " +
                                       "FILENAME = '{1}\\{0}Log.ldf', " +
                                       "SIZE = 100MB, " +
                                       "MAXSIZE = 500MB, " +
                                       "FILEGROWTH = 10%)", 
                                       DBConfig.DBName, DBConfig.DBPath);
            try {
                ExecuteSql(sql, true);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("创建数据库失败，初次运行请使用管理员身份，信息：" + ex.Message);
                Application.Exit();
            }
        }

        public static void DropDB()
        {
            List<string> sqls = new List<string>();
            string sql = "USE MASTER";
            sqls.Add(sql);
            sql = "DECLARE @dbname SYSNAME " +
                    "SET @dbname = '" + DBConfig.DBName + "' " +
                    "DECLARE @s NVARCHAR(1000) " +
                    "DECLARE tb CURSOR LOCAL " +
                    "FOR " +
                    "SELECT s = 'kill ' + CAST(spid AS VARCHAR) " +
                    "FROM MASTER..sysprocesses " +
                    "WHERE dbid = DB_ID(@dbname) " +
                    "OPEN tb " +
                    "FETCH NEXT FROM tb INTO @s " +
                    "WHILE @@fetch_status = 0 " +
                    "BEGIN " +
                    "EXEC (@s) " +
                    "FETCH NEXT FROM tb INTO @s " +
                    "END " +
                    "CLOSE tb " +
                    "DEALLOCATE tb ";
            sqls.Add(sql);
            sql = "Drop database " + DBConfig.DBName;
            sqls.Add(sql);
            try
            {
                ExecuteMultipleSql(sqls, true);
            }
            catch (SqlException ex)
            {
                MessageBox.Show("无法删除数据库，错误信息：" + ex.Message);
            }
            
        }

        private static void CreateTables(SqlConnection conn)
        {
            List<string> sqls = new List<string>();
            string sql;

            sql = "CREATE TABLE stocksCode " +
                "(stockId DECIMAL(7, 0) NOT NULL PRIMARY KEY, name VARCHAR(36) NOT NULL, code INT NOT NULL, " + 
                "type VARCHAR(24) NOT NULL, subtype1 VARCHAR(24), subtype2 VARCHAR(24), abbr VARCHAR(10));";
            sqls.Add(sql);

            sql = "CREATE TYPE stockCode AS TABLE " +
                "(stockId DECIMAL(7, 0), name VARCHAR(36), code INT, type VARCHAR(24), " + 
                "subtype1 VARCHAR(24), subtype2 VARCHAR(24), abbr VARCHAR(10));";
            sqls.Add(sql);

            sql = "CREATE TABLE stocksHistory " +
                "(stockId DECIMAL(7, 0) REFERENCES stocksCode (stockId), date DATE, " + 
                "openP REAL, closeP REAL, high REAL, low REAL, volume DECIMAL, " + 
                "CONSTRAINT PK_HISTORY PRIMARY KEY (date, stockId));";
            sqls.Add(sql);

            sql = "CREATE TYPE stockHistory AS TABLE " +
                "(stockId DECIMAL(7, 0), date DATE, openP REAL, closeP REAL, high REAL, low REAL, volume DECIMAL);";
            sqls.Add(sql);

            sql = "CREATE TABLE stocksPerbid " +
                "(stockId DECIMAL(7, 0) REFERENCES stocksCode (stockId), date DATE, time TIME, price REAL, vol INT, type INT);";
            sqls.Add(sql);

            sql = "CREATE TYPE stockPerbid AS TABLE " +
                  "(stockId DECIMAL(7, 0), date DATE, time TIME, price REAL, vol INT, type INT);";
            sqls.Add(sql);

            sql = "CREATE TABLE stocksRealtime " + 
                "(stockId DECIMAL(7, 0) REFERENCES stocksCode (stockId), name VARCHAR(36), " + 
                "date DATE DEFAULT('1990-01-01'), time TIME DEFAULT('00:00:00'), " + 
                "openP REAL DEFAULT 0.0, closeP REAL DEFAULT 0.0, price REAL DEFAULT 0.0, high REAL DEFAULT 0.0, " + 
                "low REAL DEFAULT 0.0, volume DECIMAL DEFAULT 0.0, turnover DECIMAL DEFAULT 0.0, status INT DEFAULT 0, " + 
                "ask1Price REAL DEFAULT 0.0, ask1Vol INT DEFAULT 0, bid1Price REAL DEFAULT 0.0, bid1Vol INT DEFAULT 0, " + 
                "ask2Price REAL DEFAULT 0.0, ask2Vol INT DEFAULT 0, bid2Price REAL DEFAULT 0.0, bid2Vol INT DEFAULT 0, " + 
                "ask3Price REAL DEFAULT 0.0, ask3Vol INT DEFAULT 0, bid3Price REAL DEFAULT 0.0, bid3Vol INT DEFAULT 0, " + 
                "ask4Price REAL DEFAULT 0.0, ask4Vol INT DEFAULT 0, bid4Price REAL DEFAULT 0.0, bid4Vol INT DEFAULT 0, " + 
                "ask5Price REAL DEFAULT 0.0, ask5Vol INT DEFAULT 0, bid5Price REAL DEFAULT 0.0, bid5Vol INT DEFAULT 0, " + 
                "CONSTRAINT PK_REALTIME PRIMARY KEY (stockId, date, time));";
            sqls.Add(sql);

            sql = "CREATE TYPE stockRealtime AS TABLE " +
                "(stockId DECIMAL(7, 0), name VARCHAR(36), " +
                "date DATE, time TIME, openP REAL, closeP REAL, price REAL, high REAL, " +
                "low REAL, volume DECIMAL, turnover DECIMAL, status INT, ask1Price REAL, " +
                "ask1Vol INT, bid1Price REAL, bid1Vol INT, ask2Price REAL, ask2Vol INT, " +
                "bid2Price REAL, bid2Vol INT, ask3Price REAL, ask3Vol INT, bid3Price REAL, " +
                "bid3Vol INT, ask4Price REAL, ask4Vol INT, bid4Price REAL, bid4Vol INT, " +
                "ask5Price REAL, ask5Vol INT, bid5Price REAL, bid5Vol INT);";
            sqls.Add(sql);

            sql = "CREATE TABLE myStock " +
                "(stockId DECIMAL(7, 0) REFERENCES stocksCode(stockId) PRIMARY KEY);";
            sqls.Add(sql);

            sql = "CREATE TABLE myFormula " +
                "(name VARCHAR(36) NOT NULL PRIMARY KEY, formula VARCHAR(1024));";
            sqls.Add(sql);

            try{
                ExecuteMultipleSql(sqls);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("创建表出错，请联系开发人员，信息：" + ex.Message);
                DropDB();
                Application.Exit();
            }
        }

        private static void LoadStocksCode(SqlConnection conn)
        {
            DataTable stocksCode = new DataTable();
            stocksCode.Columns.AddRange(new DataColumn[]{
                new DataColumn("stockId", typeof(int)),
                new DataColumn("name", typeof(string)),
                new DataColumn("code", typeof(int)),
                new DataColumn("type", typeof(string)),
                new DataColumn("subtype1", typeof(string)),
                new DataColumn("subtype2", typeof(string)),
                new DataColumn("abbr", typeof(string))
            });
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(DBConfig.InitStocksListFile, Encoding.UTF8);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                MessageBox.Show("未找到股票初始信息文件，请确认程序安装完整性！信息：" + ex.Message);
                DropDB();
                Application.Exit();
            }
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                DataRow data = stocksCode.NewRow();
                try
                {
                    String[] contents = line.Split(' ');

                    data[1] = contents[1];
                    data[2] = int.Parse(contents[0].ToString());
                    data[3] = contents[2];
                    data[4] = contents[3];
                    data[5] = contents[4];
                    data[6] = contents[5];

                    if (contents[2].Contains("深圳"))
                    {
                        data[0] = 1000000 + (int)data[2];
                    }
                    else
                    {
                        data[0] = (int)data[2];
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("加载股票列表文件错误，请联系开发人员，信息" + ex.Message);
                    DropDB();
                    Application.Exit();
                }

                stocksCode.Rows.Add(data);
            }
            InsertIntoStocksCode(stocksCode, conn);
        }

        private static void InitValiableStocksCode()
        {

        }

        private static void InsertIntoStocksCode(DataTable stocks, SqlConnection conn)
        {
            //使用这种插入方式能显著提高批量插入的性能
            string sql = "INSERT INTO stocksCode " +
                                "SELECT stockId, name, code, type, subtype1, subtype2, abbr " +
                                "FROM @NewStocksCode;";
            try
            {
                ExecuteSqlWithDataTable(sql, "@NewStocksCode", stocks, "stockCode");
            }
            catch (Exception ex)
            {
                MessageBox.Show("股票列表无法插入数据库！请联系开发人员，信息：" + ex.Message);
                DropDB();
                Application.Exit();
            }
            sql = "INSERT INTO stocksRealtime(stockId, name) " +
                  "     SELECT stocksCode.stockId, stocksCode.name " +
                  "     FROM stocksCode;";
            ExecuteSql(sql);
        }

        public static DataTable GetStocksCode()
        {
            DataTable stocksCode = new DataTable("stocksCode");

            string sql = "SELECT " + 
                         "stockId, name, code, type, subtype1, subtype2, abbr " + 
                         "FROM stocksCode " + 
                         "ORDER BY code;";

            DataSet result = ExecuteSqlGetDataSet(sql);
          
            return result.Tables[0];
        }

        //==============================================================================================
        //Sql逻辑
        private static void ExecuteSql(string sql, bool asMaster = false)
        {
            string connectStr = asMaster ? conAsMasterStr : conStr;
            using (SqlConnection connection = new SqlConnection(connectStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        private static void ExecuteMultipleSql(List<string> sqls, bool asMaster = false)
        {
            string connectStr = asMaster ? conAsMasterStr : conStr;
            using (SqlConnection connection = new SqlConnection(connectStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    try
                    {
                        connection.Open();               
                        foreach (string sql in sqls)
                        {
                            cmd.CommandText = sql;
                            int rows = cmd.ExecuteNonQuery();
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        private static void ExecuteSqlWithDataTable(string sql, string paraName, DataTable table, string typeName, bool asMaster = false)
        {
            string connectStr = asMaster ? conAsMasterStr : conStr;
            using (SqlConnection connection = new SqlConnection(connectStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sql;

                    SqlParameter para = cmd.Parameters.AddWithValue(paraName, table);
                    para.SqlDbType = SqlDbType.Structured;
                    para.TypeName = typeName;

                    try
                    {
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        private static Object ExecuteSqlGetSingle(string sql, bool asMaster = false)
        {
            string connectStr = asMaster ? conAsMasterStr : conStr;
            using (SqlConnection connection = new SqlConnection(connectStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sql;
                    try
                    {
                        connection.Open();
                        return cmd.ExecuteScalar();
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

        private static DataSet ExecuteSqlGetDataSet(string sql, bool asMaster = false)
        {
            string connectStr = asMaster ? conAsMasterStr : conStr;
            DataSet result = null;
            using (SqlConnection connection = new SqlConnection(connectStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = sql;
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    result = new DataSet();
                    try
                    {
                        connection.Open();
                        adapter.SelectCommand.ExecuteNonQuery();
                        adapter.Fill(result);
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        connection.Close();
                        throw e;
                    }
                }
            }
            return result;
        }

        //==============================================================================================
        //单次访问

        public static void RemoveStockCode(String stockId)
        {
            string sql_delete = "DELETE FROM stocksRealtime WHERE stockId = " + stockId + ";";
            ExecuteSql(sql_delete);
            sql_delete = "DELETE FROM stocksHistory WHERE stockId = " + stockId + ";";
            ExecuteSql(sql_delete);
            sql_delete = "DELETE FROM stocksPerbid WHERE stockId = " + stockId + ";";
            ExecuteSql(sql_delete);
            sql_delete = "DELETE FROM stocksCode WHERE stockId = " + stockId + ";";
            ExecuteSql(sql_delete);
        }
        //------------------------------------------------------------
        //读

        /*public static string GetName(string stockId)
        {
            string sql = "SELECT name " +
                         "FROM stocksCode " +
                         "WHERE stockId = " + int.Parse(stockId) + ";";
            
            return (string)ExecuteSqlGetSingle(sql);
        }*/

        public static DataTable GetRealtime()
        {
            DataTable realtime = new DataTable();

            string sql = "SELECT " +
                         "stocksRealtime.stockId, stocksRealtime.name, CONVERT(char(10), date,126) date, time, openP, closeP, price, high, " +
                         "low, volume, turnover, status, ask1Price, ask1Vol, bid1Price, bid1Vol, ask2Price, ask2Vol, " +
                         "bid2Price, bid2Vol, ask3Price, ask3Vol, bid3Price, bid3Vol, ask4Price, ask4Vol, bid4Price, bid4Vol, " +
                         "ask5Price, ask5Vol, bid5Price, bid5Vol " + 
                         "FROM stocksRealtime, stocksCode " + 
                         "WHERE stocksRealtime.stockId = stocksCode.stockId " + 
                         "ORDER BY stocksCode.code ASC;";

            DataSet result = ExecuteSqlGetDataSet(sql);

            return result.Tables[0];
        }

        public static DataRow GetStockRealTime(String stockId)
        {
            DataTable realtime = new DataTable();

            string sql = "SELECT " +
                         "stocksRealtime.stockId, stocksRealtime.name, CONVERT(char(10), date,126) date, time, openP, closeP, price, high, " +
                         "low, volume, turnover, status, ask1Price, ask1Vol, bid1Price, bid1Vol, ask2Price, ask2Vol, " +
                         "bid2Price, bid2Vol, ask3Price, ask3Vol, bid3Price, bid3Vol, ask4Price, ask4Vol, bid4Price, bid4Vol, " +
                         "ask5Price, ask5Vol, bid5Price, bid5Vol " +
                         "FROM stocksRealtime " +
                         "WHERE stocksRealtime.stockId=" + stockId;

            DataSet result = ExecuteSqlGetDataSet(sql);

            return result.Tables[0].Rows[0];
        }

        public static String GetNewestHistoryDate(string stockId)
        {
            string sql = "SELECT TOP 1 date " +
                         "FROM stocksHistory " +
                         "WHERE stockId = " + stockId + " " +
                         "ORDER BY date DESC;";
            object result = ExecuteSqlGetSingle(sql);
            return (result != null) ? result.ToString() : "";
        }

        public static string GetNewestPerbidTime(string stockId)
        {
            string sql = "SELECT TOP 1 time " +
                         "FROM stocksPerbid " +
                         "WHERE stockId = " + stockId + " " +
                         "ORDER BY time DESC;";
            object result = ExecuteSqlGetSingle(sql);
            return (result != null) ? result.ToString() : "09:20:00";
        }

        public static DataTable GetHistories(string stockId)
        {
            DataTable histories = new DataTable("histories" + stockId);

            string sql = "SELECT " +
                         "CONVERT(char(10), date,126) date, openP, closeP, high, low, volume " +
                         "FROM stocksHistory " +
                         "WHERE stocksHistory.stockId = " + stockId + " " +
                         "ORDER BY date ASC;";

            DataSet result = ExecuteSqlGetDataSet(sql);

            return result.Tables[0];
        }

        public static DataTable GetPerbid(string stockId)
        {
            DataTable perBids = new DataTable("perBids" + stockId);

            string sql = "SELECT " +
                         "CONVERT(char(10), date,126) date, time, price, vol, type " +
                         "FROM stocksPerbid " +
                         "WHERE stocksPerbid.stockId = " + stockId + " " +
                         "ORDER BY time ASC;";

            DataSet result = ExecuteSqlGetDataSet(sql);

            return result.Tables[0];
        }

        public static DataTable GetFormula()
        {
            string sql = "SELECT " +
                         "name, formula " +
                         "FROM myFormula;";

            DataSet result = ExecuteSqlGetDataSet(sql);

            return result.Tables[0];
        }

        //--------------------------------------------------------------------
        //写
        public static void SaveFormula(String name, String formula)
        {
            string sql = "INSERT INTO " +
                        "myFormula(name, formula) " +
                        "VALUES('" + name + "', '" + formula + "');";
            ExecuteSql(sql);
        }

        public static void UpdateRealtime(DataTable realtime)
        {
            string sql = "DELETE FROM stocksRealtime;";
            ExecuteSql(sql);
            Thread.Sleep(100);
            sql = "INSERT INTO " +
                  "stocksRealtime " +
                  "SELECT " +
                  "* " +
                  "FROM @NewRealtimeTable;";
            ExecuteSqlWithDataTable(sql, "@NewRealtimeTable", realtime, "stockRealtime");
        }

        public static void InsertIntoHistory(DataTable histories)
        {
            //批量插入并去除重复部分，这里子查询可能影响性能，应继续考虑性能优化
            string sql = "INSERT INTO " +
                                "stocksHistory " +
                                "SELECT " +
                                "stockId, date, openP, closeP, high, low, volume " +
                                "FROM @NewStocksHistory nt " +
                                "WHERE NOT EXISTS( " +
                                "                   SELECT stockId " +
                                "                   FROM stocksHistory " +
                                "                   WHERE stocksHistory.stockId = nt.stockId " +
                                "                   AND   stocksHistory.date = nt.date " +
                                "                 );";
            ExecuteSqlWithDataTable(sql, "@NewStocksHistory", histories, "stockHistory");
        }

        public static void CleanOldPerbidData(string currentTradeDate)
        {
            string sql = "DELETE FROM stocksPerbid " + 
                         "WHERE EXISTS ( " + 
                         "                     SELECT TOP 1 stockId " + 
                         "                     FROM stocksPerbid " + 
                         "                     WHERE date < '" + currentTradeDate + "' " + 
                         "                 );";
            ExecuteSql(sql);
        }

        public static void InsertIntoPerbid(DataTable stockPerbid){
            if (stockPerbid.Rows.Count == 0)
            {
                return;
            }
            string sql = "INSERT INTO stocksPerbid " +
                            "SELECT " +
                            "stockId, date, time, " +
                            "price, vol, type " +
                            "FROM @NewStocksPerbid nt " +
                            "WHERE NOT EXISTS( " +
                            "                   SELECT stockId " +
                            "                   FROM stocksPerbid " +
                            "                   WHERE stocksPerbid.stockId = nt.stockId " +
                            "                   AND   stocksPerbid.time = nt.time " +
                            "                 );";
            try{
                lock (perBidsLock)
                {
                    ExecuteSqlWithDataTable(sql, "@NewStocksPerbid", stockPerbid, "stockPerbid");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("插入分笔数据失败，请联系开发人员，信息：" + ex.Message);
            }
        }
    }
}
