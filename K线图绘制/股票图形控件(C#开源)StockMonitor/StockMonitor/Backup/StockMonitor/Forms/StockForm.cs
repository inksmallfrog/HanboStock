using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using ImageOfStock;
using System.Drawing.Drawing2D;

namespace StockMonitor.Forms
{
    public partial class StockForm : BaseForm
    {
        public StockForm()
        {
            InitializeComponent();
            InitMainMenu();
            CandleGraph();
        }

        private ToolStripMenuItem tsmiImport = null;

        private ToolStripMenuItem tsmiClose = null;

        public void InitMainMenu()
        {
            ToolStripDropDownButton tsddbFile = new ToolStripDropDownButton("�ļ�(&F)");
            this.menuBar.Items.Add(tsddbFile);
            tsmiImport = new ToolStripMenuItem("����(&I)");
            tsddbFile.DropDownItems.Add(tsmiImport);
            tsmiImport.Click += new EventHandler(tsmiOpen_Click);
            tsmiClose = new ToolStripMenuItem("�ر�(&E)");
            tsddbFile.DropDownItems.Add(tsmiClose);
            tsmiClose.Click += new EventHandler(tsmiClose_Click);
        }

        private void tsmiClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiOpen_Click(object sender, EventArgs e)
        {
            ofdFile.Filter = "XML Files(*.txt)|*.txt|All Files(*.*)|*.*";
            ofdFile.FilterIndex = 1;
            ofdFile.CheckFileExists = false;
            ofdFile.RestoreDirectory = true;
            if (ofdFile.ShowDialog() == DialogResult.OK)
            {
                List<string[]> list = new List<string[]>();
                using (FileStream fs = new FileStream(ofdFile.FileName, FileMode.Open))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                    {
                        sr.BaseStream.Seek(0, SeekOrigin.Begin);
                        while (sr.Peek() > -1)
                        {
                            string value = sr.ReadLine();
                            string[] records = value.ToString().Split('\t');
                            list.Add(records);
                        }
                    }
                }
                CandleGraph();
                this.chartGraph1.ProcessBarValue = 0;
                this.tsmiImport.Enabled = false;
                Thread refreshData = new Thread(new ParameterizedThreadStart(UpdateDataToGraph));
                refreshData.IsBackground = true;
                refreshData.Start(list);
            }
        }

        public void CandleGraph()
        {
            this.chartGraph1.ResetNullGraph();
            this.chartGraph1.UseScrollAddSpeed = true;
            this.chartGraph1.SetXScaleField("����");
            this.chartGraph1.CanDragSeries = true;
            this.chartGraph1.SetSrollStep(10, 10);
            this.chartGraph1.ShowLeftScale = true;
            this.chartGraph1.ShowRightScale = true;
            this.chartGraph1.LeftPixSpace = 85;
            this.chartGraph1.RightPixSpace = 85;
            //K��ͼ+BS��
            mainPanelID = this.chartGraph1.AddChartPanel(40);
            string candleName = "K��ͼ-1";
            this.chartGraph1.AddCandle(candleName, "OPEN", "HIGH", "LOW", "CLOSE", mainPanelID, true);
            this.chartGraph1.YMBuySellSignal(mainPanelID, candleName, "BUYEMA", "(CLOSE+HIGH+LOW)/3", "SELLEMA", "BUYEMA");
            this.chartGraph1.AddBollingerBands("MID", "UP", "DOWN", "CLOSE", 20, 2, mainPanelID);
            this.chartGraph1.SetYScaleField(mainPanelID, new string[] { "HIGH","LOW"});
            //�ɽ���
            volumePanelID = this.chartGraph1.AddChartPanel(20);
            this.chartGraph1.AddHistogram("VOL", "", candleName, volumePanelID);
            this.chartGraph1.SetHistogramStyle("VOL", Color.Red, Color.SkyBlue, 1, false);
            this.chartGraph1.AddSimpleMovingAverage("VOL-MA1", "MA5", "VOL", 5, volumePanelID);
            this.chartGraph1.SetTrendLineStyle("VOL-MA1", Color.White, Color.White, 1, DashStyle.Solid);
            this.chartGraph1.AddSimpleMovingAverage("VOL-MA2", "MA10", "VOL",10, volumePanelID);
            this.chartGraph1.SetTrendLineStyle("VOL-MA2", Color.Yellow, Color.Yellow, 1, DashStyle.Solid);
            this.chartGraph1.AddSimpleMovingAverage("VOL-MA3", "MA20", "VOL", 20, volumePanelID);
            this.chartGraph1.SetTrendLineStyle("VOL-MA3", Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 255), 1, DashStyle.Solid);
            this.chartGraph1.SetTick(volumePanelID, 1);
            this.chartGraph1.SetDigit(volumePanelID, 0);
            kdjPanelID = this.chartGraph1.AddChartPanel(20);
            this.chartGraph1.AddStochasticOscillator("K", "D", "J", 9, "CLOSE", "HIGH", "LOW", kdjPanelID);
            macdPanelID = this.chartGraph1.AddChartPanel(20);
            this.chartGraph1.AddMacd("MACD", "DIFF", "DEA", "CLOSE", 26, 12, 9, macdPanelID);
        }

        int mainPanelID = -1;
        int volumePanelID = -1;
        int kdjPanelID = -1;
        int macdPanelID = -1;

        /// <summary>
        /// ��������¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomForm_Load(object sender, EventArgs e)
        {
            this.Text = "STOCK MONITOR";
            this.Location = new Point(0, 0);
            this.Size = new Size(Screen.GetWorkingArea(this).Width, Screen.GetWorkingArea(this).Height);
            CandleGraph();
        }

        /// <summary>
        /// ���½�����
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateProcessBar(object obj)
        {
            int[] values = obj as int[];
            int total = values[0];
            int current = values[1];
            int processValue = Convert.ToInt32((double)current / (double)total * 100);
            if (processValue > this.chartGraph1.ProcessBarValue)
            {
                this.chartGraph1.ProcessBarValue = processValue;
            }
            if (current == total - 2)
            {
                this.chartGraph1.ProcessBarValue = 100;
                this.tsmiImport.Enabled = true;
                this.chartGraph1.RefreshGraph();
            }
        }

        private delegate void UpdateProcessBarDelegate(object obj);

        /// <summary>
        /// �������ݵ�ͼ��
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateDataToGraph(object obj)
        {
            List<string[]> list = obj as List<string[]>;
            string[] str = list[0];
            this.chartGraph1.SetTitle(mainPanelID, str[2] + "(" + str[1] + ") " + str[0]);
            this.chartGraph1.SetTitle(kdjPanelID, "KDJ(9,3,3)");
            this.chartGraph1.SetTitle(volumePanelID, "VOL(5,10,20)");
            this.chartGraph1.SetTitle(macdPanelID, "MACD(12,26,9)");
            string lineType = str[0];
            switch (lineType)
            {
                case "5����":
                case "15����":
                case "30����":
                case "60����":
                    this.chartGraph1.SetIntervalType(mainPanelID, ChartGraph.IntervalType.Minute);
                    this.chartGraph1.SetIntervalType(volumePanelID, ChartGraph.IntervalType.Minute);
                    this.chartGraph1.SetIntervalType(kdjPanelID, ChartGraph.IntervalType.Minute);
                    break;
                case "����":
                    this.chartGraph1.SetIntervalType(mainPanelID, ChartGraph.IntervalType.Day);
                    this.chartGraph1.SetIntervalType(volumePanelID, ChartGraph.IntervalType.Day);
                    this.chartGraph1.SetIntervalType(kdjPanelID, ChartGraph.IntervalType.Day);
                    break;
                case "����":
                    this.chartGraph1.SetIntervalType(mainPanelID, ChartGraph.IntervalType.Week);
                    this.chartGraph1.SetIntervalType(volumePanelID, ChartGraph.IntervalType.Week);
                    this.chartGraph1.SetIntervalType(kdjPanelID, ChartGraph.IntervalType.Week);
                    break;
                case "����":
                    this.chartGraph1.SetIntervalType(mainPanelID, ChartGraph.IntervalType.Month);
                    this.chartGraph1.SetIntervalType(volumePanelID, ChartGraph.IntervalType.Month);
                    this.chartGraph1.SetIntervalType(kdjPanelID, ChartGraph.IntervalType.Month);
                    break;
            }
            this.chartGraph1.RefreshGraph();
            for (int i = list.Count - 1; i >= 2; i--)
            {
                string[] records = list[i];
                string timeKey = records[0];
                int year = 1970;
                int month = 1;
                int day = 1;
                int hour = 0;
                int minute = 0;
                switch (lineType)
                {
                    case "5����":
                    case "15����":
                    case "30����":
                    case "60����":
                        month = Convert.ToInt32(timeKey.Substring(0, 1));
                        day = Convert.ToInt32(timeKey.Substring(1, 2));
                        hour = Convert.ToInt32(timeKey.Substring(3, 2));
                        minute = Convert.ToInt32(timeKey.Substring(5, 2));
                        break;
                    case "����":
                    case "����":
                        year = Convert.ToInt32(timeKey.Substring(0, 4));
                        month = Convert.ToInt32(timeKey.Substring(4, 2));
                        day = Convert.ToInt32(timeKey.Substring(6, 2));
                        break;
                    case "����":
                        year = Convert.ToInt32(timeKey.Substring(0, 4));
                        month = Convert.ToInt32(timeKey.Substring(4, 2));
                        break;
                }
                DateTime dt = new DateTime(year, month, day, hour, minute, 0);
                this.chartGraph1.SetValue("OPEN", records[1], dt);
                this.chartGraph1.SetValue("HIGH", records[2], dt);
                this.chartGraph1.SetValue("LOW", records[3], dt);
                this.chartGraph1.SetValue("CLOSE", records[4], dt);
                this.chartGraph1.SetValue("VOL", records[6], dt);
                double ymValue = (Convert.ToDouble(records[4]) + Convert.ToDouble(records[3]) + Convert.ToDouble(records[2])) / 3;
                this.chartGraph1.SetValue("(CLOSE+HIGH+LOW)/3",ymValue,dt);
                this.BeginInvoke(new UpdateProcessBarDelegate(UpdateProcessBar), new int[] { list.Count, list.Count - i });
            }       
            this.chartGraph1.Enabled = true;
        }
    }
}