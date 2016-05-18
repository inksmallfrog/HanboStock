using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading;
using System.Collections;
using System.Data;

using Data;

namespace main
{
    class DrawList
    {
        private const int LIST_MAX_SIZE = 27;
        private int listStartPos = 0;

        private DoubleBufferListView stockList;
        private String[] header = null;
        private int[] header_width = null;

        private Brush Yellow = System.Drawing.Brushes.Yellow;
        private Brush White = System.Drawing.Brushes.White;
        private Brush Green = System.Drawing.Brushes.LawnGreen;
        private Brush Red = System.Drawing.Brushes.Red;
        private Brush DeepSkyBlue = System.Drawing.Brushes.DeepSkyBlue;

        private ListViewItem[] lvis = new ListViewItem[LIST_MAX_SIZE];
//        private WaitHandle[] waitHandles = new WaitHandle[LIST_MAX_SIZE];

        public DrawList(DoubleBufferListView list)
        {
            stockList = list;

            list.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(lv_DrawColumnHeader);
            list.DrawItem += new DrawListViewItemEventHandler(lv_DrawItem);
            list.DrawSubItem += new DrawListViewSubItemEventHandler(lv_DrawSubItem);
            list.ColumnWidthChanged += new ColumnWidthChangedEventHandler(lv_ColumnWidthChanged);
            list.ColumnWidthChanging += new ColumnWidthChangingEventHandler(lv_ColumnWidthChanging);
            list.MouseWheel += new MouseEventHandler(lv_MouseWheel);

            DrawHeader(list);
            DrawBody(list);
        }

        public void ShowList(){
            stockList.Visible = true;
            stockList.Focus();
        }

        public void HideList()
        {
            stockList.Visible = false;
        }

        public ListViewItem GetFocusedItem()
        {
            return stockList.FocusedItem;
        }

        public void FocuseOn(ListViewItem lvi)
        {
            if (stockList.Items.Contains(lvi))
            {
                stockList.FocusedItem = lvi;
            }
            else
            {
                stockList.FocusedItem = stockList.Items[0];
            }
        }

        public string GetFocusedStockId()
        {
            return stockList.FocusedItem.Name;
        }
        
