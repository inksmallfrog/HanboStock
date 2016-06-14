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
        #region ί��
        public delegate void OnNodeEventHandler(string Value, out string OutValue);             //??
        public delegate void OnMouseTagMoveHandler(MouseSetNode Args, out string OutValue);     //??
        #endregion

        #region �������
        public DataTable resultSetData = new DataTable();                                       //��������                                      
        public bool hasCrossing = false;                                                        //�Ƿ�ʮ��
        public ArrayList LMSN = new ArrayList();

        private String stockName = "����";
        private String stockPrice = "�ǵ� 0.00";
        private String stockUpdown = "�ǵ� 0.00";
        private String stockPercent = "�Ƿ� 0.00";
        private String stockVol = "���� 0";

        private int stockState = 0;
        #endregion


        #region �ؼ�����


        #region ��ֵ����

        public int mouseX = 0;
        public int mouseY = 0;

        /// <summary>
        /// �ɽ��������ֵ
        /// </summary>
        public double maxVol = 0;

        private long xMaxValue = 300;               //x���������
        private long yMaxValue = 9000;              //y���������
        private long xPointsNumber = 24;            //x������
        private long pointsNumber = 240;            //��������
        /// <summary>
        /// �����
        /// </summary>
        public string[] leftListStr = null;
        /// <summary>
        /// �ұ���
        /// </summary>
        public string[] rightListStr = null;
        /// <summary>
        /// �ָ���ֵ
        /// </summary>
        public long ySpliteValue = 0;//�ָ�Yֵ

        [CategoryAttribute("����"), DescriptionAttribute("X�������")]
        public long XMaxValue
        {
            get { return xMaxValue; }
            set { xMaxValue = value; }
        }
        [CategoryAttribute("����"), DescriptionAttribute("Y�������")]
        public long YMaxValue
        {
            get { return yMaxValue; }
            set { yMaxValue = value; }
        }
        /// <summary>
        /// X�ĵ����
        /// </summary>
        [CategoryAttribute("��ֵ����"), DescriptionAttribute("���߷ָ�ĵ����")]
        public long XPointsNumber
        {
            get { return xPointsNumber; }
            set { xPointsNumber = value; }
        }
        /// <summary>
        /// Y�ĵ����
        /// </summary>
        [CategoryAttribute("��ֵ����"), DescriptionAttribute("Y�߷ָ�ĵ����")]
        public long PointsNumber
        {
            get { return pointsNumber; }
            set { pointsNumber = value; }
        }
        #endregion

        #region ���

        /// <summary>
        /// �����
        /// </summary>
        public int paddingLeft = 40;

        /// <summary>
        /// �Ͼ���
        /// </summary>
        private int titleHeight = 40;
        [CategoryAttribute("���"), DescriptionAttribute("����߶�")]
        public int TitleHeight
        {
            get { return titleHeight; }
            set { titleHeight = value; }
        }

        private string xNodeValue = "ʱ��";
        /// <summary>
        /// X�Ჽ����λ
        /// </summary>
        [CategoryAttribute("���"), DescriptionAttribute("X�Ჽ����λ")]
        public string XValue
        {
            get { return xNodeValue; }
            set { xNodeValue = value; }
        }
        private string yValue = "ֵ";
        /// <summary>
        /// Y�Ჽ����λ
        /// </summary>
        [CategoryAttribute("���"), DescriptionAttribute("Y�Ჽ����λ")]
        public string YValue
        {
            get { return yValue; }
            set { yValue = value; }
        }

        private bool drawTime = true;
        /// <summary>
        /// �Ƿ���X����ʾʱ��
        /// </summary>
        [CategoryAttribute("���"), DescriptionAttribute("�Ƿ���X����ʾʱ��")]
        public bool DrawTime
        {
            get { return drawTime; }
            set { drawTime = value; }
        }

        private bool isDrawTitle = false;
        /// <summary>
        /// �Ƿ��ڶ�����ʾ��Ʊ��Ϣ
        /// </summary>
        [CategoryAttribute("���"), DescriptionAttribute("�Ƿ��ڶ�����ʾ��Ʊ��Ϣ")]
        public bool IsDrawTitle
        {
            get { return isDrawTitle; }
            set { isDrawTitle = value; }
        }

        private int yPriceLines = 16;
        /// <summary>
        /// ��ʾ�۸������
        /// </summary>
        [CategoryAttribute("���"), DescriptionAttribute("��ʾ�۸������")]
        public int YPriceLines
        {
            get { return yPriceLines; }
            set { yPriceLines = value; }
        }

        private int yVolLines = 10;
        /// <summary>
        /// ��ʾ�ɽ���������
        /// </summary>
        [CategoryAttribute("���"), DescriptionAttribute("��ʾ�ɽ���������")]
        public int YVolLines
        {
            get { return yVolLines; }
            set { yVolLines = value; }
        }

        private Font titleFont = new Font("���� ", 10f);
        /// <summary>
        /// ��������
        /// </summary>
        [CategoryAttribute("���"), DescriptionAttribute("��������")]
        public Font TitleFont
        {
            get { return titleFont; }
            set { titleFont = value; }
        }

        #endregion

        #region ��������
        private int lineWidth = 1;
        [CategoryAttribute("��������"), DescriptionAttribute("�����Ŀ��")]
        public int LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }
        private Font lineFont = new Font("���� ", 10f);
        [CategoryAttribute("��������"), DescriptionAttribute("����βע�͵�����")]
        public Font LineFont
        {
            get { return lineFont; }
            set { lineFont = value; }
        }
        #endregion

        #region XY������
        private int xyLineMaxX = 300;
        private int xyLineMaxY = 300;
        private int xyLineWidthX = 2;
        private int xyLineWidthY = 1;
        private string xDescription = "ʱ��";
        private string yDescription = "ֵ";
        private Color xyLineColorX = Color.FromArgb(176, 0, 0);// Color.LawnGreen;
        private Color xyLineColorY = Color.FromArgb(176, 0, 0);//.LawnGreen;
        private Color yVOLLineColorY = Color.FromArgb(192, 192, 0);//.LawnGreen;
        private Font xyFontX = new Font("���� ", 10f);
        private Font xyFontY = new Font("���� ", 10f);
        /// <summary>
        /// X�Τ�
        /// </summary>
        [CategoryAttribute("XY��ѡ��"), DescriptionAttribute("X���˵��")]
        public string XDescription
        {
            get { return xDescription; }
            set { xDescription = value; }
        }
        /// <summary>
        /// X�Τ�
        /// </summary>
        [CategoryAttribute("XY��ѡ��"), DescriptionAttribute("X������ֵ")]
        public int XYLineMaxX
        {
            get { return xyLineMaxX; }
            set { xyLineMaxX = value; }
        }

        /// <summary>
        /// X�Τ�
        /// </summary>
        [CategoryAttribute("XY��ѡ��"), DescriptionAttribute("X���߿��")]
        public int XYLineWidthX
        {
            get { return xyLineWidthX; }
            set { xyLineWidthX = value; }
        }
        /// <summary>
        /// X�Τ�
        /// </summary>
        [CategoryAttribute("XY��ѡ��"), DescriptionAttribute("Y���˵��")]
        public string YDescription
        {
            get { return yDescription; }
            set { yDescription = value; }
        }
        /// <summary>
        /// Y�Τ�
        /// </summary>
        [CategoryAttribute("XY��ѡ��"), DescriptionAttribute("Y������ֵ")]
        public int XYLineMaxY
        {
            get { return xyLineMaxY; }
            set { xyLineMaxY = value; }
        }

        /// <summary>
        /// Y�Τ�
        /// </summary>
        [CategoryAttribute("XY��ѡ��"), DescriptionAttribute("Y���߿��")]
        public int XYLineWidthY
        {
            get { return xyLineWidthY; }
            set { xyLineWidthY = value; }
        }


        /// <summary>
        ///  Color�Τ�
        /// </summary>
        [CategoryAttribute("XY��ѡ��"), DescriptionAttribute("X�����ɫ")]
        public Color XYLineColorX
        {
            get { return xyLineColorX; }
            set { xyLineColorX = value; }
        }
        /// <summary>
        ///  Color�Τ�
        /// </summary>
        [CategoryAttribute("XY��ѡ��"), DescriptionAttribute("Y�����ɫ")]
        public Color XYLineColorY
        {
            get { return xyLineColorY; }
            set { xyLineColorY = value; }
        }
        /// <summary>
        /// Font�Τ�
        /// </summary>
        [CategoryAttribute("XY��ѡ��"), DescriptionAttribute("X�������")]
        public Font XYFontX
        {
            get { return xyFontX; }
            set { xyFontX = value; }
        }
        /// <summary>
        ///  Font�Τ�
        /// </summary>
        [CategoryAttribute("XY��ѡ��"), DescriptionAttribute("Y�������")]
        public Font XYFontY
        {
            get { return xyFontY; }
            set { xyFontY = value; }
        }
        #endregion

        #region ��ֵ������
        private int splitWidthX;
        private int splitWidthY;
        private Color splitColorX = Color.FromArgb(176, 0, 0);// Color.GreenYellow;
        private Color splitColorY = Color.FromArgb(176, 0, 0);// Color.GreenYellow;
        private string nodeLastCharX = "";
        private string nodeLastCharY = "";
        #region X���ֵ��

        /// <summary>
        /// Y�Τ�
        /// </summary>
        [CategoryAttribute("��ֵ��ѡ��"), DescriptionAttribute("X���ֵ�߿��")]
        public int SplitWidthX
        {
            get { return splitWidthX; }
            set { splitWidthX = value; }
        }
        [CategoryAttribute("��ֵ��ѡ��"), DescriptionAttribute("X���ֵ����ɫ")]
        public Color SplitColorX
        {
            get { return splitColorX; }
            set { splitColorX = value; }
        }
        [CategoryAttribute("��ֵ��ѡ��"), DescriptionAttribute("X���ֵ�㵥λ")]
        public string NodeLastCharX
        {
            get { return nodeLastCharX; }
            set { nodeLastCharX = value; }
        }
        #endregion
        #region Y���ֵ��
        /// <summary>
        /// Y�Τ�
        /// </summary>
        [CategoryAttribute("��ֵ��ѡ��"), DescriptionAttribute("Y���ֵ�߿��")]
        public int SplitWidthY
        {
            get { return splitWidthY; }
            set { splitWidthY = value; }
        }
        [CategoryAttribute("��ֵ��ѡ��"), DescriptionAttribute("Y���ֵ����ɫ")]
        public Color SplitColorY
        {
            get { return splitColorY; }
            set { splitColorY = value; }
        }
        [CategoryAttribute("��ֵ��ѡ��"), DescriptionAttribute("Y���ֵ�㵥λ")]
        public string NodeLastCharY
        {
            get { return nodeLastCharY; }
            set { nodeLastCharY = value; }
        }
        #endregion
        #endregion

        #region ���ȡֵ���������
        private int mouseWidth = 10;
        private int mouseHeight = 10;
        /// <summary>
        /// Y�Τ�
        /// </summary>
        [CategoryAttribute("���ȡֵ�����ѡ��"), DescriptionAttribute("���ο��")]
        public int MouseWidth
        {
            get { return mouseWidth; }
            set { mouseWidth = value; }
        }
        /// <summary>
        /// Y�Τ�
        /// </summary>
        [CategoryAttribute("���ȡֵ�����ѡ��"), DescriptionAttribute("���θ߶�")]
        public int MouseHeight
        {
            get { return mouseHeight; }
            set { mouseHeight = value; }
        }
        #endregion
        #region ����
        public System.Windows.Forms.ListView slnList=new ListView();
        List<string> myTimeStr = new List<string>();
        #endregion
        #endregion
        #region �ؼ��¼�
        [CategoryAttribute("����"), Description("��X�����ڵ�ʱ���¼�")]
        public event OnNodeEventHandler XNodeOut;
        [CategoryAttribute("����"), Description("��Y�����ڵ�ʱ���¼�")]
        public event OnNodeEventHandler YNodeOut;
        [CategoryAttribute("����"), Description("����꾭�����ʱ")]
        /// <summary>
        /// ˢ��ͼ�����
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
                DrawXY(g);//��XY�� 
                DrawXLine(g);//��X���ֵ�� 
                DrawYLine(g);//��Y���ֵ�� 
                DrawAllData(g);//������еļ۸��� 
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

            //����ʮ����
            if (hasCrossing)
            {
                Point hengxian1 = new Point(mouseX, xyLineMaxY + titleHeight);
                Point hengxian2 = new Point(mouseX, titleHeight);
                g.DrawLine(new Pen(Color.White, 1), hengxian1, hengxian2);

                Point shuxian1 = new Point(paddingLeft, mouseY);
                Point shuxian2 = new Point(maxX + paddingLeft, mouseY);
                g.DrawLine(new Pen(Color.White, 1), shuxian1, shuxian2);
            }
            //�б�
            string[] myTimeKLine = new string[] { "10:00", "10:30", "11:00", "13:00", "13:30", "14:00", "14:30" };

            //����������
            int setpvalue = maxX / 8;//ÿ��
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
        #region   ����X���ϵķ�ֵ��
        private void DrawXLine(Graphics g)
        {
            int maxX = xyLineMaxX;
            int maxY = xyLineMaxY;

            int stepLength = (int)(XYLineMaxX / (long)xPointsNumber) / 2;      //X���ֵ����

            int currentPriceLine = 0;        //��ʾ�ļ۸����꣬��16��
            int currentVolLine = 0;      //��ʾ�ĳɽ������꣬��10��

            //�������ϻ�����
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

                //λ����yPriceLines�У��۸�����
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
                //λ����yVolLines�У��ɽ�������
                else if(currentVolLine < yVolLines)
                {
                    double volMax = Convert.ToDouble(maxVol) / 100;
                    double myStep = volMax / yVolLines;
                    string MyStrValue = (volMax - currentVolLine * myStep).ToString("#0");
                    g.DrawString(MyStrValue + NodeLastCharY, XYFontY, new SolidBrush(Color.FromArgb(192, 192, 0)), new Point(maxX + paddingLeft, maxY * ((int)xPointsNumber - i) / (int)xPointsNumber - 2 + titleHeight));
                    currentVolLine++;
                }

                //�зָ�
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
        #region   ����Y���ϵķ�ֵ��
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
        #region   ����������
        private void DrawAllData(Graphics g)
        {
            LMSN.Clear();

            int maxX;
            int maxY;
            maxX = xyLineMaxX;
            //maxY = xyLineMaxY;
            maxY = (int)ySpliteValue;//

            ArrayList priceList = new ArrayList();

            //��ѭ������ѭ��ListView��ÿ��
            for (int i = 0; i < this.slnList.Items.Count; i++)
            {
                string ProName = this.slnList.Items[i].SubItems[0].Text;
                string[] p = this.slnList.Items[i].SubItems[1].Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                PointF[] price = new PointF[PointsNumber];

                //��ѭ�����ڵõ���N����Point
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
        #region   ������������
        private void DrawData(string ProName, PointF[] Price, Pen pen, Color NodeColor, Graphics g)
        {
            //�����ݽ��л���Price[0]-Price[1],Price[1]-Price[2],Price[2]-Price[3],Price[3]-Price[4] 

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

            //�����ݽ��л���Price[0]-Price[1],Price[1]-Price[2],Price[2]-Price[3],Price[3]-Price[4] 

            for (int i = 0; i < Price.Length; i++)
            {
                if (Convert.ToDecimal(Price[i]) == 0)
                {

                }
                else
                {
                    int myy = (maxX * i / (int)pointsNumber + paddingLeft);
                    #region ����߶�
                    int my_maxVolLength = maxY - (int)ySpliteValue;//��󳤶�
                    double mystemp = my_maxVolLength / maxVol;//����
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
            stockPrice = "�ּ� " + price.ToString("0.00");
            stockUpdown = "�ǵ� " + (price - close).ToString("0.00");
            stockPercent = "�Ƿ� " + ((price - close) / close * 100).ToString("0.00") + ("%");
            stockState = Math.Sign(price - close);

            ReadData(Double.Parse(current["closeP"].ToString()), Double.Parse(current["high"].ToString()), Double.Parse(current["low"].ToString()));
        }

        public void Draw()
        {

        }

        public void ReadData(double open, double high, double low)
        {
            //ȷ�������Ѱ�
            if (resultSetData.Rows.Count > 0)
            {
                this.leftListStr = new string[yPriceLines];          //����������б�
                this.rightListStr = new string[yPriceLines];         //�ұ��������б�
                double dirtaHigh = high - open;             //��߲��
                double dirtaLow = open - low;               //��Ͳ��
                double maxDirta = (dirtaHigh > dirtaLow) ? dirtaHigh : dirtaLow;    //�����
                maxDirta = 0.08 * Math.Ceiling(maxDirta / 0.08);
                if (maxDirta == 0) maxDirta = 0.08;

                double perPrice = maxDirta / (yPriceLines / 2);               //ÿ�м�ĵ����۸�
                double maxPriceShow = open + maxDirta;        //�����ʵ�۸�

                //�������¼�����ʾ�۸�
                for (int q = 0; q < yPriceLines; q++)
                {
                    double priceMark = maxPriceShow - perPrice * q;                 //�۸�
                    double percent = Math.Abs((priceMark - open) / open) * 100;     //����

                    //����λ��
                    if (q == yPriceLines / 2)
                    {
                        priceMark = open;
                        percent = 0;
                    }
                    string priceShow = priceMark.ToString("#0.00");
                    this.leftListStr[q] = priceShow;
                    this.rightListStr[q] = percent.ToString("#0.00") + "%";
                    //�ж����������Ƿ�������ʾ�۸�
                    if (priceShow.Length * 6 > this.paddingLeft)
                    {
                        this.paddingLeft = priceShow.Length * 8;
                    }
                }

                bool isNoon = false;                            //�Ƿ�Ϊ����
                maxDirta = maxDirta * 2 * 1000;                 //�����߶�
                this.YMaxValue = Convert.ToInt32(maxDirta);

                //curTimeOfDate = DateTime.Parse(resultSetData.Rows[0][0].ToString()).ToString("yyyy-MM-dd");     //��ʾ�Ľ�������

                //DateTime myTimeFirst = DateTime.Parse(curTimeOfDate + " 09:30:00");
                TimeSpan currentBidsTime = TimeSpan.Parse("09:30:00");                                          //������ʼʱ��
                TimeSpan dirtaTime = TimeSpan.Parse("00:01:00");
                TimeSpan morningEndTime = TimeSpan.Parse("11:30:00");

                string prices = "";                     //??
                string averagePrices = "";              //??
                string vols = "";                       //??
                double sumPrice = 0;                    //�۸����
                double _maxVol = 0;                     //���ɽ���

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

                    double price = Convert.ToDouble(row["price"]);     //�۸�
                    double priceDirta = price - open;                  //���
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

                ListViewItem item = new ListViewItem("��ʱ");
                item.SubItems.Add(prices);
                item.SubItems.Add("255, 255, 255");
                item.SubItems.Add("255, 255, 255");
                item.SubItems.Add(System.Drawing.Drawing2D.DashStyle.Solid.ToString());
                this.slnList.Items.Add(item);

                ListViewItem itemjx = new ListViewItem("����");
                itemjx.SubItems.Add(averagePrices);
                itemjx.SubItems.Add("255, 255, 0");
                itemjx.SubItems.Add("255, 255, 0");
                itemjx.SubItems.Add(System.Drawing.Drawing2D.DashStyle.Solid.ToString());
                this.slnList.Items.Add(itemjx);

                ListViewItem itemP = new ListViewItem("�ɽ���");
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
    #region Ӧ����
    /// <summary>
    /// ������
    /// </summary>
    public class MouseSetNode
    {
        /// <summary>
        /// ��Ŀ����
        /// </summary>
        private string _ProName = "";
        private int _X = 0;
        private int _Y = 0;
        private int _Width = 0;
        private int _Height = 0;
        private int _Index = 0;
        /// <summary>
        /// ��Ŀ��
        /// </summary>
        public string ProName
        {
            get { return _ProName; }
            set { _ProName = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public int X
        {
            get { return _X; }
            set { _X = value; }
        }
        /// <summary>
        /// ������
        /// </summary>
        public int Y
        {
            get { return _Y; }
            set { _Y = value; }
        }
        /// <summary>
        /// ���
        /// </summary>
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }
        /// <summary>
        /// �߶�
        /// </summary>
        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }
        /// <summary>
        /// ����
        /// </summary>
        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }


    #endregion
    }
}
