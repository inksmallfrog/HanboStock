using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace ImageOfStock
{
    public partial class TimeLineGraph : ContainerControl
    {
        public TimeLineGraph()
        {
            InitializeComponent();
        }
        #region 委托
        public delegate void OnNodeEventHandler(string Value, out string OutValue);             //??
        public delegate void OnMouseTagMoveHandler(MouseSetNode Args, out string OutValue);     //??
        #endregion

        #region 定义变量
        public DataTable resultSetData = new DataTable();                                       //保存数据                                      
        public bool hasCrossing = false;                                                        //是否十字
        public ArrayList LMSN = new ArrayList();

        private String stockName = "名称";
        private String stockPrice = "涨跌 0.00";
        private String stockUpdown = "涨跌 0.00";
        private String stockPercent = "涨幅 0.00";
        private String stockVol = "现量 0";

        private int stockState = 0;
        #endregion


        #region 控件属性


        #region 数值计算

        public int mouseX = 0;
        public int mouseY = 0;

        /// <summary>
        /// 成交量的最大值
        /// </summary>
        public double maxVol = 0;

        private long xMaxValue = 300;               //x轴最大坐标
        private long yMaxValue = 9000;              //y轴最大坐标
        private long xPointsNumber = 24;            //x坐标数
        private long pointsNumber = 240;            //数据数量
        /// <summary>
        /// 左边栏
        /// </summary>
        public string[] leftListStr = null;
        /// <summary>
        /// 右边栏
        /// </summary>
        public string[] rightListStr = null;
        /// <summary>
        /// 分割处最大值
        /// </summary>
        public long ySpliteValue = 0;//分割Y值

        [CategoryAttribute("坐标"), DescriptionAttribute("X最大坐标")]
        public long XMaxValue
        {
            get { return xMaxValue; }
            set { xMaxValue = value; }
        }
        [CategoryAttribute("坐标"), DescriptionAttribute("Y最大坐标")]
        public long YMaxValue
        {
            get { return yMaxValue; }
            set { yMaxValue = value; }
        }
        /// <summary>
        /// X的点个数
        /// </summary>
        [CategoryAttribute("数值计算"), DescriptionAttribute("横线分割的点个数")]
        public long XPointsNumber
        {
            get { return xPointsNumber; }
            set { xPointsNumber = value; }
        }
        /// <summary>
        /// Y的点个数
        /// </summary>
        [CategoryAttribute("数值计算"), DescriptionAttribute("Y线分割的点个数")]
        public long PointsNumber
        {
            get { return pointsNumber; }
            set { pointsNumber = value; }
        }
        #endregion

        #region 外观

        /// <summary>
        /// 左距离
        /// </summary>
        public int paddingLeft = 40;

        /// <summary>
        /// 上距离
        /// </summary>
        private int titleHeight = 40;
        [CategoryAttribute("外观"), DescriptionAttribute("标题高度")]
        public int TitleHeight
        {
            get { return titleHeight; }
            set { titleHeight = value; }
        }

        private string xNodeValue = "时间";
        /// <summary>
        /// X轴步长单位
        /// </summary>
        [CategoryAttribute("外观"), DescriptionAttribute("X轴步长单位")]
        public string XValue
        {
            get { return xNodeValue; }
            set { xNodeValue = value; }
        }
        private string yValue = "值";
        /// <summary>
        /// Y轴步长单位
        /// </summary>
        [CategoryAttribute("外观"), DescriptionAttribute("Y轴步长单位")]
        public string YValue
        {
            get { return yValue; }
            set { yValue = value; }
        }

        private bool drawTime = true;
        /// <summary>
        /// 是否在X轴显示时间
        /// </summary>
        [CategoryAttribute("外观"), DescriptionAttribute("是否在X轴显示时间")]
        public bool DrawTime
        {
            get { return drawTime; }
            set { drawTime = value; }
        }

        private bool isDrawTitle = false;
        /// <summary>
        /// 是否在顶部显示股票信息
        /// </summary>
        [CategoryAttribute("外观"), DescriptionAttribute("是否在顶部显示股票信息")]
        public bool IsDrawTitle
        {
            get { return isDrawTitle; }
            set { isDrawTitle = value; }
        }

        private int yPriceLines = 16;
        /// <summary>
        /// 显示价格的行数
        /// </summary>
        [CategoryAttribute("外观"), DescriptionAttribute("显示价格的行数")]
        public int YPriceLines
        {
            get { return yPriceLines; }
            set { yPriceLines = value; }
        }

        private int yVolLines = 10;
        /// <summary>
        /// 显示成交量的行数
        /// </summary>
        [CategoryAttribute("外观"), DescriptionAttribute("显示成交量的行数")]
        public int YVolLines
        {
            get { return yVolLines; }
            set { yVolLines = value; }
        }

        private Font titleFont = new Font("宋体 ", 10f);
        /// <summary>
        /// 标题字体
        /// </summary>
        [CategoryAttribute("外观"), DescriptionAttribute("标题字体")]
        public Font TitleFont
        {
            get { return titleFont; }
            set { titleFont = value; }
        }

        #endregion

        #region 线条属性
        private int lineWidth = 1;
        [CategoryAttribute("线条属性"), DescriptionAttribute("线条的宽度")]
        public int LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }
        private Font lineFont = new Font("宋体 ", 10f);
        [CategoryAttribute("线条属性"), DescriptionAttribute("线条尾注释的字体")]
        public Font LineFont
        {
            get { return lineFont; }
            set { lineFont = value; }
        }
        #endregion

        #region XY轴属性
        private int xyLineMaxX = 300;
        private int xyLineMaxY = 300;
        private int xyLineWidthX = 2;
        private int xyLineWidthY = 1;
        private string xDescription = "时间";
        private string yDescription = "值";
        private Color xyLineColorX = Color.FromArgb(176, 0, 0);// Color.LawnGreen;
        private Color xyLineColorY = Color.FromArgb(176, 0, 0);//.LawnGreen;
        private Color yVOLLineColorY = Color.FromArgb(192, 192, 0);//.LawnGreen;
        private Font xyFontX = new Font("宋体 ", 10f);
        private Font xyFontY = new Font("宋体 ", 10f);
        /// <summary>
        /// Xので
        /// </summary>
        [CategoryAttribute("XY轴选项"), DescriptionAttribute("X轴的说明")]
        public string XDescription
        {
            get { return xDescription; }
            set { xDescription = value; }
        }
        /// <summary>
        /// Xので
        /// </summary>
        [CategoryAttribute("XY轴选项"), DescriptionAttribute("X轴的最大值")]
        public int XYLineMaxX
        {
            get { return xyLineMaxX; }
            set { xyLineMaxX = value; }
        }

        /// <summary>
        /// Xので
        /// </summary>
        [CategoryAttribute("XY轴选项"), DescriptionAttribute("X轴线宽度")]
        public int XYLineWidthX
        {
            get { return xyLineWidthX; }
            set { xyLineWidthX = value; }
        }
        /// <summary>
        /// Xので
        /// </summary>
        [CategoryAttribute("XY轴选项"), DescriptionAttribute("Y轴的说明")]
        public string YDescription
        {
            get { return yDescription; }
            set { yDescription = value; }
        }
        /// <summary>
        /// Yので
        /// </summary>
        [CategoryAttribute("XY轴选项"), DescriptionAttribute("Y轴的最大值")]
        public int XYLineMaxY
        {
            get { return xyLineMaxY; }
            set { xyLineMaxY = value; }
        }

        /// <summary>
        /// Yので
        /// </summary>
        [CategoryAttribute("XY轴选项"), DescriptionAttribute("Y轴线宽度")]
        public int XYLineWidthY
        {
            get { return xyLineWidthY; }
            set { xyLineWidthY = value; }
        }


        /// <summary>
        ///  Colorので
        /// </summary>
        [CategoryAttribute("XY轴选项"), DescriptionAttribute("X轴的颜色")]
        public Color XYLineColorX
        {
            get { return xyLineColorX; }
            set { xyLineColorX = value; }
        }
        /// <summary>
        ///  Colorので
        /// </summary>
        [CategoryAttribute("XY轴选项"), DescriptionAttribute("Y轴的颜色")]
        public Color XYLineColorY
        {
            get { return xyLineColorY; }
            set { xyLineColorY = value; }
        }
        /// <summary>
        /// Fontので
        /// </summary>
        [CategoryAttribute("XY轴选项"), DescriptionAttribute("X轴的字体")]
        public Font XYFontX
        {
            get { return xyFontX; }
            set { xyFontX = value; }
        }
        /// <summary>
        ///  Fontので
        /// </summary>
        [CategoryAttribute("XY轴选项"), DescriptionAttribute("Y轴的字体")]
        public Font XYFontY
        {
            get { return xyFontY; }
            set { xyFontY = value; }
        }
        #endregion

        #region 分值线属性
        private int splitWidthX;
        private int splitWidthY;
        private Color splitColorX = Color.FromArgb(176, 0, 0);// Color.GreenYellow;
        private Color splitColorY = Color.FromArgb(176, 0, 0);// Color.GreenYellow;
        private string nodeLastCharX = "";
        private string nodeLastCharY = "";
        #region X轴分值线

        /// <summary>
        /// Yので
        /// </summary>
        [CategoryAttribute("分值线选项"), DescriptionAttribute("X轴分值线宽度")]
        public int SplitWidthX
        {
            get { return splitWidthX; }
            set { splitWidthX = value; }
        }
        [CategoryAttribute("分值线选项"), DescriptionAttribute("X轴分值线颜色")]
        public Color SplitColorX
        {
            get { return splitColorX; }
            set { splitColorX = value; }
        }
        [CategoryAttribute("分值线选项"), DescriptionAttribute("X轴分值点单位")]
        public string NodeLastCharX
        {
            get { return nodeLastCharX; }
            set { nodeLastCharX = value; }
        }
        #endregion
        #region Y轴分值线
        /// <summary>
        /// Yので
        /// </summary>
        [CategoryAttribute("分值线选项"), DescriptionAttribute("Y轴分值线宽度")]
        public int SplitWidthY
        {
            get { return splitWidthY; }
            set { splitWidthY = value; }
        }
        [CategoryAttribute("分值线选项"), DescriptionAttribute("Y轴分值线颜色")]
        public Color SplitColorY
        {
            get { return splitColorY; }
            set { splitColorY = value; }
        }
        [CategoryAttribute("分值线选项"), DescriptionAttribute("Y轴分值点单位")]
        public string NodeLastCharY
        {
            get { return nodeLastCharY; }
            set { nodeLastCharY = value; }
        }
        #endregion
        #endregion

        #region 鼠标取值点矩形设置
        private int mouseWidth = 10;
        private int mouseHeight = 10;
        /// <summary>
        /// Yので
        /// </summary>
        [CategoryAttribute("鼠标取值点矩形选项"), DescriptionAttribute("矩形宽度")]
        public int MouseWidth
        {
            get { return mouseWidth; }
            set { mouseWidth = value; }
        }
        /// <summary>
        /// Yので
        /// </summary>
        [CategoryAttribute("鼠标取值点矩形选项"), DescriptionAttribute("矩形高度")]
        public int MouseHeight
        {
            get { return mouseHeight; }
            set { mouseHeight = value; }
        }
        #endregion
        #region 数列
        public System.Windows.Forms.ListView slnList=new ListView();
        List<string> myTimeStr = new List<string>();
        #endregion
        #endregion
        #region 控件事件
        [CategoryAttribute("布局"), Description("画X坐标标节点时的事件")]
        public event OnNodeEventHandler XNodeOut;
        [CategoryAttribute("布局"), Description("画Y坐标标节点时的事件")]
        public event OnNodeEventHandler YNodeOut;
        [CategoryAttribute("操作"), Description("当鼠标经过标点时")]
        /// <summary>
        /// 刷新图像的锁
        /// </summary>
        private object refresh_lock = new object();
        #endregion
        public void DrawImage(Rectangle drawRectangle)
        {
            lock (refresh_lock)
            {
                BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
                BufferedGraphics myBuffer = currentContext.Allocate(this.CreateGraphics(), drawRectangle);
                Graphics g = myBuffer.Graphics;
                g.Clear(Color.Black);
                if (!isDrawTitle) { titleHeight = 0; }
                DrawXY(g);//绘XY轴 
                DrawXLine(g);//绘X轴分值线 
                DrawYLine(g);//绘Y轴分值线 
                DrawAllData(g);//绘出所有的价格线 
                if (isDrawTitle) { DrawTitle(g); }
                myBuffer.Render();
                myBuffer.Dispose();
            }
           
        }
      
        private void DrawXY(Graphics g)
        {
            int maxX;
            int maxY;
            
            maxX = xyLineMaxX;
            maxY = xyLineMaxY / 2;
 
            Rectangle drawRectangle = new Rectangle(0, 0, xyLineMaxX + paddingLeft, xyLineMaxY + titleHeight);
            g.FillRectangle(new SolidBrush(Color.Black), drawRectangle);

            Point py1 = new Point(paddingLeft, xyLineMaxY + titleHeight);
            Point py2 = new Point(paddingLeft, titleHeight);
            g.DrawLine(new Pen(XYLineColorY, 1), py1, py2);

            Point plst1 = new Point(maxX + paddingLeft, xyLineMaxY + titleHeight);
            Point plst2 = new Point(maxX + paddingLeft, titleHeight);
            g.DrawLine(new Pen(XYLineColorY, 1), plst1, plst2);

            //绘制十字线
            if (hasCrossing)
            {
                Point hengxian1 = new Point(mouseX, xyLineMaxY + titleHeight);
                Point hengxian2 = new Point(mouseX, titleHeight);
                g.DrawLine(new Pen(Color.White, 1), hengxian1, hengxian2);

                Point shuxian1 = new Point(paddingLeft, mouseY);
                Point shuxian2 = new Point(maxX + paddingLeft, mouseY);
                g.DrawLine(new Pen(Color.White, 1), shuxian1, shuxian2);
            }
            //列表
            string[] myTimeKLine = new string[] { "10:00", "10:30", "11:00", "13:00", "13:30", "14:00", "14:30" };

            //竖立的虚线
            int setpvalue = maxX / 8;//每步
            if (drawTime)
            {
                g.DrawString("9:30", XYFontX, new SolidBrush(XYLineColorX), new Point(paddingLeft, xyLineMaxY + titleHeight));
                g.DrawString("15:00", XYFontX, new SolidBrush(XYLineColorX), new Point(maxX + paddingLeft, xyLineMaxY + titleHeight));
            }
            
            for (int i = 1; i < 8; i++)
            {
                float[] dashValues = { 1, 2 };
                Point plstc1 = new Point(setpvalue * i + paddingLeft, xyLineMaxY + titleHeight);
                Point plstc2 = new Point(setpvalue * i + paddingLeft, titleHeight);
                Pen mypen = new Pen(XYLineColorY, 1);

                if (i == 4)
                {
                    g.DrawLine(new Pen(XYLineColorY, 2), plstc1, plstc2);
                }
                else
                {
                    mypen.DashPattern = dashValues;
                    g.DrawLine(mypen, plstc1, plstc2);
                }
                if (drawTime)
                {
                    g.DrawString(myTimeKLine[i - 1], XYFontX, new SolidBrush(XYLineColorX), new Point(setpvalue * i + paddingLeft, xyLineMaxY + titleHeight));
                }
            }

        }
        #region   画出X轴上的分值线
        private void DrawXLine(Graphics g)
        {
            int maxX = xyLineMaxX;
            int maxY = xyLineMaxY;

            int stepLength = (int)(XYLineMaxX / (long)xPointsNumber) / 2;      //X轴分值步长

            int currentPriceLine = 0;        //显示的价格坐标，共16行
            int currentVolLine = 0;      //显示的成交量坐标，共10行

            //从下往上画横线
            for (int i = (int)xPointsNumber; i >= 0; i--)
            {
                Point px1 = new Point(paddingLeft, maxY * (int)i / (int)xPointsNumber + titleHeight);
                Point px2 = new Point(maxX + paddingLeft, maxY * (int)i / (int)xPointsNumber + titleHeight);
                g.DrawLine(new Pen(new SolidBrush(SplitColorX), 1), px1, px2);

                //??
                string objDispName = (XYLineMaxX - i * stepLength).ToString();  
                string outString = "";
                if (YNodeOut != null)
                {
                    YNodeOut(objDispName, out outString);
                }
                else
                {
                    outString = objDispName;
                }

                //位于上yPriceLines行（价格区）
                if (currentPriceLine < yPriceLines && leftListStr != null)
                {
                    Color mycolorstring = Color.Red;
                    if (currentPriceLine == yPriceLines / 2)
                    {
                        mycolorstring = Color.White;
                    }
                    if (currentPriceLine > yPriceLines / 2)
                    {
                        mycolorstring = Color.GreenYellow;
                    }

                    g.DrawString(leftListStr[currentPriceLine] + NodeLastCharY, XYFontY, new SolidBrush(mycolorstring), new Point(0, maxY * ((int)xPointsNumber - i) / (int)xPointsNumber - 2 + titleHeight));

                    g.DrawString(rightListStr[currentPriceLine] + NodeLastCharY, XYFontY, new SolidBrush(mycolorstring), new Point(maxX + paddingLeft, maxY * ((int)xPointsNumber - i) / (int)xPointsNumber - 2 + titleHeight));

                    currentPriceLine++;
                }
                //位于下yVolLines行（成交量区）
                else if(currentVolLine < yVolLines)
                {
                    double volMax = Convert.ToDouble(maxVol) / 100;
                    double myStep = volMax / yVolLines;
                    string MyStrValue = (volMax - currentVolLine * myStep).ToString("#0");
                    g.DrawString(MyStrValue + NodeLastCharY, XYFontY, new SolidBrush(Color.FromArgb(192, 192, 0)), new Point(maxX + paddingLeft, maxY * ((int)xPointsNumber - i) / (int)xPointsNumber - 2 + titleHeight));
                    currentVolLine++;
                }

                //行分割
                if (i == yPriceLines / 2 || i == yPriceLines)
                {
                    g.DrawLine(new Pen(XYLineColorX, 2), px1, px2);
                    if (i == yPriceLines)
                    {
                        ySpliteValue = maxY * i / (int)xPointsNumber;
                    }
                }
            }
        }
        #endregion
        #region   画出Y轴上的分值线
        private void DrawYLine(Graphics g)
        {
            int MaxX;
            int MaxY;

            MaxX = xyLineMaxX;
            MaxY = xyLineMaxY - titleHeight;
            for (int i = 1; i < PointsNumber - 1; i++)
            {

                long setValue = (YMaxValue / PointsNumber) * i;
                string objDispName = setValue.ToString();
                string outString = "";
                if (XNodeOut != null)
                {
                    XNodeOut(objDispName, out outString);
                }
                else
                {
                    outString = objDispName;
                }

            }
        }
        #endregion
        #region   画出所有线
        private void DrawAllData(Graphics g)
        {
            LMSN.Clear();

            int maxX;
            int maxY;
            maxX = xyLineMaxX;
            //maxY = xyLineMaxY;
            maxY = (int)ySpliteValue;//

            ArrayList priceList = new ArrayList();

            //此循环用于循环ListView中每个
            for (int i = 0; i < this.slnList.Items.Count; i++)
            {
                string ProName = this.slnList.Items[i].SubItems[0].Text;
                string[] p = this.slnList.Items[i].SubItems[1].Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                PointF[] price = new PointF[PointsNumber];

                //此循环用于得到该N个的Point
                for (int j = 0; j < price.Length; j++)
                {
                    if (j > p.Length - 1)
                    {
                        price[j] = new PointF(0, 0);
                        continue;
                    }

                    float myfloatvalue = System.Convert.ToSingle(p[j]);
                    price[j] = new PointF(maxX * j / PointsNumber + paddingLeft, (float)maxY - (myfloatvalue * (float)maxY) / YMaxValue + titleHeight);
                }

                Color nodeColor = new Color();
                int R = 0;
                int G = 0;
                int B = 0;
                
                string[] RBG = this.slnList.Items[i].SubItems[2].Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                R = int.Parse(RBG[0]);
                G = int.Parse(RBG[1]);
                B = int.Parse(RBG[2]);
                string[] NodeRGB = this.slnList.Items[i].SubItems[3].Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                nodeColor = Color.FromArgb(int.Parse(NodeRGB[0]), int.Parse(NodeRGB[1]), int.Parse(NodeRGB[2]));

                Pen pen = new Pen(Color.FromArgb(R, G, B), lineWidth);
                if ("vol" == this.slnList.Items[i].Name)
                {
                    // vollinearray = p;
                    this.DrawVOLData(ProName, p, pen, nodeColor,g);
                }
                else
                {
                    this.DrawData(ProName, price, pen, nodeColor,g);
                }
            }
        }
        #endregion
        #region   画出单个的线
        private void DrawData(string ProName, PointF[] Price, Pen pen, Color NodeColor, Graphics g)
        {
            //对数据进行画线Price[0]-Price[1],Price[1]-Price[2],Price[2]-Price[3],Price[3]-Price[4] 

            for (int i = 0; i < Price.Length - 1; i++)
            {
                g.DrawLine(pen, Price[i], Price[i + 1]);
            }

        }
        private void DrawVOLData(string ProName, string[] Price, Pen pen, Color NodeColor, Graphics g)
        {
            int maxX;
            int maxY;
            maxX = xyLineMaxX;
            maxY = xyLineMaxY;

            //对数据进行画线Price[0]-Price[1],Price[1]-Price[2],Price[2]-Price[3],Price[3]-Price[4] 

            for (int i = 0; i < Price.Length; i++)
            {
                if (Convert.ToDecimal(Price[i]) == 0)
                {

                }
                else
                {
                    int myy = (maxX * i / (int)pointsNumber + paddingLeft);
                    #region 计算高度
                    int my_maxVolLength = maxY - (int)ySpliteValue;//最大长度
                    double mystemp = my_maxVolLength / maxVol;//步长
                    double myvaluecut = Convert.ToDouble(Price[i]);
                    double mylength = myvaluecut * mystemp;
                    int myvalue = (int)Math.Round(mylength, 0);
                    #endregion
                    Point py1 = new Point(myy, maxY - myvalue + titleHeight);
                    Point py2 = new Point(myy, maxY - 1 + titleHeight);
                    g.DrawLine(new Pen(new SolidBrush(yVOLLineColorY), 1), py1, py2);
                }
            }
        }
        private void DrawTitle(Graphics g)
        {
            g.DrawString(stockName, titleFont, System.Drawing.Brushes.White, new PointF(paddingLeft, 8));
            Brush brush = null;
            switch (stockState)
            {
                case 0:
                    brush = System.Drawing.Brushes.White;
                    break;
                case 1:
                    brush = System.Drawing.Brushes.Red;
                    break;
                case -1:
                    brush = System.Drawing.Brushes.Green;
                    break;
            }
            g.DrawString(stockPrice, titleFont, brush, new PointF(paddingLeft + 100, 8));
            g.DrawString(stockUpdown, titleFont, brush, new PointF(paddingLeft + 200, 8));
            g.DrawString(stockPercent, titleFont, brush, new PointF(paddingLeft + 300, 8));
        }

        public void BindData(DataTable minutsTable, DataRow current){
            slnList.Clear();

            resultSetData = minutsTable;

            stockName = current["name"].ToString();
            double price = Double.Parse(current["price"].ToString());
            double close = Double.Parse(current["closeP"].ToString());
            stockPrice = "现价 " + price.ToString("0.00");
            stockUpdown = "涨跌 " + (price - close).ToString("0.00");
            stockPercent = "涨幅 " + ((price - close) / close * 100).ToString("0.00") + ("%");
            stockState = Math.Sign(price - close);

            ReadData(Double.Parse(current["closeP"].ToString()), Double.Parse(current["high"].ToString()), Double.Parse(current["low"].ToString()));
        }

        public void Draw()
        {

        }

        public void ReadData(double open, double high, double low)
        {
            //确保数据已绑定
            if (resultSetData.Rows.Count > 0)
            {
                this.leftListStr = new string[yPriceLines];          //左边栏文字列表
                this.rightListStr = new string[yPriceLines];         //右边栏文字列表
                double dirtaHigh = high - open;             //最高差价
                double dirtaLow = open - low;               //最低差价
                double maxDirta = (dirtaHigh > dirtaLow) ? dirtaHigh : dirtaLow;    //最大差价
                maxDirta = 0.08 * Math.Ceiling(maxDirta / 0.08);
                if (maxDirta == 0) maxDirta = 0.08;

                double perPrice = maxDirta / (yPriceLines / 2);               //每行间的递增价格
                double maxPriceShow = open + maxDirta;        //最大现实价格

                //从上往下计算显示价格
                for (int q = 0; q < yPriceLines; q++)
                {
                    double priceMark = maxPriceShow - perPrice * q;                 //价格
                    double percent = Math.Abs((priceMark - open) / open) * 100;     //比例

                    //开盘位置
                    if (q == yPriceLines / 2)
                    {
                        priceMark = open;
                        percent = 0;
                    }
                    string priceShow = priceMark.ToString("#0.00");
                    this.leftListStr[q] = priceShow;
                    this.rightListStr[q] = percent.ToString("#0.00") + "%";
                    //判断左边栏宽度是否足以显示价格
                    if (priceShow.Length * 6 > this.paddingLeft)
                    {
                        this.paddingLeft = priceShow.Length * 8;
                    }
                }

                bool isNoon = false;                            //是否为午盘
                maxDirta = maxDirta * 2 * 1000;                 //整个高度
                this.YMaxValue = Convert.ToInt32(maxDirta);

                //curTimeOfDate = DateTime.Parse(resultSetData.Rows[0][0].ToString()).ToString("yyyy-MM-dd");     //显示的交易日期

                //DateTime myTimeFirst = DateTime.Parse(curTimeOfDate + " 09:30:00");
                TimeSpan currentBidsTime = TimeSpan.Parse("09:30:00");                                          //交易起始时间
                TimeSpan dirtaTime = TimeSpan.Parse("00:01:00");
                TimeSpan morningEndTime = TimeSpan.Parse("11:30:00");

                string prices = "";                     //??
                string averagePrices = "";              //??
                string vols = "";                       //??
                double sumPrice = 0;                    //价格求和
                double _maxVol = 0;                     //最大成交量

                for (int i = 0; i < resultSetData.Rows.Count; i++)
                {
                    DataRow row = resultSetData.Rows[i];

                    if (!isNoon)
                    {
                        if (TimeSpan.Parse(row["time"].ToString()).CompareTo(morningEndTime) > 0)
                        {
                            currentBidsTime = TimeSpan.Parse("13:00:00");
                            isNoon = true;
                        }
                    }

                    double price = Convert.ToDouble(row["price"]);     //价格
                    double priceDirta = price - open;                  //差价
                    double priceOne = ((priceDirta) * 1000 + maxDirta / 2); //??

                    if (price == 0)
                    {
                        prices += "0,";
                    }
                    else
                    {
                        prices += priceOne.ToString() + ",";
                    }
                    sumPrice = sumPrice + priceOne; 

                    double junjia = sumPrice / (i + 1); 

                    if (price == 0)
                    {
                        averagePrices += "0,";
                    }
                    else
                    {
                        averagePrices += junjia.ToString() + ",";
                    }

                    double currentVol = Convert.ToDouble(row["vol"]);
                    if (currentVol > _maxVol)
                    {
                        _maxVol = currentVol;
                    }
                    if (price == 0)
                    {
                        vols += "0,";
                    }
                    else
                    {
                        vols += currentVol.ToString() + ",";
                    }
                }

                this.maxVol = _maxVol;

                ListViewItem item = new ListViewItem("分时");
                item.SubItems.Add(prices);
                item.SubItems.Add("255, 255, 255");
                item.SubItems.Add("255, 255, 255");
                item.SubItems.Add(System.Drawing.Drawing2D.DashStyle.Solid.ToString());
                this.slnList.Items.Add(item);

                ListViewItem itemjx = new ListViewItem("均线");
                itemjx.SubItems.Add(averagePrices);
                itemjx.SubItems.Add("255, 255, 0");
                itemjx.SubItems.Add("255, 255, 0");
                itemjx.SubItems.Add(System.Drawing.Drawing2D.DashStyle.Solid.ToString());
                this.slnList.Items.Add(itemjx);

                ListViewItem itemP = new ListViewItem("成交量");
                itemP.Name = "vol";
                itemP.SubItems.Add(vols);
                itemP.SubItems.Add("192, 192, 0");
                itemP.SubItems.Add("192, 192, 0");
                itemP.SubItems.Add(System.Drawing.Drawing2D.DashStyle.DashDotDot.ToString());
                this.slnList.Items.Add(itemP);
            }
            this.DrawImage(this.DisplayRectangle);
        }
        #endregion 

        private void TimeLineGraph_Paint(object sender, PaintEventArgs e)
        {
            this.XYLineMaxX = this.Width - this.paddingLeft - 50;
            this.XYLineMaxY = this.Height - 20 - titleHeight;

            DrawImage(this.DisplayRectangle);
  
        }
    }
    #region 应用类
    /// <summary>
    /// 点属性
    /// </summary>
    public class MouseSetNode
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        private string _ProName = "";
        private int _X = 0;
        private int _Y = 0;
        private int _Width = 0;
        private int _Height = 0;
        private int _Index = 0;
        /// <summary>
        /// 项目名
        /// </summary>
        public string ProName
        {
            get { return _ProName; }
            set { _ProName = value; }
        }
        /// <summary>
        /// 横坐标
        /// </summary>
        public int X
        {
            get { return _X; }
            set { _X = value; }
        }
        /// <summary>
        /// 纵坐标
        /// </summary>
        public int Y
        {
            get { return _Y; }
            set { _Y = value; }
        }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        /// <summary>
        /// 索引
        /// </summary>
        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }


    #endregion
    }
}