        //绘制头
        private void DrawHeader(DoubleBufferListView list)
        {
            header = new String[]{ "代码", "名称", "涨幅", "现价", "涨跌", "买价", "卖价",
                                 "总量", "总金额", "今开", "最低", "最高", "昨收"};
            header_width = new int[] { 120, 120, 100, 100, 100, 100, 100, 120, 108, 100, 100, 100, 100 };
            for (int i = 0; i < header.Length; ++i)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = header[i];
                ch.Width = header_width[i];
                if (i < 2)
                {
                    ch.TextAlign = HorizontalAlignment.Center;
                }
                else
                {
                    ch.TextAlign = HorizontalAlignment.Right;
                }

                if (list.InvokeRequired)
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                    Action<ColumnHeader> actionDelegate = (x) => { list.Columns.Add(ch); };
                    // 或者
                    // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };
                    list.Invoke(actionDelegate, ch);
                }
                else
                {
                    list.Columns.Add(ch);
                }
            }
        }

        private void DrawBody(DoubleBufferListView list)
        {
            for (int i = 0; i < LIST_MAX_SIZE; ++i)
            {
                if (lvis[i] == null)
                {
                    lvis[i] = new ListViewItem();
                }

                ListViewItem lvi = lvis[i];

                lvi.UseItemStyleForSubItems = false;
                lvi.ForeColor = System.Drawing.Color.Yellow;

                ListViewItem.ListViewSubItem lsvi_name = new ListViewItem.ListViewSubItem();
                lsvi_name.ForeColor = System.Drawing.Color.Yellow;
                lvi.SubItems.Add(lsvi_name);

                ListViewItem.ListViewSubItem lsvi_increasePer = new ListViewItem.ListViewSubItem();
                lsvi_increasePer.ForeColor = System.Drawing.Color.White;
                lvi.SubItems.Add(lsvi_increasePer);

                ListViewItem.ListViewSubItem lsvi_currentPrice = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsvi_currentPrice);

                ListViewItem.ListViewSubItem lsvi_increase = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsvi_increase);

                ListViewItem.ListViewSubItem lsvi_competitivePrice = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsvi_competitivePrice);

                ListViewItem.ListViewSubItem lsvi_auctionPrice = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsvi_auctionPrice);

                ListViewItem.ListViewSubItem lsvi_totalNumber = new ListViewItem.ListViewSubItem();
                lsvi_totalNumber.ForeColor = System.Drawing.Color.Yellow;
                lvi.SubItems.Add(lsvi_totalNumber);

                ListViewItem.ListViewSubItem lsvi_turnover = new ListViewItem.ListViewSubItem();
                lsvi_turnover.ForeColor = System.Drawing.Color.DeepSkyBlue;
                lvi.SubItems.Add(lsvi_turnover);

                ListViewItem.ListViewSubItem lsvi_openningPrice = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsvi_openningPrice);

                ListViewItem.ListViewSubItem lsvi_lowestPrice = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsvi_lowestPrice);

                ListViewItem.ListViewSubItem lsvi_highestPrice = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsvi_highestPrice);

                ListViewItem.ListViewSubItem lsvi_closingPrice = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsvi_closingPrice);

                if (stockList.InvokeRequired)
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                    Action<ListViewItem> actionDelegate = (x) => { stockList.Items.Add(x); };
                    // 或者
                    // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };
                    stockList.Invoke(actionDelegate, lvi);
                }
                else
                {
                    stockList.Items.Add(lvi);
                }
            }
            ThreadPool.QueueUserWorkItem(BindAllData, true);
        }

        private void BindAllData(object refresh)
        {
            DataTable data = DataReader.GetRealTimeList();
            if (stockList.InvokeRequired)
            {
                Action<object> d = (x) => { BindAllData(x); };
                stockList.Invoke(d, refresh);
            }
            else
            {
                stockList.BeginUpdate();
                for (int i = 0; i < LIST_MAX_SIZE; ++i)
                {
                    BindData(data.Rows[(i + listStartPos) % data.Rows.Count], lvis[i], bool.Parse(refresh.ToString()));
                }
                stockList.EndUpdate();
            }
        }

        private void BindData(DataRow data, ListViewItem lvi, bool refresh = true)
        {
            double price = double.Parse(data["price"].ToString());
            double close = double.Parse(data["closeP"].ToString());
            decimal turnover = decimal.Parse(data["turnover"].ToString());

            string stockId = data["stockId"].ToString();
            if (stockId.Length < 7)
            {
                stockId = stockId.PadLeft(7, '0');
            }

            lvi.Name = stockId;
            Color bgColor;
            Color fgColor;
         
            double updown = price - close;
            if (updown > 0)
            {
                bgColor = Color.Red;
                fgColor = Color.Red;
            }
            else if (updown == 0){
                bgColor = Color.Black;
                fgColor = Color.White;
            }
            else
            {
                bgColor = Color.Green;
                fgColor = Color.Green;
            }
            if (!refresh || lvi.SubItems[2].Text == String.Empty || price == double.Parse(lvi.SubItems[2].Text)) { bgColor = Color.Black; }
            
            lvi.Text = stockId.Substring(1);
            lvi.SubItems[1].Text = data["name"].ToString();
            if (close == 0 
                || int.Parse(data["status"].ToString()) == (int)DataDefine.StockStatus.StockStatusPause)
            {
                lvi.SubItems[2].Text = "-";
            }
            else
            {
                string percent = Math.Round((price - close) / close * 100.0, 2).ToString("0.00");
                /*if(percent.Length == 1){
                    percent += '.';
                    percent = percent.PadRight(4, '0');
                }*/
                percent += '%';
                lvi.SubItems[2].Text = percent;
            }
            lvi.SubItems[2].BackColor = bgColor;
            lvi.SubItems[2].ForeColor = (bgColor == Color.Black) ? fgColor : Color.White;
            lvi.SubItems[3].Text = price.ToString("0.00");
            lvi.SubItems[3].ForeColor = fgColor;
            lvi.SubItems[4].Text = price.ToString("0.00");
            lvi.SubItems[4].ForeColor = fgColor;
            lvi.SubItems[5].Text = double.Parse(data["bid1Price"].ToString()).ToString("0.00");
            lvi.SubItems[5].ForeColor = fgColor;
            lvi.SubItems[6].Text = double.Parse(data["ask1Price"].ToString()).ToString("0.00");
            lvi.SubItems[6].ForeColor = fgColor;
            lvi.SubItems[7].Text = Math.Round(decimal.Parse(data["volume"].ToString()) / Convert.ToDecimal(100.00), 0).ToString();
            if (turnover > 99999999)
            {
                lvi.SubItems[8].Text = Math.Round(turnover / 100000000, 2).ToString() + "亿";
            }
            else if (turnover > 9999)
            {
                lvi.SubItems[8].Text = Math.Round(turnover / 10000, 2).ToString() + "万";
            }
            else
            {
                lvi.SubItems[8].Text = turnover.ToString();
            }
            
            lvi.SubItems[9].Text = double.Parse(data["openP"].ToString()).ToString("0.00");
            lvi.SubItems[9].ForeColor = fgColor;
            lvi.SubItems[10].Text = double.Parse(data["low"].ToString()).ToString("0.00");
            lvi.SubItems[10].ForeColor = fgColor;
            lvi.SubItems[11].Text = double.Parse(data["high"].ToString()).ToString("0.00");
            lvi.SubItems[11].ForeColor = fgColor;
            lvi.SubItems[12].Text = close.ToString("0.00");
            lvi.SubItems[12].ForeColor = fgColor;
        }

        private void lv_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if ((e.State & ListViewItemStates.Focused) != 0)
            {
                Rectangle r = new Rectangle(e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Width, 1);
                e.Graphics.FillRectangle(Yellow, r);
            }
        }

        private void lv_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            Rectangle rect = new Rectangle(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height - 1);
            Rectangle backRect = new Rectangle(e.Bounds.Left + 40, e.Bounds.Top, e.Bounds.Width - 40, e.Bounds.Height - 1);
            Brush brush = null;
            using (StringFormat sf = new StringFormat())
            {
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        sf.Alignment = StringAlignment.Center;
                        break;
                    case HorizontalAlignment.Right:
                        sf.Alignment = StringAlignment.Far;
                        break;
                }
                if (e.ColumnIndex == 2)
                {
                    if (e.SubItem.BackColor == Color.Red)
                    {
                        e.Graphics.FillRectangle(Red, backRect);
                    }
                    else if (e.SubItem.BackColor == Color.Black)
                    {
                        //e.Graphics.FillRectangle(White, rect);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(Brushes.Green, backRect);
                    }
                }
                if (e.SubItem.ForeColor == System.Drawing.Color.Yellow)
                {
                    brush = Yellow;
                }
                else if (e.SubItem.ForeColor == System.Drawing.Color.White)
                {
                    brush = White;
                }
                else if (e.SubItem.ForeColor == System.Drawing.Color.Red)
                {
                    brush = Red;
                }
                else if (e.SubItem.ForeColor == System.Drawing.Color.Green)
                {
                    brush = Green;
                }
                else if (e.SubItem.ForeColor == System.Drawing.Color.DeepSkyBlue)
                {
                    brush = DeepSkyBlue;
                }
                e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, brush, rect, sf);
            }
            //e.DrawText();
        }

        private void lv_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e){
            using (StringFormat sf = new StringFormat())
            {
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        sf.Alignment = StringAlignment.Center;
                        break;
                    case HorizontalAlignment.Right:
                        sf.Alignment = StringAlignment.Far;
                        break;
                }
                
                e.Graphics.FillRectangle(System.Drawing.Brushes.Black, e.Bounds);
                e.Graphics.DrawString(e.Header.Text, e.Font, System.Drawing.Brushes.White, e.Bounds, sf);
            }
            return;
        }

        private void lv_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
        }

        private void lv_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (e.ColumnIndex > 12)
            {
                return;
            }
            if (((ListView)sender).Columns[e.ColumnIndex].Width != header_width[e.ColumnIndex])
            {
                ((ListView)sender).Columns[e.ColumnIndex].Width = header_width[e.ColumnIndex];
            }
        }

        private void lv_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                listStartPos += LIST_MAX_SIZE;
                if (listStartPos > DataReader.GetRealTimeList().Rows.Count - 1)
                {
                    listStartPos = 0;
                }
            }
            else if(e.Delta > 0)
            {
                listStartPos -= LIST_MAX_SIZE;
                if (listStartPos < 0)
                {
                    listStartPos = DataReader.GetRealTimeList().Rows.Count - LIST_MAX_SIZE;
                }
            }

            ThreadPool.QueueUserWorkItem(BindAllData, false);
        }
    }
}
