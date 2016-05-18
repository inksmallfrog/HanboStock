using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Data;

using Data;

namespace main
{
    class DrawDetail
    {
        enum OtherPanelState{
            Perbid
        }

        private Panel detailPanel;

        private Panel stockTitlePanel;
        private Label stockTitle;

        private Panel abDirtaPanel;
        private Label ABS;
        private Label ABD;

        private Panel bidPanel;
        private Label[] bidPrice = new Label[5];
        private Label[] bidVol = new Label[5]; 

        private Panel askPanel;
        private Label[] askPrice = new Label[5];
        private Label[] askVol = new Label[5]; 

        private Panel detailInfoPanel;
        private Label price;
        private Label open;
        private Label updown;
        private Label high;
        private Label percent;
        private Label low;
        private Label volume;
        private Label volumeRatio;
        private Label inner;
        private Label outter;

        private Panel otherPanel;
        private DoubleBufferListView perbidList;

        private string stockId;

        private OtherPanelState otherPanelState = OtherPanelState.Perbid;
        private DrawPerbidList drawPerbidList;

        public DrawDetail(Panel _detailPanel)
        {
            detailPanel = _detailPanel;
            detailPanel.Paint += detailPanel_Paint;
            stockTitlePanel = (Panel)MainWindow.GetChildFromPanelByName(detailPanel, "StockTitlePanel");
            stockTitlePanel.Paint += stockTitlePanel_Paint;
            abDirtaPanel = (Panel)MainWindow.GetChildFromPanelByName(detailPanel, "ABDirtaPanel");
            abDirtaPanel.Paint += abDirtaPanel_Paint;
            bidPanel = (Panel)MainWindow.GetChildFromPanelByName(detailPanel, "BidPanel");
            bidPanel.Paint += bidPanel_Paint;
            askPanel = (Panel)MainWindow.GetChildFromPanelByName(detailPanel, "AskPanel");
            askPanel.Paint += askPanel_Paint;
            detailInfoPanel = (Panel)MainWindow.GetChildFromPanelByName(detailPanel, "DetailInfoPanel");
            detailInfoPanel.Paint += detailInfoaPanel_Paint;
            otherPanel = (Panel)MainWindow.GetChildFromPanelByName(detailPanel, "OtherPanel");
            otherPanel.Paint += otherPanel_Paint;
            
            stockTitle = (Label)MainWindow.GetChildFromPanelByName(stockTitlePanel, "StockTitle");

            ABS = (Label)MainWindow.GetChildFromPanelByName(abDirtaPanel, "ABS");
            ABD = (Label)MainWindow.GetChildFromPanelByName(abDirtaPanel, "ABD");

            for (int i = 0; i < 5; ++i)
            {
                bidPrice[i] = (Label)MainWindow.GetChildFromPanelByName(bidPanel, "Bid" + (i + 1) + "Price");
                bidVol[i] = (Label)MainWindow.GetChildFromPanelByName(bidPanel, "Bid" + (i + 1) + "Vol");

                askPrice[i] = (Label)MainWindow.GetChildFromPanelByName(askPanel, "Ask" + (i + 1) + "Price");
                askVol[i] = (Label)MainWindow.GetChildFromPanelByName(askPanel, "Ask" + (i + 1) + "Vol");
            }
           
            price = (Label)MainWindow.GetChildFromPanelByName(detailInfoPanel, "Price");
            open = (Label)MainWindow.GetChildFromPanelByName(detailInfoPanel, "Open");
            updown = (Label)MainWindow.GetChildFromPanelByName(detailInfoPanel, "UpDown");
            high = (Label)MainWindow.GetChildFromPanelByName(detailInfoPanel, "High");
            percent = (Label)MainWindow.GetChildFromPanelByName(detailInfoPanel, "Percent");
            low = (Label)MainWindow.GetChildFromPanelByName(detailInfoPanel, "Low");
            volume = (Label)MainWindow.GetChildFromPanelByName(detailInfoPanel, "Volume");
            volumeRatio = (Label)MainWindow.GetChildFromPanelByName(detailInfoPanel, "VolumeRatio");
            inner = (Label)MainWindow.GetChildFromPanelByName(detailInfoPanel, "Inner");
            outter = (Label)MainWindow.GetChildFromPanelByName(detailInfoPanel, "Outter");

            perbidList = (DoubleBufferListView)MainWindow.GetChildFromPanelByName(otherPanel, "perbidList");
            drawPerbidList = new DrawPerbidList(perbidList);
        }

        public void SetStockId(string _stockId)
        {
            if (stockId != _stockId)
            {
                stockId = _stockId;
                BindData(DataReader.GetRealTimeList().Select("stockId='" + stockId + "'")[0], DataReader.GetSpecialHistories(), DataReader.GetSpecialPerbid());
            }
        }

