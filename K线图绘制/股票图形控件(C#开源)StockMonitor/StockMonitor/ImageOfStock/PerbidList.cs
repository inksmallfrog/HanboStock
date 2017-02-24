using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ImageOfStock
{
    public partial class PerbidList : ListView
    {
        #region 变量
        private int listMaxSize = 33;
        private int[] headerWidth = null;

        private Color upBgColor = Color.Red;
        private Color upFgColor = Color.Red;
        private Color downBgColor = Color.Green;
        private Color downFgColor = Color.Green;
        private Color equalBgColor = Color.Black;
        private Color equalFgColor = Color.White;
        private Color normalFgColor = Color.Yellow;
        private Color turnOverFgColor = Color.DeepSkyBlue;

        private Brush upBrush = System.Drawing.Brushes.Red;
        private Brush downBrush = System.Drawing.Brushes.Green;
        private Brush equalBrush = System.Drawing.Brushes.White;
        private Brush turnOverBrush = System.Drawing.Brushes.DeepSkyBlue;
        private Brush normalBrush = System.Drawing.Brushes.Yellow;

        private int pauseStateId = 0;

        [CategoryAttribute("大小"), DescriptionAttribute("最大容量")]
        public int ListMaxSize
        {
            get { return listMaxSize; }
            set {
                listMaxSize = value;
               // lvis = new ListViewItem[listMaxSize];
               // DrawBody();
            }
        }
        [CategoryAttribute("大小"), DescriptionAttribute("表头宽度")]
        public int[] HeaderWidth
        {
            get { return headerWidth; }
            set { headerWidth = value; }
        }

        [CategoryAttribute("股票"), DescriptionAttribute("停牌标识")]
        public int PauseStateId
        {
            get { return pauseStateId; }
            set { pauseStateId = value; }
        }
        #endregion

        private ListViewItem[] lvis = null;

        public PerbidList()
        {
            SetStyle(ControlStyles.DoubleBuffer |
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            lvis = new ListViewItem[listMaxSize];

            ColumnWidthChanged += new ColumnWidthChangedEventHandler(lv_ColumnWidthChanged);
            ColumnWidthChanging += new ColumnWidthChangingEventHandler(lv_ColumnWidthChanging);

            DrawHeader();
            DrawBody();
        }

        private void DrawHeader()
        {
            if (headerWidth == null)
            {
                headerWidth = new int[] { 100, 80, 70, 50 };
            }

            for (int i = 0; i < headerWidth.Length; ++i)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Width = headerWidth[i];

                if (i == 0)
                {
                    ch.TextAlign = HorizontalAlignment.Center;
                }
                else
                {
                    ch.TextAlign = HorizontalAlignment.Right;
                }

                if (InvokeRequired)
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                    Action<ColumnHeader> actionDelegate = (x) => { Columns.Add(ch); };
                    // 或者
                    // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };
                    Invoke(actionDelegate, ch);
                }
                else
                {
                    Columns.Add(ch);
                }
            }
        }

        private void DrawBody()
        {
            for (int i = 0; i < listMaxSize; ++i)
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

                if (InvokeRequired)
                {
                    // 当一个控件的InvokeRequired属性值为真时，说明有一个创建它以外的线程想访问它
                    Action<ListViewItem> actionDelegate = (x) => { Items.Add(x); };
                    // 或者
                    // Action<string> actionDelegate = delegate(string txt) { this.label2.Text = txt; };
                    Invoke(actionDelegate, lvi);
                }
                else
                {
                    Items.Add(lvi);
                }
            }
        }

        public void BindData(DataTable data, DataRow realtime, int startPos)
        {
            if (data.Rows.Count == 0)
            {
                return;
            }
            int dataLen = data.Rows.Count;
            BeginUpdate();
            if(startPos == -1)
            {
                for (int i = 0; i < listMaxSize && i < data.Rows.Count; ++i)
                {
                    if (data.Rows.Count < listMaxSize){
                        BindRowData(data.Rows[i], lvis[i], realtime);
                    }
                    else
                    {
                        BindRowData(data.Rows[dataLen - listMaxSize + i], lvis[i], realtime);
                    }
                }
            }
            else
            {
                for (int i = 0; i < listMaxSize && i < data.Rows.Count; ++i)
                {
                    if(startPos + i < data.Rows.Count)
                    {
                        BindRowData(data.Rows[startPos + i], lvis[i], realtime);
                    }
                }
            }
            EndUpdate();
        }

        private void BindRowData(DataRow data, ListViewItem lvi, DataRow realtime)
        {
            lvi.SubItems[0].Text = data["time"].ToString().Substring(0, 5);

            double price = double.Parse(data["price"].ToString());
            double vol = Math.Floor(int.Parse(data["vol"].ToString()) / 100.0 + 0.5);
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
            else if (type == -1)
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
            if (((ListView)sender).Columns[e.ColumnIndex].Width != headerWidth[e.ColumnIndex])
            {
                ((ListView)sender).Columns[e.ColumnIndex].Width = headerWidth[e.ColumnIndex];
            }
        }
    }
}
