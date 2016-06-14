using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace ImageOfStock
{
    public partial class StocksList : ListView
    {
        #region 变量
        private int listMaxSize = 28;
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

        private String[] header = null;

        private int pauseStateId = 0;

        [CategoryAttribute("布局"), DescriptionAttribute("最大容量")]
        public int ListMaxSize
        {
            get { return listMaxSize; }
            set { listMaxSize = value; }
        }
        [CategoryAttribute("布局"), DescriptionAttribute("表头宽度")]
        public int[] HeaderWidth
        {
            get { return headerWidth; }
            set { headerWidth = value; }
        }

        [CategoryAttribute("内容"), DescriptionAttribute("表头")]
        public String[] Header
        {
            get { return header; }
            set { header = value; }
        }

        [CategoryAttribute("行为"), DescriptionAttribute("停牌标识")]
        public int PauseStateId
        {
            get { return pauseStateId; }
            set { pauseStateId = value; }
        }
        #endregion

        private ListViewItem[] lvis = null;

        public StocksList()
        {
            DoubleBuffered = true;

            SetStyle(ControlStyles.DoubleBuffer |
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            lvis = new ListViewItem[listMaxSize];

            DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(lv_DrawColumnHeader);
            DrawItem += new DrawListViewItemEventHandler(lv_DrawItem);
            DrawSubItem += new DrawListViewSubItemEventHandler(lv_DrawSubItem);
            ColumnWidthChanged += new ColumnWidthChangedEventHandler(lv_ColumnWidthChanged);
            ColumnWidthChanging += new ColumnWidthChangingEventHandler(lv_ColumnWidthChanging);

            MouseDown += new MouseEventHandler(lv_MouseDown);
        }

        public void Draw()
        {
            DrawHeader();
            DrawBody();
        }

        new public void Show()
        {
            Visible = true;
            Focus();
        }

        new public void Hide()
        {
            Visible = false;
        }

        public ListViewItem GetFocusedItem()
        {
            return FocusedItem;
        }

        public string GetFocusedStockId()
        {
            return FocusedItem.Name;
        }

        public void FocuseOn(ListViewItem lvi)
        {
            if (Items.Contains(lvi))
            {
                FocusedItem = lvi;
            }
            else
            {
                FocusedItem = Items[0];
            }
        }

        private void DrawHeader()
        {
            if (header == null)
            {
                header = new String[]{ "代码", "名称", "涨幅", "现价", "涨跌", "买价", "卖价",
                                 "总量", "总金额", "今开", "最低", "最高", "昨收"};
            }
            if (headerWidth == null)
            {
                headerWidth = new int[] { 120, 120, 100, 100, 100, 100, 100, 120, 108, 100, 100, 100, 100 };
            }

            for (int i = 0; i < header.Length; ++i)
            {
                ColumnHeader ch = new ColumnHeader();
                ch.Text = header[i];
                ch.Width = headerWidth[i];
                if (i < 2)
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
                lvi.ForeColor = normalFgColor;

                ListViewItem.ListViewSubItem lsvi_name = new ListViewItem.ListViewSubItem();
                lsvi_name.ForeColor = normalFgColor;
                lvi.SubItems.Add(lsvi_name);

                ListViewItem.ListViewSubItem lsvi_increasePer = new ListViewItem.ListViewSubItem();
                lsvi_increasePer.ForeColor = normalFgColor;
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
                lsvi_totalNumber.ForeColor = normalFgColor;
                lvi.SubItems.Add(lsvi_totalNumber);

                ListViewItem.ListViewSubItem lsvi_turnover = new ListViewItem.ListViewSubItem();
                lsvi_turnover.ForeColor = turnOverFgColor;
                lvi.SubItems.Add(lsvi_turnover);

                ListViewItem.ListViewSubItem lsvi_openningPrice = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsvi_openningPrice);

                ListViewItem.ListViewSubItem lsvi_lowestPrice = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsvi_lowestPrice);

                ListViewItem.ListViewSubItem lsvi_highestPrice = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsvi_highestPrice);

                ListViewItem.ListViewSubItem lsvi_closingPrice = new ListViewItem.ListViewSubItem();
                lvi.SubItems.Add(lsvi_closingPrice);

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

        public void BindAllData(DataTable data, object refresh)
        {
            if (InvokeRequired)
            {
                Action<object> d = (x) => { BindAllData(data, refresh); };
                Invoke(d, refresh);
            }
            else
            {
                BeginUpdate();
                for (int i = 0; i < listMaxSize; ++i)
                {
                    BindData(data.Rows[i % data.Rows.Count], lvis[i], bool.Parse(refresh.ToString()));
                }
                EndUpdate();
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
                bgColor = upBgColor;
                fgColor = upFgColor;
            }
            else if (updown == 0)
            {
                bgColor = equalBgColor;
                fgColor = equalFgColor;
            }
            else
            {
                bgColor = downBgColor;
                fgColor = downFgColor;
            }
            if (!refresh || lvi.SubItems[2].Text == String.Empty || price == double.Parse(lvi.SubItems[2].Text)) { bgColor = equalBgColor; }

            lvi.Text = stockId.Substring(1);
            lvi.SubItems[1].Text = data["name"].ToString();
            if (close == 0 || int.Parse(data["status"].ToString()) == pauseStateId)
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
            lvi.SubItems[2].ForeColor = (bgColor == equalBgColor) ? fgColor : equalFgColor;
            lvi.SubItems[3].Text = price.ToString("0.00");
            lvi.SubItems[3].ForeColor = fgColor;
            lvi.SubItems[4].Text = updown.ToString("0.00");
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
                e.Graphics.FillRectangle(normalBrush, r);
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
                    if (e.SubItem.BackColor == upBgColor)
                    {
                        e.Graphics.FillRectangle(upBrush, backRect);
                    }
                    else if (e.SubItem.BackColor == equalBgColor)
                    {
                        //e.Graphics.FillRectangle(White, rect);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(downBrush, backRect);
                    }
                }
                if (e.SubItem.ForeColor == normalFgColor)
                {
                    brush = normalBrush;
                }
                else if (e.SubItem.ForeColor == equalFgColor)
                {
                    brush = equalBrush;
                }
                else if (e.SubItem.ForeColor == upFgColor)
                {
                    brush = upBrush;
                }
                else if (e.SubItem.ForeColor == downFgColor)
                {
                    brush = downBrush;
                }
                else if (e.SubItem.ForeColor == turnOverFgColor)
                {
                    brush = turnOverBrush;
                }
                e.Graphics.DrawString(e.SubItem.Text, e.SubItem.Font, brush, rect, sf);
            }
            //e.DrawText();
        }

        private void lv_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
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
            if (Columns[e.ColumnIndex].Width != headerWidth[e.ColumnIndex])
            {
                Columns[e.ColumnIndex].Width = headerWidth[e.ColumnIndex];
            }
        }

        private void lv_MouseDown(object sender, MouseEventArgs e)
        {
            ListViewItem li = GetItemAt(20, e.Y);
            FocusedItem = li;
        }

    }
}
