using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Dao;
using ImageOfStock;

namespace StockMonitor
{
    public partial class MainWindow : Form
    {
        private ListViewItem lastFocused;
        private RoutineLoader routineLoader = new RoutineLoader();
        private bool isListMode = true;
        private int listStartPos = 0;
        private int multiStartPos = 0;

        //private DrawSingleGraph singleGraph;
        //private DrawMultiGraph multiGraph;

        //private Form_compiler form;
        //private DrawKLine kLine;

        public MainWindow()
        {
            Visible = false;

            DataDefine.InitDataDefine();
            DBHelper.Init();
            GlobalData.InitCommonData();

            InitializeComponent();

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
            stocksList.MouseWheel += new MouseEventHandler(FormMouseWheel);
            stocksList.MouseDoubleClick += new MouseEventHandler(FormMouseDoubleClick);
            multiList.MouseWheel += new MouseEventHandler(FormMouseWheel);

            this.KeyPreview = true;
            this.KeyUp += new KeyEventHandler(FormKeyUp);
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
        }

        public void ToSingleMode(String stockId)
        {
            stocksList.Hide();
            multiList.Hide();
            singleGraph.StockId = stockId;
            singleGraph.BindData();
            singleGraph.Show();
        }

        private void ReSetCurrentShowed()
        {
            List<String> stockIdShowed = new List<String>();
            int multiPos = multiStartPos % stocksList.ListMaxSize;
            for (int i = 0; (i < stocksList.ListMaxSize || i < multiPos + 18) && i + listStartPos < GlobalData.StocksCode.Rows.Count; ++i)
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
        //    form.Visible = true;
        //    form.Text = "公式编辑器     股票代码：" + Program.codes;
        //    form.Enabled = false;
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("还未实现", "重要提示", MessageBoxButtons.OK);
        }

        private void WindowCloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void stockFreshTimer_Tick(object sender, EventArgs e)
        {
            //Thread t = new Thread(new ThreadStart(DrawList));
            //t.Start();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            DBHelper.UpdateRealtime(GlobalData.StocksTable);
        }

        private void kdjButton_Click(object sender, EventArgs e)
        {
            //singleGraph.ChangeKLinePanel(KLineGraph.IndicatorType.StochasticOscillator);
        }

        private void macdBtn_Click(object sender, EventArgs e)
        {
            //singleGraph.ChangeKLinePanel(KLineGraph.IndicatorType.MACD);
        }

        private void FormMouseWheel(object sender, MouseEventArgs e)
        {
            if(isListMode){
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
                    listStartPos = GlobalData.StocksCode.Rows.Count - stocksList.ListMaxSize -1;
                }
                else if (listStartPos > GlobalData.StocksCode.Rows.Count - stocksList.ListMaxSize - 1)
                {
                    listStartPos = 0;
                }
                multiStartPos = listStartPos;
            }
            else{
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

        private void FormMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (isListMode)
            {
                ToSingleMode(stocksList.GetFocusedItem().Name);
            }
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
        }
    }
}
