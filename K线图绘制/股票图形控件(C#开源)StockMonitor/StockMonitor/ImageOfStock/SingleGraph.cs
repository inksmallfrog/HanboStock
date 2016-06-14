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
            perbid.cycleTime = 6000;
            BackGround.getBackGround().Execute(history);
            BackGround.getBackGround().AddTask(perbid);
        }

        public void ToggleMode()
        {
            if (isKLineMode) { ToTimeLineMode(); }
            else { ToKLineMode(); }
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
            detailPanel.BindData(loader.Current, loader.Histories, loader.Perbid);
            if (isKLineMode) { kLinePanel.BindData(loader.Histories, loader.Current); }
            else { timeLineGraph.BindData(loader.Perminut, loader.Current); }
        }
    }
}
