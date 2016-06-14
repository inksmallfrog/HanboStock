using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dao;
using System.Threading;

namespace ImageOfStock
{
    public partial class SimpleTimeLineGraph : UserControl
    {
        SingleLoader loader = new SingleLoader();
        private String stockId = "";

        public SimpleTimeLineGraph()
        {
            InitializeComponent();
            timeLineGraph.MouseWheel += new MouseEventHandler(StlMouseWheel);
        }

        public void Draw()
        {
            timeLineGraph.Draw();
        }

        public void BindData(String _stockId)
        {
            ThreadPool.QueueUserWorkItem(BindDataCallBack, _stockId);
        }

        private void BindDataCallBack(object __stockId)
        {
            String _stockId = __stockId.ToString();
            if (!stockId.Equals(_stockId))
            {
                loader.Stop();
                stockId = _stockId;
                loader.StockId = stockId;
                loader.Start();
            }
            DataRow current = GlobalData.StocksTable.Select("stockId='" + stockId + "'")[0];
            timeLineGraph.BindData(loader.Perminut, current);

            if (InvokeRequired)
            {
                // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                //Action<ListViewItem> actionDelegate = (x) => { Items.Add(x); };
                // 或者
                Action<DataRow> actionDelegate = delegate(DataRow _current)
                {
                    name.Text = _current["name"].ToString() + "(" + _current["stockId"].ToString().Substring(1) + ")";
                    price.Text = _current["price"].ToString();
                    updown.Text = (Double.Parse(_current["price"].ToString()) - Double.Parse(_current["closeP"].ToString())).ToString();
                };
                Invoke(actionDelegate, current);
            }
            else
            {
                name.Text = current["name"].ToString() + "(" + current["stockId"].ToString().Substring(1) + ")";
                price.Text = current["price"].ToString();
                updown.Text = (Double.Parse(current["price"].ToString()) - Double.Parse(current["closeP"].ToString())).ToString();
            }
        }

        private void StlMouseWheel(object sender, MouseEventArgs e)
        {
            OnMouseWheel(e);
        }
    }
}
