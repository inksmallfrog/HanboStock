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

namespace main
{
    class DrawPerbidList
    {
        private const int LIST_MAX_SIZE = 15;

        private DoubleBufferListView perbidList;
        private int[] header_width = null;

        private ListViewItem[] lvis = new ListViewItem[LIST_MAX_SIZE];
//        private WaitHandle[] waitHandles = new WaitHandle[LIST_MAX_SIZE];

        public DrawPerbidList(DoubleBufferListView list)
        {
            perbidList = list;

            list.ColumnWidthChanged += new ColumnWidthChangedEventHandler(lv_ColumnWidthChanged);
            list.ColumnWidthChanging += new ColumnWidthChangingEventHandler(lv_ColumnWidthChanging);

            DrawHeader();
            DrawBody();
        }

        public void ShowList(){
            perbidList.Visible = true;
        }

        public void HideList()
        {
            perbidList.Visible = false;
        }
        
        //绘制头
        private void DrawHeader()
        {
            int[] headerWidth = new int[] { 80, 60, 69, 30 };
            for (int i = 0; i < headerWidth.Length; ++i)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Width = headerWidth[i];

                if (i < 2)
                {
                    ch.TextAlign = HorizontalAlignment.Center;
                }
                else
                {
                    ch.TextAlign = HorizontalAlignment.Right;
                }

                if (perbidList.InvokeRequired)
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                    Action<ColumnHeader> actionDelegate = (x) => { perbidList.Columns.Add(ch); };
                    // 或者
                    // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };
                    perbidList.Invoke(actionDelegate, ch);
                }
                else
                {
                    perbidList.Columns.Add(ch);
                }
            }
        }

        private void DrawBody()
        {
            for (int i = 0; i < LIST_MAX_SIZE; ++i)
            {
                if (lvis[i] == null)
                {
                    lvis[i] = new ListViewItem();
                }

                ListViewItem lvi = lvis[i];

                lvi.UseItemStyleForSubItems = false;
                lvi.ForeColor = System.Drawing.Color.White;

                ListViewItem.ListViewSubItem lsviPrice = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsviPrice);

                ListViewItem.ListViewSubItem lsviVol = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsviVol);

                ListViewItem.ListViewSubItem lsviType = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsviType);

                if (perbidList.InvokeRequired)
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                    Action<ListViewItem> actionDelegate = (x) => { perbidList.Items.Add(x); };
                    // 或者
                    // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };
                    perbidList.Invoke(actionDelegate, lvi);
                }
                else
                {
                    perbidList.Items.Add(lvi);
                }
            }
        }

        public void BindData(DataTable data, DataRow realtime)
        {
            if (data.Rows.Count == 0)
            {
                return;
            }
            int dataLen = data.Rows.Count;
            perbidList.BeginUpdate();
            for (int i = 0; i < LIST_MAX_SIZE; ++i)
            {
                BindRowData(data.Rows[dataLen - i - 1], lvis[i], realtime);
            }
            perbidList.EndUpdate();
        }

        private void BindRowData(DataRow data, ListViewItem lvi, DataRow realtime)
        {
            lvi.SubItems[0].Text = data["time"].ToString();

            double price = double.Parse(data["price"].ToString());
            decimal vol = decimal.Parse(data["vol"].ToString()) / 100;
            int type = int.Parse(data["type"].ToString());
            string typeStr = "";
            double close = double.Parse(realtime["closeP"].ToString());
            
            if (price > close) { lvi.SubItems[1].ForeColor = Color.Red; }
            else if (price < close) { lvi.SubItems[1].ForeColor = Color.LawnGreen; }
            else { lvi.SubItems[1].ForeColor = Color.White; }

            if (vol > 500) { lvi.SubItems[2].ForeColor = Color.Purple; }
            else { lvi.SubItems[2].ForeColor = Color.Yellow; }

            if (type == 1)
            {
                typeStr = "B";
                lvi.SubItems[3].ForeColor = Color.Red;
            }
            else if(type == -1)
            {
                typeStr = "S";
                lvi.SubItems[3].ForeColor = Color.Green;
            }

            lvi.SubItems[1].Text = price.ToString("0.00");
            lvi.SubItems[2].Text = vol.ToString();
            lvi.SubItems[3].Text = typeStr;
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
    }
}
