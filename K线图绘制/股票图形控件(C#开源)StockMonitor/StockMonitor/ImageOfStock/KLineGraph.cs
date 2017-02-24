﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;

namespace ImageOfStock
{
    [ToolboxItem(true)]
    public partial class KLineGraph : ContainerControl
    {
        //预计本周代码行数将达到8000+
        //1.趋势线拖动
        //2.标记可以被设置为可拖动
        //3.大菜单 技术分析以及右键菜单 添加指标
        //4.删除指标
        //5.MACD数据少时不准确
        //6.EMA可能有错误
        //7.布林带不准确(优先A)
        //8.SAR指标(优先C)
        //9.日期的格式化
        //10.要能滚动看不见的变量

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public KLineGraph()
        {
            InitializeComponent();
            grammaParser.Graph = this;  
            //加载事件
            this.HandleCreated += new EventHandler(ChartGraph_HandleCreated);
            this.SizeChanged += new EventHandler(ChartGraph_SizeChanged);
            this.Paint += new PaintEventHandler(PicGraph_Paint);
            this.PreviewKeyDown += new PreviewKeyDownEventHandler(PicGraph_PreviewKeyDown);
            this.KeyUp += new KeyEventHandler(ChartGraph_KeyUp);
            this.MouseMove += new MouseEventHandler(ChartGraph_MouseMove);
            this.MouseUp += new MouseEventHandler(ChartGraph_MouseUp);
            this.MouseDown += new MouseEventHandler(ChartGraph_MouseDown);
            this.MouseWheel += new MouseEventHandler(ChartGraph_MouseWheel);
        }
        #endregion

        #region 字段
        /// <summary>
        /// X轴显示的数据字段
        /// </summary>
        private string timekeyField;

        [Browsable(false)]
        public string TimekeyField
        {
            get { return timekeyField; }
            set { timekeyField = value; }
        }

        /// <summary>
        /// 空值常量
        /// </summary>
        public const double NULL = double.MinValue;

        /// <summary>
        /// 十字线的Y坐标
        /// </summary>
        private int crossHair_y = -1;

        /// <summary>
        /// 鼠标点击时的x坐标
        /// </summary>
        private int mouse_x = -1;

        /// <summary>
        /// 鼠标点击时的y坐标
        /// </summary>
        private int mouse_y = -1;

        private GrammaParser grammaParser = new GrammaParser();
        public GrammaParser Gramma
        {
            get
            {
                return grammaParser;
            }
        }

        /// <summary>
        /// 保存接收到的所有数据的DataTable.
        /// </summary>
        private DataTable dtAllMsg = new DataTable();

        public DataTable DtAllMsg
        {
            get { return dtAllMsg; }
            set { dtAllMsg = value; }
        }

        /// <summary>
        /// 是否显示十字线
        /// </summary>
        private bool showCrossHair = false;

        [Browsable(false)]
        public bool ShowCrossHair
        {
            get { return showCrossHair; }
            set { showCrossHair = value; }
        }

        /// <summary>
        /// 每条数据所占的空间
        /// </summary>
        private int axisSpace;

        [Browsable(true)]
        public int AxisSpace
        {
            get { return axisSpace; }
            set { axisSpace = value; }
        }

        private int showPercent;
        [Browsable(true)]
        public int ShowPercent
        {
            get { return showPercent; }
            set { showPercent = value; }
        }

        /// <summary>
        /// 左侧坐标轴的空隙
        /// </summary>
        private int leftPixSpace;

        [Browsable(true)]
        public int LeftPixSpace
        {
            get
            {
                return leftPixSpace;
            }
            set
            {
                leftPixSpace = value;
            }
        }

        /// <summary>
        /// 右侧左标轴的空隙
        /// </summary>
        private int rightPixSpace;

        [Browsable(true)]
        public int RightPixSpace
        {
            get
            {
                return rightPixSpace;
            }
            set
            {
                rightPixSpace = value;
            }
        }

        /// <summary>
        /// 是否显示右侧的坐标轴
        /// </summary>
        [DefaultValue(true)]
        private bool showRightScale;

        [Browsable(true)]
        public bool ShowRightScale
        {
            get
            {
                return showRightScale;
            }
            set
            {
                showRightScale = value;
            }
        }

        /// <summary>
        /// 是否显示左侧的坐标轴
        /// </summary>
        private bool showLeftScale;

        [Browsable(true)]
        public bool ShowLeftScale
        {
            get { return showLeftScale; }
            set
            {
                showLeftScale = value;
            }
        }

        /// <summary>
        /// 第一条可见的记录
        /// </summary>
        private int firstVisibleRecord = 0;

        [Browsable(false)]
        public int FirstVisibleRecord
        {
            get { return firstVisibleRecord; }
            set
            {
                firstVisibleRecord = value;
            }
        }

        /// <summary>
        /// 最后一条可见的记录
        /// </summary>
        private int lastVisibleRecord = 0;

        [Browsable(false)]
        public int LastVisibleRecord
        {
            get { return lastVisibleRecord; }
            set
            {
                lastVisibleRecord = value;
                if (this.dtAllMsg.Rows.Count > 0)
                {
                    try { this.lastVisibleTimeKey = dtAllMsg.Rows[lastVisibleRecord - 1][timekeyField].ToString(); }
                    catch { };
                    if (LastVisibleRecord == this.dtAllMsg.Rows.Count)
                    {
                        lastRecordIsVisible = true;
                    }
                    else
                    {
                        lastRecordIsVisible = false;
                    }
                }
                else
                {
                    this.lastVisibleTimeKey = string.Empty;
                    lastRecordIsVisible = false;
                }
            }
        }

        /// <summary>
        /// 最后一条记录是否可见
        /// </summary>
        private bool lastRecordIsVisible = false;

        /// <summary>
        /// 最后一条可见记录的时间
        /// </summary>
        private string lastVisibleTimeKey = string.Empty;

        /// <summary>
        /// 鼠标选中的记录索引,从0开始
        /// </summary>
        private int crossOverIndex;

        [Browsable(false)]
        public int CrossOverIndex
        {
            get { return crossOverIndex; }
            set { crossOverIndex = value; }
        }

        /// <summary>
        /// 图像左滚的幅度
        /// </summary>
        private int scrollLeftStep = 1;

        [Browsable(true)]
        public int ScrollLeftStep
        {
            get { return scrollLeftStep; }
            set { scrollLeftStep = value; }
        }

        /// <summary>
        /// 图像右滚的幅度
        /// </summary>
        private int scrollRightStep = 1;

        [Browsable(true)]
        public int ScrollRightStep
        {
            get { return scrollRightStep; }
            set { scrollRightStep = value; }
        }

        /// <summary>
        /// 可以拖动线条
        /// </summary>
        private bool canDragSeries = false;

        public bool CanDragSeries
        {
            get { return canDragSeries; }
            set { canDragSeries = value; }
        }

        /// <summary>
        /// 进度条的值
        /// </summary>
        private int processBarValue = 0;

        [Browsable(false)]
        public int ProcessBarValue
        {
            get { return processBarValue; }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    processBarValue = value;
                }
                int pieR = 100;
                if (this.IsHandleCreated)
                {
                    Rectangle ellipseRect = new Rectangle(this.Width / 2 - pieR, this.Height / 2 - pieR, pieR * 2, pieR * 2);
                    DrawGraph(ellipseRect);
                }
            }
        }

        /// <summary>
        /// 股指提示的记录索引
        /// </summary>
        private int vp_index = -1;

        /// <summary>
        /// 保存面板的表
        /// </summary>
        private Dictionary<int, ChartPanel> dicChartPanel = new Dictionary<int, ChartPanel>();

        private ChartPanel bottomPanel;

        /// <summary>
        /// 刷新图像的锁
        /// </summary>
        private object refresh_lock = new object();

        /// <summary>
        /// 被选中的线条对象
        /// </summary>
        private object selectedObject;

        /// <summary>
        /// 鼠标最后一次移动的事件
        /// </summary>
        private DateTime lastMouseMoveTime = DateTime.Now;

        /// <summary>
        /// 标识是否准备绘制股指提示框
        /// </summary>
        private bool drawValuePanelFlag = false;

        /// <summary>
        /// 绘制显示股指提示框的委托
        /// </summary>
        private delegate void ShowValuePanelDelegate();

        /// <summary>
        /// 绘制图像的委托
        /// </summary>
        private delegate void DrawGraphDelegate();

        /// <summary>
        /// 控件标题占据的矩形作为键,控件本身作为值的表
        /// </summary>
        private Dictionary<RectangleF, object> objectRectDic = new Dictionary<RectangleF, object>();

        /// <summary>
        /// 按键滚动的幅度
        /// </summary>
        private int currentScrollStep = 1;

        /// <summary>
        /// 启用滚动加速效果
        /// </summary>
        private bool useScrollAddSpeed = false;

        public bool UseScrollAddSpeed
        {
            get { return useScrollAddSpeed; }
            set { useScrollAddSpeed = value; }
        }

        /// <summary>
        /// 正在改变大小的图层
        /// </summary>
        private ChartPanel userResizePanel = null;

        private List<IndicatorMovingAverage> indMovingAverageList = new List<IndicatorMovingAverage>();

        /// <summary>
        /// 简单移动平均线的集合
        /// </summary>
        private List<IndicatorSimpleMovingAverage> indSimpleMovingAverageList = new List<IndicatorSimpleMovingAverage>();

        /// <summary>
        /// 指数移动平均线的集合
        /// </summary>
        private List<IndicatorExponentialMovingAverage> indExponentialMovingAverageList = new List<IndicatorExponentialMovingAverage>();

        /// <summary>
        /// 随机指标的集合
        /// </summary>
        private List<IndicatorStochasticOscillator> indStochasticOscillatorList = new List<IndicatorStochasticOscillator>();
        private IndicatorStochasticOscillator indStochasticOscillator = null;

        /// <summary>
        /// 指数平滑异同移动平均线的集合
        /// </summary>
        private List<IndicatorMACD> indMacdList = new List<IndicatorMACD>();
        private IndicatorMACD indMACD = null;

        /// <summary>
        /// 布林带的集合
        /// </summary>
        private List<IndicatorBollingerBands> indBollList = new List<IndicatorBollingerBands>();
        #endregion

        #region 事件
        /// <summary>
        /// 鼠标滚动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                ZoomIn(2);
            }
            else
            {
                ZoomOut(2);
            }
            this.setVisibleExtremeValue();
            ResetCrossOverRecord();
            DrawGraph();
            this.Focus();
        }


        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_MouseMove(object sender, MouseEventArgs e)
        {
            vp_index = -1;
            if (showCrossHair)
            {
                selectedObject = null;
            }
            if (selectedObject != null)
            {
                if (lastMouseMoveTime.AddTicks(1000000) < DateTime.Now)
                {
                    DrawGraph();
                }
                lastMouseMoveTime = DateTime.Now;
                drawValuePanelFlag = true;
            }
            if (this.userResizePanel == null)
            {
                if (!showCrossHair && !(e.Button==MouseButtons.Left))
                {
                    int pIndex = 0;
                    //当鼠标到纵向下边线上时，认为是需要调整大小
                    foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                    {
                        pIndex++;
                        if (pIndex == dicChartPanel.Count)
                        {
                            break;
                        }
                        Rectangle resizeRect = new Rectangle(0, chartPanel.RectPanel.Bottom - 2, chartPanel.RectPanel.Width, 4);
                        if (resizeRect.Contains(e.Location))
                        {
                            this.Cursor = Cursors.SizeNS;
                            goto OutLoop;
                        }
                    }
                    if (this.Cursor == Cursors.SizeNS)
                    {
                        this.Cursor = Cursors.Default;
                    }
                OutLoop: ;
                }
            }
        }

        /// <summary>
        /// 鼠标弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_MouseUp(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Default;
            Point mp = GetCrossHairPoint();
            //实现线条的拖放
            if (canDragSeries)
            {
                if (!showCrossHair && selectedObject != null)
                {
                    foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                    {
                        if (mp.Y >= chartPanel.RectPanel.Y && mp.Y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height)
                        {
                            if (selectedObject is TrendLineSeries)
                            {
                                TrendLineSeries tls = selectedObject as TrendLineSeries;
                                if (!chartPanel.TrendLineSeriesList.Contains(tls))
                                {
                                    TitleField tfWaitToDrag = null;
                                    foreach (ChartPanel cp in this.dicChartPanel.Values)
                                    {
                                        if (cp.TrendLineSeriesList.Contains(tls))
                                        {
                                            cp.TrendLineSeriesList.Remove(tls);
                                            if (cp.YScaleField.Contains(tls.Field))
                                            {
                                                cp.YScaleField.Remove(tls.Field);
                                            }
                                            foreach (TitleField tf in cp.TitleFieldList)
                                            {
                                                if (!tf.MainFlag && tf.RelateSeriesField == tls.Field)
                                                {
                                                    tfWaitToDrag = tf;
                                                }
                                            }
                                            if (tfWaitToDrag != null)
                                            {
                                                cp.TitleFieldList.Remove(tfWaitToDrag); 
                                            }
                                            break;
                                        }
                                    }
                                    chartPanel.TrendLineSeriesList.Add(tls);
                                    chartPanel.YScaleField.Add(tls.Field);
                                    if (tfWaitToDrag != null)
                                    {
                                        chartPanel.TitleFieldList.Add(tfWaitToDrag);
                                    }
                                    selectedObject = null;
                                    RefreshGraph();
                                }
                            }
                            else if (selectedObject is HistogramSeries)
                            {
                                HistogramSeries hs = selectedObject as HistogramSeries;
                                if (!chartPanel.HistoramSeriesList.Contains(hs))
                                {
                                    TitleField tfWaitToDrag = null;
                                    foreach (ChartPanel cp in this.dicChartPanel.Values)
                                    {
                                        if (cp.HistoramSeriesList.Contains(hs))
                                        {
                                            if (cp.YScaleField.Contains(hs.Field))
                                            {
                                                cp.YScaleField.Remove(hs.Field);
                                            }
                                            cp.HistoramSeriesList.Remove(hs);
                                            foreach (TitleField tf in cp.TitleFieldList)
                                            {
                                                if (!tf.MainFlag && tf.RelateSeriesField == hs.Field)
                                                {
                                                    tfWaitToDrag = tf;
                                                }
                                            }
                                            if (tfWaitToDrag != null)
                                            {
                                                cp.TitleFieldList.Remove(tfWaitToDrag);
                                                
                                            }
                                            break;
                                        }
                                    }
                                    if (tfWaitToDrag != null)
                                    {
                                        chartPanel.TitleFieldList.Add(tfWaitToDrag);
                                    }
                                    chartPanel.HistoramSeriesList.Add(hs);
                                    chartPanel.YScaleField.Add(hs.Field);
                                    selectedObject = null;
                                    RefreshGraph();
                                }
                            }
                            else if (selectedObject is CandleSeries)
                            {
                                CandleSeries cs = selectedObject as CandleSeries;
                                if (!chartPanel.CandleSeriesList.Contains(cs))
                                {
                                    List<TitleField> waitToRemoveTfList = new List<TitleField>();
                                    foreach (ChartPanel cp in this.dicChartPanel.Values)
                                    {
                                        if (cp.CandleSeriesList.Contains(cs))
                                        {
                                            if (cp.YScaleField.Contains(cs.CloseField))
                                            {
                                                cp.YScaleField.Remove(cs.CloseField);
                                            }
                                            if (cp.YScaleField.Contains(cs.HighField))
                                            {
                                                cp.YScaleField.Remove(cs.HighField);
                                            }
                                            if (cp.YScaleField.Contains(cs.LowField))
                                            {
                                                cp.YScaleField.Remove(cs.LowField);
                                            }
                                            if (cp.YScaleField.Contains(cs.OpenField))
                                            {
                                                cp.YScaleField.Remove(cs.OpenField);
                                            }
                                            cp.CandleSeriesList.Remove(cs);
                                            foreach (TitleField tf in cp.TitleFieldList)
                                            {
                                                if (!tf.MainFlag)
                                                {
                                                    if (tf.RelateSeriesField == cs.OpenField || tf.RelateSeriesField == cs.HighField
                                                        || tf.RelateSeriesField == cs.LowField || tf.RelateSeriesField == cs.CloseField)
                                                    {
                                                        waitToRemoveTfList.Add(tf);
                                                    }
                                                }
                                            }
                                            foreach (TitleField tf in waitToRemoveTfList)
                                            {
                                                cp.TitleFieldList.Remove(tf);
                                            }
                                        }
                                    }
                                    chartPanel.CandleSeriesList.Add(cs);
                                    chartPanel.YScaleField.Add(cs.OpenField);
                                    chartPanel.YScaleField.Add(cs.HighField);
                                    chartPanel.YScaleField.Add(cs.CloseField);
                                    chartPanel.YScaleField.Add(cs.LowField);
                                    foreach (TitleField tf in waitToRemoveTfList)
                                    {
                                        chartPanel.TitleFieldList.Add(tf);
                                    }
                                    selectedObject = null;
                                    RefreshGraph();
                                }
                            }
                        }
                    }
                }
            }
            DragChartPanel();
        }

        /// <summary>
        /// 鼠标单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_MouseDown(object sender, MouseEventArgs e)
        {
            Point mp = GetCrossHairPoint();
            mouse_x = mp.X;
            mouse_y = mp.Y;
            if (e.Button == MouseButtons.Left && processBarValue == 0)
            {
                crossHair_y = mp.Y;
                if (e.Clicks == 1)
                {
                    //单击改变十字线准星位置
                    this.crossOverIndex = this.GetCrossOverIndex();
                    object obj = JudgeSelectedSeries(crossOverIndex, crossHair_y, true);
                    if (obj != null && !showCrossHair && canDragSeries)
                    {
                        this.Cursor = Cursors.Cross;
                    }
                    DrawGraph();
                }
                else if (e.Clicks == 2)
                {
                    //双击显示或隐藏十字线
                    this.ShowCrossHair = !this.ShowCrossHair;
                    this.crossOverIndex = this.GetCrossOverIndex();
                    selectedObject = null;
                    DrawGraph();
                }
            }
            //判断是否要进行resize
            if (!showCrossHair)
            {
                int pIndex = 0;
                //当鼠标到纵向下边线上时，认为是需要调整大小
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    pIndex++;
                    if (pIndex == dicChartPanel.Count)
                    {
                        break;
                    }
                    Rectangle resizeRect = new Rectangle(0, chartPanel.RectPanel.Bottom - 2, chartPanel.RectPanel.Width, 4);
                    if (resizeRect.Contains(mp))
                    {
                        this.Cursor = Cursors.SizeNS;
                        userResizePanel = chartPanel;
                        DrawGraph();
                        goto OutLoop;
                    }
                }
            OutLoop: ;
            }
            else
            {
                userResizePanel = null;
            }
            this.Focus();
        }

        /// <summary>
        /// 重绘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PicGraph_Paint(object sender, PaintEventArgs e)
        {
            DrawGraph();
        }


        /// <summary>
        /// 图形大小改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_SizeChanged(object sender, EventArgs e)
        {
            if (this.Size.Width != 0 && this.Size.Height != 0)
            {
                ResizeGraph();
                RefreshGraph();
            }
        }

        /// <summary>
        /// 键盘弹起事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_KeyUp(object sender, KeyEventArgs e)
        {
            currentScrollStep = 1;
        }

        /// <summary>
        /// 键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PicGraph_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            vp_index = -1;
            if (processBarValue == 0)
            {
                bool flag = false;
                bool locateCrossHairFlag = false;
                switch (e.KeyData)
                {
                    case Keys.Left:
                        flag = true;
                        if (showCrossHair)
                        {
                            CrossHairScrollLeft();
                            locateCrossHairFlag = true;
                        }
                        else
                        {
                            this.ScrollLeft(currentScrollStep);
                        }
                        break;
                    case Keys.Right:
                        flag = true;
                        if (showCrossHair)
                        {
                            CrossHairScrollRight();
                            locateCrossHairFlag = true;
                        }
                        else
                        {
                            this.ScrollRight(currentScrollStep);
                        }
                        break;
                    case Keys.Up:
                        flag = true;
                        this.ZoomIn(currentScrollStep);
                        break;
                    case Keys.Down:
                        flag = true;
                        this.ZoomOut(currentScrollStep);

                        break;
                }
                if (flag)
                {
                    this.setVisibleExtremeValue();
                    ResetCrossOverRecord();
                    if (locateCrossHairFlag)
                    {
                        LocateCrossHair();
                    }
                    DrawGraph();
                }
            }
            if (useScrollAddSpeed)
            {
                if (currentScrollStep < 40)
                {
                    currentScrollStep += 5;
                }
            }
            else
            {
                currentScrollStep = 1;
            }
            this.Focus();
        }

        /// <summary>
        /// 初始化界面属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartGraph_HandleCreated(object sender, EventArgs e)
        {
            Thread checkMouseMoveThread = new Thread(new ThreadStart(checkMouseMoveLoop));
            checkMouseMoveThread.IsBackground = true;
            checkMouseMoveThread.Start();
        }
        #endregion

        private int mainPanelID;
        private int volumePanelID;
        private int bottomPanelID;

        public void CandleGraph()
        {
            ResetNullGraph();
            UseScrollAddSpeed = true;
            SetXScaleField("date");
            CanDragSeries = true;
            SetSrollStep(50, 50);
            ShowLeftScale = false;
            ShowRightScale = true;
            LeftPixSpace = 0;
            RightPixSpace = 85;

            //K线图+BS点
            mainPanelID = AddChartPanel(60);
            string candleName = "K线图-1";
            CandleSeries cs = AddCandle(candleName, "openP", "high", "low", "closeP", true);
            SetCandleToPanel("openP", "high", "low", "closeP", cs, mainPanelID);
            YMBuySellSignal(mainPanelID, candleName, "BUYEMA", "(CLOSE+HIGH+LOW)/3", "SELLEMA", "BUYEMA");
            AddBollingerBands("MID", "UP", "DOWN", "closeP", 20, 2, mainPanelID);
            SetYScaleField(mainPanelID, new string[] { "high", "low" });

            //成交量
            volumePanelID = AddChartPanel(20);
            HistogramSeries hs = AddHistogram("volume", "", candleName);
            SetHistogramToPanel("volume", hs, volumePanelID);
            SetHistogramStyle("volume", Color.Red, Color.SkyBlue, 1, false);
            AddSimpleMovingAverage("VOL-MA1", "MA5", "volume", 5, volumePanelID);
            SetTrendLineStyle("VOL-MA1", Color.White, Color.White, 1, DashStyle.Solid);
            AddSimpleMovingAverage("VOL-MA2", "MA10", "volume", 10, volumePanelID);
            SetTrendLineStyle("VOL-MA2", Color.Yellow, Color.Yellow, 1, DashStyle.Solid);
            AddSimpleMovingAverage("VOL-MA3", "MA20", "volume", 20, volumePanelID);
            SetTrendLineStyle("VOL-MA3", Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 255), 1, DashStyle.Solid);
            SetTick(volumePanelID, 1);
            SetDigit(volumePanelID, 0);

            bottomPanelID = AddChartPanel(20);
            indStochasticOscillator = AddStochasticOscillator("K", "D", "J", 9, "closeP", "high", "low");
            indMACD = AddMacd("MACD", "DIFF", "DEA", "closeP", 26, 12, 9);
            //SetStochasticOscillatorToPanel("K", "D", "J", indStochasticOscillator, bottomPanelID);
            SetMacdToPanel("MACD", "DIFF", "DEA", indMACD, bottomPanelID);
        }
        //"MACD", "DIFF", "DEA",

        //Table内容："stockId"
        //           "date"
        //           "openP"
        //           "closeP"
        //           "high"
        //           "low"
        //           "volume"
        public void BindData(DataTable data)
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {            
                foreach (string field in chartPanel.YScaleField)
                {
                    if (data.Columns.Contains(field))
                    {
                        continue;
                    }
                    DataColumn c = new DataColumn(field);
                    data.Columns.Add(c);
                }
            }
            dtAllMsg = data;
            int dataCount = GetMaxVisibleRecord();

            if (dataCount >= data.Rows.Count)
            {
                firstVisibleRecord = 0;
                lastVisibleRecord = data.Rows.Count - 1;
            }

            foreach (IndicatorSimpleMovingAverage indSMA in this.indSimpleMovingAverageList)
            {
                indSMA.DataSource = data;
            }

            foreach (IndicatorExponentialMovingAverage indEMA in this.indExponentialMovingAverageList)
            {
                indEMA.DataSource = data;
            }

            foreach (IndicatorStochasticOscillator indSO in this.indStochasticOscillatorList)
            {
                indSO.DataSource = data;
            }

            foreach (IndicatorMACD indMacd in this.indMacdList)
            {
                indMacd.DataSource = data;
            }

            foreach (IndicatorBollingerBands indBOLL in this.indBollList)
            {
                indBOLL.DataSource = data;
            }

            //简单移动平均线
            bool ind_flag = false;
            for (int m = 0; m < data.Rows.Count; m++)
            {
                double value = (double.Parse(data.Rows[m]["closeP"].ToString()) + double.Parse(data.Rows[m]["high"].ToString()) 
                              + double.Parse(data.Rows[m]["low"].ToString())) / 3;
                SetValue(data.Rows[m], "(CLOSE+HIGH+LOW)/3", value);
                if (this.indSimpleMovingAverageList.Count > 0)
                {
                    foreach (IndicatorSimpleMovingAverage indSMA in this.indSimpleMovingAverageList)
                    {
                        ind_flag = true;
                        CalcutaIndicator(indSMA, data, m, m);
                    }
                }
                //指数移动平均线
                if (this.indExponentialMovingAverageList.Count > 0)
                {
                    foreach (IndicatorExponentialMovingAverage indEMA in this.indExponentialMovingAverageList)
                    {
                        ind_flag = true;
                        CalcutaIndicator(indEMA, data, m, m);
                    }
                }
                //随机指标
                if (this.indStochasticOscillatorList.Count > 0)
                {
                    foreach (IndicatorStochasticOscillator indSO in this.indStochasticOscillatorList)
                    {
                        ind_flag = true;
                        CalcutaIndicator(indSO, data, m, m);
                    }
                }
                //MACD指标
                if (this.indMacdList.Count > 0)
                {
                    foreach (IndicatorMACD indMacd in this.indMacdList)
                    {
                        ind_flag = true;
                        CalcutaIndicator(indMacd, data, m, m);
                    }
                }
                //BOLL线
                if (this.indBollList.Count > 0)
                {
                    foreach (IndicatorBollingerBands indBOLL in this.indBollList)
                    {
                        ind_flag = true;
                        CalcutaIndicator(indBOLL, data, m, m);
                    }
                }
                if (!ind_flag)
                {
                    break;
                }
            }
        }

        #region 界面设置
        /// <summary>
        /// 清除选中
        /// </summary>
        public void ClearSelectObj()
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                {
                    cs.HasSelect = false;
                }
                foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                {
                    hs.HasSelect = false;
                }
                foreach (TrendLineSeries tls in chartPanel.TrendLineSeriesList)
                {
                    tls.HasSelect = false;
                }
            }
            selectedObject = null;
        }

        /// <summary>
        /// 添加标记
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="timeKey"></param>
        /// <param name="st"></param>
        public SignalSeries AddSignal(int panelID, string timeKey, SignalType st, Color stColor, double value, bool canDrag)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                SignalSeries ss = new SignalSeries(value, st, stColor,canDrag);
                if (!chartPanel.SignalSeriesDic.ContainsKey(timeKey))
                {
                    chartPanel.SignalSeriesDic[timeKey] = new List<SignalSeries>();
                }
                chartPanel.SignalSeriesDic[timeKey].Add(ss);
                return ss;
            }
            return null;
        }

        /// <summary>
        /// 移除标记
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="timeKey"></param>
        public void RemoveSignal(int panelID, string timeKey)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                if (chartPanel.SignalSeriesDic.ContainsKey(timeKey))
                {
                    chartPanel.SignalSeriesDic.Remove(timeKey);
                }
            }
        }

        /// <summary>
        /// 判断是否选中了线条
        /// </summary>
        public object JudgeSelectedSeries(int curIndex, int mpY, bool setSelect)
        {
            bool hasSelect = false;
            object obj = null;
            if (setSelect)
            {
                Point mp = GetCrossHairPoint();
                foreach (RectangleF titleRect in this.objectRectDic.Keys)
                {
                    if (titleRect.Contains(mp))
                    {
                        hasSelect = true;
                        obj = objectRectDic[titleRect];
                        this.selectedObject = obj;
                        break;
                    }
                }
            }
            if (firstVisibleRecord != 0 && LastVisibleRecord != 0 && processBarValue == 0)
            {
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    if (chartPanel.TrendLineSeriesList.Count > 0)
                    {
                        foreach (TrendLineSeries tls in chartPanel.TrendLineSeriesList)
                        {
                            if (hasSelect)
                            {
                                if (setSelect)
                                {
                                    if (obj != null && obj == tls)
                                    {
                                        tls.HasSelect = true;
                                    }
                                    else
                                    {
                                        tls.HasSelect = false;
                                    }
                                }
                            }
                            else
                            {
                                if (curIndex > LastVisibleRecord - 1 || this.dtAllMsg.Rows[curIndex][tls.Field].ToString() == "")
                                {
                                    if (setSelect)
                                    {
                                        tls.HasSelect = false;
                                    }
                                    continue;
                                }
                                double lineValue = Convert.ToDouble(this.dtAllMsg.Rows[curIndex][tls.Field].ToString());
                                float scaleX = this.leftPixSpace + (curIndex + 2 - firstVisibleRecord) * axisSpace - axisSpace / 2;
                                int topY;
                                try { topY = Convert.ToInt32(GetValueYPixel(chartPanel, lineValue)); }
                                catch(Exception e) { return null; }
                                Point crossHairP = GetCrossHairPoint();
                                int judgeTop = 0;
                                float judgeScaleX = scaleX;
                                if (crossHairP.X >= scaleX)
                                {
                                    if (curIndex < this.LastVisibleRecord - 1 && this.dtAllMsg.Rows[curIndex + 1][tls.Field].ToString() != "")
                                    {
                                        double rightValue = Convert.ToDouble(this.dtAllMsg.Rows[curIndex + 1][tls.Field].ToString());
                                        judgeTop = Convert.ToInt32(GetValueYPixel(chartPanel, rightValue));
                                        if (judgeTop > chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height || judgeTop < chartPanel.RectPanel.Y + chartPanel.TitleHeight)
                                        {
                                            if (setSelect)
                                            {
                                                tls.HasSelect = false;
                                            }
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        judgeTop = topY;
                                    }
                                }
                                else
                                {
                                    judgeScaleX = scaleX - axisSpace;
                                    if (curIndex > 0 && this.dtAllMsg.Rows[curIndex - 1][tls.Field].ToString() != "")
                                    {
                                        double leftValue = Convert.ToDouble(this.dtAllMsg.Rows[curIndex - 1][tls.Field].ToString());
                                        judgeTop = Convert.ToInt32(GetValueYPixel(chartPanel, leftValue));
                                        if (judgeTop > chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height || judgeTop < chartPanel.RectPanel.Y + chartPanel.TitleHeight)
                                        {
                                            if (setSelect)
                                            {
                                                tls.HasSelect = false;
                                            }
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        judgeTop = topY;
                                    }
                                }
                                Rectangle judgeRect = new Rectangle();
                                if (judgeTop >= topY)
                                {
                                    judgeRect = new Rectangle((int)judgeScaleX, topY, axisSpace, judgeTop - topY < 1 ? 1 : judgeTop - topY);
                                }
                                else
                                {
                                    judgeRect = new Rectangle((int)judgeScaleX, judgeTop, axisSpace, topY - judgeTop < 1 ? 1 : topY - judgeTop);
                                }
                                if (judgeRect.Contains(crossHairP))
                                {
                                    if (setSelect)
                                    {
                                        selectedObject = tls;
                                        tls.HasSelect = true;
                                    }
                                    obj = tls;
                                    hasSelect = true;
                                }
                                else
                                {
                                    if (setSelect)
                                    {
                                        tls.HasSelect = false;
                                    }
                                }
                            }
                        }
                    }
                    if (chartPanel.HistoramSeriesList.Count > 0)
                    {
                        foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                        {
                            if (hasSelect)
                            {
                                if (setSelect)
                                {
                                    if (obj != null && obj == hs)
                                    {
                                        hs.HasSelect = true;
                                    }
                                    else
                                    {
                                        hs.HasSelect = false;
                                    }
                                }
                            }
                            else
                            {
                                if (curIndex > LastVisibleRecord - 1 || this.dtAllMsg.Rows[curIndex][hs.Field].ToString() == "")
                                {
                                    hs.HasSelect = false;
                                    continue;
                                }
                                double volumn = Convert.ToDouble(this.dtAllMsg.Rows[curIndex][hs.Field].ToString());
                                int topY = Convert.ToInt32(GetValueYPixel(chartPanel, volumn));
                                int bottomY = Convert.ToInt32(GetValueYPixel(chartPanel, 0));
                                if (volumn < 0)
                                {
                                    topY = Convert.ToInt32(GetValueYPixel(chartPanel, 0));
                                    bottomY = Convert.ToInt32(GetValueYPixel(chartPanel, volumn));
                                }
                                if (topY >= chartPanel.RectPanel.Y && bottomY <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height
                                    && mpY >= topY && mpY <= bottomY)
                                {
                                    if (setSelect)
                                    {
                                        selectedObject = hs;
                                        hs.HasSelect = true;
                                    }
                                    obj = hs;
                                    hasSelect = true;
                                }
                                else
                                {
                                    if (setSelect)
                                    {
                                        hs.HasSelect = false;
                                    }
                                }
                            }
                        }
                    }
                    if (chartPanel.CandleSeriesList.Count > 0)
                    {
                        foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                        {
                            if (hasSelect)
                            {
                                if (setSelect)
                                {
                                    if (obj != null && obj == cs)
                                    {
                                        cs.HasSelect = true;
                                    }
                                    else
                                    {
                                        cs.HasSelect = false;
                                    }
                                }
                            }
                            else
                            {
                                if (curIndex > LastVisibleRecord - 1
                                    || this.dtAllMsg.Rows[curIndex][cs.HighField].ToString() == ""
                                    || this.dtAllMsg.Rows[curIndex][cs.LowField].ToString() == "")
                                {
                                    if (setSelect)
                                    {
                                        cs.HasSelect = false;
                                    }
                                    continue;
                                }
                                double highValue = Convert.ToDouble(this.dtAllMsg.Rows[curIndex][cs.HighField].ToString());
                                double lowValue = Convert.ToDouble(this.dtAllMsg.Rows[curIndex][cs.LowField].ToString());
                                int topY = Convert.ToInt32(GetValueYPixel(chartPanel, highValue));
                                int bottomY = Convert.ToInt32(GetValueYPixel(chartPanel, lowValue));
                                if (topY >= chartPanel.RectPanel.Y && bottomY <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height
                                    && mpY >= topY && mpY <= bottomY)
                                {
                                    if (setSelect)
                                    {
                                        cs.HasSelect = true;
                                        selectedObject = cs;
                                    }
                                    obj = cs;
                                    hasSelect = true;
                                }
                                else
                                {
                                    if (setSelect)
                                    {
                                        cs.HasSelect = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (obj == null)
            {
                selectedObject = null;
            }
            return obj;
        }

        /// <summary>
        /// 重新调整图像的大小
        /// </summary>
        public void ResizeGraph()
        {
            int locationY = 0;
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                chartPanel.RectPanel = new Rectangle(0, locationY, this.Width, Convert.ToInt32((double)chartPanel.VerticalPercent / 100 * this.Height));
                locationY += Convert.ToInt32((double)chartPanel.VerticalPercent / 100 * this.Height);
            }
        }

        /// <summary>
        /// 用户自己拖动图像改变大小
        /// </summary>
        public void DragChartPanel()
        {
            if (userResizePanel != null)
            {
                Point mp = GetCrossHairPoint();
                ChartPanel nextCP = null;
                bool rightP = false;
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    if (rightP)
                    {
                        nextCP = chartPanel;
                        break;
                    }
                    if (chartPanel == userResizePanel)
                    {
                        rightP = true;
                    }
                }
                int originalVP = userResizePanel.VerticalPercent;
                if (userResizePanel.RectPanel.Contains(mp))
                {
                    userResizePanel.VerticalPercent = Convert.ToInt32(((double)mp.Y - (double)userResizePanel.RectPanel.Top) / (double)this.Height * 100);
                    if (userResizePanel.VerticalPercent < 1)
                    {
                        userResizePanel.VerticalPercent = 1;
                    }
                    if (nextCP != null)
                    {
                        nextCP.VerticalPercent += originalVP - userResizePanel.VerticalPercent;
                    }
                }
                else
                {
                    if (nextCP != null && nextCP.RectPanel.Contains(mp))
                    {
                        userResizePanel.VerticalPercent = Convert.ToInt32(((double)mp.Y - (double)userResizePanel.RectPanel.Top) / (double)this.Height * 100);
                        if (userResizePanel.VerticalPercent >= originalVP + nextCP.VerticalPercent)
                        {
                            userResizePanel.VerticalPercent -= 1;
                        }
                        nextCP.VerticalPercent = originalVP + nextCP.VerticalPercent - userResizePanel.VerticalPercent;
                    }
                }
                userResizePanel = null;
                ResizeGraph();
                DrawGraph();
            }
        }

        /// <summary>
        /// 定位十字线
        /// </summary>
        public void LocateCrossHair()
        {
            if (this.dtAllMsg.Rows.Count > 0)
            {
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    if (crossHair_y >= chartPanel.RectPanel.Y && crossHair_y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height)
                    {
                        if (GetWorkSpaceY(chartPanel.PanelID) > 0)
                        {
                            if (this.crossOverIndex >= 0 && this.crossOverIndex < this.dtAllMsg.Rows.Count)
                            {
                                if (chartPanel.CandleSeriesList.Count > 0)
                                {
                                    double closeValue = Convert.ToDouble(this.dtAllMsg.Rows[this.crossOverIndex][chartPanel.CandleSeriesList[0].CloseField]);
                                    crossHair_y = Convert.ToInt32(GetValueYPixel(chartPanel, closeValue));
                                    return;
                                }
                                if (chartPanel.HistoramSeriesList.Count > 0)
                                {
                                    double volumn = Convert.ToDouble(this.dtAllMsg.Rows[this.crossOverIndex][chartPanel.HistoramSeriesList[0].Field]);
                                    crossHair_y = Convert.ToInt32(GetValueYPixel(chartPanel, volumn));
                                    return;
                                }
                                if (chartPanel.TrendLineSeriesList.Count > 0)
                                {
                                    double lineValue = Convert.ToDouble(this.dtAllMsg.Rows[this.crossOverIndex][chartPanel.TrendLineSeriesList[0].Field]);
                                    crossHair_y = Convert.ToInt32(GetValueYPixel(chartPanel, lineValue));
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 十字线左滚
        /// </summary>
        public void CrossHairScrollLeft()
        {
            int currentRecord = this.crossOverIndex;
            this.crossOverIndex = currentRecord - 1;
            if (crossOverIndex < 0)
            {
                crossOverIndex = 0;
            }
            if (currentRecord < this.firstVisibleRecord)
            {
                ScrollLeft(1);
            }
        }

        /// <summary>
        /// 十字线右滚
        /// </summary>
        public void CrossHairScrollRight()
        {
            int currentRecord = this.crossOverIndex;
            this.crossOverIndex = currentRecord + 1;
            int maxRecord = GetMaxVisibleRecord();
            if (this.dtAllMsg.Rows.Count < maxRecord)
            {
                if (crossOverIndex >= maxRecord-1)
                {
                    crossOverIndex = maxRecord - 1;
                }
            }
            if (currentRecord >= this.LastVisibleRecord - 1)
            {
                ScrollRight(1);
            }
        }

        /// <summary>
        /// 设置指定面板的买卖线字段
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="buyField"></param>
        /// <param name="sellField"></param>
        public void SetCandleBuySellField(string candleName, string buyField, string sellField)
        {
            foreach (ChartPanel cp in this.dicChartPanel.Values)
            {
                foreach (CandleSeries cs in cp.CandleSeriesList)
                {
                    if (cs.CandleName == candleName)
                    {
                        cs.IndBuySellField[0] = buyField;
                        cs.IndBuySellField[1] = sellField;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 设置信息地雷
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="info"></param>
        /// <param name="dt"></param>
        public void SetInfoBombText(int panelID,string info,DateTime dt)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                string field = chartPanel.InfoBombField;
                if (!this.dtAllMsg.Columns.Contains(field))
                {
                    this.dtAllMsg.Columns.Add(field);
                }
                
                SetValue(dtAllMsg.Rows.Find(dt.ToShortDateString()), field, info);
            }
        }

        /// <summary>
        /// 设置面板的时间类型
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="intervalType"></param>
        public void SetIntervalType(int panelID, IntervalType intervalType)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].Interval = intervalType;
            }
        }

        /// <summary>
        /// 设置信息地雷的样式
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="bgColor"></param>
        /// <param name="selectedColor"></param>
        public void SetInfoBombStyle(int panelID, Color bgColor, Color selectedColor,Color tipBgColor,Color tipTextColor)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                chartPanel.InfoBombColor = bgColor;
                chartPanel.InfoBombSelectedColor = selectedColor;
                chartPanel.InfoBombTipColor = tipBgColor;
                chartPanel.InfoBombTipTextColor = tipTextColor;
            }
        }

        public void GetMaxAndMin(ChartPanel panel)
            {
                foreach(TrendLineSeries series in panel.TrendLineSeriesList){
                    for (int i = 0; i < dtAllMsg.Rows.Count; ++i)
                    {
                        if (Double.Parse(dtAllMsg.Rows[i][series.Field].ToString()) > panel.MaxValue)
                        {
                            panel.MaxValue = Double.Parse(dtAllMsg.Rows[i][series.Field].ToString());
                        }
                        if (Double.Parse(dtAllMsg.Rows[i][series.Field].ToString()) < panel.MinValue)
                        {
                            panel.MinValue = Double.Parse(dtAllMsg.Rows[i][series.Field].ToString());
                        }
                    }
                }
            }

        /// <summary>
        /// 设置买卖文字的样式
        /// </summary>
        /// <param name="candleName"></param>
        /// <param name="buyText"></param>
        /// <param name="sellText"></param>
        /// <param name="buyColor"></param>
        /// <param name="sellColor"></param>
        /// <param name="bsFont"></param>
        public void SetCandleBuySellStyle(string candleName, string buyText, string sellText, Color buyColor, Color sellColor, Font bsFont)
        {
            foreach (ChartPanel cp in this.dicChartPanel.Values)
            {
                foreach (CandleSeries cs in cp.CandleSeriesList)
                {
                    if (cs.CandleName == candleName)
                    {
                        cs.BuyText = buyText;
                        cs.SellText = sellText;
                        cs.BuyColor = buyColor;
                        cs.SellColor = sellColor;
                        cs.BsFont = bsFont;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 设置指定面板的最小变动值
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="tick"></param>
        public void SetTick(int panelID, double tick)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].YScaleTick = tick;
            }
        }

        /// <summary>
        /// 设置面板的数值保留小数位数
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="digit"></param>
        public void SetDigit(int panelID, int digit)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].Digit = digit;
            }
        }

        /// <summary>
        /// 设置滚动的幅度
        /// </summary>
        /// <param name="leftStep"></param>
        /// <param name="rightStep"></param>
        public void SetSrollStep(int leftStep, int rightStep)
        {
            this.scrollLeftStep = leftStep;
            this.scrollRightStep = rightStep;
        }

        /// <summary>
        /// 设置指定面板的标题
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="title"></param>
        public void SetTitle(int panelID, string title)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].PanelTitle = title;
            }
        }

        /// <summary>
        /// 设置面板的网格线的间隔
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="gridInterval"></param>
        public void SetGridInterval(int panelID, int gridInterval)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].GridInterval = gridInterval;
            }
        }

        /// <summary>
        /// 设置十字线的样式
        /// </summary>
        /// <param name="lineColor"></param>
        /// <param name="weight"></param>
        public void SetCrossHairStyle(int panelID,Color crossHairColor, int weight)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                chartPanel.CrossHair_Pen.Dispose();
                chartPanel.CrossHair_Pen = new Pen(crossHairColor, weight);
            }
        }

        /// <summary>
        /// 设置X轴提示框的样式
        /// </summary>
        /// <param name="backColor"></param>
        /// <param name="fontColor"></param>
        public void SetXTipStyle(int panelID, Color backColor, Color fontColor, Font xTipFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].Xtip_Brush.Dispose();
                this.dicChartPanel[panelID].Xtip_Brush = new SolidBrush(backColor);
                this.dicChartPanel[panelID].XTipFont_Brush.Dispose();
                this.dicChartPanel[panelID].XTipFont_Brush = new SolidBrush(fontColor);
                this.dicChartPanel[panelID].XTipFont = xTipFont;
                this.dicChartPanel[panelID].XTipFont_Pen.Color = fontColor;
            }
        }

        /// <summary>
        /// 设置左侧Y轴提示框的样式
        /// </summary>
        /// <param name="backColor"></param>
        /// <param name="fontColor"></param>
        public void SetLeftYTipStyle(int panelID, Color backColor, Color fontColor, Font xTipFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].LeftyTip_Brush.Dispose();
                this.dicChartPanel[panelID].LeftyTip_Brush = new SolidBrush(backColor);
                this.dicChartPanel[panelID].LeftyTipFont_Brush.Dispose();
                this.dicChartPanel[panelID].LeftyTipFont_Brush = new SolidBrush(fontColor);
                this.dicChartPanel[panelID].LeftyTipFont = xTipFont;
                this.dicChartPanel[panelID].LeftTipFont_Pen.Color = fontColor;
            }
        }

        /// <summary>
        /// 设置右侧Y轴提示框的样式
        /// </summary>
        /// <param name="backColor"></param>
        /// <param name="fontColor"></param>
        public void SetRightYTipStyle(int panelID, Color backColor, Color fontColor, Font xTipFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].RightyTip_Brush.Dispose();
                this.dicChartPanel[panelID].RightyTip_Brush = new SolidBrush(backColor);
                this.dicChartPanel[panelID].RightyTipFont_Brush.Dispose();
                this.dicChartPanel[panelID].RightyTipFont_Brush = new SolidBrush(fontColor);
                this.dicChartPanel[panelID].RightyTipFont = xTipFont;
                this.dicChartPanel[panelID].RightyTipFont_Pen.Color = fontColor;
            }
        }

        /// <summary>
        /// 设置面板的背景色
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="bgColor"></param>
        public void SetBackColor(int panelID, Color bgColor)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].BgBrush.Dispose();
                this.dicChartPanel[panelID].BgBrush = new SolidBrush(bgColor);
            }
        }

        /// <summary>
        /// 设置面板的边线
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="borderColor"></param>
        /// <param name="dashStyle"></param>
        /// <param name="width"></param>
        public void SetBorderStyle(int panelID, Color borderColor, DashStyle dashStyle, int width)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].PanelBorder_Pen.Dispose();
                this.dicChartPanel[panelID].PanelBorder_Pen = new Pen(borderColor, width);
                if (dashStyle != DashStyle.Custom)
                {
                    this.dicChartPanel[panelID].PanelBorder_Pen.DashStyle = dashStyle;
                }
            }
        }

        /// <summary>
        /// 设置标题的样式
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="titleColor"></param>
        /// <param name="titleFont"></param>
        public void SetTitleStyle(int panelID, Color titleColor, Font titleFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].TitleFont_Brush.Dispose();
                this.dicChartPanel[panelID].TitleFont_Brush = new SolidBrush(titleColor);
                this.dicChartPanel[panelID].TitleFont = titleFont;
                this.dicChartPanel[panelID].TitleHeight = this.CreateGraphics().MeasureString(" ", titleFont).Height + 5f;
            }
        }

        /// <summary>
        /// 设置网格线的样式
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="gridColor"></param>
        /// <param name="dashStyle"></param>
        /// <param name="width"></param>
        public void SetGridStyle(int panelID, Color gridColor, DashStyle dashStyle, bool showGrid, int width)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].Grid_Pen.Color = gridColor;
                this.dicChartPanel[panelID].Grid_Pen.DashStyle = dashStyle;
                this.dicChartPanel[panelID].ShowGrid = showGrid;
            }
        }

        /// <summary>
        /// 设置X轴的样式
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="fontColor"></param>
        /// <param name="scaleFont"></param>
        public void SetXScaleStyle(int panelID,Color scaleColor, Color fontColor, Font scaleFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].XScalePen.Dispose();
                this.dicChartPanel[panelID].XScalePen = new Pen(scaleColor);
                this.dicChartPanel[panelID].CoordinateXFont_Brush.Dispose();
                this.dicChartPanel[panelID].CoordinateXFont_Brush = new SolidBrush(fontColor);
                this.dicChartPanel[panelID].CoordinateXFont = scaleFont;
                this.dicChartPanel[panelID].ScaleX_Height = (int)(this.CreateGraphics().MeasureString(" ", scaleFont).Height * 1.2) + 2;
            }
        }

        /// <summary>
        /// 设置左侧Y轴的样式
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="fontColor"></param>
        /// <param name="scaleFont"></param>
        public void SetLeftYScaleStyle(int panelID,Color scaleColor, Color fontColor, Font scaleFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].LeftScalePen.Dispose();
                this.dicChartPanel[panelID].LeftScalePen = new Pen(scaleColor);
                this.dicChartPanel[panelID].LeftYFont_Brush.Dispose();
                this.dicChartPanel[panelID].LeftYFont_Brush = new SolidBrush(fontColor);
                this.dicChartPanel[panelID].LeftYFont = scaleFont;
            }
        }

        /// <summary>
        /// 设置右侧Y轴的样式
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="fontColor"></param>
        /// <param name="scaleFont"></param>
        public void SetRightYScaleStyle(int panelID,Color scaleColor, Color fontColor, Font scaleFont)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                this.dicChartPanel[panelID].RightScalePen.Dispose();
                this.dicChartPanel[panelID].RightScalePen = new Pen(scaleColor);
                this.dicChartPanel[panelID].RightYFont_Brush.Dispose();
                this.dicChartPanel[panelID].RightYFont_Brush = new SolidBrush(fontColor);
                this.dicChartPanel[panelID].RightYFont = scaleFont;
            }
        }

        /// <summary>
        /// 添加新的面板
        /// </summary>
        /// <returns></returns>
        public int AddChartPanel(int verticalPercent)
        {
            int locationY = 0;
            foreach (ChartPanel cp in this.dicChartPanel.Values)
            {
                locationY += Convert.ToInt32((double)cp.VerticalPercent / 100 * this.Height);
            }
            int panelHeight = Convert.ToInt32((double)verticalPercent / 100 * this.Height);
            ChartPanel chartPanel = new ChartPanel();
            chartPanel.VerticalPercent = verticalPercent;
            chartPanel.PanelID = CommonClass.GetPanelID();
            chartPanel.RectPanel = new Rectangle(0, locationY, this.Width, panelHeight);
            this.dicChartPanel[chartPanel.PanelID] = chartPanel;
            if (bottomPanel != null)
            {
                bottomPanel.ScaleX_Height = 0;
            }
            bottomPanel = chartPanel;
            return chartPanel.PanelID;
        }

        /// <summary>
        /// 重置为空的图像
        /// </summary>
        public void ResetNullGraph()
        {
            ClearGraph();
            this.dicChartPanel.Clear();
            this.indExponentialMovingAverageList.Clear();
            this.indSimpleMovingAverageList.Clear();
            this.indMacdList.Clear();
            this.indBollList.Clear();
            this.indStochasticOscillatorList.Clear();
            this.dtAllMsg.Clear();
            this.dtAllMsg.Dispose();
            this.dtAllMsg = new DataTable();
            this.timekeyField = string.Empty;
            this.crossHair_y = -1;
            this.axisSpace = 8;
            this.leftPixSpace = 0;
            this.rightPixSpace = 0;
            this.showLeftScale = false;
            this.showRightScale = false;
            scrollLeftStep = 1;
            canDragSeries = false;
            vp_index = -1;
            drawValuePanelFlag = false;
            DrawGraph();
        }

        /// <summary>
        /// 设置K线的字段标题色
        /// </summary>
        /// <param name="openColor"></param>
        /// <param name="highColor"></param>
        /// <param name="lowColor"></param>
        /// <param name="closeColor"></param>
        public void SetCandleTitleColor(string candleName, Color openColor, Color highColor, Color lowColor, Color closeColor, int panelID)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                foreach (CandleSeries candleSeries in chartPanel.CandleSeriesList)
                {
                    if (candleSeries.CandleName == candleName)
                    {
                        candleSeries.OpenTitleColor = openColor;
                        candleSeries.HighTitleColor = highColor;
                        candleSeries.LowTitleColor = lowColor;
                        candleSeries.CloseTitleColor = closeColor;
                    }
                }
            }
        }

        /// <summary>
        /// 显示股票相关的值的窗体
        /// </summary>
        public void ShowValuePanel()
        {
            if (selectedObject != null)
            {
                int curRecord = GetCrossOverIndex();
                if (JudgeSelectedSeries(curRecord, GetCrossHairPoint().Y, false) == selectedObject)
                {
                    vp_index = curRecord;
                    DrawGraph();
                }
            }
        }

        /// <summary>
        /// 监测鼠标移动的循环
        /// </summary>
        public void checkMouseMoveLoop()
        {
            int tick_dely = 5000000;
            while (true)
            {
                if (lastMouseMoveTime.AddTicks(tick_dely) <= DateTime.Now)
                {
                    if (drawValuePanelFlag)
                    {
                        drawValuePanelFlag = !drawValuePanelFlag;
                        if (selectedObject != null && this.IsHandleCreated)
                        {
                            if (this.IsHandleCreated)
                            {
                                this.BeginInvoke(new ShowValuePanelDelegate(ShowValuePanel));
                            }
                        }
                    }
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 追加K线
        /// </summary>
        /// <returns></returns>
        public CandleSeries AddCandle(string candleName, string openfield, string highfield, string lowfield, string closefield, bool displayTitleField)
        {
            if (openfield == null || highfield == null || lowfield == null || closefield == null)
            {
                return null;
            }
            if (this.dtAllMsg.Columns.Contains(openfield) ||
                this.dtAllMsg.Columns.Contains(highfield) ||
                this.dtAllMsg.Columns.Contains(lowfield) ||
                this.dtAllMsg.Columns.Contains(closefield))
            {
                return null;
            }

            CandleSeries candleSeries = new CandleSeries();
            candleSeries = new CandleSeries();
            candleSeries.CandleName = candleName;
            candleSeries.OpenField = openfield;
            candleSeries.HighField = highfield;
            candleSeries.LowField = lowfield;
            candleSeries.CloseField = closefield;
                
            candleSeries.Down_Color = Color.SkyBlue;
            candleSeries.Up_Color = Color.Red;
            candleSeries.DisplayTitleField = displayTitleField;
            DataColumn dcOpen = new DataColumn(openfield);
            DataColumn dcHigh = new DataColumn(highfield);
            DataColumn dcLow = new DataColumn(lowfield);
            DataColumn dcClose = new DataColumn(closefield);
            this.dtAllMsg.Columns.Add(dcOpen);
            this.dtAllMsg.Columns.Add(dcHigh);
            this.dtAllMsg.Columns.Add(dcLow);
            this.dtAllMsg.Columns.Add(dcClose);

            return candleSeries;
        }

        public void SetCandleToPanel(string openfield, string highfield, string lowfield, string closefield, CandleSeries candleSeries, int panelID){
            if (openfield == null || highfield == null || lowfield == null || closefield == null)
            {
                return;
            }
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                chartPanel.YScaleField.AddRange(new string[] { openfield, highfield, lowfield, closefield });
                chartPanel.CandleSeriesList.Add(candleSeries);
            }
        }

        /// <summary>
        /// 设置K线柱的样式
        /// </summary>
        /// <param name="candleName"></param>
        /// <param name="upColor"></param>
        /// <param name="downColor"></param>
        /// <param name="middleColor"></param>
        public void SetCandleStyle(string candleName, Color upColor, Color downColor, Color middleColor)
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                {
                    if (cs.CandleName == candleName)
                    {
                        cs.Up_Color = upColor;
                        cs.Down_Color = downColor;
                        cs.Middle_Color = middleColor;
                    }
                }
            }
        }

        /// <summary>
        /// 设置柱状图的样式
        /// </summary>
        /// <param name="field"></param>
        /// <param name="upColor"></param>
        /// <param name="downColor"></param>
        /// <param name="width"></param>
        public void SetHistogramStyle(string field, Color upColor, Color downColor, int width, bool lineStyle)
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                {
                    if (hs.Field == field)
                    {
                        hs.LineStyle = lineStyle;
                        hs.LineWidth = width;
                        hs.Up_LineColor = upColor;
                        hs.Down_lineColor = downColor;
                        hs.LineStyle = lineStyle;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 设置趋势线的样式
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lineColor"></param>
        /// <param name="width"></param>
        /// <param name="dashStyle"></param>
        public void SetTrendLineStyle(string field, Color upLineColor, Color downLineColor, int width, DashStyle dashStyle)
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                foreach (TrendLineSeries tls in chartPanel.TrendLineSeriesList)
                {
                    if (tls.Field == field)
                    {
                        tls.Up_LineColor = upLineColor;
                        tls.Up_LinePen.Width = width;
                        tls.Down_LineColor = downLineColor;
                        tls.Down_linePen.Width = width;
                        if (dashStyle != DashStyle.Custom)
                        {
                            tls.Up_LinePen.DashStyle = dashStyle;
                            tls.Down_linePen.DashStyle = dashStyle;
                        }
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 追加成交量
        /// </summary>
        /// <param name="field"></param>
        /// <param name="hisColor"></param>
        /// <returns></returns>
        public HistogramSeries AddHistogram(string field, string displayName, string relateCandleName)
        {
            if (field == null || this.dtAllMsg.Columns.Contains(field))
            {
                return null;
            }
            else
            {
                HistogramSeries histogramSeries = new HistogramSeries();
                histogramSeries.Up_LineColor = Color.Lime;
                histogramSeries.RelateCandleName = relateCandleName;
                histogramSeries.Down_lineColor = Color.Lime;
                histogramSeries.Field = field;
                histogramSeries.DisplayName = displayName;
                DataColumn dc = new DataColumn(field);
                this.dtAllMsg.Columns.Add(dc);
                return histogramSeries;
            }
        }

        public void SetHistogramToPanel(string field, HistogramSeries histogramSeries, int panelID)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                chartPanel.YScaleField.Add(field);
                chartPanel.HistoramSeriesList.Add(histogramSeries);
            }
        }

        /// <summary>
        /// 追加趋势线
        /// </summary>
        /// <param name="field"></param>
        public TrendLineSeries AddTrendLine(string field, string displayName)
        {
            if (field == null || this.dtAllMsg.Columns.Contains(field))
            {
                return null;
            }
            TrendLineSeries lineSeries = new TrendLineSeries();
            lineSeries.Up_LineColor = Color.Yellow;
            lineSeries.Down_LineColor = Color.Yellow;
            lineSeries.Field = field;

            if (displayName != null && displayName.Length > 0)
            {
                lineSeries.DisplayName = displayName;
            }

            DataColumn dc = new DataColumn(field);
            this.dtAllMsg.Columns.Add(dc);
            return lineSeries;
        }

        public void Generate()
        {
            grammaParser.Generate();
        }

        public void SetTrendLineToPanel(string field, TrendLineSeries lineSeries, int panelID)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                chartPanel.YScaleField.Add(field);
                chartPanel.TrendLineSeriesList.Add(lineSeries);
            }
        }

        public IndicatorMovingAverage AddMovingAverage(string field, string displayName, string target, int cycle, int panelID)
        {
            if (field == null || this.dtAllMsg.Columns.Contains(field))
            {
                return null;
            }
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                IndicatorMovingAverage indMA = new IndicatorMovingAverage();
                indMA.Target = target;
                indMA.Cycle = cycle;
                indMA.DataSource = this.dtAllMsg;
                indMA.TrendLineSeries = AddTrendLine(field, displayName);
                SetTrendLineToPanel(field, indMA.TrendLineSeries, panelID);
                this.indMovingAverageList.Add(indMA);
                return indMA;
            }
            return null;
        }

        /// <summary>
        /// 追加SMA曲线
        /// </summary>
        /// <returns></returns>
        public IndicatorSimpleMovingAverage AddSimpleMovingAverage(string field, string displayName, string target, int cycle, int panelID)
        {
            if (field == null || this.dtAllMsg.Columns.Contains(field))
            {
                return null;
            }
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                IndicatorSimpleMovingAverage indSMA = new IndicatorSimpleMovingAverage();
                indSMA.Target = target;
                indSMA.Cycle = cycle;
                indSMA.DataSource = this.dtAllMsg;
                indSMA.TrendLineSeries = AddTrendLine(field, displayName);
                SetTrendLineToPanel(field, indSMA.TrendLineSeries, panelID);
                this.indSimpleMovingAverageList.Add(indSMA);
                return indSMA;
            }
            return null;
        }

        /// <summary>
        /// 追加KDJ指标
        /// </summary>
        public IndicatorStochasticOscillator AddStochasticOscillator(string k, string d, string j, int kPeriod,
            string close, string high, string low)
        {
            IndicatorStochasticOscillator indSO = new IndicatorStochasticOscillator();
            this.indStochasticOscillatorList.Add(indSO);
            indSO.DataSource = dtAllMsg;
            indSO.Close = close;
            indSO.High = high;
            indSO.Low = low;
            indSO.KPeriods = kPeriod;

            indSO.TlsK = AddTrendLine(k, k);
            indSO.TlsD = AddTrendLine(d, d);
            indSO.TlsJ = AddTrendLine(j, j);

            return indSO;
        }

        public void SetStochasticOscillatorToPanel(string k, string d, string j, IndicatorStochasticOscillator indSO, int panelID)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                //K
                SetTrendLineToPanel(k, indSO.TlsK, panelID);
                SetTrendLineStyle(k, Color.White, Color.White, 1, DashStyle.Solid);
                //D
                SetTrendLineToPanel(d, indSO.TlsD, panelID);
                SetTrendLineStyle(d, Color.Yellow, Color.Yellow, 1, DashStyle.Solid);
                //J
                SetTrendLineToPanel(j, indSO.TlsJ, panelID);
                SetTrendLineStyle(j, Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 255), 1, DashStyle.Solid);
            }
        }

        /// <summary>
        /// 添加EMA曲线
        /// </summary>
        /// <param name="field"></param>
        /// <param name="lineColor"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public IndicatorExponentialMovingAverage AddExponentialMovingAverage(string field, string displayName, int cycle, string target, int panelID)
        {
            if (field == null || this.dtAllMsg.Columns.Contains(field))
            {
                return null;
            }
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                IndicatorExponentialMovingAverage indEMA = new IndicatorExponentialMovingAverage();
                indEMA.Cycle = cycle;
                indEMA.Target = target;
                indEMA.DataSource = this.dtAllMsg;
                indEMA.TrendLineSeries = AddTrendLine(field, displayName);
                SetTrendLineToPanel(field, indEMA.TrendLineSeries, panelID);
                this.indExponentialMovingAverageList.Add(indEMA);
                return indEMA;
            }
            return null;
        }

        /// <summary>
        /// 追加MACD指标
        /// </summary>
        /// <returns></returns>
        public IndicatorMACD AddMacd(string macd,string diff,string dea, string close, int longCycle,int shortCycle,int signalPeriods)
        {
            IndicatorMACD indicatorMACD = new IndicatorMACD();
            indicatorMACD.LongCycle = longCycle;
            indicatorMACD.ShortCycle = shortCycle;
            indicatorMACD.SignalPeriods = signalPeriods;
            string zeroLine = CommonClass.GetGuid();

            indicatorMACD.HsMACD = AddHistogram(macd, macd, null);
            indicatorMACD.TlsDiff = AddTrendLine(diff, diff);
            indicatorMACD.TlsDea = AddTrendLine(dea, dea);

            indicatorMACD.Close = close;
            this.dtAllMsg.Columns.Add(indicatorMACD.LongCycleEMA);
            this.dtAllMsg.Columns.Add(indicatorMACD.ShortCycleEMA);
            indicatorMACD.DataSource = this.dtAllMsg;
            this.indMacdList.Add(indicatorMACD);
            return indicatorMACD;
        }

        public void SetMacdToPanel(string macd,string diff,string dea, IndicatorMACD indicatorMACD, int panelID)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                SetHistogramToPanel(macd, indicatorMACD.HsMACD, panelID);
                SetHistogramStyle(macd, Color.Red, Color.SkyBlue, 1, true);
                SetTrendLineToPanel(diff, indicatorMACD.TlsDiff, panelID);
                SetTrendLineStyle(diff, Color.White, Color.White, 1, DashStyle.Solid);
                SetTrendLineToPanel(dea, indicatorMACD.TlsDea, panelID);
                SetTrendLineStyle(dea, Color.Yellow, Color.Yellow, 1, DashStyle.Solid);
            }
        }

        /// <summary>
        /// 追加BOLL线
        /// </summary>
        /// <param name="mid"></param>
        /// <param name="up"></param>
        /// <param name="down"></param>
        /// <param name="close"></param>
        /// <param name="periods"></param>
        /// <param name="standardDeviations"></param>
        /// <param name="panelID"></param>
        /// <returns></returns>
        public IndicatorBollingerBands AddBollingerBands(string mid, string up, string down, string close,
            int periods, int standardDeviations, int panelID)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                IndicatorBollingerBands indBoll = new IndicatorBollingerBands();
                indBoll.TlsM = AddTrendLine(mid, mid);
                SetTrendLineToPanel(mid, indBoll.TlsM, panelID);
                SetTrendLineStyle(mid, Color.White, Color.White, 1, DashStyle.Solid);
                indBoll.TlsU = AddTrendLine(up, up);
                SetTrendLineToPanel(up, indBoll.TlsU, panelID);
                SetTrendLineStyle(up, Color.Yellow, Color.Yellow, 1, DashStyle.Solid);
                indBoll.TlsD = AddTrendLine(down, down);
                SetTrendLineToPanel(down, indBoll.TlsD, panelID);
                SetTrendLineStyle(down, Color.FromArgb(255, 0, 255), Color.FromArgb(255, 0, 255), 1, DashStyle.Solid);
                indBoll.Close = close;
                indBoll.DataSource = this.dtAllMsg;
                indBoll.Periods = periods;
                indBoll.StandardDeviations = standardDeviations;
                this.indBollList.Add(indBoll);
                return indBoll;
            }
            return null;
        }

        /// <summary>
        /// 重置十字线穿越的字段
        /// </summary>
        public void ResetCrossOverRecord()
        {
            if (this.dtAllMsg.Rows.Count >= this.GetMaxVisibleRecord())
            {
                if (this.dtAllMsg.Rows.Count > 0 && showCrossHair)
                {
                    if (this.crossOverIndex < firstVisibleRecord - 1)
                    {
                        this.crossOverIndex = firstVisibleRecord - 1;
                    }
                    if (this.crossOverIndex > LastVisibleRecord - 1)
                    {
                        this.crossOverIndex = LastVisibleRecord - 1;
                    }
                }
            }
        }

        /// <summary>
        /// 向左滚动
        /// </summary>
        /// <param name="step"></param>
        private void ScrollLeft(int step)
        {
            if (this.dtAllMsg.Rows.Count > 1 && firstVisibleRecord > 1)
            {
                if (this.dtAllMsg.Rows.Count > GetMaxVisibleRecord())
                {
                    if (firstVisibleRecord - step >= 1)
                    {
                        firstVisibleRecord = firstVisibleRecord - step;
                        LastVisibleRecord = LastVisibleRecord - step;
                    }
                    else
                    {
                        LastVisibleRecord = LastVisibleRecord - firstVisibleRecord;
                        firstVisibleRecord = 1;
                    }
                }
            }
        }

        /// <summary>
        /// 向右滚动
        /// </summary>
        /// <param name="step"></param>
        private void ScrollRight(int step)
        {
            if (this.dtAllMsg.Rows.Count > 1 && LastVisibleRecord < this.dtAllMsg.Rows.Count)
            {
                if (this.dtAllMsg.Rows.Count > GetMaxVisibleRecord())
                {
                    if (LastVisibleRecord + step > this.dtAllMsg.Rows.Count)
                    {
                        firstVisibleRecord = firstVisibleRecord + (this.dtAllMsg.Rows.Count - LastVisibleRecord);
                        LastVisibleRecord = this.dtAllMsg.Rows.Count;
                    }
                    else
                    {
                        firstVisibleRecord = firstVisibleRecord + step;
                        LastVisibleRecord = LastVisibleRecord + step;
                    }
                }
            }
        }

        /// <summary>
        /// 放大
        /// </summary>
        /// <param name="step"></param>
        private void ZoomIn(int step)
        {
            if (this.axisSpace < 50)
            {
                int oriMax = GetMaxVisibleRecord();
                bool dealWith = false;
                if (this.dtAllMsg.Rows.Count < oriMax)
                {
                    dealWith = true;
                }
                this.axisSpace = this.axisSpace + 1;
                int nowMax = GetMaxVisibleRecord();
                int subRecord = oriMax - nowMax;
                if (this.dtAllMsg.Rows.Count >= nowMax)
                {
                    if (dealWith)
                    {
                        firstVisibleRecord = 1;
                        LastVisibleRecord = nowMax;
                    }
                    else
                    {
                        this.firstVisibleRecord = this.firstVisibleRecord + subRecord;
                        this.LastVisibleRecord = this.lastVisibleRecord;
                    }
                }
            }
        }

        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="step"></param>
        private void ZoomOut(int step)
        {
            if (this.axisSpace > 1)
            {
                int oriMax = GetMaxVisibleRecord();
                this.axisSpace = this.axisSpace - 1;
                int nowMax = GetMaxVisibleRecord();
                int subRecord = nowMax - oriMax;
                int f = this.firstVisibleRecord - subRecord;
                int l = this.LastVisibleRecord;
                if (f < 1 && l > this.dtAllMsg.Rows.Count)
                {
                    firstVisibleRecord = 1;
                    LastVisibleRecord = this.dtAllMsg.Rows.Count;
                }
                else if (f < 1)
                {
                    LastVisibleRecord = LastVisibleRecord + subRecord - (firstVisibleRecord - 1);
                    firstVisibleRecord = 1;
                    if (LastVisibleRecord > this.dtAllMsg.Rows.Count)
                    {
                        LastVisibleRecord = this.dtAllMsg.Rows.Count;
                    }
                }
                else if (l > this.dtAllMsg.Rows.Count)
                {
                    firstVisibleRecord = firstVisibleRecord - (subRecord - (this.dtAllMsg.Rows.Count - this.LastVisibleRecord));
                    LastVisibleRecord = this.dtAllMsg.Rows.Count;
                    if (firstVisibleRecord < 1)
                    {
                        firstVisibleRecord = 1;
                    }
                }
                else
                {
                    this.firstVisibleRecord = f;
                    this.LastVisibleRecord = l;
                }
            }
        }

        /// <summary>
        /// 设置X轴所使用的字段
        /// </summary>
        /// <returns></returns>
        public void SetXScaleField(string field)
        {
            if (this.timekeyField == null || this.timekeyField.Length == 0)
            {
                this.timekeyField = field;
                DataColumn dcTimekey = new DataColumn(timekeyField);
                this.dtAllMsg.Columns.Add(dcTimekey);
                if (this.dtAllMsg.PrimaryKey != new DataColumn[] { dcTimekey })
                    this.dtAllMsg.PrimaryKey = new DataColumn[] { dcTimekey };
            }
        }

        /// <summary>
        /// 设置Y轴所使用的字段
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="field"></param>
        public void SetYScaleField(int panelID, string[] field)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                ChartPanel chartPanel = this.dicChartPanel[panelID];
                chartPanel.YScaleField.Clear();
                foreach (string ch in field)
                {
                    chartPanel.YScaleField.Add(ch);
                }
            }
        }


        /// <summary>
        /// 刷新图像
        /// </summary>
        public void RefreshGraph()
        {
            InitFirstAndLastVisibleRecord();
            setVisibleExtremeValue();
            ResetCrossOverRecord();
            DrawGraph();
        }

        /// <summary>
        /// 清除图像
        /// </summary>
        public void ClearGraph()
        {
            this.dtAllMsg.Clear();
            this.firstVisibleRecord = 0;
            this.LastVisibleRecord = 0;
            showCrossHair = false;
            RefreshGraph();
        }

        /// <summary>
        /// 自动设置首先可见和最后可见的记录号
        /// </summary>
        public void InitFirstAndLastVisibleRecord()
        {
            if (this.dtAllMsg.Rows.Count == 0)
            {
                this.firstVisibleRecord = 0;
                this.LastVisibleRecord = 0;
            }
            else
            {
                int maxVisibleRecord = GetMaxVisibleRecord();
                if (this.dtAllMsg.Rows.Count < maxVisibleRecord)
                {
                    firstVisibleRecord = 1;
                    this.LastVisibleRecord = this.dtAllMsg.Rows.Count;
                }
                else
                {
                    if (firstVisibleRecord != 0 && LastVisibleRecord != 0 && !lastRecordIsVisible)
                    {
                        DataRow dr = this.dtAllMsg.Rows.Find(lastVisibleTimeKey);
                        if (dr != null)
                        {
                            int index = this.dtAllMsg.Rows.IndexOf(dr);
                            LastVisibleRecord = index + 1;
                            firstVisibleRecord = LastVisibleRecord - maxVisibleRecord + 1;
                            if (firstVisibleRecord < 1)
                            {
                                firstVisibleRecord = 1;
                            }
                        }
                    }
                    else
                    {
                        LastVisibleRecord = this.dtAllMsg.Rows.Count;
                        firstVisibleRecord = LastVisibleRecord - maxVisibleRecord + 1;
                        if (firstVisibleRecord < 1)
                        {
                            firstVisibleRecord = 1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置可见部分的最大值和最小值
        /// </summary>
        private void setVisibleExtremeValue()
        {
            if (GetWorkSpaceX() > 0)
            {
                int firstR = firstVisibleRecord - 1;
                int lastR = LastVisibleRecord;
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    Dictionary<CandleSeries, List<object[]>> kValueList = new Dictionary<CandleSeries, List<object[]>>();
                    List<double> valueList = new List<double>();
                    if (this.dtAllMsg.Rows.Count > 0)
                    {
                        for (int i = firstR; i < lastR; i++)
                        {
                            string timeKey = this.dtAllMsg.Rows[i][timekeyField].ToString();
                            //获取数据
                            DataRow dr = dtAllMsg.Rows[i];
                            foreach (string field in chartPanel.YScaleField)
                            {
                                double fieldValue = 0;
                                Double.TryParse(dr[field].ToString(), out fieldValue);

                                valueList.Add(fieldValue);
                            }
                            //K线柱的最大和最小值对应的记录号
                            foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                            {
                                if (!kValueList.ContainsKey(cs))
                                {
                                    kValueList.Add(cs, new List<object[]>());
                                }
                                double open = 0;
                                Double.TryParse(dr[cs.OpenField].ToString(), out open);
                                double high = 0;
                                Double.TryParse(dr[cs.HighField].ToString(), out high);
                                double low = 0;
                                Double.TryParse(dr[cs.LowField].ToString(), out low);
                                double close = 0;
                                Double.TryParse(dr[cs.CloseField].ToString(), out close);
                                kValueList[cs].Add(new object[] { i, open });
                                kValueList[cs].Add(new object[] { i, high });
                                kValueList[cs].Add(new object[] { i, low });
                                kValueList[cs].Add(new object[] { i, close });
                                if (chartPanel.YScaleField.Count == 0)
                                {
                                    valueList.Add(open);
                                    valueList.Add(high);
                                    valueList.Add(low);
                                    valueList.Add(close);
                                }
                                else
                                {
                                    if (chartPanel.YScaleField.Contains(cs.OpenField))
                                    {
                                        valueList.Add(open);
                                    }
                                    if (chartPanel.YScaleField.Contains(cs.HighField))
                                    {
                                        valueList.Add(high);
                                    }
                                    if (chartPanel.YScaleField.Contains(cs.LowField))
                                    {
                                        valueList.Add(low);
                                    }
                                    if (chartPanel.YScaleField.Contains(cs.CloseField))
                                    {
                                        valueList.Add(close);
                                    }
                                }
                            }
                            foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                            {
                                double volume = 0;
                                Double.TryParse(dr[hs.Field].ToString(), out volume);
                                if (chartPanel.YScaleField.Count == 0)
                                {
                                    valueList.Add(0);
                                    valueList.Add(volume);
                                }
                                else
                                {
                                    if (chartPanel.YScaleField.Contains(hs.Field))
                                    {
                                        valueList.Add(0);
                                        valueList.Add(volume);
                                    }
                                }
                            }
                            foreach (TrendLineSeries tls in chartPanel.TrendLineSeriesList)
                            {
                                double lineValue = 0;
                                Double.TryParse(dr[tls.Field].ToString(), out lineValue);
                                if (chartPanel.YScaleField.Count == 0)
                                {
                                    valueList.Add(lineValue);
                                }
                                else
                                {

                                    if (chartPanel.YScaleField.Contains(tls.Field))
                                    {
                                        valueList.Add(lineValue);
                                    }
                                }
                            }
                        }
                    }
                    chartPanel.MaxValue = CommonClass.GetHighValue(valueList);
                    chartPanel.MinValue = CommonClass.GetLowValue(valueList);
                    foreach (CandleSeries cs in kValueList.Keys)
                    {
                        cs.MaxRecord = CommonClass.GetHighRecord(kValueList[cs]);
                        cs.MinRecord = CommonClass.GetLoweRecord(kValueList[cs]);
                    }
                }
            }
        }

        /// <summary>
        /// 对指定指标进行计算
        /// </summary>
        /// <param name="indicator"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        public void CalcutaIndicator(object indicator, DataTable table, int startIndex,int endIndex)
        {
            for (int i = startIndex; i <= endIndex; i++)
            {
                DataRow dr = table.Rows[i];
                //简单移动平均线
                if (indicator is IndicatorMovingAverage)
                {
                    IndicatorMovingAverage indMA = indicator as IndicatorMovingAverage;
                    double maValue = indMA.Calculate(i);
                    if (maValue != NULL)
                    {
                        SetValue(dr, indMA.TrendLineSeries.Field, maValue);
                    }
                }
                //平滑移动平均线
                else if (indicator is IndicatorSimpleMovingAverage)
                {
                    IndicatorSimpleMovingAverage indSMA = indicator as IndicatorSimpleMovingAverage;
                    double smaValue = indSMA.Calculate(i); 
                    if (smaValue != NULL)
                    {
                        SetValue(dr, indSMA.TrendLineSeries.Field, smaValue);
                    }
                }
                //指数移动平均线
                else if (indicator is IndicatorExponentialMovingAverage)
                {
                    IndicatorExponentialMovingAverage indEMA = indicator as IndicatorExponentialMovingAverage;
                    double emaValue = indEMA.Calculate(i);
                    if (emaValue != NULL)
                    {
                        SetValue(dr, indEMA.TrendLineSeries.Field, emaValue);
                    }
                }
                //随机指标
                else if (indicator is IndicatorStochasticOscillator)
                {
                    IndicatorStochasticOscillator indSO = indicator as IndicatorStochasticOscillator;
                    try
                    {
                        if (!(dr["high"] is DBNull) && !(dr["low"] is DBNull) && !(dr["closeP"] is DBNull))
                        {
                            //indSO.DataSource = dtAllMsg;   
                            double[] kdj = indSO.Calculate(i);
                            SetValue(dr, indSO.TlsK.Field, kdj[0]);
                            SetValue(dr, indSO.TlsD.Field, kdj[1]);
                            SetValue(dr, indSO.TlsJ.Field, kdj[2]);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                }
                //指数平滑异同移动平均线
                else if (indicator is IndicatorMACD)
                {
                    IndicatorMACD indMacd = indicator as IndicatorMACD;
                    if (!(dr["closeP"] is DBNull))
                    {
                        double shortEMA = CommonClass.CalculateExponentialMovingAvg(indMacd.ShortCycleEMA, indMacd.Close, indMacd.ShortCycle, indMacd.DataSource, i);
                        double longEMA = CommonClass.CalculateExponentialMovingAvg(indMacd.LongCycleEMA, indMacd.Close, indMacd.LongCycle, indMacd.DataSource, i);
                        SetValue(dr, indMacd.ShortCycleEMA, shortEMA);
                        SetValue(dr, indMacd.LongCycleEMA, longEMA);
                        double[] macdValue = indMacd.Calulate(i);
                        SetValue(dr, indMacd.HsMACD.Field, macdValue[0]);
                        SetValue(dr, indMacd.TlsDiff.Field, macdValue[1]);
                        SetValue(dr, indMacd.TlsDea.Field, macdValue[2]);
                    }
                }
                //布林带
                else if (indicator is IndicatorBollingerBands)
                {
                    IndicatorBollingerBands indBoll = indicator as IndicatorBollingerBands;
                    double[] bollValue = indBoll.Calculate(i);
                    SetValue(dr, indBoll.TlsM.Field, bollValue[0]);
                    SetValue(dr, indBoll.TlsU.Field, bollValue[1]);
                    SetValue(dr, indBoll.TlsD.Field, bollValue[2]);
                }
            }
        }

        /// <summary>
        /// 设置值，自动排序
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="dateTime"></param>
        public void SetValue(DataRow row, string fieldName, object value)
        {
            if (!row.Table.Columns.Contains(fieldName))
            {
                row.Table.Columns.Add(fieldName);
            }

            row[fieldName] = value;

            /*
            //指标计算
            int r = this.dtAllMsg.Rows.IndexOf(dr);
            //简单移动平均线
            bool ind_flag = false;
            for (int m = r; m < this.dtAllMsg.Rows.Count; m++)
            {
                if (this.indSimpleMovingAverageList.Count > 0)
                {
                    foreach (IndicatorSimpleMovingAverage indSMA in this.indSimpleMovingAverageList)
                    {
                        if (indSMA.Target == fieldName)
                        {
                            ind_flag = true;
                            CalcutaIndicator(indSMA, m, m);
                        }
                    }
                }
                //指数移动平均线
                if (this.indExponentialMovingAverageList.Count > 0)
                {
                    foreach (IndicatorExponentialMovingAverage indEMA in this.indExponentialMovingAverageList)
                    {
                        if (indEMA.Target == fieldName)
                        {
                            ind_flag = true;
                            CalcutaIndicator(indEMA, m, m);
                        }
                    }
                }
                //随机指标
                if (this.indStochasticOscillatorList.Count > 0)
                {
                    foreach (IndicatorStochasticOscillator indSO in this.indStochasticOscillatorList)
                    {
                        if (fieldName == indSO.High || fieldName == indSO.Low || fieldName == indSO.Close)
                        {
                            ind_flag = true;
                            CalcutaIndicator(indSO, m, m);
                        }
                    }
                }
                //MACD指标
                if (this.indMacdList.Count > 0)
                {
                    foreach (IndicatorMACD indMacd in this.indMacdList)
                    {
                        if (fieldName == indMacd.Close)
                        {
                            ind_flag = true;
                            CalcutaIndicator(indMacd, m, m);
                        }
                    }
                }
                //BOLL线
                if (this.indBollList.Count > 0)
                {
                    foreach (IndicatorBollingerBands indBOLL in this.indBollList)
                    {
                        if (fieldName == indBOLL.Close)
                        {
                            ind_flag = true;
                            CalcutaIndicator(indBOLL, m, m);
                        }
                    }
                }
                if (!ind_flag)
                {
                    break;
                }
            }*/
        }

        /// <summary>
        /// 获取当前鼠标所在Panel的ID
        /// </summary>
        /// <returns></returns>
        public int GetCurrentPanel()
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                if (crossHair_y >= chartPanel.RectPanel.Y && crossHair_y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height)
                {
                    return chartPanel.PanelID;
                }
            }
            return -1;
        }

        /// <summary>
        /// 获取某一值的纵坐标
        /// </summary>
        /// <param name="chartPanel"></param>
        /// <param name="value"></param>
        public float GetValueYPixel(ChartPanel chartPanel, double chartValue)
        {
            return Convert.ToSingle((chartPanel.MaxValue - chartValue) / (chartPanel.MaxValue - chartPanel.MinValue) * GetWorkSpaceY(chartPanel.PanelID)
                + chartPanel.TitleHeight + chartPanel.RectPanel.Y);
        }

        /// <summary>
        /// 获取屏幕内显示的记录数
        /// </summary>
        /// <returns></returns>
        public int GetMaxVisibleRecord()
        {
            if (this.axisSpace == 0)
            {
                return this.GetWorkSpaceX();
            }
            else
            {
                return this.GetWorkSpaceX() / this.AxisSpace;
            }
        }

        /// <summary>
        /// 获取十字线的坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Point GetCrossHairPoint()
        {
            int x = this.PointToClient(MousePosition).X;
            int y = this.PointToClient(MousePosition).Y;
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                if (y >= chartPanel.RectPanel.Y && y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height)
                {
                    if (y > this.Height - chartPanel.ScaleX_Height)
                    {
                        y = this.Height - chartPanel.ScaleX_Height;
                    }
                    if (showLeftScale)
                    {
                        if (x < leftPixSpace)
                        {
                            x = leftPixSpace;
                        }
                    }
                    if (showRightScale)
                    {
                        if (x > this.Width - rightPixSpace)
                        {
                            x = this.Width - rightPixSpace;
                        }
                    }
                    break;
                }
            }
            return new Point(x, y);
        }

        /// <summary>
        /// 获取鼠标选中的记录索引,从0开始
        /// </summary>
        /// <returns></returns>
        public int GetCrossOverIndex()
        {
            Point mousePoint = GetCrossHairPoint();
            return (mousePoint.X - this.LeftPixSpace) / this.AxisSpace + this.firstVisibleRecord - 1;
        }

        /// <summary>
        /// 获取鼠标对应的值
        /// </summary>
        /// <returns></returns>
        public double GetCurrentValue()
        {
            Point mouseP = GetCrossHairPoint();
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                if (crossHair_y >= chartPanel.RectPanel.Y && crossHair_y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height)
                {
                    if (GetWorkSpaceY(chartPanel.PanelID) > 0)
                    {
                        double everyPointValue = (chartPanel.MaxValue - chartPanel.MinValue) / GetWorkSpaceY(chartPanel.PanelID);
                        return chartPanel.MaxValue - (crossHair_y - chartPanel.TitleHeight - chartPanel.RectPanel.Y) * everyPointValue;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取工作区的横向长度
        /// </summary>
        /// <returns></returns>
        public int GetWorkSpaceX()
        {
            return this.Width - this.LeftPixSpace - this.RightPixSpace;
        }

        /// <summary>
        /// 获取工作区的纵向长度
        /// </summary>
        /// <returns></returns>
        public int GetWorkSpaceY(int panelID)
        {
            if (this.dicChartPanel.ContainsKey(panelID))
            {
                return dicChartPanel[panelID].RectPanel.Height - dicChartPanel[panelID].ScaleX_Height - (int)dicChartPanel[panelID].TitleHeight;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        #region 绘图
        /// <summary>
        /// 画边线
        /// </summary>
        /// <param name="g"></param>
        private void DrawBackGround(Graphics g)
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                Rectangle drawRect = new Rectangle(0, chartPanel.RectPanel.Y, this.Width - 2, chartPanel.RectPanel.Height);
                g.FillRectangle(chartPanel.BgBrush, drawRect);
                g.DrawRectangle(chartPanel.PanelBorder_Pen, drawRect);
            }
        }

        /// <summary>
        /// 画十字线
        /// </summary>
        /// <param name="g"></param>
        public void DrawCrossHair(Graphics g)
        {
            Point mousePoint = GetCrossHairPoint();
            if (crossHair_y != -1)
            {
                mousePoint.Y = crossHair_y;
            }
            if (showCrossHair)
            {
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    if (mousePoint.Y >= chartPanel.RectPanel.Y +chartPanel.TitleHeight && mousePoint.Y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height)
                    {
                        //横向的线
                        g.DrawLine(chartPanel.CrossHair_Pen, LeftPixSpace, mousePoint.Y, this.Width - RightPixSpace, mousePoint.Y);
                    }
                    int verticalX = leftPixSpace + axisSpace * (crossOverIndex - firstVisibleRecord + 1) + axisSpace / 2;
                    //纵向的线
                    SizeF titleHeight=g.MeasureString(" ",chartPanel.TitleFont);
                    if (this.crossOverIndex == -1 || this.crossOverIndex < firstVisibleRecord - 1 || this.crossOverIndex > LastVisibleRecord - 1)
                    {
                        g.DrawLine(chartPanel.CrossHair_Pen, verticalX, chartPanel.RectPanel.Y + 5 + titleHeight.Height, verticalX, chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height);
                        continue;
                    }
                    else
                    {
                        float y = chartPanel.RectPanel.Y + 5 + titleHeight.Height;
                        if (this.dtAllMsg.Columns.Contains(chartPanel.InfoBombField))
                        {

                            if (!(this.dtAllMsg.Rows[crossOverIndex][chartPanel.InfoBombField] is DBNull)
                                && this.dtAllMsg.Rows[crossOverIndex][chartPanel.InfoBombField].ToString() != string.Empty)
                            {
                                y = chartPanel.RectPanel.Y + 10 + titleHeight.Height;
                            }
                        }
                        g.DrawLine(chartPanel.CrossHair_Pen, verticalX, y, verticalX, chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height);
                    }
                    SizeF xTipFontSize = g.MeasureString(CommonClass.GetCalenderFormatTimeKey(this.dtAllMsg.Rows[this.crossOverIndex][timekeyField].ToString(), chartPanel.Interval), chartPanel.XTipFont);
                    //X轴提示框及文字
                    if (chartPanel == bottomPanel)
                    {
                        RectangleF xRt = new RectangleF(verticalX - xTipFontSize.Width / 2 - 2, chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height + 1, xTipFontSize.Width + 4, xTipFontSize.Height + 2);
                        GraphicsPath gpXRT = CommonClass.GetRoundRectangle(1, xRt);
                        g.FillPath(chartPanel.Xtip_Brush, gpXRT);
                        g.DrawPath(chartPanel.XTipFont_Pen, gpXRT);
                        gpXRT.Dispose();
                        g.DrawString(CommonClass.GetCalenderFormatTimeKey(this.dtAllMsg.Rows[this.crossOverIndex][timekeyField].ToString(), chartPanel.Interval), chartPanel.XTipFont,
                           chartPanel.XTipFont_Brush, xRt);
                    }
                    
                    if (mousePoint.Y >= chartPanel.RectPanel.Y+chartPanel.TitleHeight && mousePoint.Y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height - chartPanel.ScaleX_Height)
                    {
                        double value = GetCurrentValue();
                        //显示左侧Y轴的提示框及文字
                        if (showLeftScale)
                        {
                            string leftValue = CommonClass.GetValueByDigit(value, chartPanel.Digit);
                            SizeF leftYTipFontSize = g.MeasureString(leftValue, chartPanel.LeftyTipFont);
                            RectangleF lRt = new RectangleF(this.LeftPixSpace - leftYTipFontSize.Width - 4, mousePoint.Y - leftYTipFontSize.Height / 2, leftYTipFontSize.Width + 4, leftYTipFontSize.Height + 1);
                            GraphicsPath gpLRT = CommonClass.GetRoundRectangle(1, lRt);
                            g.FillPath(chartPanel.LeftyTip_Brush, gpLRT);
                            g.DrawPath(chartPanel.LeftTipFont_Pen, gpLRT);
                            gpLRT.Dispose();
                            g.DrawString(leftValue, chartPanel.LeftyTipFont, chartPanel.LeftyTipFont_Brush, lRt);
                        }
                        //显示右侧Y轴的提示框及文字
                        if (ShowRightScale)
                        {
                            string rightValue = CommonClass.GetValueByDigit(value, chartPanel.Digit);
                            SizeF rightYTipFontSize = g.MeasureString(rightValue, chartPanel.RightyTipFont);
                            RectangleF rRt = new RectangleF(this.Width - RightPixSpace + 1, mousePoint.Y - rightYTipFontSize.Height / 2, rightYTipFontSize.Width + 4, rightYTipFontSize.Height + 1);
                            GraphicsPath gpRRT = CommonClass.GetRoundRectangle(2, rRt);
                            g.FillPath(chartPanel.RightyTip_Brush, gpRRT);
                            g.DrawPath(chartPanel.RightyTipFont_Pen, gpRRT);
                            gpRRT.Dispose();
                            g.DrawString(rightValue, chartPanel.RightyTipFont, chartPanel.RightyTipFont_Brush, rRt);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 画标题
        /// </summary>
        /// <param name="g"></param>
        private void DrawTitle(Graphics g)
        {
            objectRectDic.Clear();
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                float titleLeftPadding = this.LeftPixSpace;
                //创建字符串
                Font titleFont = chartPanel.TitleFont;
                int rightPadding = this.Width - this.rightPixSpace - 2;
                if (canDragSeries && this.dtAllMsg.Rows.Count > 0)
                {
                    //画拖动标记
                    foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                    {
                        SizeF sizeK = new SizeF(15f, 16f);
                        RectangleF rectCs = new RectangleF(rightPadding - sizeK.Width, chartPanel.RectPanel.Y + 2, sizeK.Width, sizeK.Height);
                        if (!showCrossHair && cs.HasSelect)
                        {
                            g.FillRectangle(cs.UpLine_TransparentBrush, rectCs);
                            g.DrawRectangle(cs.UpLine_Pen, rectCs.X, rectCs.Y, rectCs.Width, rectCs.Height);
                        }
                        g.DrawLine(cs.DownLine_Pen, rectCs.X + 4, rectCs.Y + 6, rectCs.X + 4, rectCs.Bottom - 2);
                        g.DrawLine(cs.UpLine_Pen, rectCs.X + 9, rectCs.Y + 2, rectCs.X + 9, rectCs.Bottom - 4);
                        g.FillRectangle(cs.DownLine_Brush, new RectangleF(rectCs.X + 3, rectCs.Y + 8, 3, 5));
                        g.FillRectangle(cs.UpLine_Brush, new RectangleF(rectCs.X + 8, rectCs.Y + 4, 3, 5));
                        rightPadding -= (int)sizeK.Width + 2;
                        objectRectDic[rectCs] = cs;
                    }
                    foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                    {
                        SizeF sizeK = new SizeF(15f, 16f);
                        RectangleF rectCs = new RectangleF(rightPadding - sizeK.Width, chartPanel.RectPanel.Y + 2, sizeK.Width, sizeK.Height);
                        float lineWidth = hs.Up_Pen.Width;
                        hs.Up_Pen.Width = 1f;
                        if (!showCrossHair && hs.HasSelect)
                        {
                            g.FillRectangle(hs.Up_TransparentBrush, rectCs);
                            g.DrawRectangle(hs.Up_Pen, rectCs.X, rectCs.Y, rectCs.Width, rectCs.Height);
                        }
                        g.FillRectangle(hs.Up_LineBrush, rectCs.X + 1, rectCs.Y + 10, 3, rectCs.Bottom - rectCs.Y - 11);
                        g.FillRectangle(hs.Up_LineBrush, rectCs.X + 6, rectCs.Y + 3, 3, rectCs.Bottom - rectCs.Y - 4);
                        g.FillRectangle(hs.Up_LineBrush, rectCs.X + 11, rectCs.Y + 8, 3, rectCs.Bottom - rectCs.Y - 9);
                        rightPadding -= (int)sizeK.Width + 2;
                        objectRectDic[rectCs] = hs;
                        hs.Up_Pen.Width = lineWidth;
                    }
                    foreach (TrendLineSeries tls in chartPanel.TrendLineSeriesList)
                    {
                        SizeF sizeK = new SizeF(15f, 16f);
                        float lineWidth = tls.Up_LinePen.Width;
                        tls.Up_LinePen.Width = 1f;
                        RectangleF rectCs = new RectangleF(rightPadding - sizeK.Width, chartPanel.RectPanel.Y + 2, sizeK.Width, sizeK.Height);
                        if (!showCrossHair && tls.HasSelect)
                        {
                            g.FillRectangle(tls.TransParentLineBrush, rectCs);
                            g.DrawRectangle(tls.Up_LinePen, rectCs.X, rectCs.Y, rectCs.Width, rectCs.Height);
                        }
                        g.DrawLine(tls.Up_LinePen, rectCs.X + 2, rectCs.Y + 5, rectCs.X + 12, rectCs.Y + 1);
                        g.DrawLine(tls.Up_LinePen, rectCs.X + 2, rectCs.Y + 10, rectCs.X + 12, rectCs.Y + 6);
                        g.DrawLine(tls.Up_LinePen, rectCs.X + 2, rectCs.Y + 15, rectCs.X + 12, rectCs.Y + 11);
                        rightPadding -= (int)sizeK.Width + 2;
                        objectRectDic[rectCs] = tls;
                        tls.Up_LinePen.Width = lineWidth;
                    }
                }
                //画标题下方的线
                SizeF sizeTitle = g.MeasureString(" ", titleFont);
                g.DrawLine(chartPanel.Grid_Pen, this.leftPixSpace, chartPanel.RectPanel.Y + 5 + sizeTitle.Height,
                    this.Width - this.rightPixSpace, chartPanel.RectPanel.Y + 5 + sizeTitle.Height);
                //画标题
                if (chartPanel.UserDefinedTitle)
                {
                    StringFormat sf = new StringFormat();
                    foreach (TitleField titleField in chartPanel.TitleFieldList)
                    {
                        string field = titleField.RelateSeriesField;
                        Color fieldColor = titleField.FieldColor;
                        string drawTitle = titleField.DisplayTitle;
                        SizeF sizeF = g.MeasureString(drawTitle, titleFont, 1000, sf);
                        Rectangle titleRect = new Rectangle((int)titleLeftPadding, chartPanel.RectPanel.Y + 2, (int)sizeF.Width, (int)sizeF.Height);
                        RectangleF drawRect = new RectangleF(titleLeftPadding, chartPanel.RectPanel.Y + 2, sizeF.Width, sizeF.Height);
                        if (titleLeftPadding + sizeF.Width <= rightPadding)
                        {
                            g.DrawString(drawTitle, titleFont, titleField.FieldBrush, drawRect);
                            if (canDragSeries)
                            {
                                foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                                {
                                    if (cs.OpenField == field || cs.HighField == field
                                        || cs.LowField == field || cs.CloseField == field)
                                    {
                                        objectRectDic[titleRect] = cs;
                                    }
                                }
                                foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                                {
                                    if (hs.Field == field)
                                    {
                                        objectRectDic[titleRect] = hs;
                                    }
                                }
                                foreach (TrendLineSeries tls in chartPanel.TrendLineSeriesList)
                                {
                                    if (tls.Field == field)
                                    {
                                        objectRectDic[titleRect] = tls;
                                    }
                                }
                            }
                        }
                        titleLeftPadding += sizeF.Width;
                    }
                    sf.Dispose();
                }
                else
                {
                    SizeF layNameSize = g.MeasureString(chartPanel.PanelTitle, titleFont);
                    if (titleLeftPadding + layNameSize.Width <= this.Width - RightPixSpace)
                    {
                        g.DrawString(chartPanel.PanelTitle, titleFont, chartPanel.TitleFont_Brush, new PointF(titleLeftPadding, chartPanel.RectPanel.Y + 2));
                    }
                    titleLeftPadding += layNameSize.Width;
                    DataRow dr = null;
                    if (this.dtAllMsg.Rows.Count > 0 && LastVisibleRecord > 0 & processBarValue == 0)
                    {
                        int displayIndex = LastVisibleRecord - 1;
                        if (showCrossHair)
                        {
                            if (crossOverIndex <= LastVisibleRecord)
                            {
                                displayIndex = crossOverIndex;
                            }
                        }
                        if (displayIndex >= 0 && displayIndex < this.dtAllMsg.Rows.Count)
                        {
                            dr = this.dtAllMsg.Rows[displayIndex];
                        }
                        foreach (DataColumn dataColumn in this.dtAllMsg.Columns)
                        {
                            string condition = dataColumn.ColumnName;
                            string displayName = null;
                            bool drawFlag = false;
                            Color titleColor = Color.White;
                            object selectedObj = null;
                            foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                            {
                                if (cs.DisplayTitleField)
                                {
                                    if (condition == cs.OpenField)
                                    {
                                        titleColor = cs.OpenTitleColor;
                                        drawFlag = true;
                                        selectedObj = cs;
                                        goto CompleteSet;
                                    }
                                    else if (condition == cs.HighField)
                                    {
                                        titleColor = cs.HighTitleColor;
                                        drawFlag = true;
                                        selectedObj = cs;
                                        goto CompleteSet;
                                    }
                                    else if (condition == cs.LowField)
                                    {
                                        titleColor = cs.LowTitleColor;
                                        drawFlag = true;
                                        selectedObj = cs;
                                        goto CompleteSet;
                                    }
                                    else if (condition == cs.CloseField)
                                    {
                                        titleColor = cs.CloseTitleColor;
                                        drawFlag = true;
                                        selectedObj = cs;
                                        goto CompleteSet;
                                    }
                                }
                            }
                            if (chartPanel.TrendLineSeriesList != null && chartPanel.TrendLineSeriesList.Count > 0)
                            {
                                foreach (TrendLineSeries ls in chartPanel.TrendLineSeriesList)
                                {
                                    if (ls.Field == condition && ls.DisplayName != null)
                                    {
                                        titleColor = ls.Up_LineColor;
                                        drawFlag = true;
                                        displayName = ls.DisplayName;
                                        selectedObj = ls;
                                        goto CompleteSet;
                                    }
                                }
                            }
                            if (chartPanel.HistoramSeriesList != null && chartPanel.HistoramSeriesList.Count > 0)
                            {
                                foreach (HistogramSeries hs in chartPanel.HistoramSeriesList)
                                {
                                    if (hs.Field == condition && hs.DisplayName != null)
                                    {
                                        titleColor = hs.Down_lineColor;
                                        drawFlag = true;
                                        displayName = hs.DisplayName;
                                        selectedObj = hs;
                                        goto CompleteSet;
                                    }
                                }
                            }
                        CompleteSet: ;
                            if (drawFlag)
                            {
                                string showTitle = condition.Replace("P", "").ToUpper() + " ";
                                if (displayName != null)
                                {
                                    showTitle = displayName + " ";
                                    if (displayName == string.Empty)
                                    {
                                        showTitle = displayName;
                                    }
                                }
                                if (dr != null)
                                {
                                    if (dtAllMsg.Columns.Contains(condition))
                                    {
                                        double value = 0;
                                        try {
                                            double.TryParse(dr[condition].ToString(), out value); }
                                        catch(Exception e) { }
                                        showTitle += CommonClass.GetValueByDigit(value, chartPanel.Digit);
                                    }
                                }
                                SizeF conditionSize = g.MeasureString(showTitle, titleFont);
                                if (titleLeftPadding + conditionSize.Width <= rightPadding)
                                {
                                    Brush titleBrush = new SolidBrush(titleColor);
                                    g.DrawString(showTitle, titleFont, titleBrush, new PointF(titleLeftPadding, chartPanel.RectPanel.Y + 2));
                                    titleBrush.Dispose();
                                    if (selectedObj != null)
                                    {
                                        RectangleF titleRect = new RectangleF(titleLeftPadding, chartPanel.RectPanel.Y + 2, conditionSize.Width, conditionSize.Height);
                                        objectRectDic[titleRect] = selectedObj;
                                    }
                                }
                                titleLeftPadding += conditionSize.Width;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 画坐标轴
        /// </summary>
        /// <param name="g"></param>
        public void DrawScale(Graphics g)
        {
            foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
            {
                double sMin = 0;
                double step = 0;
                int gN = 0;
                int panelBottm = chartPanel.RectPanel.Y + chartPanel.RectPanel.Height;
                int workSpaceY = GetWorkSpaceY(chartPanel.PanelID);
                //画X轴
                if (chartPanel == bottomPanel && this.Height >= chartPanel.ScaleX_Height)
                {
                    g.DrawLine(chartPanel.XScalePen, 0, panelBottm - chartPanel.ScaleX_Height, this.Width, panelBottm - chartPanel.ScaleX_Height);
                }
                //画左侧Y轴
                bool drawingGridFlag = false;
                if (showLeftScale && this.leftPixSpace <= this.Width)
                {
                    if (this.LeftPixSpace <= this.Width)
                    {
                        g.DrawLine(chartPanel.LeftScalePen, leftPixSpace, chartPanel.RectPanel.Y + 1, leftPixSpace, panelBottm - chartPanel.ScaleX_Height);
                    }
                    if (processBarValue == 0)
                    {
                        Font leftYFont = chartPanel.LeftYFont;
                        SizeF leftYSize = g.MeasureString(" ", leftYFont);
                        gN = (int)(workSpaceY / leftYSize.Height) * 40 / 30;
                        CommonClass.GridScale(chartPanel.MinValue, chartPanel.MaxValue, gN, ref sMin, ref step, chartPanel.YScaleTick);
                        int interval = 0;
                        int gridInterval = chartPanel.GridInterval;
                        while (sMin <= chartPanel.MaxValue)
                        {
                            if (sMin > chartPanel.MinValue)
                            {
                                if (interval != 0 && interval % gridInterval == 0)
                                {
                                    leftYSize = g.MeasureString(CommonClass.GetValueByDigit(sMin, chartPanel.Digit), leftYFont);
                                    g.DrawLine(chartPanel.LeftScalePen, this.LeftPixSpace - 10,
                                        GetValueYPixel(chartPanel, sMin), this.LeftPixSpace, GetValueYPixel(chartPanel, sMin));
                                    g.DrawString(CommonClass.GetValueByDigit(sMin, chartPanel.Digit),
                                        leftYFont, chartPanel.LeftYFont_Brush,
                                        new RectangleF(this.LeftPixSpace - 10 - leftYSize.Width,
                                        GetValueYPixel(chartPanel, sMin) - leftYSize.Height / 2, leftYSize.Width, leftYSize.Height));
                                    drawingGridFlag = true;
                                    if (chartPanel.ShowGrid)
                                    {
                                        g.DrawLine(chartPanel.Grid_Pen, LeftPixSpace,
                                        GetValueYPixel(chartPanel, sMin), this.Width - RightPixSpace, GetValueYPixel(chartPanel, sMin));
                                    }
                                }
                                else
                                {
                                    g.DrawLine(chartPanel.LeftScalePen, this.LeftPixSpace - 5, GetValueYPixel(chartPanel, sMin), this.LeftPixSpace, GetValueYPixel(chartPanel, sMin));
                                }
                            }
                            sMin += step;
                            interval++;
                            if (sMin < 0)
                                break;
                        }
                    }
                }
                //画右侧Y轴
                if (showRightScale && this.rightPixSpace <= this.Width)
                {
                    if (this.Width - RightPixSpace >= LeftPixSpace)
                    {
                        g.DrawLine(chartPanel.RightScalePen, this.Width - rightPixSpace, chartPanel.RectPanel.Y + 1, this.Width - rightPixSpace, panelBottm - chartPanel.ScaleX_Height);
                    }
                    if (processBarValue == 0)
                    {
                        Font rightYFont = chartPanel.RightYFont;
                        SizeF rightYSize = g.MeasureString(" ", rightYFont);
                        gN = (int)(workSpaceY / rightYSize.Height) * 40 / 30;
                        CommonClass.GridScale(chartPanel.MinValue, chartPanel.MaxValue, gN, ref sMin, ref step, chartPanel.YScaleTick);
                        int interval = 0;
                        int gridInterval = chartPanel.GridInterval;
                        while (sMin <= chartPanel.MaxValue)
                        {
                            if (sMin > chartPanel.MinValue)
                            {
                                if (interval != 0 && interval % gridInterval == 0)
                                {
                                    g.DrawLine(chartPanel.RightScalePen, this.Width - RightPixSpace,
                                        GetValueYPixel(chartPanel, sMin),
                                        this.Width - RightPixSpace + 10, GetValueYPixel(chartPanel, sMin));
                                    g.DrawString(CommonClass.GetValueByDigit(sMin, chartPanel.Digit),
                                        rightYFont, chartPanel.RightYFont_Brush,
                                        new RectangleF(this.Width - RightPixSpace + 10,
                                        GetValueYPixel(chartPanel, sMin) - rightYSize.Height / 2, this.RightPixSpace, rightYSize.Height));
                                    if (!drawingGridFlag)
                                    {
                                        drawingGridFlag = true;
                                        if (chartPanel.ShowGrid)
                                        {
                                            g.DrawLine(chartPanel.Grid_Pen, LeftPixSpace,
                                            GetValueYPixel(chartPanel, sMin),
                                            this.Width - RightPixSpace,
                                            GetValueYPixel(chartPanel, sMin));
                                        }
                                    }
                                }
                                else
                                {
                                    g.DrawLine(chartPanel.RightScalePen, this.Width - RightPixSpace, GetValueYPixel(chartPanel, sMin), this.Width - RightPixSpace + 5, GetValueYPixel(chartPanel, sMin));
                                }
                            }
                            sMin += step;
                            interval++;
                            if (sMin < 0)
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制图形
        /// </summary>
        /// <param name="g"></param>
        public void DrawSeries(Graphics g)
        {
            //设置最大值和最小值
            int minIndex = 0;
            int fRecord = firstVisibleRecord - 1;
            int lRecord = LastVisibleRecord;
            if (fRecord < 0 || lRecord < 1)
            {
                return;
            }
            List<object[]> signalList = new List<object[]>();
            Dictionary<int, string> infoBombDic = new Dictionary<int, string>();
            for (int i = fRecord; i < lRecord; i++)
            {
                string timeKeyShow = this.dtAllMsg.Rows[i][timekeyField].ToString();
                DataRow dr = dtAllMsg.Rows[i];
                if (dr == null)
                {
                    continue;
                }
                //画X轴的刻度
                float scaleX = this.leftPixSpace + (i + 2 - firstVisibleRecord) * axisSpace - axisSpace / 2;
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    //string timeKey = CommonClass.GetCalenderFormatTimeKey(timeKeyShow, chartPanel.Interval);
                    SizeF timeKeySize = g.MeasureString(timeKeyShow, chartPanel.CoordinateXFont);
                    int panelBottom = chartPanel.RectPanel.Y + chartPanel.RectPanel.Height;
                    //画X轴
                    if (chartPanel == bottomPanel)
                    {
                        if (i == firstVisibleRecord - 1)
                        {
                            g.DrawLine(chartPanel.XScalePen, scaleX, panelBottom - chartPanel.ScaleX_Height,
                                scaleX, panelBottom - chartPanel.ScaleX_Height + 6);
                            g.DrawString(timeKeyShow, chartPanel.CoordinateXFont, chartPanel.CoordinateXFont_Brush,
                                new PointF(scaleX - timeKeySize.Width / 2, panelBottom - chartPanel.ScaleX_Height + 6));
                        }
                        if (scaleX - LeftPixSpace > timeKeySize.Width * 2 && minIndex == 0)
                        {
                            minIndex = i - (firstVisibleRecord - 1);
                        }
                        if (minIndex != 0 && (i - (firstVisibleRecord - 1)) % minIndex == 0)
                        {
                            g.DrawString(timeKeyShow, chartPanel.CoordinateXFont, chartPanel.CoordinateXFont_Brush,
                                new PointF(scaleX - timeKeySize.Width / 2, panelBottom - chartPanel.ScaleX_Height + 6));
                            g.DrawLine(chartPanel.XScalePen, scaleX, panelBottom - chartPanel.ScaleX_Height,
                                scaleX, panelBottom - chartPanel.ScaleX_Height + 6);
                        }
                        else
                        {
                            g.DrawLine(chartPanel.XScalePen, scaleX, panelBottom - chartPanel.ScaleX_Height, scaleX,
                                panelBottom - chartPanel.ScaleX_Height + 3);
                        }
                    }
                    
                    

                    //画K线
                    foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                    {
                        double open = Convert.ToDouble(dr[cs.OpenField]);
                        double high = Convert.ToDouble(dr[cs.HighField]);
                        double low = Convert.ToDouble(dr[cs.LowField]);
                        double close = Convert.ToDouble(dr[cs.CloseField]);
                        if (GetWorkSpaceY(chartPanel.PanelID) > 0)
                        {
                            int buySellSignal = 0;
                            //画买卖点
                            if (cs.IndBuySellField[0] != null && cs.IndBuySellField[1] != null)
                            {
                                double buySignalValue = Convert.ToDouble(dr[cs.IndBuySellField[0]]);
                                double sellSignalValue = Convert.ToDouble(dr[cs.IndBuySellField[1]]);
                                if (buySignalValue >= sellSignalValue)
                                {
                                    buySellSignal = 1;
                                }
                                else
                                {
                                    buySellSignal = 2;
                                }
                                int flag = 0;
                                if (i <dtAllMsg.Rows.Count-1)
                                {
                                    double lastBuy = Convert.ToDouble(dtAllMsg.Rows[i+1][cs.IndBuySellField[0]]);
                                    double lastSell = Convert.ToDouble(dtAllMsg.Rows[i+1][cs.IndBuySellField[1]]);
                                    if (lastBuy >= lastSell)
                                    {
                                        if (buySellSignal != 1)
                                        {
                                            flag = 2;
                                            buySellSignal = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (buySellSignal != 2)
                                        {
                                            flag = 1;
                                            buySellSignal = 2;
                                        }
                                    }
                                }
                                StringFormat sf = new StringFormat();
                                if (flag == 1)
                                {
                                    Font bsFont = cs.BsFont;
                                    SizeF bsFontSize = g.MeasureString(cs.SellText, bsFont, 1000, sf);
                                    Brush sBrush = new SolidBrush(cs.SellColor);
                                    g.DrawString(cs.SellText, bsFont, sBrush, new PointF((int)scaleX - bsFontSize.Width / 2,
                                        GetValueYPixel(chartPanel, high) - bsFontSize.Height));
                                    sBrush.Dispose();
                                }
                                else if (flag == 2)
                                {
                                    Font bsFont = cs.BsFont;
                                    SizeF bsFontSize = g.MeasureString(cs.BuyText, bsFont, 1000, sf);
                                    Brush bBrush = new SolidBrush(cs.BuyColor);
                                    g.DrawString(cs.BuyText, bsFont, bBrush, new PointF((int)scaleX - bsFontSize.Width / 2,
                                    GetValueYPixel(chartPanel, low) + 2));
                                    bBrush.Dispose();
                                }
                                sf.Dispose();
                            }
                            //阳线
                            if (open <= close)
                            {
                                float recth = close - open != 0 ? (float)((close - open) / (chartPanel.MaxValue - chartPanel.MinValue) * GetWorkSpaceY(chartPanel.PanelID)) : 1;
                                if (recth < 1)
                                {
                                    recth = 1;
                                }
                                RectangleF rcUp = new RectangleF((int)scaleX - (int)(axisSpace / 4), GetValueYPixel(chartPanel, close), (int)(axisSpace / 4) * 2 + 1, recth);
                                Pen upPen = null;
                                switch (buySellSignal)
                                {
                                    case 0:
                                        upPen = cs.UpLine_Pen;
                                        break;
                                    case 1:
                                        upPen = Pens.Red;
                                        break;
                                    case 2:
                                        upPen = Pens.SkyBlue;
                                        break;
                                }
                                //先画竖线
                                if (cs.MiddleLine_Pen != null)
                                {
                                    g.DrawLine(cs.MiddleLine_Pen, scaleX, GetValueYPixel(chartPanel, high), scaleX, GetValueYPixel(chartPanel, low));
                                }
                                else
                                {
                                    g.DrawLine(upPen, scaleX, GetValueYPixel(chartPanel, high), scaleX, GetValueYPixel(chartPanel, low));
                                }
                                try { g.FillRectangle(chartPanel.BgBrush, new Rectangle((int)rcUp.X + 1, (int)rcUp.Y + 1, (int)rcUp.Width - 2, (int)rcUp.Height - 1));
                                    g.DrawRectangle(upPen, new Rectangle((int)rcUp.X, (int)rcUp.Y, (int)rcUp.Width, (int)rcUp.Height));
                                }
                                catch { }
                               
                            }
                            //阴线
                            else
                            {
                                float recth = open - close != 0 ? (float)((open - close) / (chartPanel.MaxValue - chartPanel.MinValue) * GetWorkSpaceY(chartPanel.PanelID)) : 1;
                                if (recth < 1)
                                {
                                    recth = 1;
                                }
                                RectangleF rcDown = new RectangleF((int)scaleX - (int)(axisSpace / 4), GetValueYPixel(chartPanel, open), (int)(axisSpace / 4) * 2 + 1, recth);
                                Brush downBrush = null;
                                Pen downPen = null;
                                switch (buySellSignal)
                                {
                                    case 0:
                                        downBrush = cs.DownLine_Brush;
                                        downPen = cs.DownLine_Pen;
                                        break;
                                    case 1:
                                        downBrush = Brushes.Red;
                                        downPen = Pens.Red;
                                        break;
                                    case 2:
                                        downBrush = Brushes.SkyBlue;
                                        downPen = Pens.SkyBlue;
                                        break;
                                }
                                if (cs.MiddleLine_Pen != null)
                                {
                                    g.DrawLine(cs.MiddleLine_Pen, scaleX, GetValueYPixel(chartPanel, high), scaleX, GetValueYPixel(chartPanel, low));
                                }
                                else
                                {
                                    g.DrawLine(downPen, scaleX, GetValueYPixel(chartPanel, high), scaleX, GetValueYPixel(chartPanel, low));
                                }
                                g.FillRectangle(downBrush, rcDown);
                            }
                            //显示选中
                            if (cs.HasSelect)
                            {
                                if (showCrossHair)
                                {
                                    cs.HasSelect = false;
                                }
                                else
                                {
                                    int kPInterval = GetMaxVisibleRecord() / 30;
                                    if (kPInterval < 2)
                                    {
                                        kPInterval = 3;
                                    }
                                    if (i % kPInterval == 0)
                                    {
                                        RectangleF rect = new RectangleF((int)scaleX - 3, GetValueYPixel(chartPanel, close), 6, 6);
                                        g.FillRectangle(Brushes.White, rect);
                                    }
                                }
                            }
                        }
                    }
                    //画柱状图
                    foreach (HistogramSeries his in chartPanel.HistoramSeriesList)
                    {
                        if (dr[his.Field].ToString() != "")
                        {
                            double value = Convert.ToDouble(dr[his.Field]);
                            RectangleF rcHis = new RectangleF();
                            if (value >= 0)
                            {
                                rcHis = new RectangleF((int)scaleX - (int)(axisSpace / 4),
                                 GetValueYPixel(chartPanel, value), (int)(axisSpace / 5) * 2 + 1,
                                 GetValueYPixel(chartPanel, 0) - GetValueYPixel(chartPanel, value));
                            }
                            else
                            {
                                rcHis = new RectangleF((int)scaleX - (int)(axisSpace / 4),
                                 GetValueYPixel(chartPanel, 0), (int)(axisSpace / 5) * 2 + 1,
                                 GetValueYPixel(chartPanel, value) - GetValueYPixel(chartPanel, 0));
                            } 
                            if (his.RelateCandleName != null && his.RelateCandleName != string.Empty)
                            {
                                foreach (ChartPanel cp in this.dicChartPanel.Values)
                                {
                                    foreach (CandleSeries candleSeries in cp.CandleSeriesList)
                                    {
                                        if (candleSeries.CandleName == his.RelateCandleName)
                                        {
                                            double targetOpen = Convert.ToDouble(dr[candleSeries.OpenField]);
                                            double targetClose = Convert.ToDouble(dr[candleSeries.CloseField]);
                                            if (his.LineStyle)
                                            {
                                                PointF startP = new PointF((int)scaleX, GetValueYPixel(chartPanel, 0));
                                                PointF endP = new PointF((int)scaleX, GetValueYPixel(chartPanel, value));
                                                Pen linePen = null;
                                                int lineWidth = his.LineWidth;
                                                if (lineWidth > this.axisSpace)
                                                {
                                                    lineWidth = this.axisSpace;
                                                }
                                                if (targetOpen >= targetClose)
                                                {
                                                    linePen = his.Up_Pen;
                                                }
                                                else
                                                {
                                                    linePen = his.Down_Pen;
                                                }
                                                linePen.Width = lineWidth;
                                                if (startP.Y <= chartPanel.RectPanel.Y)
                                                {
                                                    startP.Y = chartPanel.RectPanel.Y;
                                                }
                                                if (startP.Y >= chartPanel.RectPanel.Bottom)
                                                {
                                                    startP.Y = chartPanel.RectPanel.Bottom;
                                                }
                                                if (endP.Y <= chartPanel.RectPanel.Y)
                                                {
                                                    endP.Y = chartPanel.RectPanel.Y;
                                                }
                                                if (endP.Y >= chartPanel.RectPanel.Bottom)
                                                {
                                                    endP.Y = chartPanel.RectPanel.Bottom;
                                                }
                                                g.DrawLine(linePen, startP, endP);
                                                linePen.Width = 1;
                                            }
                                            else
                                            {
                                                if (targetOpen >= targetClose)
                                                {
                                                    g.FillRectangle(chartPanel.BgBrush, rcHis);
                                                    g.DrawRectangle(his.Up_Pen, new Rectangle((int)rcHis.X,
                                                        (int)rcHis.Y, (int)rcHis.Width, (int)rcHis.Height + 1));
                                                }
                                                else
                                                {
                                                    g.FillRectangle(his.Down_lineBrush, rcHis);
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (his.LineStyle)
                                {
                                    PointF startP = new PointF((int)scaleX, GetValueYPixel(chartPanel, 0));
                                    PointF endP = new PointF((int)scaleX, GetValueYPixel(chartPanel, value));
                                    Pen linePen = null;
                                    int lineWidth = his.LineWidth;
                                    if (lineWidth > this.axisSpace)
                                    {
                                        lineWidth = this.axisSpace;
                                    }
                                    if (value >= 0)
                                    {
                                        linePen = his.Up_Pen;
                                    }
                                    else
                                    {
                                        linePen = his.Down_Pen;
                                    }
                                    linePen.Width = lineWidth;
                                    if (startP.Y <= chartPanel.RectPanel.Y)
                                    {
                                        startP.Y = chartPanel.RectPanel.Y;
                                    }
                                    if (startP.Y >= chartPanel.RectPanel.Bottom)
                                    {
                                        startP.Y = chartPanel.RectPanel.Bottom;
                                    }
                                    if (endP.Y <= chartPanel.RectPanel.Y)
                                    {
                                        endP.Y = chartPanel.RectPanel.Y;
                                    }
                                    if (endP.Y >= chartPanel.RectPanel.Bottom)
                                    {
                                        endP.Y = chartPanel.RectPanel.Bottom;
                                    }
                                    g.DrawLine(linePen, startP, endP);
                                    linePen.Width = 1;
                                }
                                else
                                {
                                    if (value >= 0)
                                    {
                                        g.FillRectangle(chartPanel.BgBrush, rcHis);
                                        g.DrawRectangle(his.Up_Pen, new Rectangle((int)rcHis.X, (int)rcHis.Y, (int)rcHis.Width, (int)rcHis.Height + 1));
                                    }
                                    else
                                    {
                                        g.FillRectangle(his.Down_lineBrush, rcHis);
                                    }
                                }
                            }
                            if (his.HasSelect == true)
                            {
                                if (showCrossHair)
                                {
                                    his.HasSelect = false;
                                }
                                else
                                {
                                    int kPInterval = GetMaxVisibleRecord() / 30;
                                    if (kPInterval < 2)
                                    {
                                        kPInterval = 2;
                                    }
                                    if (i % kPInterval == 0)
                                    {
                                        RectangleF rect = new RectangleF((int)scaleX - 3, GetValueYPixel(chartPanel, value) - 3, 6, 6);
                                        g.FillRectangle(Brushes.Yellow, rect);
                                    }
                                }
                            }
                        }
                        //画零线
                        if (chartPanel.MinValue < 0)
                        {
                            g.DrawLine(his.Down_Pen, leftPixSpace, GetValueYPixel(chartPanel, 0), this.Width - rightPixSpace, GetValueYPixel(chartPanel, 0));
                        }
                    }
                    //画趋势线
                    for (int lsJ = 0; lsJ < chartPanel.TrendLineSeriesList.Count; lsJ++)
                    {
                        TrendLineSeries ls = chartPanel.TrendLineSeriesList[lsJ];
                        PointF pStart = new PointF();
                        PointF pEnd = new PointF();
                        if (!(dr[ls.Field] is DBNull) && dr[ls.Field].ToString() != "")
                        {
                            double value = Convert.ToDouble(dr[ls.Field]);
                            if (dtAllMsg.Rows.Count == 1)
                            {
                                pStart = new PointF((int)scaleX - (int)(axisSpace / 4), GetValueYPixel(chartPanel, value));
                                pEnd = new PointF((int)scaleX - (int)(axisSpace / 4) + (int)(axisSpace / 4) * 2 + 1, GetValueYPixel(chartPanel, value));
                            }
                            else
                            {
                                DataRow drLast = null;
                                double lastValue = 0;
                                for (int j = i - 1; j >= fRecord; j--)
                                {
                                    string tk = this.dtAllMsg.Rows[j][timekeyField].ToString();
                                    DataRow drOld = dtAllMsg.Rows[j];
                                    if (!(drOld[ls.Field] is DBNull) && drOld[ls.Field].ToString() != "")
                                    {
                                        int left = this.leftPixSpace + (j + 2 - firstVisibleRecord) * axisSpace - axisSpace / 2;
                                        lastValue = Convert.ToDouble(drOld[ls.Field]);
                                        pStart = new PointF((int)left, GetValueYPixel(chartPanel, lastValue));
                                        if (j != i - 1)
                                        {
                                            int right = this.leftPixSpace + (i + 1 - firstVisibleRecord) * axisSpace - axisSpace / 2;
                                            pEnd = new PointF((int)right, GetValueYPixel(chartPanel, lastValue));
                                            if (pStart.Y <= panelBottom - chartPanel.ScaleX_Height + 1
                                                && pStart.Y >= chartPanel.RectPanel.Y + chartPanel.TitleHeight - 1
                                                && pEnd.Y < panelBottom - chartPanel.ScaleX_Height + 1
                                                && pEnd.Y >= chartPanel.RectPanel.Y + chartPanel.TitleHeight - 1)
                                            {
                                                if (lastValue >= value)
                                                {
                                                    g.DrawLine(ls.Up_LinePen, pStart, pEnd);
                                                }
                                                else
                                                {
                                                    g.DrawLine(ls.Down_linePen, pStart, pEnd);
                                                }
                                            }
                                            pStart = new PointF((int)right, GetValueYPixel(chartPanel, lastValue));
                                        }
                                        drLast = drOld;
                                        break;
                                    }
                                }
                                pEnd = new PointF((int)scaleX, GetValueYPixel(chartPanel, value));
                                if (drLast != null)
                                {
                                    if (pStart.Y <= panelBottom - chartPanel.ScaleX_Height + 1
                                        && pStart.Y >= chartPanel.RectPanel.Y + chartPanel.TitleHeight - 1
                                        && pEnd.Y < panelBottom - chartPanel.ScaleX_Height + 1
                                        && pEnd.Y >= chartPanel.RectPanel.Y + chartPanel.TitleHeight - 1)
                                    {
                                        if (lastValue >= value)
                                        {
                                            g.DrawLine(ls.Up_LinePen, pStart, pEnd);
                                        }
                                        else
                                        {
                                            g.DrawLine(ls.Down_linePen, pStart, pEnd);
                                        }
                                    }
                                }
                            }
                            //显示选中
                            if (ls.HasSelect)
                            {
                                if (showCrossHair)
                                {
                                    ls.HasSelect = false;
                                }
                                else
                                {
                                    int kPInterval = GetMaxVisibleRecord() / 30;
                                    if (kPInterval < 1)
                                    {
                                        kPInterval = 1;
                                    }
                                    if (i % kPInterval == 0)
                                    {
                                        RectangleF rect = new RectangleF((int)scaleX - 3, GetValueYPixel(chartPanel, value) - 3, 6, 6);
                                        if (rect.Y < panelBottom - chartPanel.ScaleX_Height)
                                        {
                                            g.FillRectangle(ls.Up_LineBrush, rect);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //画信息地雷
                    if (this.dtAllMsg.Columns.Contains(chartPanel.InfoBombField))
                    {
                        if (!(dr[chartPanel.InfoBombField] is DBNull) && dr[chartPanel.InfoBombField].ToString() != string.Empty)
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                            RectangleF rectBomb = new RectangleF(scaleX - 3, chartPanel.RectPanel.Y + chartPanel.TitleHeight - 6, 6, 6);
                            Color bombColor = chartPanel.InfoBombColor;
                            if (showCrossHair && i == crossOverIndex)
                            {
                                if (i == crossOverIndex)
                                {
                                    bombColor = chartPanel.InfoBombSelectedColor;
                                    infoBombDic[chartPanel.PanelID] = dr[chartPanel.InfoBombField].ToString();
                                }
                            }
                            Pen bombPen = new Pen(bombColor);
                            Brush bombBrush = new SolidBrush(bombColor);
                            g.FillEllipse(bombBrush, rectBomb);
                            g.DrawLine(bombPen, scaleX, rectBomb.Top - 1, scaleX, rectBomb.Bottom + 1);
                            g.DrawLine(bombPen, scaleX - 4, rectBomb.Top + 3, scaleX + 4, rectBomb.Top + 3);
                            g.DrawLine(bombPen, scaleX - 4, rectBomb.Top - 1, scaleX + 4, rectBomb.Bottom + 1);
                            g.DrawLine(bombPen, scaleX - 4, rectBomb.Bottom + 1, scaleX + 4, rectBomb.Top - 1);
                            g.SmoothingMode = SmoothingMode.Default;
                            bombPen.Dispose();
                            bombBrush.Dispose();
                        }
                    }
                    //保存要绘画的标记
                    if (chartPanel.SignalSeriesDic.ContainsKey(timeKeyShow))
                    {
                        foreach (SignalSeries uds in chartPanel.SignalSeriesDic[timeKeyShow])
                        {
                            if (uds.Value >= chartPanel.MinValue && uds.Value <= chartPanel.MaxValue)
                            {
                                GraphicsPath gp = uds.GetGPByType(scaleX, GetValueYPixel(chartPanel, uds.Value), this.axisSpace);
                                Brush signalBrush = new SolidBrush(Color.FromArgb(100, uds.SignalColor));
                                Pen signalPen = new Pen(uds.SignalColor);
                                signalList.Add(new object[] { gp, signalBrush, signalPen });
                            }
                        }
                    }
                }
            }
            //绘制标记
            foreach (object[] obj in signalList)
            {
                GraphicsPath gp = obj[0] as GraphicsPath;
                Brush signalBrush = obj[1] as Brush;
                Pen signalPen = obj[2] as Pen;
                g.FillPath(signalBrush, gp);
                g.DrawPath(signalPen, gp);
                signalPen.Dispose();
                signalBrush.Dispose();
                gp.Dispose();
            }
            //显示K线的最大和最小值
            if (this.dtAllMsg.Rows.Count > 0)
            {
                foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                {
                    foreach (CandleSeries cs in chartPanel.CandleSeriesList)
                    {
                        if (cs.MaxRecord != -1 && cs.MinRecord != -1)
                        {
                            //画K线的最大值
                            DataRow drMax = this.dtAllMsg.Rows[cs.MaxRecord];
                            double maxValue = Convert.ToDouble(drMax[cs.HighField]);
                            float scaleXMax = this.leftPixSpace + (cs.MaxRecord + 2 - firstVisibleRecord) * axisSpace - axisSpace / 2;
                            float scaleYMax = GetValueYPixel(chartPanel, maxValue);
                            SizeF maxSize = g.MeasureString(maxValue.ToString("0.00"), this.Font);
                            PointF maxP = new PointF();
                            if (scaleXMax < this.leftPixSpace + maxSize.Width)
                            {
                                maxP = new PointF(scaleXMax, scaleYMax + maxSize.Height / 2);
                            }
                            else if (scaleXMax > this.Width - this.rightPixSpace - maxSize.Width)
                            {
                                maxP = new PointF(scaleXMax - maxSize.Width, scaleYMax + maxSize.Height / 2);
                            }
                            else
                            {
                                if (scaleXMax < this.Width / 2)
                                {
                                    maxP = new PointF(scaleXMax - maxSize.Width, scaleYMax + maxSize.Height / 2);
                                }
                                else
                                {
                                    maxP = new PointF(scaleXMax, scaleYMax + maxSize.Height / 2);
                                }
                            }
                            g.DrawString(maxValue.ToString("0.00"), this.Font, Brushes.White, maxP);
                            g.DrawLine(Pens.White, scaleXMax, scaleYMax, maxP.X + maxSize.Width / 2, maxP.Y);
                            //画K线的最小值
                            DataRow drMin = this.dtAllMsg.Rows[cs.MinRecord];
                            double minValue = Convert.ToDouble(drMin[cs.LowField]);
                            SizeF minSize = g.MeasureString(minValue.ToString("0.00"), this.Font);
                            float scaleXMin = this.leftPixSpace + (cs.MinRecord + 2 - firstVisibleRecord) * axisSpace - axisSpace / 2;
                            float scaleYMin = GetValueYPixel(chartPanel, minValue);
                            PointF minP = new PointF();
                            if (scaleXMin < this.leftPixSpace + minSize.Width)
                            {
                                minP = new PointF(scaleXMin, scaleYMin - minSize.Height * 3 / 2);
                            }
                            else if (scaleXMin > this.Width - this.rightPixSpace - minSize.Width)
                            {
                                minP = new PointF(scaleXMin - minSize.Width, scaleYMin - minSize.Height * 3 / 2);
                            }
                            else
                            {
                                if (scaleXMin < this.Width / 2)
                                {
                                    minP = new PointF(scaleXMin - minSize.Width, scaleYMin - minSize.Height * 3 / 2);
                                }
                                else
                                {
                                    minP = new PointF(scaleXMin, scaleYMin - minSize.Height * 3 / 2);
                                }
                            }
                            g.DrawString(minValue.ToString("0.00"), this.Font, Brushes.White, minP);
                            g.DrawLine(Pens.White, scaleXMin, scaleYMin, minP.X + minSize.Width / 2, minP.Y + minSize.Height);
                        }
                    }
                }
                //画信息地雷的信息提示
                foreach (int panelID in infoBombDic.Keys)
                {
                    ChartPanel chartPanel = this.dicChartPanel[panelID];
                    string bombInfoText = infoBombDic[panelID];
                    Color bgColor = chartPanel.InfoBombTipColor;
                    Color strColor = chartPanel.InfoBombTipTextColor;
                    Pen tipBorderPen = new Pen(bgColor);
                    tipBorderPen.DashStyle = DashStyle.Dot;
                    Brush bgBrush = new SolidBrush(bgColor);
                    Brush strBrush = new SolidBrush(strColor);
                    SizeF sizeF = g.MeasureString(bombInfoText, new Font("宋体",10));
                    g.DrawRectangle(tipBorderPen, this.leftPixSpace + 1, (int)chartPanel.RectPanel.Y + chartPanel.TitleHeight + 5, (int)sizeF.Width, (int)sizeF.Height);
                    g.FillRectangle(bgBrush, new Rectangle(this.leftPixSpace + 2, 
                        (int)chartPanel.RectPanel.Y + (int)chartPanel.TitleHeight + 6, (int)sizeF.Width-1, (int)sizeF.Height - 1));
                    g.DrawString(bombInfoText, new Font("宋体", 10), strBrush, this.leftPixSpace + 2, chartPanel.RectPanel.Y + chartPanel.TitleHeight + 7);
                    tipBorderPen.Dispose();
                    bgBrush.Dispose();
                    strBrush.Dispose();
                }
            }
        }

        /// <summary>
        /// 绘制进度条
        /// </summary>
        public void DrawProcessBar(Graphics g)
        {
            int pieR = 100;
            g.SmoothingMode = SmoothingMode.HighQuality;
            Rectangle ellipseRect = new Rectangle(this.Width / 2 - pieR, this.Height / 2 - pieR, pieR * 2, pieR * 2);
            if (processBarValue > 0 && processBarValue <= 100)
            {
                Color processColor = Color.SkyBlue;
                StringBuilder sbProcess = new StringBuilder();
                if (processBarValue < 50)
                {
                    sbProcess.Append("Loading...\r\n");
                }
                else if (processBarValue >= 50 && processBarValue < 100)
                {
                    sbProcess.Append("Loading...\r\n");
                    processColor = Color.Red;
                }
                else
                {
                    sbProcess.Append("Complete\r\n");
                    processColor = Color.Teal;
                }
                sbProcess.Append(processBarValue.ToString() + "%");
                Brush brush = new SolidBrush(Color.FromArgb(90, processColor));
                Pen processPen = new Pen(processColor, 2);
                int startAngle = 270;
                int endAngle = Convert.ToInt32((double)processBarValue / 100 * 360);
                g.FillPie(brush, ellipseRect, startAngle, endAngle);
                Rectangle drawRectangle = new Rectangle(ellipseRect.X, ellipseRect.Y, ellipseRect.Width - 2, ellipseRect.Height - 2);
                if (processBarValue == 100)
                {
                    g.DrawEllipse(processPen, drawRectangle);
                    processBarValue = 0;
                }
                else
                {
                    g.DrawPie(processPen, drawRectangle, startAngle, endAngle + 1);
                }
                Font strFont = new Font("New Times Roman", 12, FontStyle.Bold | FontStyle.Italic);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                SizeF fontSize = g.MeasureString(sbProcess.ToString(), strFont);
                g.DrawString(sbProcess.ToString(), strFont, Brushes.White, new PointF(this.Width / 2, this.Height / 2 - fontSize.Height / 2), sf);
                brush.Dispose();
                processPen.Dispose();
                sf.Dispose();
            }
            g.SmoothingMode = SmoothingMode.Default;
        }

        /// <summary>
        /// 绘股指提示框
        /// </summary>
        /// <param name="g"></param>
        public void DrawValuePanel(Graphics g)
        {
            if (selectedObject != null)
            {
                if (vp_index > 0 && vp_index <= LastVisibleRecord - 1)
                {
                    Point mouseP = GetCrossHairPoint();
                    //获取鼠标位置面板的digit值
                    int digit = 2;
                    IntervalType curIntervalType = IntervalType.Day;
                    foreach (ChartPanel chartPanel in this.dicChartPanel.Values)
                    {
                        if (mouseP.Y >= chartPanel.RectPanel.Y && mouseP.Y <= chartPanel.RectPanel.Y + chartPanel.RectPanel.Height)
                        {
                            if (GetWorkSpaceY(chartPanel.PanelID) > 0)
                            {
                                digit = chartPanel.Digit;
                                curIntervalType = chartPanel.Interval;
                            }
                        }
                    }
                    DataRow dr = this.dtAllMsg.Rows[vp_index];
                    Point mp = new Point(GetCrossHairPoint().X + 10, GetCrossHairPoint().Y);
                    string timeKey = timekeyField + ":" + CommonClass.GetCalenderFormatTimeKey(dr[timekeyField].ToString(), curIntervalType);
                    Font tipFont = new Font("New Times Roman", 10, FontStyle.Bold);
                    SizeF timeKeySize = g.MeasureString(timeKey, tipFont);
                    double pWidth = 0;
                    double pHeight = 0;
                    List<double> wList = new List<double>();
                    StringBuilder sbValue = new StringBuilder();
                    sbValue.Append(timeKey + "\r\n");
                    Color pColor = Color.Turquoise;
                    //根据显示的字符获取框体的大小
                    if (selectedObject is CandleSeries)
                    {
                        CandleSeries cs = selectedObject as CandleSeries;
                        double open = 0;
                        Double.TryParse(dr[cs.OpenField].ToString(), out open);
                        double high = 0;
                        Double.TryParse(dr[cs.HighField].ToString(), out high);
                        double low = 0;
                        Double.TryParse(dr[cs.LowField].ToString(), out low);
                        double close = 0;
                        Double.TryParse(dr[cs.CloseField].ToString(), out close);
                        string strOpen = cs.OpenField + ":" + CommonClass.GetValueByDigit(open, digit);
                        sbValue.Append(strOpen + "\r\n");
                        SizeF openSize = g.MeasureString(strOpen, tipFont);
                        string strHigh = cs.HighField + ":" + CommonClass.GetValueByDigit(high, digit);
                        sbValue.Append(strHigh + "\r\n");
                        SizeF highSize = g.MeasureString(strHigh, tipFont);
                        string strLow = cs.LowField + ":" + CommonClass.GetValueByDigit(low, digit);
                        sbValue.Append(strLow + "\r\n");
                        SizeF lowSize = g.MeasureString(strLow, tipFont);
                        string strClose = cs.CloseField + ":" + CommonClass.GetValueByDigit(close, digit);
                        sbValue.Append(strClose);
                        SizeF closeSize = g.MeasureString(strClose, tipFont);
                        wList.AddRange(new double[] { timeKeySize.Width, openSize.Width, highSize.Width, lowSize.Width, closeSize.Width });
                        pWidth = CommonClass.GetHighValue(wList);
                        pHeight = timeKeySize.Height + openSize.Height + highSize.Height + lowSize.Height + closeSize.Height;
                    }
                    else if (selectedObject is HistogramSeries)
                    {
                        HistogramSeries hs = selectedObject as HistogramSeries;
                        double volumn = 0;
                        Double.TryParse(dr[hs.Field].ToString(), out volumn);
                        string strVolume = hs.Field + ":" + CommonClass.GetValueByDigit(volumn, digit);
                        sbValue.Append(strVolume);
                        SizeF volumeSize = g.MeasureString(strVolume, tipFont);
                        wList.AddRange(new double[] { timeKeySize.Width, volumeSize.Width });
                        pWidth = CommonClass.GetHighValue(wList);
                        pHeight = timeKeySize.Height + volumeSize.Height;
                        pColor = Color.Yellow;
                    }
                    else if (selectedObject is TrendLineSeries)
                    {
                        TrendLineSeries tls = selectedObject as TrendLineSeries;
                        double lineValue = 0;
                        Double.TryParse(dr[tls.Field].ToString(), out lineValue);
                        string strLine = tls.DisplayName != null ? tls.DisplayName + ":" + CommonClass.GetValueByDigit(lineValue, digit) : tls.Field + ":" + CommonClass.GetValueByDigit(lineValue, digit);
                        sbValue.Append(strLine);
                        SizeF lineSize = g.MeasureString(strLine, tipFont);
                        wList.AddRange(new double[] { timeKeySize.Width, lineSize.Width });
                        pWidth = CommonClass.GetHighValue(wList);
                        pHeight = timeKeySize.Height + lineSize.Height;
                        pColor = tls.Up_LineColor;
                    }
                    pWidth += 4;
                    pHeight += 1;
                    Rectangle rectP = new Rectangle(GetCrossHairPoint().X + 10, GetCrossHairPoint().Y, (int)pWidth, (int)pHeight);
                    Brush pbgBrush = new SolidBrush(Color.FromArgb(100, Color.Black));
                    Pen pPen = new Pen(pColor);
                    Brush pBrush = new SolidBrush(pColor);
                    g.FillRectangle(pbgBrush, rectP);
                    g.DrawRectangle(pPen, rectP);
                    g.DrawString(sbValue.ToString(), tipFont, pBrush, new PointF(GetCrossHairPoint().X + 10, GetCrossHairPoint().Y + 2));
                    pbgBrush.Dispose();
                    pPen.Dispose();
                    pBrush.Dispose();
                }
            }
        }

        /// <summary>
        /// 绘制整个图像
        /// </summary>
        public void DrawGraph()
        {
            PaintGraph(this.DisplayRectangle);
        }

        /// <summary>
        /// 绘制图像的一部分
        /// </summary>
        /// <param name="drawRectangle"></param>
        public void DrawGraph(Rectangle drawRectangle)
        {
            PaintGraph(drawRectangle);
        }

        /// <summary>
        /// 绘制图像到Image
        /// </summary>
        /// <returns></returns>
        public Image DrawToBitmap()
        {
            lock (refresh_lock)
            {
                Image image = new Bitmap(this.Width, this.Height);
                Graphics g = Graphics.FromImage(image);
                DrawBackGround(g);
                DrawTitle(g);
                DrawScale(g);
                if (processBarValue == 0)
                {
                    DrawSeries(g);
                    if (showCrossHair)
                    {
                        DrawCrossHair(g);
                    }
                    DrawValuePanel(g);
                }
                DrawProcessBar(g);
                return image;
            }
        }

        /// <summary>
        /// 绘制图象
        /// </summary>
        public void PaintGraph(Rectangle drawRectangle)
        {
            lock (refresh_lock)
            {
                BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
                BufferedGraphics myBuffer = currentContext.Allocate(this.CreateGraphics(), drawRectangle);
                Graphics g = myBuffer.Graphics;
                //画背景
                DrawBackGround(g);
                //画标题
                DrawTitle(g);
                //画坐标轴
                DrawScale(g);
                if (processBarValue == 0)
                {
                    //画线条
                    DrawSeries(g);
                    //画十字线
                    if (showCrossHair)
                    {
                        DrawCrossHair(g);
                    }
                    //画股指提示
                    DrawValuePanel(g);
                }
                //画进度条
                DrawProcessBar(g);
                myBuffer.Render();
                myBuffer.Dispose();
            }
        }
        #endregion

        #region 成员对象
        /// <summary>
        /// 公共类
        /// </summary>
        public class CommonClass
        {
            /// <summary>
            /// 获取Guid
            /// </summary>
            /// <returns></returns>
            public static string GetGuid()
            {
                return System.Guid.NewGuid().ToString();
            }

            public static double CalculateMovingAvg(int r, int cycle, String field, DataTable dataSource)
            {
                double sumValue = 0.0;
                if (r < cycle)
                {
                    for (int i = 0; i < r + 1; ++i)
                    {
                        sumValue += Convert.ToDouble(dataSource.Rows[i][field]);
                    }
                    return sumValue / ((r == 0) ? 1 : r);
                }
                else
                {
                    for (int i = r - cycle; i < r + 1; ++i)
                    {
                        sumValue += Convert.ToDouble(dataSource.Rows[i][field]);
                    }
                    return sumValue / cycle;
                }
            }

            /// <summary>
            /// 获取指定索引的移动平均值
            /// </summary>
            /// <param name="r"></param>
            /// <param name="field"></param>
            /// <param name="target"></param>
            /// <param name="curValue"></param>
            /// <returns></returns>
            public static double CalcuteSimpleMovingAvg(int r, int cycle,string field, string target, double curValue,DataTable dataSource)
            {
                double sumValue = 0;
                if (r == cycle - 1)
                {
                    for (int i = 0; i <= r - 1; i++)
                    {
                        sumValue += Convert.ToDouble(dataSource.Rows[i][target]);
                    }
                    sumValue += curValue;
                    return sumValue / cycle;
                }
                else if (r > cycle - 1)
                {
                    sumValue = Convert.ToDouble(dataSource.Rows[r - 1][field]) * cycle;
                    sumValue -= Convert.ToDouble(dataSource.Rows[r - cycle][target]);
                    sumValue += curValue;
                    return sumValue / cycle;
                }
                else
                {
                    return NULL;
                }
            }

            /// <summary>
            /// 获取指定索引的指数移动平均值
            /// </summary>
            /// <param name="field"></param>
            /// <param name="target"></param>
            /// <param name="cycle"></param>
            /// <param name="dataSource"></param>
            /// <param name="r"></param>
            /// <returns></returns>
            public static double CalculateExponentialMovingAvg(string field,string target, int cycle, DataTable dataSource,int r)
            {
                DataRow dr = dataSource.Rows[r];
                double closeValue = Convert.ToDouble(dr[target]);
                double lastEMA = 0;
                double newEmaValue = 0;
                if (r > 0)
                {
                    lastEMA = Convert.ToDouble(dataSource.Rows[r - 1][field]);
                    newEmaValue = (closeValue * 2 + lastEMA * (cycle - 1)) / (cycle + 1);
                }
                return newEmaValue;
            }

            /// <summary>
            /// 获取最新的面板的ID
            /// </summary>
            private static int panelID = 0;

            public static int GetPanelID()
            {
                return panelID++;
            }

            /// <summary>
            /// 根据DateTime获取Timekey
            /// </summary>
            /// <param name="dt"></param>
            /// <returns></returns>
            public static string GetTimeKey(DateTime dt)
            {
                string month = dt.Month.ToString().Length == 1 ? "0" + dt.Month.ToString() : dt.Month.ToString();
                string day = dt.Day.ToString().Length == 1 ? "0" + dt.Day.ToString() : dt.Day.ToString();
                string hour = dt.Hour.ToString().Length == 1 ? "0" + dt.Hour.ToString() : dt.Hour.ToString();
                string minute = dt.Minute.ToString().Length == 1 ? "0" + dt.Minute.ToString() : dt.Minute.ToString();
                string second = dt.Second.ToString().Length == 1 ? "0" + dt.Second.ToString() : dt.Second.ToString();
                return dt.Year + month + day + hour + minute + second;
            }

            /// <summary>
            /// 根据timeKey获取datetime
            /// </summary>
            /// <param name="timeKey"></param>
            /// <returns></returns>
            public static DateTime GetDateTimeByTimeKey(string timeKey)
            {
                int year = timeKey.Length >= 4 ? Convert.ToInt32(timeKey.Substring(0, 4)) : 1970;
                int month = timeKey.Length >= 6 ? Convert.ToInt32(timeKey.Substring(4, 2)) : 1;
                int day = timeKey.Length >= 8 ? Convert.ToInt32(timeKey.Substring(6, 2)) : 1;
                int hr = timeKey.Length >= 10 ? Convert.ToInt32(timeKey.Substring(8, 2)) : 0;
                int mn = timeKey.Length >= 12 ? Convert.ToInt32(timeKey.Substring(10, 2)) : 0;
                int sc = timeKey.Length >= 14 ? Convert.ToInt32(timeKey.Substring(12, 2)) : 0;
                DateTime dt = new DateTime(year, month, day, hr, mn, sc);
                return dt;
            }

            /// <summary>
            /// 根据日期类型和时间获取经过处理后的TimeKey
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public static string GetCalenderFormatTimeKey(string value, IntervalType interval)
            {
                string timeKey = string.Empty;
                switch (interval)
                {
                    case IntervalType.Year:
                        timeKey = value.Substring(0, 4);
                        break;
                    case IntervalType.Month:
                        timeKey = value.Substring(0, 4) + "/" + value.Substring(4, 2);
                        break;
                    case IntervalType.Week:
                    case IntervalType.Day:
                        timeKey = value.Substring(0, 4) + "/" + value.Substring(4, 2) + "/" + value.Substring(6, 2);
                        break;
                    case IntervalType.Minute:
                        timeKey = value.Substring(8, 2) + ":" + value.Substring(10, 2);
                        break;
                    case IntervalType.Second:
                        timeKey = value.Substring(8, 2) + ":" + value.Substring(10, 2) + ":" + value.Substring(12, 2);
                        break;
                }
                return timeKey;
            }

            /// <summary>
            /// 返回一组数据的最小值
            /// </summary>
            /// <param name="valueList"></param>
            /// <returns></returns>
            public static double GetLowValue(List<double> valueList)
            {
                double low = 0;
                for (int i = 0; i < valueList.Count; i++)
                {
                    if (i == 0)
                    {
                        low = valueList[i];
                    }
                    else
                    {
                        if (low > valueList[i])
                        {
                            low = valueList[i];
                        }
                    }
                }
                return low;
            }

            /// <summary>
            /// 返回一组数组的最大值
            /// </summary>
            /// <param name="valueList"></param>
            /// <returns></returns>
            public static double GetHighValue(List<double> valueList)
            {
                double high = 0;
                for (int i = 0; i < valueList.Count; i++)
                {
                    if (i == 0)
                    {
                        high = valueList[i];
                    }
                    else
                    {
                        if (high < valueList[i])
                        {
                            high = valueList[i];
                        }
                    }
                }
                return high;
            }

            /// <summary>
            /// 获取一个表中的数据的最小值的记录号
            /// </summary>
            /// <param name="dicValues"></param>
            /// <returns></returns>
            public static int GetLoweRecord(List<object[]> dicValues)
            {
                double low = 0;
                int index = -1;
                for (int i = 0; i < dicValues.Count; i++)
                {
                    int j = Convert.ToInt32(dicValues[i][0]);
                    double value = Convert.ToDouble(dicValues[i][1]);
                    if (i == 0)
                    {
                        index = j;
                        low = value;
                    }
                    else
                    {
                        if (low > value)
                        {
                            index = j;
                            low = value;
                        }
                    }
                }
                return index;
            }

            /// <summary>
            /// 获取一个表中的数据的最大值的记录号
            /// </summary>
            /// <param name="values"></param>
            /// <returns></returns>
            public static int GetHighRecord(List<object[]> dicValues)
            {
                double high = 0;
                int index = -1;
                for (int i = 0; i < dicValues.Count; i++)
                {
                    int j = Convert.ToInt32(dicValues[i][0]);
                    double value = Convert.ToDouble(dicValues[i][1]);
                    if (i == 0)
                    {
                        index = j;
                        high = value;
                    }
                    else
                    {
                        if (high < value)
                        {
                            high = value;
                            index = j;
                        }
                    }
                }
                return index;
            }

            /// <summary>
            /// 根据保留小数的位置将double型转化为string型
            /// </summary>
            /// <param name="value"></param>
            /// <param name="digit"></param>
            /// <returns></returns>
            public static string GetValueByDigit(double value, int digit)
            {
                if (digit > 0)
                {
                    StringBuilder sbFormat = new StringBuilder();
                    string strValue = value.ToString();
                    if (strValue.IndexOf(".") != -1)
                    {
                        sbFormat.Append(strValue.Substring(0, strValue.IndexOf(".") + 1));
                        for (int i = 0; i < digit; i++)
                        {
                            int pos = strValue.IndexOf(".") + (i + 1);
                            if (pos <= strValue.Length - 1)
                            {
                                sbFormat.Append(strValue.Substring(pos,1));
                            }
                            else
                            {
                                sbFormat.Append("0");
                            }
                        }
                    }
                    else
                    {
                        sbFormat.Append(strValue+".");
                        for (int i = 0; i < digit; i++)
                        {
                            sbFormat.Append("0");
                        }
                    }
                    return sbFormat.ToString();
                }
                return value.ToString();
            }

            /// <summary>
            /// 获取一个带圆弧角的矩形
            /// </summary>
            /// <param name="cornerRadius"></param>
            /// <param name="rect"></param>
            /// <returns></returns>
            public static GraphicsPath GetRoundRectangle(int cornerRadius, RectangleF rect)
            {
                GraphicsPath roundedRect = new GraphicsPath();
                roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
                roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
                roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
                roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
                roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
                roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
                roundedRect.CloseFigure();
                return roundedRect;
            }

            /// <summary>
            /// 计算坐标轴的函数
            /// </summary>
            /// <param name="XMin"></param>
            /// <param name="XMax"></param>
            /// <param name="N"></param>
            /// <param name="SMin"></param>
            /// <param name="Step"></param>
            /// <returns></returns>
            public static int GridScale(double XMin, double XMax, int N, ref double SMin, ref double Step, double m_fTick)
            {
                int iNegScl;
                int iNm1;
                double lfIniStep;
                double lfSclStep;
                double lfTmp;
                double lfSMax;
                int it;
                int i;
                int[] Steps = { 10, 12, 15, 16, 20, 25, 30, 40, 50, 60, 75, 80, 100, 120, 150 };
                int iNS = Steps.Length;
                if (XMin > XMax)
                {
                    lfTmp = XMin;
                    XMin = XMax;
                    XMax = lfTmp;
                }
                if (XMin == XMax)
                    XMax = XMin == 0.0 ? 1.0 : XMin + Math.Abs(XMin) / 10.0;
                if (XMax <= 0)
                {
                    iNegScl = 1;
                    lfTmp = XMin;
                    XMin = -XMax;
                    XMax = -lfTmp;
                }
                else
                    iNegScl = 0;
                if (N < 2)
                    N = 2;
                iNm1 = N - 1;
                for (it = 0; it < 3; it++)
                {
                    lfIniStep = (XMax - XMin) / iNm1;
                    lfSclStep = lfIniStep;

                    int pow10 = 0;

                    for (; lfSclStep < 10.0; lfSclStep *= 10.0) pow10--;
                    for (; lfSclStep > 100.0; lfSclStep /= 10.0) pow10++;
                    for (i = 0; i < iNS && lfSclStep > Steps[i]; i++) ;
                    do
                    {
                        Step = Steps[i] * Math.Pow(10, (double)pow10);

                        if (m_fTick != 0.0)
                        {
                            Step = Math.Floor(Step / m_fTick) * m_fTick;
                        }

                        SMin = Math.Floor(XMin / Step) * Step;
                        lfSMax = SMin + iNm1 * Step;
                        if (XMax <= lfSMax)
                        {
                            if (iNegScl == 1)
                                SMin = -lfSMax;
                            Step *= iNm1 / (N - 1);
                            return 1;
                        }
                        i++;
                    }
                    while (i < iNS);
                    iNm1 *= 2;
                }
                return 0;
            }
        }

        /// <summary>
        /// 面板属性设置
        /// </summary>
        public class ChartPanel
        {
            public ChartPanel()
            {
                infoBombField = CommonClass.GetGuid();
                panelBorder_Pen.DashStyle = DashStyle.Solid;
                grid_Pen.DashStyle = DashStyle.Dash;
            }

            ~ChartPanel()
            {
                if (bgBrush != null) { bgBrush.Dispose(); }
                if (xtip_Brush != null) { xtip_Brush.Dispose(); }
                if (xTipFont_Brush != null) { xTipFont_Brush.Dispose(); }
                if (xTipFont_Pen != null) { xTipFont_Pen.Dispose(); }
                if (leftyTip_Brush != null) { leftyTip_Brush.Dispose(); }
                if (leftyTipFont_Brush != null) { leftyTipFont_Brush.Dispose(); }
                if (leftTipFont_Pen != null) { leftTipFont_Pen.Dispose(); }
                if (rightyTip_Brush != null) { rightyTip_Brush.Dispose(); }
                if (rightyTipFont_Brush != null) { rightyTipFont_Brush.Dispose(); }
                if (panelBorder_Pen != null) { panelBorder_Pen.Dispose(); }
                if (titleFont_Brush != null) { titleFont_Brush.Dispose(); }
                if (grid_Pen != null) { grid_Pen.Dispose(); }
                if (coordinateXFont_Brush != null) { coordinateXFont_Brush.Dispose(); }
                if (leftYFont_Brush != null) { leftYFont_Brush.Dispose(); }
                if (rightYFont_Brush != null) { rightYFont_Brush.Dispose(); }
                if (rightyTipFont_Pen != null) { rightyTipFont_Pen.Dispose(); }
                if (xScalePen != null) { xScalePen.Dispose(); }
                if (leftScalePen != null) { leftScalePen.Dispose(); }
                if (rightScalePen != null) { rightScalePen.Dispose(); }
                if (crossHair_Pen != null) { crossHair_Pen.Dispose(); }
            }

            /// <summary>
            /// 十字线的画笔
            /// </summary>
            private Pen crossHair_Pen = new Pen(Color.White);

            public Pen CrossHair_Pen
            {
                get { return crossHair_Pen; }
                set { crossHair_Pen = value; }
            }

            /// <summary>
            /// 网格线的幅度
            /// </summary>
            private int gridInterval = 3;

            public int GridInterval
            {
                get { return gridInterval; }
                set { gridInterval = value; }
            }

            /// <summary>
            /// 信息地雷的字段
            /// </summary>
            private string infoBombField = string.Empty;

            public string InfoBombField
            {
                get { return infoBombField; }
                set { infoBombField = value; }
            }

            /// <summary>
            /// 信息地雷的颜色
            /// </summary>
            private Color infoBombColor = Color.White;

            public Color InfoBombColor
            {
                get { return infoBombColor; }
                set { infoBombColor = value; }
            }

            /// <summary>
            /// 信息地雷的选中色
            /// </summary>
            private Color infoBombSelectedColor = Color.FromArgb(255, 255, 153);

            public Color InfoBombSelectedColor
            {
                get { return infoBombSelectedColor; }
                set { infoBombSelectedColor = value; }
            }

            /// <summary>
            /// 信息地雷提示框的背景色
            /// </summary>
            private Color infoBombTipColor = Color.FromArgb(255, 255, 153);

            public Color InfoBombTipColor
            {
                get { return infoBombTipColor; }
                set { infoBombTipColor = value; }
            }

            /// <summary>
            /// 信息地雷提示框的文字颜色
            /// </summary>
            private Color infoBombTipTextColor = Color.Black;

            public Color InfoBombTipTextColor
            {
                get { return infoBombTipTextColor; }
                set { infoBombTipTextColor = value; }
            }

            /// <summary>
            /// 保存标记的集合
            /// </summary>
            private Dictionary<string, List<SignalSeries>> signalSeriesDic = new Dictionary<string, List<SignalSeries>>();

            public Dictionary<string, List<SignalSeries>> SignalSeriesDic
            {
                get { return signalSeriesDic; }
                set { signalSeriesDic = value; }
            }

            /// <summary>
            /// 绘制标题的集合
            /// </summary>
            private List<TitleField> titleFieldList = new List<TitleField>();

            public List<TitleField> TitleFieldList
            {
                get { return titleFieldList; }
                set { titleFieldList = value; }
            }

            /// <summary>
            /// 用户自定义标题
            /// </summary>
            private bool userDefinedTitle = false;

            public bool UserDefinedTitle
            {
                get { return userDefinedTitle; }
                set { userDefinedTitle = value; }
            }

            /// <summary>
            /// 面板显示数值保留小数的位数
            /// </summary>
            private int digit = 2;

            public int Digit
            {
                get { return digit; }
                set { digit = value; }
            }

            /// <summary>
            /// 面板的ID
            /// </summary>
            private int panelID;

            public int PanelID
            {
                get { return panelID; }
                set { panelID = value; }
            }

            /// <summary>
            /// 纵向比例
            /// </summary>
            private int verticalPercent = 0;

            public int VerticalPercent
            {
                get { return verticalPercent; }
                set { verticalPercent = value; }
            }

            /// <summary>
            /// Y轴的Tick
            /// </summary>
            private double yScaleTick = 0.01;

            public double YScaleTick
            {
                get { return yScaleTick; }
                set { yScaleTick = value; }
            }

            /// <summary>
            /// 卖点对应的字段
            /// </summary>
            private string sellSignalField = string.Empty;

            [Browsable(false)]
            public string SellSignalField
            {
                get { return sellSignalField; }
                set { sellSignalField = value; }
            }

            /// <summary>
            /// 面板的标题
            /// </summary>
            private string panelTitle = string.Empty;

            public string PanelTitle
            {
                get { return panelTitle; }
                set { panelTitle = value; }
            }

            /// <summary>
            /// 是否显示网格线
            /// </summary>
            private bool showGrid = true;

            public bool ShowGrid
            {
                get { return showGrid; }
                set { showGrid = value; }
            }

            /// <summary>
            /// 最大值
            /// </summary>
            private double maxValue;

            [Browsable(false)]
            public double MaxValue
            {
                get { return maxValue; }
                set { maxValue = value; }
            }

            /// <summary>
            /// 最小值
            /// </summary>
            private double minValue;

            [Browsable(false)]
            public double MinValue
            {
                get { return minValue; }
                set { minValue = value; }
            }

            /// <summary>
            /// 当前显示的线条的日期类型
            /// </summary>
            private IntervalType interval = IntervalType.Day;

            [Browsable(true)]
            public IntervalType Interval
            {
                get { return interval; }
                set { interval = value; }
            }

            /// <summary>
            /// 面板的矩形
            /// </summary>
            private Rectangle rectPanel = new Rectangle();

            public Rectangle RectPanel
            {
                get { return rectPanel; }
                set { rectPanel = value; }
            }

            /// <summary>
            /// K线柱的集合
            /// </summary>
            private List<CandleSeries> candleSeriesList = new List<CandleSeries>();

            public List<CandleSeries> CandleSeriesList
            {
                get { return candleSeriesList; }
                set { candleSeriesList = value; }
            }

            /// <summary>
            /// 柱状图的集合
            /// </summary>
            private List<HistogramSeries> historamSeriesList = new List<HistogramSeries>();

            public List<HistogramSeries> HistoramSeriesList
            {
                get { return historamSeriesList; }
                set { historamSeriesList = value; }
            }

            /// <summary>
            /// 线条的集合
            /// </summary>
            private List<TrendLineSeries> trendLineSeriesList = new List<TrendLineSeries>();

            public List<TrendLineSeries> TrendLineSeriesList
            {
                get { return trendLineSeriesList; }
                set { trendLineSeriesList = value; }
            }

            public void ClearPanel()
            {
                yScaleTick = 0.01;
                yScaleField.Clear();
                maxValue = 0.0;
                minValue = 0.0;
                candleSeriesList.Clear();
                historamSeriesList.Clear();
                trendLineSeriesList.Clear();
            }

            /// <summary>
            /// 计算Y轴所使用的字段
            /// </summary>
            private List<string> yScaleField = new List<string>();

            public List<string> YScaleField
            {
                get { return yScaleField; }
                set { yScaleField = value; }
            }

            /// <summary>
            /// 背景色刷
            /// </summary>
            private Brush bgBrush = new SolidBrush(Color.Black);

            public Brush BgBrush
            {
                get { return bgBrush; }
                set { bgBrush = value; }
            }

            /// <summary>
            /// X轴的线条画笔
            /// </summary>
            private Pen xScalePen = new Pen(Color.Red);

            public Pen XScalePen
            {
                get { return xScalePen; }
                set { xScalePen = value; }
            }

            /// <summary>
            /// 左侧Y轴的画笔
            /// </summary>
            private Pen leftScalePen = new Pen(Color.Red);

            public Pen LeftScalePen
            {
                get { return leftScalePen; }
                set { leftScalePen = value; }
            }

            /// <summary>
            /// 右侧Y轴的画笔
            /// </summary>
            private Pen rightScalePen = new Pen(Color.Red);

            public Pen RightScalePen
            {
                get { return rightScalePen; }
                set { rightScalePen = value; }
            }

            /// <summary>
            /// X轴提示框背景色的刷子
            /// </summary>
            private Brush xtip_Brush = new SolidBrush(Color.FromArgb(100, Color.Red));

            public Brush Xtip_Brush
            {
                get { return xtip_Brush; }
                set { xtip_Brush = value; }
            }

            /// <summary>
            /// X轴提示框文子色的画笔
            /// </summary>
            private Pen xTipFont_Pen = new Pen(Color.White);

            public Pen XTipFont_Pen
            {
                get { return xTipFont_Pen; }
                set { xTipFont_Pen = value; }
            }

            /// <summary>
            /// X轴提示框文字色的刷子
            /// </summary>
            private Brush xTipFont_Brush = new SolidBrush(Color.White);

            public Brush XTipFont_Brush
            {
                get { return xTipFont_Brush; }
                set { xTipFont_Brush = value; }
            }

            /// <summary>
            /// X轴提示框文字的字体
            /// </summary>
            private Font xTipFont = new Font("New Times Roman", 10, FontStyle.Bold);

            public Font XTipFont
            {
                get { return xTipFont; }
                set { xTipFont = value; }
            }

            /// <summary>
            /// 左侧Y轴提示框背景色的刷子
            /// </summary>
            /// </summary>
            private Brush leftyTip_Brush = new SolidBrush(Color.FromArgb(100, Color.Red));

            public Brush LeftyTip_Brush
            {
                get { return leftyTip_Brush; }
                set { leftyTip_Brush = value; }
            }

            /// <summary>
            /// 左侧Y轴提示框文字色的画笔
            /// </summary>
            private Pen leftTipFont_Pen = new Pen(Color.White);

            public Pen LeftTipFont_Pen
            {
                get { return leftTipFont_Pen; }
                set { leftTipFont_Pen = value; }
            }

            /// <summary>
            /// 左侧Y轴提示框文字色的刷子
            /// </summary>
            private Brush leftyTipFont_Brush = new SolidBrush(Color.White);

            public Brush LeftyTipFont_Brush
            {
                get { return leftyTipFont_Brush; }
                set { leftyTipFont_Brush = value; }
            }

            /// <summary>
            /// 左侧Y轴提示框文字的字体
            /// </summary>
            private Font leftyTipFont = new Font("New Times Roman", 10, FontStyle.Bold);

            public Font LeftyTipFont
            {
                get { return leftyTipFont; }
                set { leftyTipFont = value; }
            }

            /// <summary>
            /// 右侧Y轴提示框背景色的刷子
            /// </summary>
            /// </summary>
            private Brush rightyTip_Brush = new SolidBrush(Color.FromArgb(100, Color.Red));

            public Brush RightyTip_Brush
            {
                get { return rightyTip_Brush; }
                set { rightyTip_Brush = value; }
            }

            /// <summary>
            /// 右侧Y轴提示框文字色的刷子
            /// </summary>
            private Brush rightyTipFont_Brush = new SolidBrush(Color.White);

            public Brush RightyTipFont_Brush
            {
                get { return leftyTipFont_Brush; }
                set { leftyTipFont_Brush = value; }
            }

            /// <summary>
            /// 右侧Y轴提示框文字色的画笔
            /// </summary>
            private Pen rightyTipFont_Pen = new Pen(Color.White);

            public Pen RightyTipFont_Pen
            {
                get { return rightyTipFont_Pen; }
                set { rightyTipFont_Pen = value; }
            }

            /// <summary>
            /// 右侧Y轴提示框文字的字体
            /// </summary>
            private Font rightyTipFont = new Font("New Times Roman", 10, FontStyle.Bold);

            public Font RightyTipFont
            {
                get { return leftyTipFont; }
                set { leftyTipFont = value; }
            }

            /// <summary>
            /// 面板边线的画笔
            /// </summary>
            private Pen panelBorder_Pen = new Pen(Color.Red);

            public Pen PanelBorder_Pen
            {
                get { return panelBorder_Pen; }
                set { panelBorder_Pen = value; }
            }

            /// <summary>
            /// 标题的笔刷
            /// </summary>
            private Brush titleFont_Brush = new SolidBrush(Color.White);

            public Brush TitleFont_Brush
            {
                get { return titleFont_Brush; }
                set { titleFont_Brush = value; }
            }

            /// <summary>
            /// 标题的字体
            /// </summary>
            private Font titleFont = new Font("New Times Roman", 9);

            public Font TitleFont
            {
                get { return titleFont; }
                set { titleFont = value; }
            }

            /// <summary>
            /// 网格线的画笔
            /// </summary>
            private Pen grid_Pen = new Pen(Color.FromArgb(100, Color.Red));

            public Pen Grid_Pen
            {
                get { return grid_Pen; }
                set { grid_Pen = value; }
            }

            /// <summary>
            /// X轴文字的刷子
            /// </summary>
            private Brush coordinateXFont_Brush = new SolidBrush(Color.White);

            public Brush CoordinateXFont_Brush
            {
                get { return coordinateXFont_Brush; }
                set { coordinateXFont_Brush = value; }
            }

            /// <summary>
            /// X轴文字的字体
            /// </summary>
            private Font coordinateXFont = new Font("New Times Roman", 9);

            public Font CoordinateXFont
            {
                get { return coordinateXFont; }
                set { coordinateXFont = value; }
            }

            /// <summary>
            /// 左侧Y轴文字的刷子
            /// </summary>
            private Brush leftYFont_Brush = new SolidBrush(Color.White);

            public Brush LeftYFont_Brush
            {
                get { return leftYFont_Brush; }
                set { leftYFont_Brush = value; }
            }

            /// <summary>
            /// 左侧Y轴文字的字体
            /// </summary>
            private Font leftYFont = new Font("New Times Roman", 9);

            public Font LeftYFont
            {
                get { return leftYFont; }
                set { leftYFont = value; }
            }

            /// <summary>
            /// 右侧Y轴文字的刷子
            /// </summary>
            private Brush rightYFont_Brush = new SolidBrush(Color.White);

            public Brush RightYFont_Brush
            {
                get { return rightYFont_Brush; }
                set { rightYFont_Brush = value; }
            }

            /// <summary>
            /// 右侧Y轴文字的字体
            /// </summary>
            private Font rightYFont = new Font("New Times Roman", 9);

            public Font RightYFont
            {
                get { return rightYFont; }
                set { rightYFont = value; }
            }

            /// <summary>
            /// 标题的高度
            /// </summary>
            private float titleHeight = 30;

            [Browsable(true)]
            public float TitleHeight
            {
                get { return titleHeight; }
                set { titleHeight = value; }
            }


            /// <summary>
            /// X轴的间隙
            /// </summary>
            private int scaleX_Height = 25;

            public int ScaleX_Height
            {
                get { return scaleX_Height; }
                set { scaleX_Height = value; }
            }
        }

        /// <summary>
        /// K线柱的属性
        /// </summary>
        public class CandleSeries
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            public CandleSeries()
            {
            }

            /// <summary>
            /// 析构函数,释放对象
            /// </summary>
            ~CandleSeries()
            {
                if (this.upLine_Brush != null) upLine_Brush.Dispose();
                if (this.downLine_Brush != null) downLine_Brush.Dispose();
                if (this.middleLine_Pen != null) middleLine_Pen.Dispose();
                if (this.upLine_Pen != null) upLine_Pen.Dispose();
                if (this.upLine_TransparentBrush != null) upLine_TransparentBrush.Dispose();
                if (this.downLine_Pen != null) downLine_Pen.Dispose();
            }

            /// <summary>
            /// 买卖标识的字体
            /// </summary>
            private Font bsFont = new Font("宋体", 12, FontStyle.Bold);

            public Font BsFont
            {
                get { return bsFont; }
                set { bsFont = value; }
            }

            /// <summary>
            /// 买点标识色
            /// </summary>
            private Color buyColor = Color.Red;

            public Color BuyColor
            {
                get { return buyColor; }
                set { buyColor = value; }
            }

            /// <summary>
            /// 卖点标识色
            /// </summary>
            private Color sellColor = Color.SkyBlue;

            public Color SellColor
            {
                get { return sellColor; }
                set { sellColor = value; }
            }

            /// <summary>
            /// 买点的文字
            /// </summary>
            private string buyText = "B";

            public string BuyText
            {
                get { return buyText; }
                set { buyText = value; }
            }

            /// <summary>
            /// 卖点的文字
            /// </summary>
            private string sellText = "S";

            public string SellText
            {
                get { return sellText; }
                set { sellText = value; }
            }

            /// <summary>
            /// K线的名称
            /// </summary>
            private string candleName;

            public string CandleName
            {
                get { return candleName; }
                set { candleName = value; }
            }

            /// <summary>
            /// 买点对应的字段
            /// </summary>
            private string[] indBuySellField = new string[2];

            public string[] IndBuySellField
            {
                get { return indBuySellField; }
                set { indBuySellField = value; }
            }

            /// <summary>
            /// 最高价字段
            /// </summary>
            private string highField;

            public string HighField
            {
                get { return highField; }
                set { highField = value; }
            }

            /// <summary>
            /// 开盘价字段
            /// </summary>
            private string openField;

            public string OpenField
            {
                get { return openField; }
                set { openField = value; }
            }

            /// <summary>
            /// 收盘价字段
            /// </summary>
            private string closeField;

            public string CloseField
            {
                get { return closeField; }
                set { closeField = value; }
            }

            /// <summary>
            /// 最低价字段
            /// </summary>
            private string lowField;

            public string LowField
            {
                get { return lowField; }
                set { lowField = value; }
            }

            /// <summary>
            /// 是否被选中
            /// </summary>
            private bool hasSelect = false;

            public bool HasSelect
            {
                get { return hasSelect; }
                set { hasSelect = value; }
            }

            /// <summary>
            /// K线最大值对应的记录号
            /// </summary>
            private int maxRecord = 0;

            public int MaxRecord
            {
                get { return maxRecord; }
                set { maxRecord = value; }
            }

            /// <summary>
            ///  K线最小值对应的记录号
            /// </summary>
            private int minRecord = 0;

            public int MinRecord
            {
                get { return minRecord; }
                set { minRecord = value; }
            }

            /// <summary>
            /// 阳线刷
            /// </summary>
            private Brush upLine_Brush;

            public Brush UpLine_Brush
            {
                get { return upLine_Brush; }
                set { upLine_Brush = value; }
            }

            /// <summary>
            /// 阴线刷
            /// </summary>
            private Brush downLine_Brush;

            public Brush DownLine_Brush
            {
                get { return downLine_Brush; }
                set { downLine_Brush = value; }
            }

            /// <summary>
            /// 中线的画笔
            /// </summary>
            private Pen middleLine_Pen;

            public Pen MiddleLine_Pen
            {
                get { return middleLine_Pen; }
                set { middleLine_Pen = value; }
            }

            /// <summary>
            /// 阳线笔
            /// </summary>
            private Pen upLine_Pen;

            public Pen UpLine_Pen
            {
                get { return upLine_Pen; }
                set { upLine_Pen = value; }
            }

            /// <summary>
            /// 阴线笔
            /// </summary>
            private Pen downLine_Pen;

            public Pen DownLine_Pen
            {
                get { return downLine_Pen; }
                set { downLine_Pen = value; }
            }

            /// <summary>
            /// 阳线透明刷
            /// </summary>
            private Brush upLine_TransparentBrush;

            public Brush UpLine_TransparentBrush
            {
                get { return upLine_TransparentBrush; }
                set { upLine_TransparentBrush = value; }
            }

            /// <summary>
            /// 阳线颜色
            /// </summary>
            private Color up_Color;

            public Color Up_Color
            {
                get { return up_Color; }
                set
                {
                    up_Color = value;
                    if (upLine_Brush != null)
                    {
                        upLine_Brush.Dispose();
                    }
                    upLine_Brush = new SolidBrush(value);
                    if (upLine_Pen != null)
                    {
                        upLine_Pen.Dispose();
                    }
                    upLine_Pen = new Pen(value);
                    if (upLine_TransparentBrush != null)
                    {
                        upLine_TransparentBrush.Dispose();
                    }
                    upLine_TransparentBrush = new SolidBrush(Color.FromArgb(100, value));
                }
            }

            /// <summary>
            /// 中线的颜色
            /// </summary>
            private Color middle_Color = Color.White;

            public Color Middle_Color
            {
                get { return middle_Color; }
                set
                {
                    middle_Color = value;
                    if (middleLine_Pen != null)
                    {
                        middleLine_Pen.Dispose();
                    }
                    middleLine_Pen = null;
                    if (value != Color.Empty)
                    {
                        middleLine_Pen = new Pen(value);
                    }
                }
            }

            /// <summary>
            /// 阴线的颜色
            /// </summary>
            private Color down_Color;

            public Color Down_Color
            {
                get { return down_Color; }
                set
                {
                    down_Color = value;
                    if (downLine_Brush != null)
                    {
                        downLine_Brush.Dispose();
                    }
                    downLine_Brush = new SolidBrush(value);
                    if (downLine_Pen != null)
                    {
                        downLine_Pen.Dispose();
                    }
                    downLine_Pen = new Pen(value);
                }
            }

            /// <summary>
            /// 开盘价的标题色
            /// </summary>
            private Color openTitleColor = Color.White;

            public Color OpenTitleColor
            {
                get { return openTitleColor; }
                set { openTitleColor = value; }
            }

            /// <summary>
            /// 最高价的标题色
            /// </summary>
            private Color highTitleColor = Color.Red;

            public Color HighTitleColor
            {
                get { return highTitleColor; }
                set { highTitleColor = value; }
            }

            /// <summary>
            /// 最低价的标题色
            /// </summary>
            private Color lowTitleColor = Color.Orange;

            public Color LowTitleColor
            {
                get { return lowTitleColor; }
                set { lowTitleColor = value; }
            }

            /// <summary>
            /// 收盘价的标题色
            /// </summary>
            private Color closeTitleColor = Color.Yellow;

            public Color CloseTitleColor
            {
                get { return closeTitleColor; }
                set { closeTitleColor = value; }
            }

            /// <summary>
            /// 是否在标题上显示字段
            /// </summary>
            private bool displayTitleField = true;

            public bool DisplayTitleField
            {
                get { return displayTitleField; }
                set { displayTitleField = value; }
            }
        }

        /// <summary>
        /// 柱状图的属性
        /// </summary>
        public class HistogramSeries
        {
            public HistogramSeries()
            {
            }

            /// <summary>
            /// 析构函数,释放对象
            /// </summary>
            ~HistogramSeries()
            {
                if (up_lineBrush != null)
                {
                    up_lineBrush.Dispose();
                }
                if (down_lineBrush != null)
                {
                    down_lineBrush.Dispose();
                }
                if (up_TransparentBrush != null)
                {
                    up_TransparentBrush.Dispose();
                }
                if (up_Pen != null)
                {
                    up_Pen.Dispose();
                }
                if (down_Pen != null)
                {
                    down_Pen.Dispose();
                }
            }

            /// <summary>
            /// 是否被选中
            /// </summary>
            private bool hasSelect = false;

            public bool HasSelect
            {
                get { return hasSelect; }
                set { hasSelect = value; }
            }

            /// <summary>
            /// 字段名
            /// </summary>
            private string field;

            public string Field
            {
                get { return field; }
                set { field = value; }
            }

            /// <summary>
            /// 对应的K线名,设置后可以显示阴阳线
            /// </summary>
            private string relateCandleName;

            public string RelateCandleName
            {
                get { return relateCandleName; }
                set { relateCandleName = value; }
            }

            /// <summary>
            /// 透明的阳线画刷
            /// </summary>
            private Brush up_TransparentBrush;

            public Brush Up_TransparentBrush
            {
                get { return up_TransparentBrush; }
                set { up_TransparentBrush = value; }
            }

            /// <summary>
            /// 保存的阳线画笔
            /// </summary>
            private Pen up_Pen;

            public Pen Up_Pen
            {
                get { return up_Pen; }
                set { up_Pen = value; }
            }

            /// <summary>
            /// 保存的阴线画笔
            /// </summary>
            private Pen down_Pen;

            public Pen Down_Pen
            {
                get { return down_Pen; }
                set { down_Pen = value; }
            }

            /// <summary>
            /// 保存的阳线画刷
            /// </summary>
            private Brush up_lineBrush;

            public Brush Up_LineBrush
            {
                get { return up_lineBrush; }
                set { up_lineBrush = value; }
            }

            /// <summary>
            /// 保存的阴线画刷
            /// </summary>
            private Brush down_lineBrush;

            public Brush Down_lineBrush
            {
                get { return down_lineBrush; }
                set { down_lineBrush = value; }
            }

            /// <summary>
            /// 阳线的颜色
            /// </summary>
            private Color up_lineColor;

            public Color Up_LineColor
            {
                get { return up_lineColor; }
                set
                {
                    up_lineColor = value;
                    if (up_lineBrush != null)
                    {
                        up_lineBrush.Dispose();
                    }
                    up_lineBrush = new SolidBrush(value);
                    if (up_TransparentBrush != null)
                    {
                        up_TransparentBrush.Dispose();
                    }
                    up_TransparentBrush = new SolidBrush(Color.FromArgb(100, value));
                    if (up_Pen != null)
                    {
                        up_Pen.Dispose();
                    }
                    up_Pen = new Pen(value);
                }
            }

            /// <summary>
            /// 阴线的颜色
            /// </summary>
            private Color down_lineColor;

            public Color Down_lineColor
            {
                get { return down_lineColor; }
                set
                {
                    down_lineColor = value;
                    if (down_lineBrush != null)
                    {
                        down_lineBrush.Dispose();
                    }
                    Down_lineBrush = new SolidBrush(value);
                    if (down_Pen != null)
                    {
                        down_Pen.Dispose();
                    }
                    down_Pen = new Pen(value);
                }
            }

            /// <summary>
            /// 显示的标题名称
            /// </summary>
            private string displayName = string.Empty;

            public string DisplayName
            {
                get { return displayName; }
                set { displayName = value; }
            }

            /// <summary>
            /// 使用线的样式
            /// </summary>
            private bool lineStyle = false;

            public bool LineStyle
            {
                get { return lineStyle; }
                set { lineStyle = value; }
            }

            /// <summary>
            /// 线的宽度
            /// </summary>
            private int lineWidth = 1;

            public int LineWidth
            {
                get { return lineWidth; }
                set { lineWidth = value; }
            }
        }

        /// <summary>
        /// 线条的属性
        /// </summary>
        public class TrendLineSeries
        {
            public TrendLineSeries()
            {
            }

            /// <summary>
            /// 析构函数,释放对象
            /// </summary>
            ~TrendLineSeries()
            {
                if (this.up_linePen != null)
                {
                    this.up_linePen.Dispose();
                }
                if (this.up_lineBrush != null)
                {
                    this.up_lineBrush.Dispose();
                }
                if (this.transParentLineBrush != null)
                {
                    this.transParentLineBrush.Dispose();
                }
                if (this.up_linePen != null)
                {
                    this.up_linePen.Dispose();
                }
                if (this.down_lineBrush != null)
                {
                    this.down_lineBrush.Dispose();
                }
            }

            /// <summary>
            /// 是否被选中
            /// </summary>
            private bool hasSelect = false;

            public bool HasSelect
            {
                get { return hasSelect; }
                set { hasSelect = value; }
            }

            /// <summary>
            /// 字段
            /// </summary>
            private string field = string.Empty;

            public string Field
            {
                get { return field; }
                set { field = value; }
            }

            /// <summary>
            /// 如果设置了该字段,则标题显示时使用该字段
            /// </summary>
            private string displayName = string.Empty;

            public string DisplayName
            {
                get { return displayName; }
                set { displayName = value; }
            }

            /// <summary>
            /// 保存的画笔
            /// </summary>
            private Pen up_linePen;

            public Pen Up_LinePen
            {
                get { return up_linePen; }
                set { up_linePen = value; }
            }

            /// <summary>
            /// 保存的画刷
            /// </summary>
            private Brush up_lineBrush;

            public Brush Up_LineBrush
            {
                get { return up_lineBrush; }
                set { up_lineBrush = value; }
            }

            /// <summary>
            /// 线的颜色
            /// </summary>
            private Color up_lineColor;

            public Color Up_LineColor
            {
                get { return up_lineColor; }
                set
                {
                    up_lineColor = value;
                    if (up_linePen != null)
                    {
                        up_linePen.Dispose();
                    }
                    up_linePen = new Pen(value);
                    if (up_lineBrush != null)
                    {
                        up_lineBrush.Dispose();
                    }
                    up_lineBrush = new SolidBrush(value);
                    if (transParentLineBrush != null)
                    {
                        transParentLineBrush.Dispose();
                    }
                    transParentLineBrush = new SolidBrush(Color.FromArgb(100, value));
                }
            }

            /// <summary>
            /// 阴线的画笔
            /// </summary>
            private Pen down_linePen = new Pen(Color.Yellow);

            public Pen Down_linePen
            {
                get { return down_linePen; }
                set { down_linePen = value; }
            }

            /// <summary>
            /// 阴线的笔刷
            /// </summary>
            private Brush down_lineBrush;

            public Brush Down_lineBrush
            {
                get { return down_lineBrush; }
                set { down_lineBrush = value; }
            }

            /// <summary>
            /// 阴线的颜色
            /// </summary>
            private Color down_LineColor;

            public Color Down_LineColor
            {
                get { return down_LineColor; }
                set
                {
                    down_LineColor = value;
                    if (down_linePen != null)
                    {
                        down_linePen.Dispose();
                    }
                    down_linePen = new Pen(value);
                    if (down_lineBrush != null)
                    {
                        down_lineBrush.Dispose();
                    }
                    down_lineBrush = new SolidBrush(value);
                }
            }

            /// <summary>
            /// 透明色画刷
            /// </summary>
            private Brush transParentLineBrush;

            public Brush TransParentLineBrush
            {
                get { return transParentLineBrush; }
                set { transParentLineBrush = value; }
            }
        }

        /// <summary>
        /// 标记的属性
        /// </summary>
        public class SignalSeries
        {
            public SignalSeries(double value, SignalType st, Color stColor,bool canDrag)
            {
                this.value = value;
                this.signal = st;
                this.signalColor = stColor;
                this.canDrag = canDrag;
            }

            /// <summary>
            /// 标记的类型
            /// </summary>
            private SignalType signal = SignalType.UpArrow;

            public SignalType Signal
            {
                get { return signal; }
                set { signal = value; }
            }

            /// <summary>
            /// 标记的值
            /// </summary>
            private double value;

            public double Value
            {
                get { return this.value; }
                set { this.value = value; }
            }

            private bool canDrag = false;

            public bool CanDrag
            {
                get { return canDrag; }
                set { canDrag = value; }
            }

            /// <summary>
            /// 标记的颜色
            /// </summary>
            private Color signalColor = Color.Red;

            public Color SignalColor
            {
                get { return signalColor; }
                set { signalColor = value; }
            }

            /// <summary>
            /// 根据类型获取GraphicsPath.
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="width"></param>
            /// <returns></returns>
            public GraphicsPath GetGPByType(float x, float y, int width)
            {
                if (width > 10)
                {
                    width = 14;
                }
                GraphicsPath gp = new GraphicsPath();
                switch (signal)
                {
                    case SignalType.UpArrow:
                        gp.AddLine(x, y, x + width / 2, y + width);
                        gp.AddLine(x + width / 2, y + width, x + width / 4, y + width);
                        gp.AddLine(x + width / 4, y + width, x + width / 4, y + width * 3 / 2);
                        gp.AddLine(x + width / 4, y + width * 3 / 2, x - width / 4, y + width * 3 / 2);
                        gp.AddLine(x - width / 4, y + width * 3 / 2, x - width / 4, y + width);
                        gp.AddLine(x - width / 4, y + width, x - width / 2, y + width);
                        gp.AddLine(x - width / 2, y + width, x, y);
                        gp.CloseFigure();
                        break;
                    case SignalType.DownArrow:
                        gp.AddLine(x, y, x + width / 2, y - width);
                        gp.AddLine(x + width / 2, y - width, x + width / 4, y - width);
                        gp.AddLine(x + width / 4, y - width, x + width / 4, y - width * 3 / 2);
                        gp.AddLine(x + width / 4, y - width * 3 / 2, x - width / 4, y - width * 3 / 2);
                        gp.AddLine(x - width / 4, y - width * 3 / 2, x - width / 4, y - width);
                        gp.AddLine(x - width / 4, y - width, x - width / 2, y - width);
                        gp.AddLine(x - width / 2, y - width, x, y);
                        gp.CloseFigure();
                        break;
                    case SignalType.UpArrowWithOutTail:
                        gp.AddLine(x, y, x + width / 2, y + width);
                        gp.AddLine(x + width / 2, y + width, x - width / 2, y + width);
                        gp.AddLine(x - width / 2, y + width, x, y);
                        gp.CloseFigure();
                        break;
                    case SignalType.DownArrowWithOutTail:
                        gp.AddLine(x, y, x + width / 2, y - width);
                        gp.AddLine(x + width / 2, y - width, x - width / 2, y - width);
                        gp.AddLine(x - width / 2, y - width, x, y);
                        gp.CloseFigure();
                        break;
                    case SignalType.LeftArrow:
                        gp.AddLine(x + width / 2, y, x - width / 2, y - width / 2);
                        gp.AddLine(x - width / 2, y - width / 2, x - width / 2, y + width / 2);
                        gp.AddLine(x - width / 2, y + width / 2, x + width / 2, y);
                        gp.CloseFigure();
                        break;
                    case SignalType.RightArrow:
                        gp.AddLine(x - width / 2, y, x + width / 2, y - width / 2);
                        gp.AddLine(x + width / 2, y - width / 2, x + width / 2, y + width / 2);
                        gp.AddLine(x + width / 2, y + width / 2, x - width / 2, y);
                        gp.CloseFigure();
                        break;
                }
                return gp;
            }
        }

        public class IndicatorMovingAverage
        {
             public IndicatorMovingAverage()
            {

            }

            /// <summary>
            /// 目标字段
            /// </summary>
            private string target = string.Empty;

            public string Target
            {
                get { return target; }
                set { target = value; }
            }

            /// <summary>
            /// 周期
            /// </summary>
            private int cycle = 5;

            public int Cycle
            {
                get { return cycle; }
                set { cycle = value; }
            }

            /// <summary>
            /// 趋势线对象
            /// </summary>
            private TrendLineSeries trendLineSeries = null;

            public TrendLineSeries TrendLineSeries
            {
                get { return trendLineSeries; }
                set { trendLineSeries = value; }
            }

            /// <summary>
            /// 数据源
            /// </summary>
            private DataTable dataSource;

            public DataTable DataSource
            {
                get { return dataSource; }
                set { dataSource = value; }
            }

            /// <summary>
            /// 计算简单移动平均线的值
            /// </summary>
            /// <param name="field"></param>
            /// <param name="n"></param>
            /// <param name="dt"></param>
            /// <returns></returns>
            public double Calculate(int r)
            {
                if (dataSource.Columns.Contains(trendLineSeries.Field) && dataSource.Columns.Contains(target))
                {
                    return CommonClass.CalculateMovingAvg(r, cycle, trendLineSeries.Field, dataSource);
                }
                return NULL;
            }
        }

        /// <summary>
        /// 平滑移动平均线(SMA)
        /// </summary>
        /// 
        public class IndicatorSimpleMovingAverage
        {
            public IndicatorSimpleMovingAverage()
            {

            }

            /// <summary>
            /// 目标字段
            /// </summary>
            private string target = string.Empty;

            public string Target
            {
                get { return target; }
                set { target = value; }
            }

            /// <summary>
            /// 周期
            /// </summary>
            private int cycle = 5;

            public int Cycle
            {
                get { return cycle; }
                set { cycle = value; }
            }

            /// <summary>
            /// 趋势线对象
            /// </summary>
            private TrendLineSeries trendLineSeries = null;

            public TrendLineSeries TrendLineSeries
            {
                get { return trendLineSeries; }
                set { trendLineSeries = value; }
            }

            /// <summary>
            /// 数据源
            /// </summary>
            private DataTable dataSource;

            public DataTable DataSource
            {
                get { return dataSource; }
                set { dataSource = value; }
            }

            /// <summary>
            /// 计算简单移动平均线的值
            /// </summary>
            /// <param name="field"></param>
            /// <param name="n"></param>
            /// <param name="dt"></param>
            /// <returns></returns>
            public double Calculate(int r)
            {
                if (dataSource.Columns.Contains(trendLineSeries.Field) && dataSource.Columns.Contains(target))
                {
                    DataRow dr = dataSource.Rows[r];
                    double curValue = Convert.ToDouble(dataSource.Rows[r][target]);
                    return CommonClass.CalcuteSimpleMovingAvg(r, cycle, trendLineSeries.Field, target, curValue, dataSource);
                }
                return NULL;
            }
        }

        /// <summary>
        /// 指数移动平均线(EMA)
        /// </summary>
        public class IndicatorExponentialMovingAverage
        {
            public IndicatorExponentialMovingAverage()
            {

            }

            /// <summary>
            /// 目标字段
            /// </summary>
            private string target = string.Empty;

            public string Target
            {
                get { return target; }
                set { target = value; }
            }

            /// <summary>
            /// 周期
            /// </summary>
            private int cycle = 5;

            public int Cycle
            {
                get { return cycle; }
                set { cycle = value; }
            }

            /// <summary>
            /// 趋势线对象
            /// </summary>
            private TrendLineSeries trendLineSeries = null;

            public TrendLineSeries TrendLineSeries
            {
                get { return trendLineSeries; }
                set { trendLineSeries = value; }
            }

            /// <summary>
            /// 数据源
            /// </summary>
            private DataTable dataSource;

            public DataTable DataSource
            {
                get { return dataSource; }
                set { dataSource = value; }
            }

            /// <summary>
            /// 计算指数平均数指标的值
            /// </summary>
            /// <param name="field"></param>
            /// <param name="target"></param>
            /// <param name="n"></param>
            /// <param name="r"></param>
            /// <returns></returns>
            public double Calculate(int r)
            {
                return CommonClass.CalculateExponentialMovingAvg(trendLineSeries.Field, target, cycle, dataSource, r);
            }
        }

        /// <summary>
        /// 随机指标(KDJ)
        /// </summary>
        public class IndicatorStochasticOscillator
        {
            public IndicatorStochasticOscillator()
            {
            }

            /// <summary>
            /// K值趋势线
            /// </summary>
            private TrendLineSeries tlsK = null;

            public TrendLineSeries TlsK
            {
                get { return tlsK; }
                set { tlsK = value; }
            }

            /// <summary>
            /// D值趋势线
            /// </summary>
            private TrendLineSeries tlsD = null;

            public TrendLineSeries TlsD
            {
                get { return tlsD; }
                set { tlsD = value; }
            }

            /// <summary>
            /// J值趋势线
            /// </summary>
            private TrendLineSeries tlsJ = null;

            public TrendLineSeries TlsJ
            {
                get { return tlsJ; }
                set { tlsJ = value; }
            }

            /// <summary>
            /// 数据源
            /// </summary>
            private DataTable dataSource;

            public DataTable DataSource
            {
                get { return dataSource; }
                set { dataSource = value; }
            }

            /// <summary>
            /// K值周期
            /// </summary>
            private int kPeriods = 9;

            public int KPeriods
            {
              get { return kPeriods; }
              set { kPeriods = value; }
            }

            /// <summary>
            /// 慢速K值周期
            /// </summary>
            private int kSlowing = 3;

            public int KSlowing
            {
                get { return kSlowing; }
                set { kSlowing = value; }
            }

            /// <summary>
            /// D值周期
            /// </summary>
            private int dPeriods = 9;

            public int DPeriods
            {
                get { return dPeriods; }
                set { dPeriods = value; }
            }

            /// <summary>
            /// 收盘价字段
            /// </summary>
            private string close = string.Empty;

            public string Close
            {
                get { return close; }
                set { close = value; }
            }

            /// <summary>
            /// 最高价字段
            /// </summary>
            private string high = string.Empty;

            public string High
            {
                get { return high; }
                set { high = value; }
            }

            /// <summary>
            /// 最低价字段
            /// </summary>
            private string low = string.Empty;

            public string Low
            {
                get { return low; }
                set { low = value; }
            }

            /// <summary>
            /// 获取指定索引的K,D,J的值
            /// K=昨日K*2/3+RSV/3;
            /// D=昨日D*2/3+K/3;
            /// J=3*K-2*D
            /// </summary>
            /// <param name="panelID"></param>
            /// <returns></returns>
            public double[] Calculate(int r)
            {
                double k = 100;
                double d = 100;
                if (r > 0)
                {
                    double lastK = Convert.ToDouble(dataSource.Rows[r - 1][tlsK.Field]);
                    double lastD = Convert.ToDouble(dataSource.Rows[r - 1][tlsD.Field]);
                    k = lastK * 2 / 3 + RSV(r) / 3;
                    d = lastD * 2 / 3 + k / 3;
                }
                double j = 3 * k - 2 * d;
                return new double[] { k,d,j};
            }

            /// <summary>
            /// 获取未成熟随机指标值
            /// </summary>
            /// <param name="interval"></param>
            /// <param name="rowIndex"></param>
            /// <param name="field"></param>
            /// <returns></returns>
            public double RSV(int r)
            {
                int endIndex = r - (kPeriods-1);
                if (endIndex < 0)
                {
                    endIndex = 0;
                }
                double currentClose = Convert.ToDouble(dataSource.Rows[r][close]);
                List<double> highList = new List<double>();
                List<double> lowList = new List<double>();
                for (int i = r; i >= endIndex; i--)
                {
                    double h = Convert.ToDouble(dataSource.Rows[i][high]);
                    double l = Convert.ToDouble(dataSource.Rows[i][low]);
                    highList.Add(h);
                    lowList.Add(l);
                }
                double nHigh = CommonClass.GetHighValue(highList);
                double nLow = CommonClass.GetLowValue(lowList);
                return (currentClose - nLow) / (nHigh - nLow) * 100;
            }
        }

        /// <summary>
        /// 指数平滑异同移动平均线(MACD)
        /// </summary>
        public class IndicatorMACD
        {
            public IndicatorMACD()
            {

            }

            /// <summary>
            /// MACD柱状图
            /// </summary>
            private HistogramSeries hsMACD = null;

            public HistogramSeries HsMACD
            {
                get { return hsMACD; }
                set { hsMACD = value; }
            }

            /// <summary>
            /// Diff线
            /// </summary>
            private TrendLineSeries tlsDiff = null;

            public TrendLineSeries TlsDiff
            {
              get { return tlsDiff; }
              set { tlsDiff = value; }
            }

            /// <summary>
            /// Dea线
            /// </summary>
            private TrendLineSeries tlsDea = null;

            public TrendLineSeries TlsDea
            {
                get { return tlsDea; }
                set { tlsDea = value; }
            }

            /// <summary>
            /// 长周期
            /// </summary>
            private int longCycle = 26;

            public int LongCycle
            {
                get { return longCycle; }
                set { longCycle = value; }
            }

            /// <summary>
            /// 短周期
            /// </summary>
            private int shortCycle = 12;

            public int ShortCycle
            {
                get { return shortCycle; }
                set { shortCycle = value; }
            }

            /// <summary>
            /// 标记周期
            /// </summary>
            private int signalPeriods = 9;

            public int SignalPeriods
            {
                get { return signalPeriods; }
                set { signalPeriods = value; }
            }

            /// <summary>
            /// 收盘价字段
            /// </summary>
            private string close = string.Empty;

            public string Close
            {
                get { return close; }
                set { close = value; }
            }

            /// <summary>
            /// 长周期EMA字段
            /// </summary>
            private string longCycleEMA = CommonClass.GetGuid();

            public string LongCycleEMA
            {
                get { return longCycleEMA; }
                set { longCycleEMA = value; }
            }

            /// <summary>
            /// 短周期EMA字段
            /// </summary>
            private string shortCycleEMA = CommonClass.GetGuid();

            public string ShortCycleEMA
            {
                get { return shortCycleEMA; }
                set { shortCycleEMA = value; }
            }

            /// <summary>
            /// 数据源
            /// </summary>
            private DataTable dataSource = null;

            public DataTable DataSource
            {
                get { return dataSource; }
                set { dataSource = value; }
            }

            /// <summary>
            /// 根据指定索引的MACD,DIFF,DEA的值
            /// DIFF=EMA(C,ShortCycle)-EMA(C,LongCycle);
            /// DEA=DIFF*0.2-昨日DEA*0.8
            /// MACD=2*(DIFF-DEA)
            /// </summary>
            /// <param name="r"></param>
            /// <returns></returns>
            public double[] Calulate(int r)
            {
                double macd = 0;
                double diff = 0;
                double dea = 0;
                if (r > 1)
                {
                    DataRow dr = dataSource.Rows[r];
                    double longEMA = Convert.ToDouble(dr[longCycleEMA]);
                    double shortEMA = Convert.ToDouble(dr[shortCycleEMA]);
                    diff = shortEMA - longEMA;
                    double lastDea = Convert.ToDouble(dataSource.Rows[r - 1][tlsDea.Field]);
                    dea = diff * 0.2 + lastDea * 0.8;
                    macd = 2 * (diff - dea);
                }
                return new double[] { macd,diff,dea};
            }
        }

        /// <summary>
        /// 布林带
        /// </summary>
        public class IndicatorBollingerBands
        {
            public IndicatorBollingerBands()
            {

            }

            /// <summary>
            /// 中间的布林线
            /// </summary>
            private TrendLineSeries tlsM = null;

            public TrendLineSeries TlsM
            {
                get { return tlsM; }
                set { tlsM = value; }
            }

            /// <summary>
            /// 上边的布林线
            /// </summary>
            private TrendLineSeries tlsU = null;

            public TrendLineSeries TlsU
            {
                get { return tlsU; }
                set { tlsU = value; }
            }

            /// <summary>
            /// 下边的布林线
            /// </summary>
            private TrendLineSeries tlsD = null;

            public TrendLineSeries TlsD
            {
                get { return tlsD; }
                set { tlsD = value; }
            }

            /// <summary>
            /// 收盘价字段
            /// </summary>
            private string close = string.Empty;

            public string Close
            {
                get { return close; }
                set { close = value; }
            }

            /// <summary>
            /// 周期
            /// </summary>
            private int periods = 20;

            public int Periods
            {
                get { return periods; }
                set { periods = value; }
            }

            /// <summary>
            /// 标准差
            /// </summary>
            private int standardDeviations = 2;

            public int StandardDeviations
            {
                get { return standardDeviations; }
                set { standardDeviations = value; }
            }

            /// <summary>
            /// 数据源
            /// </summary>
            private DataTable dataSource = null;

            public DataTable DataSource
            {
                get { return dataSource; }
                set { dataSource = value; }
            }

            /// <summary>
            /// 计算指定索引的MID,DOWN,UP值
            /// </summary>
            /// <param name="r"></param>
            /// <returns></returns>
            public double[] Calculate(int r)
            {
                double closeValue = Convert.ToDouble(dataSource.Rows[r][this.Close]);
                double mid = CommonClass.CalcuteSimpleMovingAvg(r, periods, TlsM.Field, close, closeValue, dataSource);
                if (mid == NULL)
                {
                    mid = 0;
                }
                double md = 0;
                if (r >= periods - 1)
                {
                    double sumValue = (closeValue - mid) * (closeValue - mid);
                    for (int i = r-1; i >= r - (periods - 1); i--)
                    {
                        double curClose=Convert.ToDouble(dataSource.Rows[i][this.Close]);
                        double curMA = Convert.ToDouble(dataSource.Rows[i][this.tlsM.Field]);
                        sumValue += (curClose - curMA) * (curClose - curMA);
                    }
                    md = standardDeviations* Math.Sqrt(sumValue / periods);
                }
                double up = mid + 2 * md;
                double down = mid - 2 * md;
                return new double[] { mid,up,down};
            }
        }

        /// <summary>
        /// 标题字段的属性
        /// </summary>
        public class TitleField
        {
            public TitleField(string relateSeriesField, string field,
                string displayTitle, Color fieldColor, bool onlyShow)
            {
                this.relateSeriesField = relateSeriesField;
                this.field = field;
                this.displayTitle = displayTitle;
                this.FieldColor = fieldColor;
                this.mainFlag = onlyShow;
            }

            ~TitleField()
            {
                if (fieldBrush != null) fieldBrush.Dispose();
            }

            /// <summary>
            /// 相关的线条字段的名称
            /// </summary>
            private string relateSeriesField = string.Empty;

            public string RelateSeriesField
            {
                get { return relateSeriesField; }
                set { relateSeriesField = value; }
            }

            /// <summary>
            /// 字段名
            /// </summary>
            private string field = string.Empty;

            public string Field
            {
                get { return field; }
                set { field = value; }
            }

            /// <summary>
            /// 显示的名称
            /// </summary>
            private string displayTitle = string.Empty;

            public string DisplayTitle
            {
                get { return displayTitle; }
                set { displayTitle = value; }
            }

            /// <summary>
            /// 字段的颜色
            /// </summary>
            private Color fieldColor = Color.White;

            public Color FieldColor
            {
                get { return fieldColor; }
                set
                {
                    fieldColor = value;
                    if (fieldBrush != null)
                    {
                        fieldBrush.Dispose();
                    }
                    fieldBrush = new SolidBrush(value);
                }
            }

            /// <summary>
            /// 字段颜色的刷子
            /// </summary>
            private Brush fieldBrush = new SolidBrush(Color.White);

            public Brush FieldBrush
            {
                get { return fieldBrush; }
                set { fieldBrush = value; }
            }

            /// <summary>
            /// 主标题flag
            /// </summary>
            private bool mainFlag = false;

            public bool MainFlag
            {
                get { return mainFlag; }
                set { mainFlag = value; }
            }
        }

        /// <summary>
        /// 标记的类型
        /// </summary>
        public enum SignalType
        {
            UpArrow,
            DownArrow,
            UpArrowWithOutTail,
            DownArrowWithOutTail,
            LeftArrow,
            RightArrow
        }

        /// <summary>
        /// 周期类型
        /// </summary>
        public enum IntervalType
        {
            Second,
            Minute,
            Day,
            Week,
            Month,
            Year
        }

        /// <summary>
        /// 指标的类型
        /// </summary>
        public enum IndicatorType
        {
            SimpleMovingAverage,
            ExponentialMovingAverage,
            StochasticOscillator,
            MACD
        }
        #endregion

        #region 指标计算
        /// <summary>
        /// 操盘手买卖信号
        /// </summary>
        /// <param name="panelID"></param>
        /// <param name="candleName"></param>
        /// <param name="buyEMA"></param>
        /// <param name="buyTarget"></param>
        /// <param name="sellEMA"></param>
        /// <param name="sellTarget"></param>
        public void YMBuySellSignal(int panelID, string candleName, string buyEMA, string buyTarget, string sellEMA, string sellTarget)
        {
            this.AddExponentialMovingAverage(buyEMA, buyEMA, 6, buyTarget, panelID);
            this.SetTrendLineStyle(buyEMA, Color.SkyBlue, Color.Red, 1, DashStyle.Solid);
            this.AddExponentialMovingAverage(sellEMA, sellEMA, 5, sellTarget, panelID);
            this.SetTrendLineStyle(sellEMA, Color.Yellow, Color.Yellow, 1, DashStyle.Solid);
            this.SetCandleBuySellField(candleName, buyEMA, sellEMA);
        }

        /// <summary>
        /// 选择显示指标
        /// </summary>
        /// <param name="indicatorType"></param>
        public void ChoseIndicator(IndicatorType indicatorType)
        {
            switch (indicatorType)
            {
                case IndicatorType.StochasticOscillator:
                    bottomPanel.ClearPanel();
                    SetStochasticOscillatorToPanel("K", "D", "J", indStochasticOscillator, bottomPanelID);
                    RefreshGraph();
                    break;
                case IndicatorType.MACD:
                    bottomPanel.ClearPanel();
                    SetMacdToPanel("MACD", "DIFF", "DEA", indMACD, bottomPanelID);
                    RefreshGraph();
                    break;
            }
        }
        #endregion

        public class GrammaParser
        {
            LatexParser latex = LatexParser.Instance;
            KLineGraph graph;
            string lastId;
            string lastFunc;
            string lastTerm;

            Stack<double> parameter = new Stack<double>();
            Stack<string> operators = new Stack<string>();
            int operatorPri = -1;

            int brakes = 0;
                
            List<String> parameters = new List<string>();
            List<String> dataId = new List<String>(new String[] {"close", "open", "high", "low", "volume"});
            List<String> funcId = new List<String>(new String[] { "MA", "SMA", "EMA" });
            Color[] lineColors = new Color[4] {Color.White, Color.Yellow, Color.Purple, Color.Blue};
            int lineNumber = 0;

            List<LatexParser.LatexUnit> units = new List<LatexParser.LatexUnit>();
            int currentUnit = 0;

            public LatexParser Latex { get { return latex; } }

            public KLineGraph Graph { set { graph = value; } }

            public void Generate()
            {
                latex.Reset();
                latex.ReadNext();
                lineNumber = 0;
                graph.bottomPanel.ClearPanel();
                IsGramma();
                graph.RefreshGraph();
            }

            public bool IsGramma()
            {
                if (!IsStatement()) { return false; }
                if (latex.Unit.type != LatexParser.LatexType.Semicolon) { return false; }
                else { latex.ReadNext(); }
                if (!latex.End) { return IsGramma(); }
                
                return true;
            }

            public bool IsStatement()
            {
                if (latex.Unit.type != LatexParser.LatexType.Id) { return false; }
                else { lastId = latex.Unit.id; latex.ReadNext(); }
                return IsAfterStatement();
            }

            public bool IsAfterStatement()
            {
                if (latex.Unit.type == LatexParser.LatexType.Assign)
                {
                    latex.ReadNext();
                    if (!IsOutput()) { return false; }
                    return IsOptionalState();
                }
                else if (latex.Unit.type == LatexParser.LatexType.Output)
                {
                    latex.ReadNext();
                    return IsOutput();
                }
                else return false;
            }

            public bool IsOptionalState()
            {
                if (latex.Unit.type == LatexParser.LatexType.Comma)
                {
                    latex.ReadNext();
                    return latex.Unit.type == LatexParser.LatexType.Id;
                }
                return true;
            }

            public bool IsOutput()
            {
                if (!dataId.Contains(latex.Unit.id.ToLower()) && 
                    !funcId.Contains(latex.Unit.id.ToUpper()) && 
                    latex.Unit.type != LatexParser.LatexType.Number &&
                    latex.Unit.type != LatexParser.LatexType.LeftBrake) 
                    return false;
                TrendLineSeries tls = graph.AddTrendLine(lastId, lastId);
                parameters.Clear();
                parameter.Clear();
                operators.Clear();
                operatorPri = -1;
                units.Clear();
                GetFormula();
                LatexParser.LatexUnit lunit = new LatexParser.LatexUnit();
                lunit.id = ";";
                lunit.type = LatexParser.LatexType.Semicolon;
                units.Add(lunit);
                for (int i = 0; i < graph.DtAllMsg.Rows.Count; ++i)
                {
                    currentUnit = 0;
                    graph.SetValue(graph.dtAllMsg.Rows[i], lastId, CalculateValue(i));
                }
                graph.SetTrendLineToPanel(lastId, tls, graph.bottomPanelID);
                graph.SetTrendLineStyle(lastId, lineColors[lineNumber], lineColors[lineNumber], 1, DashStyle.Solid);
                ++lineNumber;
                return true;
            }

            public void GetFormula()
            {
                if (latex.Unit.type == LatexParser.LatexType.LeftBrake)
                {
                    ++brakes;
                    units.Add(latex.Unit);
                    latex.ReadNext();
                    GetFormula();
                    return;
                }
                if (latex.Unit.type == LatexParser.LatexType.Number)
                {
                    units.Add(latex.Unit);
                    latex.ReadNext();
                    GetTerm();
                    return;
                }
                if (latex.Unit.type != LatexParser.LatexType.Id) { return; }
                if (dataId.Contains(latex.Unit.id.ToLower()))
                {
                    units.Add(latex.Unit);
                    latex.ReadNext();
                    GetTerm();
                    return;
                }
                else if (funcId.Contains(latex.Unit.id.ToUpper()))
                {
                    units.Add(latex.Unit);
                    latex.ReadNext();
                    GetFunc();
                    return;
                }
            }

            void GetTerm()
            {
                if (latex.Unit.type == LatexParser.LatexType.Operator)
                {
                    units.Add(latex.Unit);
                    latex.ReadNext();
                    GetFormula();
                    return;
                }
                if (latex.Unit.type == LatexParser.LatexType.RightBrake && brakes > 0)
                {
                    units.Add(latex.Unit);
                    --brakes;
                    latex.ReadNext();
                    GetTerm();
                    return;
                }
            }

            void GetFunc()
            {
                if (latex.Unit.type != LatexParser.LatexType.LeftBrake) { units.Clear(); return; }
                else { units.Add(latex.Unit); latex.ReadNext(); }
                if (!GetParameters()) { units.Clear();  return; }
                if (latex.Unit.type != LatexParser.LatexType.RightBrake) { units.Clear(); return; }
                else { units.Add(latex.Unit); latex.ReadNext(); }
            }

            bool GetParameters()
            {
                GetFormula();
                if (!GetAfterParameters()) { units.Clear(); return false; }
                return true;
            }

            bool GetAfterParameters()
            {
                if (latex.Unit.type != LatexParser.LatexType.Comma) return true;
                else
                {
                    units.Add(latex.Unit);
                    latex.ReadNext();
                    GetParameters();
                }
                return true;
            }


            public double CalculateValue(int i)
            {
                if (units[currentUnit].type == LatexParser.LatexType.LeftBrake)
                {
                    lastTerm += units[currentUnit].id;
                    operators.Push("(");
                    operatorPri = -1;
                    ++currentUnit;

                    return CalculateValue(i);
                }
                if (units[currentUnit].type == LatexParser.LatexType.Number)
                {
                    lastTerm += units[currentUnit].id;
                    parameter.Push(Double.Parse(units[currentUnit].id));
                    ++currentUnit;
                    
                    return CalculateTerm(i);
                }
                if (units[currentUnit].type != LatexParser.LatexType.Id)
                {
                    while (operators.Count != 0)
                    {
                        parameter.Push(Cal(parameter.Pop(), parameter.Pop(), operators.Pop()));
                    }
                    return parameter.Pop();
                }
                if (dataId.Contains(units[currentUnit].id.ToLower()))
                {
                    string id = units[currentUnit].id.ToLower();
                    id = id.Replace("close", "closeP");
                    id = id.Replace("open", "openP");
                    lastTerm += id;
                    parameter.Push(Double.Parse(graph.DtAllMsg.Rows[i][id].ToString()));
                    ++currentUnit;
                    return CalculateTerm(i);
                }
                else if (funcId.Contains(units[currentUnit].id.ToUpper()))
                {
                    string funcName = units[currentUnit].id.ToUpper();
                    ++currentUnit;
                    parameter.Push(CalculateFunc(funcName, i));
                }

                while (operators.Count != 0)
                {
                    parameter.Push(Cal(parameter.Pop(), parameter.Pop(), operators.Pop()));
                }
                return parameter.Pop();
            }

            public double CalculateTerm(int i)
            {
                if (units[currentUnit].type == LatexParser.LatexType.Operator || 
                    (units[currentUnit].type == LatexParser.LatexType.RightBrake && operators.Contains("(")))
                {
                    lastTerm += units[currentUnit].id;
                    switch (units[currentUnit].id)
                    {
                        case "+":
                        case "-":
                            if (operatorPri > 0)
                            {
                                parameter.Push(Cal(parameter.Pop(), parameter.Pop(), operators.Pop()));
                            }
                            operators.Push(units[currentUnit].id);
                            operatorPri = 0;
                            break;
                        case "*":
                        case "/":
                            operators.Push(units[currentUnit].id);
                            operatorPri = 1;
                            break;
                        case ")":
                            string nextop = operators.Peek();
                            while (nextop != "(")
                            {
                                parameter.Push(Cal(parameter.Pop(), parameter.Pop(), operators.Pop()));
                                nextop = operators.Peek();
                            }
                            operators.Pop();
                            if (operators.Count > 0)
                            {
                                operatorPri = (operators.Peek() == "+" || operators.Peek() == "-") ? 0 : 1;
                            }
                            ++currentUnit;
                            return CalculateTerm(i);
                            
                    }
                    ++currentUnit;
                    return CalculateValue(i);
                }
                else
                {
                    while (operators.Count != 0)
                    {
                        parameter.Push(Cal(parameter.Pop(), parameter.Pop(), operators.Pop()));
                    }
                    return parameter.Pop();
                }
            }

            public double CalculateFunc(string funcName, int i)
            {
                if (units[currentUnit].type != LatexParser.LatexType.LeftBrake) return 0.0;
                else { ++currentUnit; }
                if (!IsParameters(i)) return 0.0;
                if (units[currentUnit].type != LatexParser.LatexType.RightBrake) return 0.0;
                else { ++currentUnit; }
          
                switch (funcName)
                {
                    case "MA":
                            return CommonClass.CalculateMovingAvg(i, Int32.Parse(parameters[1]), parameters[0], graph.DtAllMsg);
                    case "EMA":
                            return CommonClass.CalculateExponentialMovingAvg(lastId, parameters[0], Int32.Parse(parameters[1]), graph.DtAllMsg, i);
                    case "SMA":
                            return CommonClass.CalcuteSimpleMovingAvg(i, Int32.Parse(parameters[1]), lastId, parameters[0], Double.Parse(graph.DtAllMsg.Rows[i][parameters[0]].ToString()), graph.DtAllMsg);
                    default:
                        break;
                }
                return 0.0;
            }

            private double Cal(double n1, double n2, string op)
            {
                switch (op)
                {
                    case "+":
                        return n2 + n1;
                    case "-":
                        return n2 - n1;
                    case "*":
                        return n2 * n1;
                    case "/":
                        return n2 / n1;
                    default:
                        break;
                }
                return 0.0;
            }

            public bool IsParameters(int i)
            {
                lastTerm = "";
                double value = CalculateValue(i);
                parameters.Add(lastTerm);
                if (!dataId.Contains(lastTerm))
                    graph.SetValue(graph.dtAllMsg.Rows[i], lastTerm, value);

                if (!IsAfterParameters(i)) return false;
                return true;
            }

            public bool IsAfterParameters(int i)
            {
                if (units[currentUnit].type != LatexParser.LatexType.Comma) return true;
                else
                {
                    ++currentUnit;
                    return IsParameters(i);
                }
            }

            public bool IsBoolParameter()
            {
                if (latex.Unit.type != LatexParser.LatexType.Id) return false;
                else { latex.ReadNext(); }
                if (latex.Unit.type != LatexParser.LatexType.BoolOperator) return false;
                else { latex.ReadNext(); }
                if (latex.Unit.type != LatexParser.LatexType.Id) return false;
                else { latex.ReadNext(); }
                return IsAfterBoolParameter();
            }

            public bool IsAfterBoolParameter()
            {
                if (latex.Unit.type != LatexParser.LatexType.BoolOperator) return true;
                else { latex.ReadNext(); }
                if (latex.Unit.type != LatexParser.LatexType.Id) return false;
                else { latex.ReadNext(); }
                return IsAfterBoolParameter();
            }
        }
    }

    public class LatexParser
    {
        public enum LatexType
        {
            Id,
            Number,
            Comma,
            Semicolon,
            Assign,
            Output,
            LeftBrake,
            RightBrake,
            Operator,
            BoolOperator,
        }

        public struct LatexUnit
        {
            public string id;
            public LatexType type;
            public bool isHandled;
        }

        //单例
        private static LatexParser instance = new LatexParser();
        public static LatexParser Instance { get { return instance; } }

        //词法分析作用的文本框
        private TextBox textBox;
        public TextBox TextBox { set { textBox = value; } }

        //文本框内容转换为流
        private MemoryStream memoryStream;

        //词法单元
        private LatexUnit unit;
        public LatexUnit Unit { get { return unit; } }

        //下一个处理的字符
        private char next;

        //行号
        private int lineNum;
        public int LineNum { get { return lineNum; } }

        //列号
        private int columnNum;
        public int ColumnNum { get { return columnNum; } }

        //是否处理完成
        private bool end;
        public bool End { get { return end; } }

        //读入下一个词法单元
        public void ReadNext()
        {
            //判断是否读完
            if (next == 65535) { end = true; return; }

            //重置词法单元处理状态
            unit.isHandled = false;

            ReadBlack();        //读取空字符
            ReadSymbol();       //读取符号
            ReadOperator();     //读取运算符
            ReadNumber();       //读取数字
            ReadId();           //读取id
        }

        private void ReadBlack()
        {
            bool quit = false;
            while (!quit)
            {
                switch (next)
                {
                    case ' ':
                    case '\t':
                        next = (char)memoryStream.ReadByte();
                        break;
                    case '\r':
                        next = (char)memoryStream.ReadByte();
                        break;
                    case '\n':
                        ++lineNum;
                        columnNum = 0;
                        next = (char)memoryStream.ReadByte();

                        break;
                    default:
                        quit = true;
                        break;
                }
            }
        }

        private void ReadSymbol()
        {
            switch (next)
            {
                case ';':
                    unit.id = ";";
                    unit.type = LatexType.Semicolon;
                    unit.isHandled = true;
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    break;
                case ',':
                    unit.id = ",";
                    unit.type = LatexType.Comma;
                    unit.isHandled = true;
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    break;
                default:
                    break;
            }
        }

        private void ReadOperator()
        {
            if (unit.isHandled) return;
            switch (next)
            {
                case ':':
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    if (next == '=')
                    {
                        unit.id = ":=";
                        unit.type = LatexType.Assign;
                        next = (char)memoryStream.ReadByte();
                        ++columnNum;
                    }
                    else
                    {
                        unit.id = ":";
                        unit.type = LatexType.Output;
                    }
                    unit.isHandled = true;
                    break;
                case '+':
                case '-':
                case '*':
                case '/':
                    unit.id = "" + next;
                    unit.type = LatexType.Operator;
                    unit.isHandled = true;
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    break;
                case '<':
                case '>':
                case '=':
                    unit.id = "" + next;
                    unit.type = LatexType.BoolOperator;
                    unit.isHandled = true;
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    break;
                case '(':
                    unit.id = "(";
                    unit.type = LatexType.LeftBrake;
                    unit.isHandled = true;
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    break;
                case ')':
                    unit.id = ")";
                    unit.type = LatexType.RightBrake;
                    unit.isHandled = true;
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    break;
                default:
                    break;
            }
        }

        private void ReadNumber()
        {
            if (unit.isHandled) return;

            bool isDouble = false;
            int num = 0;
            int point = 1;

            try
            {
                num = num * 10 + Int16.Parse(next.ToString());
                if (isDouble) point *= 10;
            }
            catch (FormatException e)
            {
                if (next == '.') isDouble = true;
                else return;
            }

            unit.isHandled = true;
            unit.type = LatexType.Number;

            while (true)
            {
                next = (char)memoryStream.ReadByte();
                ++columnNum;
                try
                {
                    num = num * 10 + Int16.Parse(next.ToString());
                    if (isDouble) point *= 10;
                }
                catch (FormatException e)
                {
                    if (next == '.')
                    {
                        if (!isDouble) { isDouble = true; }
                        else { }//出错
                    }
                    else break;
                }
            }

            if (point > 0) unit.id = (num * 1.0 / point).ToString();
            else unit.id = num.ToString();
        }
        private void ReadId()
        {
            if (unit.isHandled) return;

            if (Char.IsLetter(next))
            {
                unit.isHandled = true;
                unit.id = "" + next;
                unit.type = LatexType.Id;
                while (true)
                {
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    if (Char.IsDigit(next) || Char.IsLetter(next))
                    {
                        unit.id += next;
                    }
                    else return;
                }
            }

            else { } //出错
        }

        //重置词法处理对象
        public void Reset()
        {
            lineNum = 0;
            columnNum = 0;
            end = false;
            memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(textBox.Text), false);
            next = (char)memoryStream.ReadByte();
        }
    }
}

