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

namespace ImageOfStock
{
    public partial class DetailPanel : UserControl
    {
        private object refresh_lock = new Object();

        private String stockId = "0000001";
        public String StockId
        {
            set
            {
                stockId = value;
            }
        }
        private Label[] bidsPrice = new Label[5];
        private Label[] bidsVol = new Label[5];
        private Label[] asksPrice = new Label[5];
        private Label[] asksVol = new Label[5];

        public DetailPanel()
        {
            InitializeComponent();

            bidsPrice[0] = bid1Price;
            bidsPrice[1] = bid2Price;
            bidsPrice[2] = bid3Price;
            bidsPrice[3] = bid4Price;
            bidsPrice[4] = bid5Price;

            bidsVol[0] = bid1Vol;
            bidsVol[1] = bid2Vol;
            bidsVol[2] = bid3Vol;
            bidsVol[3] = bid4Vol;
            bidsVol[4] = bid5Vol;

            asksPrice[0] = ask1Price;
            asksPrice[1] = ask2Price;
            asksPrice[2] = ask3Price;
            asksPrice[3] = ask4Price;
            asksPrice[4] = ask5Price;

            asksVol[0] = ask1Vol;
            asksVol[1] = ask2Vol;
            asksVol[2] = ask3Vol;
            asksVol[3] = ask4Vol;
            asksVol[4] = ask5Vol;


            titlePanel.Paint += new PaintEventHandler(detail_Paint);
            dirtabidPanel.Paint += new PaintEventHandler(detail_Paint);
            bidPanel.Paint += new PaintEventHandler(detail_Paint);
            askPanel.Paint += new PaintEventHandler(detail_Paint);
            infoPanel.Paint += new PaintEventHandler(detail_Paint);
            otherPanel.Paint += new PaintEventHandler(detail_Paint);
            
        }

        public void BindData(DataRow realtime, DataTable histories, DataTable perbid)
        {
            titleCode.Text = stockId.Substring(1);
            titleName.Text = realtime["name"].ToString();

            double dataPrice = double.Parse(realtime["price"].ToString());
            double dataClose = double.Parse(realtime["closeP"].ToString());
            double dataOpen = double.Parse(realtime["openP"].ToString());

            int realtimeBidVol;
            double realtimeBidPrice;
            int realtimeAskVol;
            double realtimeAskPrice;

            int askAll = 0;
            int bidAll = 0;
            for (int i = 1; i < 6; ++i)
            {
                realtimeBidVol = int.Parse(realtime["bid" + i + "Vol"].ToString());
                bidAll += realtimeBidVol;
                realtimeBidPrice = double.Parse(realtime["bid" + i + "Price"].ToString());

                realtimeAskVol = int.Parse(realtime["ask" + i + "Vol"].ToString());
                askAll += realtimeAskVol;
                realtimeAskPrice = double.Parse(realtime["ask" + i + "Price"].ToString());

                bidsVol[i - 1].Text = (realtimeBidVol / 100).ToString();
                asksVol[i - 1].Text = (realtimeAskVol / 100).ToString();

                bidsPrice[i - 1].Text = realtimeBidPrice.ToString("0.00");
                SetRGLabelColor(bidsPrice[i - 1], realtimeBidPrice, dataClose);

                asksPrice[i - 1].Text = realtimeAskPrice.ToString("0.00");
                SetRGLabelColor(asksPrice[i - 1], realtimeAskPrice, dataClose);
            }

            int abd = bidAll - askAll;
            double abs = Math.Round(abd * 1.0 / (askAll + bidAll), 4) * 100;
            dirtabid.Text = (abd / 100).ToString();
            dirtabidPercent.Text = abs.ToString() + "%";
            if (abd > 0)
            {
                dirtabid.ForeColor = Color.Red;
                dirtabidPercent.ForeColor = Color.Red;
            }
            else if (abd < 0)
            {
                dirtabid.ForeColor = Color.LawnGreen;
                dirtabidPercent.ForeColor = Color.LawnGreen;
            }
            else
            {
                dirtabid.ForeColor = Color.White;
                dirtabidPercent.ForeColor = Color.White;
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
                volumePercent.ForeColor = Color.Red;
            }
            else if (dirta < 0)
            {
                updown.ForeColor = Color.LawnGreen;
                percent.ForeColor = Color.LawnGreen;
                volumePercent.ForeColor = Color.LawnGreen;
            }
            else
            {
                updown.ForeColor = Color.White;
                percent.ForeColor = Color.White;
                volumePercent.ForeColor = Color.White;
            }
            updown.Text = (dirta).ToString("0.00");
            percent.Text = (Math.Round(dirta / dataClose, 4) * 100).ToString() + "%";

            high.Text = (double.Parse(realtime["high"].ToString())).ToString("0.00");
            SetRGLabelColor(high, double.Parse(realtime["high"].ToString()), dataClose);

            low.Text = double.Parse(realtime["low"].ToString()).ToString("0.00");
            SetRGLabelColor(low, double.Parse(realtime["low"].ToString()), dataClose);

            volume.Text = (decimal.Parse(realtime["volume"].ToString()) / 100).ToString("0");

            volumePercent.Text = CalculateVR(realtime, histories).ToString("0.00");

            Int64[] innerAndOutter = CalculateIO(perbid);
            inner.Text = (innerAndOutter[0] / 100).ToString("0");
            outer.Text = (innerAndOutter[1] / 100).ToString("0");

            perbidList.BindData(perbid, realtime);
        }

        private double CalculateVR(DataRow realtime, DataTable histories)
        {
            if (histories.Rows.Count < 5)
            {
                return 0.0;
            }
            Int64 historiesVolAll = 0;
            int lastPos = histories.Rows.Count - 1;
            for (int i = 0; i < 5; ++i)
            {
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
            switch (GlobalData.TradeTimeState())
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
                case DataDefine.TradeTimeState.BeforeNoon:
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

        public Int64[] CalculateIO(DataTable perbid)
        {
            if (perbid.Rows.Count == 0)
            {
                return new Int64[2] { 0, 0 };
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

        private void detail_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(System.Drawing.Pens.Red, new PointF(0, 0), new PointF(Width, 0));
            e.Graphics.DrawLine(System.Drawing.Pens.Red, new PointF(0, 0), new PointF(0, Height));
            e.Graphics.DrawLine(System.Drawing.Pens.Red, new PointF(Width - 1, 0), new PointF(Width - 1, Height));
            e.Graphics.DrawLine(System.Drawing.Pens.Red, new PointF(0, Height - 1), new PointF(Width, Height - 1));
        }
    }
}
