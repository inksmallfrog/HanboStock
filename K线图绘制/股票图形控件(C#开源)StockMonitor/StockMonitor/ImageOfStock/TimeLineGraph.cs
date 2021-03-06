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
        #region 溜熔
        public delegate void OnNodeEventHandler(string Value, out string OutValue);             //??
        public delegate void OnMouseTagMoveHandler(MouseSetNode Args, out string OutValue);     //??
        #endregion

        #region 協吶延楚
        public DataTable resultSetData = new DataTable();                                       //隠贋方象                                      
        public bool hasCrossing = false;                                                        //頁倦噴忖
        public ArrayList LMSN = new ArrayList();

        private String stockName = "兆各";
        private String stockPrice = "嫻鋸 0.00";
        private String stockUpdown = "嫻鋸 0.00";
        private String stockPercent = "嫻嫌 0.00";
        private String stockVol = "�崛� 0";

        private int stockState = 0;
        #endregion


        #region 陣周奉來


        #region 方峙柴麻

        public int mouseX = 0;
        public int mouseY = 0;

        /// <summary>
        /// 撹住楚議恷寄峙
        /// </summary>
        public double maxVol = 0;

        private long xMaxValue = 300;               //x已恷寄恫炎
        private long yMaxValue = 9000;              //y已恷寄恫炎
        private long xPointsNumber = 24;            //x恫炎方
        private long pointsNumber = 240;            //方象方楚
        /// <summary>
        /// 恣円生
        /// </summary>
        public string[] leftListStr = null;
        /// <summary>
        /// 嘔円生
        /// </summary>
        public string[] rightListStr = null;
        /// <summary>
        /// 蛍護侃恷寄峙
        /// </summary>
        public long ySpliteValue = 0;//蛍護Y峙

        [CategoryAttribute("恫炎"), DescriptionAttribute("X恷寄恫炎")]
        public long XMaxValue
        {
            get { return xMaxValue; }
            set { xMaxValue = value; }
        }
        [CategoryAttribute("恫炎"), DescriptionAttribute("Y恷寄恫炎")]
        public long YMaxValue
        {
            get { return yMaxValue; }
            set { yMaxValue = value; }
        }
        /// <summary>
        /// X議泣倖方
        /// </summary>
        [CategoryAttribute("方峙柴麻"), DescriptionAttribute("罪�澤峺邉諜禪�方")]
        public long XPointsNumber
        {
            get { return xPointsNumber; }
            set { xPointsNumber = value; }
        }
        /// <summary>
        /// Y議泣倖方
        /// </summary>
        [CategoryAttribute("方峙柴麻"), DescriptionAttribute("Y�澤峺邉諜禪�方")]
        public long PointsNumber
        {
            get { return pointsNumber; }
            set { pointsNumber = value; }
        }
        #endregion

        #region 翌鉱

        /// <summary>
        /// 恣鉦宣
        /// </summary>
        public int paddingLeft = 40;

        /// <summary>
        /// 貧鉦宣
        /// </summary>
        private int titleHeight = 40;
        [CategoryAttribute("翌鉱"), DescriptionAttribute("炎籾互業")]
        public int TitleHeight
        {
            get { return titleHeight; }
            set { titleHeight = value; }
        }

        private string xNodeValue = "扮寂";
        /// <summary>
        /// X已化海汽了
        /// </summary>
        [CategoryAttribute("翌鉱"), DescriptionAttribute("X已化海汽了")]
        public string XValue
        {
            get { return xNodeValue; }
            set { xNodeValue = value; }
        }
        private string yValue = "峙";
        /// <summary>
        /// Y已化海汽了
        /// </summary>
        [CategoryAttribute("翌鉱"), DescriptionAttribute("Y已化海汽了")]
        public string YValue
        {
            get { return yValue; }
            set { yValue = value; }
        }

        private bool drawTime = true;
        /// <summary>
        /// 頁倦壓X已�塋省閏�
        /// </summary>
        [CategoryAttribute("翌鉱"), DescriptionAttribute("頁倦壓X已�塋省閏�")]
        public bool DrawTime
        {
            get { return drawTime; }
            set { drawTime = value; }
        }

        private bool isDrawTitle = false;
        /// <summary>
        /// 頁倦壓競何�塋捷鋲頴渡�
        /// </summary>
        [CategoryAttribute("翌鉱"), DescriptionAttribute("頁倦壓競何�塋捷鋲頴渡�")]
        public bool IsDrawTitle
        {
            get { return isDrawTitle; }
            set { isDrawTitle = value; }
        }

        private int yPriceLines = 16;
        /// <summary>
        /// �塋昭杆餤槻佇�
        /// </summary>
        [CategoryAttribute("翌鉱"), DescriptionAttribute("�塋昭杆餤槻佇�")]
        public int YPriceLines
        {
            get { return yPriceLines; }
            set { yPriceLines = value; }
        }

        private int yVolLines = 10;
        /// <summary>
        /// �塋廠表餐慎槻佇�
        /// </summary>
        [CategoryAttribute("翌鉱"), DescriptionAttribute("�塋廠表餐慎槻佇�")]
        public int YVolLines
        {
            get { return yVolLines; }
            set { yVolLines = value; }
        }

        private Font titleFont = new Font("卜悶 ", 10f);
        /// <summary>
        /// 炎籾忖悶
        /// </summary>
        [CategoryAttribute("翌鉱"), DescriptionAttribute("炎籾忖悶")]
        public Font TitleFont
        {
            get { return titleFont; }
            set { titleFont = value; }
        }

        #endregion

        #region �潴�奉來
        private int lineWidth = 1;
        [CategoryAttribute("�潴�奉來"), DescriptionAttribute("�潴�議錐業")]
        public int LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }
        private Font lineFont = new Font("卜悶 ", 10f);
        [CategoryAttribute("�潴�奉來"), DescriptionAttribute("�潴�硫廣瞥議忖悶")]
        public Font LineFont
        {
            get { return lineFont; }
            set { lineFont = value; }
        }
        #endregion

        #region XY已奉來
        private int xyLineMaxX = 300;
        private int xyLineMaxY = 300;
        private int xyLineWidthX = 2;
        private int xyLineWidthY = 1;
        private string xDescription = "扮寂";
        private string yDescription = "峙";
        private Color xyLineColorX = Color.FromArgb(176, 0, 0);// Color.LawnGreen;
        private Color xyLineColorY = Color.FromArgb(176, 0, 0);//.LawnGreen;
        private Color yVOLLineColorY = Color.FromArgb(192, 192, 0);//.LawnGreen;
        private Font xyFontX = new Font("卜悶 ", 10f);
        private Font xyFontY = new Font("卜悶 ", 10f);
        /// <summary>
        /// Xので
        /// </summary>
        [CategoryAttribute("XY已僉��"), DescriptionAttribute("X已議傍苧")]
        public string XDescription
        {
            get { return xDescription; }
            set { xDescription = value; }
        }
        /// <summary>
        /// Xので
        /// </summary>
        [CategoryAttribute("XY已僉��"), DescriptionAttribute("X已議恷寄峙")]
        public int XYLineMaxX
        {
            get { return xyLineMaxX; }
            set { xyLineMaxX = value; }
        }

        /// <summary>
        /// Xので
        /// </summary>
        [CategoryAttribute("XY已僉��"), DescriptionAttribute("X已�濘躑�")]
        public int XYLineWidthX
        {
            get { return xyLineWidthX; }
            set { xyLineWidthX = value; }
        }
        /// <summary>
        /// Xので
        /// </summary>
        [CategoryAttribute("XY已僉��"), DescriptionAttribute("Y已議傍苧")]
        public string YDescription
        {
            get { return yDescription; }
            set { yDescription = value; }
        }
        /// <summary>
        /// Yので
        /// </summary>
        [CategoryAttribute("XY已僉��"), DescriptionAttribute("Y已議恷寄峙")]
        public int XYLineMaxY
        {
            get { return xyLineMaxY; }
            set { xyLineMaxY = value; }
        }

        /// <summary>
        /// Yので
        /// </summary>
        [CategoryAttribute("XY已僉��"), DescriptionAttribute("Y已�濘躑�")]
        public int XYLineWidthY
        {
            get { return xyLineWidthY; }
            set { xyLineWidthY = value; }
        }


        /// <summary>
        ///  Colorので
        /// </summary>
        [CategoryAttribute("XY已僉��"), DescriptionAttribute("X已議冲弼")]
        public Color XYLineColorX
        {
            get { return xyLineColorX; }
            set { xyLineColorX = value; }
        }
        /// <summary>
        ///  Colorので
        /// </summary>
        [CategoryAttribute("XY已僉��"), DescriptionAttribute("Y已議冲弼")]
        public Color XYLineColorY
        {
            get { return xyLineColorY; }
            set { xyLineColorY = value; }
        }
        /// <summary>
        /// Fontので
        /// </summary>
        [CategoryAttribute("XY已僉��"), DescriptionAttribute("X已議忖悶")]
        public Font XYFontX
        {
            get { return xyFontX; }
            set { xyFontX = value; }
        }
        /// <summary>
        ///  Fontので
        /// </summary>
        [CategoryAttribute("XY已僉��"), DescriptionAttribute("Y已議忖悶")]
        public Font XYFontY
        {
            get { return xyFontY; }
            set { xyFontY = value; }
        }
        #endregion

        #region 蛍峙�瀛�來
        private int splitWidthX;
        private int splitWidthY;
        private Color splitColorX = Color.FromArgb(176, 0, 0);// Color.GreenYellow;
        private Color splitColorY = Color.FromArgb(176, 0, 0);// Color.GreenYellow;
        private string nodeLastCharX = "";
        private string nodeLastCharY = "";
        #region X已蛍峙��

        /// <summary>
        /// Yので
        /// </summary>
        [CategoryAttribute("蛍峙�瀾］�"), DescriptionAttribute("X已蛍峙�濘躑�")]
        public int SplitWidthX
        {
            get { return splitWidthX; }
            set { splitWidthX = value; }
        }
        [CategoryAttribute("蛍峙�瀾］�"), DescriptionAttribute("X已蛍峙�瀾嬋�")]
        public Color SplitColorX
        {
            get { return splitColorX; }
            set { splitColorX = value; }
        }
        [CategoryAttribute("蛍峙�瀾］�"), DescriptionAttribute("X已蛍峙泣汽了")]
        public string NodeLastCharX
        {
            get { return nodeLastCharX; }
            set { nodeLastCharX = value; }
        }
        #endregion
        #region Y已蛍峙��
        /// <summary>
        /// Yので
        /// </summary>
        [CategoryAttribute("蛍峙�瀾］�"), DescriptionAttribute("Y已蛍峙�濘躑�")]
        public int SplitWidthY
        {
            get { return splitWidthY; }
            set { splitWidthY = value; }
        }
        [CategoryAttribute("蛍峙�瀾］�"), DescriptionAttribute("Y已蛍峙�瀾嬋�")]
        public Color SplitColorY
        {
            get { return splitColorY; }
            set { splitColorY = value; }
        }
        [CategoryAttribute("蛍峙�瀾］�"), DescriptionAttribute("Y已蛍峙泣汽了")]
        public string NodeLastCharY
        {
            get { return nodeLastCharY; }
            set { nodeLastCharY = value; }
        }
        #endregion
        #endregion

        #region 報炎函峙泣裳侘譜崔
        private int mouseWidth = 10;
        private int mouseHeight = 10;
        /// <summary>
        /// Yので
        /// </summary>
        [CategoryAttribute("報炎函峙泣裳侘僉��"), DescriptionAttribute("裳侘錐業")]
        public int MouseWidth
        {
            get { return mouseWidth; }
            set { mouseWidth = value; }
        }
        /// <summary>
        /// Yので
        /// </summary>
        [CategoryAttribute("報炎函峙泣裳侘僉��"), DescriptionAttribute("裳侘互業")]
        public int MouseHeight
        {
            get { return mouseHeight; }
            set { mouseHeight = value; }
        }
        #endregion
        #region 方双
        public System.Windows.Forms.ListView slnList=new ListView();
        List<string> myTimeStr = new List<string>();
        #endregion
        #endregion
        #region 陣周並周
        [CategoryAttribute("下蕉"), Description("鮫X恫炎炎准泣扮議並周")]
        public event OnNodeEventHandler XNodeOut;
        [CategoryAttribute("下蕉"), Description("鮫Y恫炎炎准泣扮議並周")]
        public event OnNodeEventHandler YNodeOut;
        [CategoryAttribute("荷恬"), Description("輝報炎将狛炎泣扮")]
        /// <summary>
        /// 泡仟夕�餤痛�
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
                DrawXY(g);//紙XY已 
                DrawXLine(g);//紙X已蛍峙�� 
                DrawYLine(g);//紙Y已蛍峙�� 
                DrawAllData(g);//紙竃侭嗤議勺鯉�� 
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

            //紙崙噴忖��
            if (hasCrossing)
            {
                Point hengxian1 = new Point(mouseX, xyLineMaxY + titleHeight);
                Point hengxian2 = new Point(mouseX, titleHeight);
                g.DrawLine(new Pen(Color.White, 1), hengxian1, hengxian2);

                Point shuxian1 = new Point(paddingLeft, mouseY);
                Point shuxian2 = new Point(maxX + paddingLeft, mouseY);
                g.DrawLine(new Pen(Color.White, 1), shuxian1, shuxian2);
            }
            //双燕
            string[] myTimeKLine = new string[] { "10:00", "10:30", "11:00", "13:00", "13:30", "14:00", "14:30" };

            //抱羨議倡��
            int setpvalue = maxX / 8;//耽化
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
        #region   鮫竃X已貧議蛍峙��
        private void DrawXLine(Graphics g)
        {
            int maxX = xyLineMaxX;
            int maxY = xyLineMaxY;

            int stepLength = (int)(XYLineMaxX / (long)xPointsNumber) / 2;      //X已蛍峙化海

            int currentPriceLine = 0;        //�塋承勅杆騅�炎��慌16佩
            int currentVolLine = 0;      //�塋承蝶表餐迅�炎��慌10佩

            //貫和吏貧鮫罪��
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

                //了噐貧yPriceLines佩�┝杆馭���
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
                //了噐和yVolLines佩�┳表餐診���
                else if(currentVolLine < yVolLines)
                {
                    double volMax = Convert.ToDouble(maxVol) / 100;
                    double myStep = volMax / yVolLines;
                    string MyStrValue = (volMax - currentVolLine * myStep).ToString("#0");
                    g.DrawString(MyStrValue + NodeLastCharY, XYFontY, new SolidBrush(Color.FromArgb(192, 192, 0)), new Point(maxX + paddingLeft, maxY * ((int)xPointsNumber - i) / (int)xPointsNumber - 2 + titleHeight));
                    currentVolLine++;
                }

                //佩蛍護
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
        #region   鮫竃Y已貧議蛍峙��
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
        #region   鮫竃侭嗤��
        private void DrawAllData(Graphics g)
        {
            LMSN.Clear();

            int maxX;
            int maxY;
            maxX = xyLineMaxX;
            //maxY = xyLineMaxY;
            maxY = (int)ySpliteValue;//

            ArrayList priceList = new ArrayList();

            //緩儉桟喘噐儉桟ListView嶄耽倖
            for (int i = 0; i < this.slnList.Items.Count; i++)
            {
                string ProName = this.slnList.Items[i].SubItems[0].Text;
                string[] p = this.slnList.Items[i].SubItems[1].Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                PointF[] price = new PointF[resultSetData.Rows.Count];

                //緩儉桟喘噐誼欺乎N倖議Point
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
        #region   鮫竃汽倖議��
        private void DrawData(string ProName, PointF[] Price, Pen pen, Color NodeColor, Graphics g)
        {
            //斤方象序佩鮫��Price[0]-Price[1],Price[1]-Price[2],Price[2]-Price[3],Price[3]-Price[4] 

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

            //斤方象序佩鮫��Price[0]-Price[1],Price[1]-Price[2],Price[2]-Price[3],Price[3]-Price[4] 

            for (int i = 0; i < Price.Length; i++)
            {
                if (Convert.ToDecimal(Price[i]) == 0)
                {

                }
                else
                {
                    int myy = (maxX * i / (int)pointsNumber + paddingLeft);
                    #region 柴麻互業
                    int my_maxVolLength = maxY - (int)ySpliteValue;//恷寄海業
                    double mystemp = my_maxVolLength / maxVol;//化海
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
            stockPrice = "�崋� " + price.ToString("0.00");
            stockUpdown = "嫻鋸 " + (price - close).ToString("0.00");
            stockPercent = "嫻嫌 " + ((price - close) / close * 100).ToString("0.00") + ("%");
            stockState = Math.Sign(price - close);

            ReadData(Double.Parse(current["closeP"].ToString()), Double.Parse(current["high"].ToString()), Double.Parse(current["low"].ToString()));
        }

        public void Draw()
        {

        }

        public void ReadData(double open, double high, double low)
        {
            //鳩隠方象厮鰯協
            if (resultSetData.Rows.Count > 0)
            {
                this.leftListStr = new string[yPriceLines];          //恣円生猟忖双燕
                this.rightListStr = new string[yPriceLines];         //嘔円生猟忖双燕
                double dirtaHigh = high - open;             //恷互餓勺
                double dirtaLow = open - low;               //恷詰餓勺
                double maxDirta = (dirtaHigh > dirtaLow) ? dirtaHigh : dirtaLow;    //恷寄餓勺
                maxDirta = 0.08 * Math.Ceiling(maxDirta / 0.08);
                if (maxDirta == 0) maxDirta = 0.08;

                double perPrice = maxDirta / (yPriceLines / 2);               //耽佩寂議弓奐勺鯉
                double maxPriceShow = open + maxDirta;        //恷寄�嵎擬杆�

                //貫貧吏和柴麻�塋昭杆�
                for (int q = 0; q < yPriceLines; q++)
                {
                    double priceMark = maxPriceShow - perPrice * q;                 //勺鯉
                    double percent = Math.Abs((priceMark - open) / open) * 100;     //曳箭

                    //蝕徒了崔
                    if (q == yPriceLines / 2)
                    {
                        priceMark = open;
                        percent = 0;
                    }
                    string priceShow = priceMark.ToString("#0.00");
                    this.leftListStr[q] = priceShow;
                    this.rightListStr[q] = percent.ToString("#0.00") + "%";
                    //登僅恣円生錐業頁倦怎參�塋昭杆�
                    if (priceShow.Length * 6 > this.paddingLeft)
                    {
                        this.paddingLeft = priceShow.Length * 8;
                    }
                }

                bool isNoon = false;                            //頁倦葎怜徒
                maxDirta = maxDirta * 2 * 1000;                 //屁倖互業
                this.YMaxValue = Convert.ToInt32(maxDirta);

                //curTimeOfDate = DateTime.Parse(resultSetData.Rows[0][0].ToString()).ToString("yyyy-MM-dd");     //�塋承捗子徃嫺�

                //DateTime myTimeFirst = DateTime.Parse(curTimeOfDate + " 09:30:00");
                TimeSpan currentBidsTime = TimeSpan.Parse("09:30:00");                                          //住叟軟兵扮寂
                TimeSpan dirtaTime = TimeSpan.Parse("00:01:00");
                TimeSpan morningEndTime = TimeSpan.Parse("11:30:00");

                string prices = "";                     //??
                string averagePrices = "";              //??
                string vols = "";                       //??
                double sumPrice = 0;                    //勺鯉箔才
                double _maxVol = 0;                     //恷寄撹住楚

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

                    double price = Convert.ToDouble(row["price"]);     //勺鯉
                    double priceDirta = price - open;                  //餓勺
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

                ListViewItem item = new ListViewItem("蛍扮");
                item.SubItems.Add(prices);
                item.SubItems.Add("255, 255, 255");
                item.SubItems.Add("255, 255, 255");
                item.SubItems.Add(System.Drawing.Drawing2D.DashStyle.Solid.ToString());
                this.slnList.Items.Add(item);

                ListViewItem itemjx = new ListViewItem("譲��");
                itemjx.SubItems.Add(averagePrices);
                itemjx.SubItems.Add("255, 255, 0");
                itemjx.SubItems.Add("255, 255, 0");
                itemjx.SubItems.Add(System.Drawing.Drawing2D.DashStyle.Solid.ToString());
                this.slnList.Items.Add(itemjx);

                ListViewItem itemP = new ListViewItem("撹住楚");
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
    #region 哘喘窃
    /// <summary>
    /// 泣奉來
    /// </summary>
    public class MouseSetNode
    {
        /// <summary>
        /// �酊臣�各
        /// </summary>
        private string _ProName = "";
        private int _X = 0;
        private int _Y = 0;
        private int _Width = 0;
        private int _Height = 0;
        private int _Index = 0;
        /// <summary>
        /// �酊臣�
        /// </summary>
        public string ProName
        {
            get { return _ProName; }
            set { _ProName = value; }
        }
        /// <summary>
        /// 罪恫炎
        /// </summary>
        public int X
        {
            get { return _X; }
            set { _X = value; }
        }
        /// <summary>
        /// 忿恫炎
        /// </summary>
        public int Y
        {
            get { return _Y; }
            set { _Y = value; }
        }
        /// <summary>
        /// 錐業
        /// </summary>
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        /// <summary>
        /// 互業
        /// </summary>
        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        /// <summary>
        /// 沫哈
        /// </summary>
        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }


    #endregion
    }
}