        public void BindData(DataRow realtime, DataTable histories, DataTable perbid)
        {
            stockTitle.Text = stockId.Substring(1) + " " + realtime["name"].ToString();

            double dataPrice = double.Parse(realtime["price"].ToString());
            double dataClose = double.Parse(realtime["closeP"].ToString());
            double dataOpen = double.Parse(realtime["openP"].ToString());

            int realtimeBidVol;
            double realtimeBidPrice;
            int realtimeAskVol;
            double realtimeAskPrice;

            int askAll = 0;
            int bidAll = 0;
            for (int i = 1; i < 6; ++i){
                realtimeBidVol = int.Parse(realtime["bid" + i + "Vol"].ToString());
                bidAll += realtimeBidVol;
                realtimeBidPrice = double.Parse(realtime["bid" + i + "Price"].ToString());

                realtimeAskVol = int.Parse(realtime["ask" + i + "Vol"].ToString());
                askAll += realtimeAskVol;
                realtimeAskPrice = double.Parse(realtime["ask" + i + "Price"].ToString());

                bidVol[i - 1].Text = (realtimeBidVol / 100).ToString();
                askVol[i - 1].Text = (realtimeAskVol / 100).ToString();

                bidPrice[i - 1].Text = realtimeBidPrice.ToString("0.00");
                SetRGLabelColor(bidPrice[i - 1], realtimeBidPrice, dataClose);

                askPrice[i - 1].Text = realtimeAskPrice.ToString("0.00");
                SetRGLabelColor(askPrice[i - 1], realtimeAskPrice, dataClose);
            }

            int abd = bidAll - askAll;
            double abs = Math.Round(abd * 1.0 / (askAll + bidAll), 4) * 100;
            ABD.Text = (abd / 100).ToString();
            ABS.Text = abs.ToString() + "%";
            if (abd > 0)
            {
                ABD.ForeColor = Color.Red;
                ABS.ForeColor = Color.Red;
            }
            else if(abd < 0)
            {
                ABD.ForeColor = Color.LawnGreen;
                ABS.ForeColor = Color.LawnGreen;
            }
            else
            {
                ABD.ForeColor = Color.White;
                ABS.ForeColor = Color.White;
            }

            price.Text = Math.Round(dataPrice, 2).ToString("0.00");
            SetRGLabelColor(price, dataPrice, dataClose);
            open.Text = (double.Parse(realtime["openP"].ToString())).ToString("0.00");
            SetRGLabelColor(open, double.Parse(realtime["openP"].ToString()), dataClose);
            double dirta = dataPrice - dataClose;
            if (dirta > 0)
            {
                updown.ForeColor = Color.Red;
                percent.ForeColor = Color.Red;
                volumeRatio.ForeColor = Color.Red;
            }
            else if (dirta < 0)
            {
                updown.ForeColor = Color.LawnGreen;
                percent.ForeColor = Color.LawnGreen;
                volumeRatio.ForeColor = Color.LawnGreen;
            }
            else
            {
                updown.ForeColor = Color.White;
                percent.ForeColor = Color.White;
                volumeRatio.ForeColor = Color.White;
            }
            updown.Text = (dirta).ToString("0.00");
            percent.Text = (Math.Round(dirta / dataClose, 4) * 100).ToString() + "%";

            high.Text = (double.Parse(realtime["high"].ToString())).ToString("0.00");
            SetRGLabelColor(high, double.Parse(realtime["high"].ToString()), dataClose);

            low.Text = double.Parse(realtime["low"].ToString()).ToString("0.00");
            SetRGLabelColor(low, double.Parse(realtime["low"].ToString()), dataClose);

            volume.Text = (decimal.Parse(realtime["volume"].ToString()) / 100).ToString("0");

            volumeRatio.Text = CalculateVR(realtime, histories).ToString("0.00");

            Int64[] innerAndOutter = CalculateIO(perbid);
            inner.Text = (innerAndOutter[0] / 100).ToString("0");
            outter.Text = (innerAndOutter[1] / 100).ToString("0");

            switch (otherPanelState)
            {
                case OtherPanelState.Perbid:
                    drawPerbidList.BindData(perbid, realtime);
                    break;
            }
        }

        private double CalculateVR(DataRow realtime, DataTable histories)
        {
            if (histories.Rows.Count < 5)
            {
                return 0.0;
            }
            Int64 historiesVolAll = 0;
            int lastPos = histories.Rows.Count - 1;
            for (int i = 0; i < 5; ++i){
                historiesVolAll += Int64.Parse(histories.Rows[lastPos - i]["volume"].ToString());
            }
            double historyVolAvg = Math.Round(historiesVolAll / 1200.0, 3);

            decimal vol = decimal.Parse(realtime["volume"].ToString());

            DateTime dt = Convert.ToDateTime(realtime["date"].ToString() + " " + realtime["time"].ToString());
            if (DateTime.Now.Day != dt.Day)
            {
                return Convert.ToDouble(vol) / 240 / historyVolAvg;
            }

            int dirtaMin = 0;
            switch (CommonData.TradeTimeState())
            {
                case DataDefine.TradeTimeState.BeforeMorning:
                case DataDefine.TradeTimeState.AfterNoon:
                case DataDefine.TradeTimeState.NotTradeDay:
                    dirtaMin = 240;
                    break;
                case DataDefine.TradeTimeState.Morning:
                    TimeSpan morningStart = new TimeSpan(9, 30, 0);
                    dirtaMin = (dt.TimeOfDay - morningStart).Minutes;
                    if (dirtaMin < 1)
                    {
                        dirtaMin = 1;
                    }
                    break;
                case DataDefine.TradeTimeState.BeforeNoon :
                    dirtaMin = 120;
                    break;
                case DataDefine.TradeTimeState.Noon:
                    TimeSpan noonStart = new TimeSpan(13, 0, 0);
                    dirtaMin = (dt.TimeOfDay - noonStart).Minutes;
                    if (dirtaMin < 1)
                    {
                        dirtaMin = 1;
                    }
                    dirtaMin += 120;
                    break;
            }

            return Convert.ToDouble(vol) / dirtaMin / historyVolAvg;
        }

