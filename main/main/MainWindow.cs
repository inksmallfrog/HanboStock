using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using Dao;
using ImageOfStock;

namespace StockMonitor
{
    public partial class MainWindow : Form
    {
        private RoutineLoader routineLoader = new RoutineLoader();
        private bool isListMode = true;
        private bool isSingleMode = false;
        private int listStartPos = 0;
        private int multiStartPos = 0;
        Point mouseOff;//鼠标移动位置变量
        bool leftFlag;//标签是否为左键

        private Formula form = new Formula();

        public MainWindow()
        {
            DBHelper.DropDB();

            Visible = false;

            DataDefine.InitDataDefine();
            DBHelper.Init();
            GlobalData.InitCommonData();

            InitializeComponent();

            form.Init();
            List<String> stockIdShowed = new List<String>();
            for (int i = 0; i < stocksList.ListMaxSize; ++i)
            {
                string stockId = GlobalData.StocksCode.Rows[i]["stockId"].ToString().PadLeft(7, '0');
                stockIdShowed.Add(stockId);
            }
            GlobalData.CurrentShowList = stockIdShowed;

            routineLoader.Start();

            ReSetCurrentShowed();

            Visible = true;
            Draw();

            FormClosed += new FormClosedEventHandler(OnClosed);

            this.MouseWheel += new MouseEventHandler(FormMouseWheel);
            menuStrip1.MouseDown += new MouseEventHandler(FormMouseDown);
            menuStrip1.MouseMove += new MouseEventHandler(FormMouseMove);
            menuStrip1.MouseUp += new MouseEventHandler(FormMouseUp);
            stocksList.MouseWheel += new MouseEventHandler(FormMouseWheel);
            stocksList.MouseDoubleClick += new MouseEventHandler(FormMouseDoubleClick);


            singleGraph.BindFormulaBox(form.FormulaBox);
            form.Graph = singleGraph;

            this.KeyPreview = true;
            this.KeyUp += new KeyEventHandler(FormKeyUp);
            this.KeyPress += new KeyPressEventHandler(FormKeyPress);

            StockSearch.ToStock toStockDel = new StockSearch.ToStock(this.ToStock);
            stockSearch1.ToStockDel = toStockDel;
        }
        
        private void Draw()
        {
            stocksList.Draw();
            multiList.Draw();

            if (isListMode)
            {
                multiList.Hide();
                stocksList.BindAllData(GlobalData.StocksTable, false);
                stocksList.Show();
                stocksList.Focus();
            }
            else
            {
                stocksList.Hide();
                multiList.BindAllData(GlobalData.CurrentShowList, multiStartPos % stocksList.ListMaxSize);
                multiList.Show();
            }
        }

        private void ToggleListMode()
        {
            if (isListMode)
            {
                isListMode = false;
                stocksList.Hide();
                multiList.Show();
                ReSetCurrentShowed();
                multiList.BindAllData(GlobalData.CurrentShowList, multiStartPos % stocksList.ListMaxSize);
            }
            else
            {
                isListMode = true;
                stocksList.Show();
                multiList.Hide();
                ReSetCurrentShowed();
                stocksList.BindAllData(GlobalData.StocksTable, false);
            }
            isSingleMode = false;
        }

        public void ToSingleMode(String stockId)
        {
            stocksList.Hide();
            multiList.Hide();
            singleGraph.StockId = stockId;
            singleGraph.BindData();
            singleGraph.Show();
            isSingleMode = true;
        }

        private void ReSetCurrentShowed()
        {
            List<String> stockIdShowed = new List<String>();
            int multiPos = multiStartPos % stocksList.ListMaxSize;
            for (int i = 0; i < stocksList.ListMaxSize && i + listStartPos < GlobalData.StocksCode.Rows.Count; ++i)
            {
                stockIdShowed.Add(GlobalData.StocksCode.Rows[i + listStartPos]["stockId"].ToString().PadLeft(7, '0'));   
            }
            GlobalData.CurrentShowList = stockIdShowed;
            routineLoader.ReadNow();

            if (isListMode)
            {
                BackGround.getBackGround().StopAllTasks();
                for (int i = multiStartPos; i < multiStartPos + 9; ++i)
                {
                    string stockId = GlobalData.StocksCode.Rows[i]["stockId"].ToString().PadLeft(7, '0');
                    BackGround.BackGroundTask task;
                    task.stockId = stockId;
                    task.taskType = BackGround.TaskType.PerbidTask;
                    task.cycleTime = -1;
                    BackGround.getBackGround().AddTask(task);
                }
            }
            else
            {
                for (int i = multiStartPos - 9; i < multiStartPos + 18; ++i)
                {
                    int realId = i;
                    if (realId < 0)
                    {
                        realId += GlobalData.StocksCode.Rows.Count;
                    }
                    else if (realId > GlobalData.StocksCode.Rows.Count - 1)
                    {
                        realId -= GlobalData.StocksCode.Rows.Count;
                    }
                    string stockId = GlobalData.StocksCode.Rows[realId]["stockId"].ToString().PadLeft(7, '0');
                    BackGround.BackGroundTask task;
                    task.stockId = stockId;
                    task.taskType = BackGround.TaskType.PerbidTask;
                    if (i < multiStartPos || i > multiStartPos + 8)
                    {
                        task.cycleTime = -1;
                    }
                    else
                    {
                        task.cycleTime = 6000;
                    }
                    BackGround.getBackGround().AddTask(task);
                }
            }
        }

