using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

namespace Dao.DataAPI
{
    public interface IDataAPI
    {
        void GetRealTimeTable(DataTable resultTable, List<String> needList);
        void GetStockRealTime(DataRow current, String stockId);
        void GetHistoriesTable(DataTable table, string stockId, int dirtaDay);
        void GetPerbidTable(DataTable table, string stockId, int count);
    }
}