        public Int64[] CalculateIO(DataTable perbid){
            if (perbid.Rows.Count == 0)
            {
                return new Int64[2]{0, 0};
            }
            Int64[] innerAndOutter = new Int64[2];
            foreach (DataRow row in perbid.Rows)
            {
                if (int.Parse(row["type"].ToString()) == 1)
                {
                    innerAndOutter[1] += int.Parse(row["vol"].ToString());
                }
                else if (int.Parse(row["type"].ToString()) == -1)
                {
                    innerAndOutter[0] += int.Parse(row["vol"].ToString());
                }
            }
            return innerAndOutter;
        }

        private void SetRGLabelColor(Label label, double price, double close)
        {
            if (price > close)
            {
                label.ForeColor = Color.Red;
            }
            else if (price < close)
            {
                label.ForeColor = Color.LawnGreen;
            }
            else
            {
                label.ForeColor = Color.White;
            }
        }

        private void detailPanel_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel)
            {
                ControlPaint.DrawBorder(e.Graphics, detailPanel.ClientRectangle,
                     Color.Red, 1, ButtonBorderStyle.Solid, //左边
                     Color.Red, 1, ButtonBorderStyle.Solid, //上边
                     Color.Red, 1, ButtonBorderStyle.Solid, //右边
                     Color.Red, 1, ButtonBorderStyle.Solid);//底边
            }
        }

        private void stockTitlePanel_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel)
            {
                ControlPaint.DrawBorder(e.Graphics, stockTitlePanel.ClientRectangle,
                     Color.Red, 0, ButtonBorderStyle.None, //左边
                     Color.Red, 0, ButtonBorderStyle.None, //上边
                     Color.Red, 0, ButtonBorderStyle.None, //右边
                     Color.Red, 1, ButtonBorderStyle.Solid);//底边
            }
        }

        private void abDirtaPanel_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel)
            {
                ControlPaint.DrawBorder(e.Graphics, abDirtaPanel.ClientRectangle,
                     Color.Red, 0, ButtonBorderStyle.None, //左边
                     Color.Red, 0, ButtonBorderStyle.None, //上边
                     Color.Red, 0, ButtonBorderStyle.None, //右边
                     Color.Red, 1, ButtonBorderStyle.Solid);//底边
            }
        }

        private void bidPanel_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel)
            {
                ControlPaint.DrawBorder(e.Graphics, bidPanel.ClientRectangle,
                     Color.Red, 0, ButtonBorderStyle.None, //左边
                     Color.Red, 0, ButtonBorderStyle.None, //上边
                     Color.Red, 0, ButtonBorderStyle.None, //右边
                     Color.Red, 1, ButtonBorderStyle.Solid);//底边
            }
        }

        private void askPanel_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel)
            {
                ControlPaint.DrawBorder(e.Graphics, askPanel.ClientRectangle,
                     Color.Red, 0, ButtonBorderStyle.None, //左边
                     Color.Red, 0, ButtonBorderStyle.None, //上边
                     Color.Red, 0, ButtonBorderStyle.None, //右边
                     Color.Red, 1, ButtonBorderStyle.Solid);//底边
            }
        }

        private void detailInfoaPanel_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel)
            {
                ControlPaint.DrawBorder(e.Graphics, detailInfoPanel.ClientRectangle,
                     Color.Red, 0, ButtonBorderStyle.None, //左边
                     Color.Red, 0, ButtonBorderStyle.None, //上边
                     Color.Red, 0, ButtonBorderStyle.None, //右边
                     Color.Red, 1, ButtonBorderStyle.Solid);//底边
            }
        }

        private void otherPanel_Paint(object sender, PaintEventArgs e)
        {
            if (sender is Panel)
            {
                ControlPaint.DrawBorder(e.Graphics, otherPanel.ClientRectangle,
                     Color.Red, 0, ButtonBorderStyle.None, //左边
                     Color.Red, 0, ButtonBorderStyle.None, //上边
                     Color.Red, 0, ButtonBorderStyle.None, //右边
                     Color.Red, 1, ButtonBorderStyle.Solid);//底边
            }
        }
    }
}
