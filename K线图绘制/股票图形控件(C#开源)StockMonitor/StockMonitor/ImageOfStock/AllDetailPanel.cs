using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageOfStock
{
    public partial class AllDetailPanel : UserControl
    {
        private int startPos = -1;

        public AllDetailPanel()
        {
            InitializeComponent();

            perbidList1.ListMaxSize = Config.GlobalConfig.StockConfig.PerbidListMaxsize;
            perbidList2.ListMaxSize = Config.GlobalConfig.StockConfig.PerbidListMaxsize;
            perbidList3.ListMaxSize = Config.GlobalConfig.StockConfig.PerbidListMaxsize;
            perbidList4.ListMaxSize = Config.GlobalConfig.StockConfig.PerbidListMaxsize;
        }

        public void BindData(DataTable data, DataRow realtime)
        {
            if(data.Rows.Count / (Config.GlobalConfig.StockConfig.PerbidListMaxsize * 4) > 0 && startPos == -1)
            {
                startPos = data.Rows.Count - Config.GlobalConfig.StockConfig.PerbidListMaxsize * 4;
            }
            else if(startPos == -1)
            {
                startPos = 0;
            }
            string code = realtime["stockId"].ToString();
            stockId.Text = (code.Length == 6) ? code : code.Substring(1);
            stockName.Text = realtime["name"].ToString();

            perbidList1.BindData(data, realtime, startPos);
            perbidList2.BindData(data, realtime, startPos + Config.GlobalConfig.StockConfig.PerbidListMaxsize * 1);
            perbidList3.BindData(data, realtime, startPos + Config.GlobalConfig.StockConfig.PerbidListMaxsize * 2);
            perbidList4.BindData(data, realtime, startPos + Config.GlobalConfig.StockConfig.PerbidListMaxsize * 3);
        }
        public void SetMouseWheel(MouseEventHandler handler)
        {
            perbidList1.MouseWheel += handler;
            perbidList2.MouseWheel += handler;
            perbidList3.MouseWheel += handler;
            perbidList4.MouseWheel += handler;
        }
        public void LastPage()
        {
            if(startPos > 0)
            {
                startPos -= Config.GlobalConfig.StockConfig.PerbidListMaxsize * 4;
                startPos = Math.Max(startPos, 0);
            }
        }
        public void NextPage(DataTable data)
        {
            if (startPos < data.Rows.Count - Config.GlobalConfig.StockConfig.PerbidListMaxsize * 4)
            {
                startPos += Config.GlobalConfig.StockConfig.PerbidListMaxsize * 4;
                startPos = Math.Min(startPos, data.Rows.Count - Config.GlobalConfig.StockConfig.PerbidListMaxsize * 4);
            }
        }
    }
}
