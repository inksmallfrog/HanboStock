using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using Dao;


namespace ImageOfStock
{
    public partial class SingleGraph : UserControl
    {
        private String stockId;
        private SingleLoader loader = new SingleLoader();
        private bool isKLineMode = true;
        private bool isAllPerbid = false;

        public String StockId{
            set{
                if(!value.Equals(stockId)){
                    loader.Stop();
                    stockId = value;
                    StartBackGround();
                    loader.StockId = stockId;
                    loader.Start();
                    detailPanel.StockId = stockId;
                }
            }
        }

        public SingleGraph()
        {
            InitializeComponent();

            kLinePanel.Draw();
            timeLineGraph.Draw();

            allDetailPanel1.SetMouseWheel(new MouseEventHandler(PerbidMouseWheel));
        }

        public void Generate()
        {
            kLinePanel.Generate();
        }

        public void BindFormulaBox(TextBox box){
            kLinePanel.BindFormulaBox(box);
        }

        private void StartBackGround()
        {
            BackGround.BackGroundTask history;
            history.stockId = stockId;
            history.taskType = BackGround.TaskType.HistoryTask;
            history.cycleTime = -1;
            BackGround.BackGroundTask perbid;
            perbid.stockId = stockId;
            perbid.taskType = BackGround.TaskType.PerbidTask;
            perbid.cycleTime = Config.GlobalConfig.ThreadConfig.PerbidReadTime;
            BackGround.getBackGround().Execute(history);
            BackGround.getBackGround().Execute(perbid);
            BackGround.getBackGround().AddTask(perbid);
        }

        public void ToggleMode()
        {
            if (isKLineMode) { ToTimeLineMode(); }
            else { ToKLineMode(); }
        }

        public void ToggleAllPerbid()
        {
            if (!isAllPerbid)
            {
                allDetailPanel1.BindData(loader.Perbid, loader.Current);
                if (isKLineMode) kLinePanel.Hide();
                else timeLineGraph.Hide();
                detailPanel.Hide();
                allDetailPanel1.Show();
                isAllPerbid = true;
                Focus();
            }
            else
            {
                allDetailPanel1.Hide();
                if (isKLineMode) kLinePanel.Show();
                else timeLineGraph.Show();
                detailPanel.Show();
                isAllPerbid = false;
            }
        }
        
        public void ToTimeLineMode()
        {
            if (!isKLineMode)
            {
                return;
            }
            isKLineMode = false;
            timeLineGraph.BindData(loader.Perminut, loader.Current);
            kLinePanel.Hide();
            timeLineGraph.Show();
        }

        public void ToKLineMode()
        {
            if (isKLineMode) { return; }
            isKLineMode = true;
            kLinePanel.BindData(loader.Histories, loader.Current);
            timeLineGraph.Hide();
            kLinePanel.Show();
        }

        public void BindData()
        {
            if(isAllPerbid) {
                allDetailPanel1.BindData(loader.Perbid, loader.Current);
                return;
            }
            detailPanel.BindData(loader.Current, loader.Histories, loader.Perbid);
            Console.WriteLine("Bind " + loader.Perbid.Rows.Count);
            if (isKLineMode) { kLinePanel.BindData(loader.Histories, loader.Current); }
            else { timeLineGraph.BindData(loader.Perminut, loader.Current); }
        }

        private void PerbidMouseWheel(object sender, MouseEventArgs e)
        {
            if (isAllPerbid)
            {
                if (e.Delta < 0)
                {
                    allDetailPanel1.NextPage(loader.Perbid);
                }
                else if (e.Delta > 0)
                {
                    allDetailPanel1.LastPage();
                }
                allDetailPanel1.BindData(loader.Perbid, loader.Current);
            }
        }
    }
}