        private void 首页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isSingleMode = false;
            singleGraph.Hide();
            if (isListMode)
            {
                stocksList.BindAllData(GlobalData.StocksTable, false);
                stocksList.Show();
            }
            else
            {
                multiList.BindAllData(GlobalData.CurrentShowList, multiStartPos % stocksList.ListMaxSize);
                multiList.Show();
            }
            /*singleGraph.HideGraph();
            list.ShowList();
            list.FocuseOn(lastFocused);*/
        }

        private void 公式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form.Visible = true;
        }

        private void WindowCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void stockFreshTimer_Tick(object sender, EventArgs e)
        {
            if (!isSingleMode)
            {
                if (isListMode)
                {
                    stocksList.BindAllData(GlobalData.StocksTable, true);
                }
                else
                {
                    multiList.BindAllData(GlobalData.CurrentShowList, multiStartPos % stocksList.ListMaxSize);
                }
            }
            else singleGraph.BindData();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            DBHelper.UpdateRealtime(GlobalData.StocksTable);
        }

        private void FormMouseWheel(object sender, MouseEventArgs e)
        {
            if (!isSingleMode)
            {
                if (isListMode)
                {
                    if (e.Delta < 0)
                    {
                        listStartPos += stocksList.ListMaxSize;
                    }
                    else if (e.Delta > 0)
                    {
                        listStartPos -= stocksList.ListMaxSize;
                    }
                    if (listStartPos < 0)
                    {
                        listStartPos = GlobalData.StocksCode.Rows.Count - stocksList.ListMaxSize - 1;
                    }
                    else if (listStartPos > GlobalData.StocksCode.Rows.Count - stocksList.ListMaxSize - 1)
                    {
                        listStartPos = 0;
                    }
                    multiStartPos = listStartPos;
                }
                else
                {
                    if (e.Delta < 0)
                    {
                        multiStartPos += 9;
                    }
                    else if (e.Delta > 0)
                    {
                        multiStartPos -= 9;
                    }
                    if (multiStartPos < 0)
                    {
                        multiStartPos = GlobalData.StocksCode.Rows.Count - 9 - 1;
                    }
                    else if (multiStartPos > GlobalData.StocksCode.Rows.Count - 9 - 1)
                    {
                        multiStartPos = 0;
                    }
                    listStartPos = multiStartPos / stocksList.ListMaxSize * stocksList.ListMaxSize;
                }

                ReSetCurrentShowed();

                if (isListMode)
                {
                    stocksList.BindAllData(GlobalData.StocksTable, false);
                }
                else
                {
                    multiList.BindAllData(GlobalData.CurrentShowList, multiStartPos % stocksList.ListMaxSize);
                }
            }
        }

        private void FormMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y); //得到变量的值
                leftFlag = true;                  //点击左键按下时标注为true;
            }
        }

        private void FormMouseMove(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);  //设置移动后的位置
                Location = mouseSet;
            }
        }

        private void FormMouseUp(object sender, MouseEventArgs e)
        {
            if (leftFlag)
            {
                leftFlag = false;//释放鼠标后标注为false;
            }
        }

        public void ToStock(string stockId)
        {
            if (!isSingleMode)
            {
                ToSingleMode(stockId);
            }
            else
            {
                singleGraph.StockId = stockId;
            }
        }

        private void FormMouseDoubleClick(object sender, MouseEventArgs e)
        {
            ToStock(stocksList.GetFocusedItem().Name);
        }

        private void FormKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.M | Keys.Control))
            {
                ToggleListMode();
            }
            else if (e.KeyData == Keys.F5)
            {
                singleGraph.ToggleMode();
            }
            else if (e.KeyData == Keys.F1)
            {
                singleGraph.ToggleAllPerbid();
            }
        }

        private void FormKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!stockSearch1.Visible && (
                (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
                (e.KeyChar >= 'A' && e.KeyChar <= 'Z') ||
                (e.KeyChar >= '0' && e.KeyChar <= '9')))
            {
                stockSearch1.Show();
                stockSearch1.BringToFront();
                stockSearch1.SearchText(e.KeyChar);
            }
        }
    }
}
