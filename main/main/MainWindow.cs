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
using Data;

namespace main
{
    public partial class MainWindow : Form
    {
        private ListViewItem lastFocused;

        private DrawList list;
        private DrawSingleGraph singleGraph;

        //private Form_compiler form;
        //private DrawKLine kLine;

        public MainWindow()
        {
            Visible = false;
            InitializeComponent();
            DataDefine.InitDataDefine();
            DBHelper.Init();
            CommonData.InitCommonData();

            DataReader.InvokeRoutingReader();

            Draw();

            Visible = true;

            FormClosed += new FormClosedEventHandler(OnClosed);
        }

        public static Control GetChildFromPanelByName(Panel panel, string name)
        {
            foreach (Control control in panel.Controls)
            {
                if (control.Name == name)
                {
                    return control;
                }
            }
            return null;
        }
        
        private void Draw()
        {
            list = new DrawList(StockList);
            singleGraph = new DrawSingleGraph(SingleGraphPanel);
            //kLine = new DrawKLine(KLineGraph);
        }

        private void 首页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            singleGraph.HideGraph();
            list.ShowList();
            list.FocuseOn(lastFocused);
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

        private void show_List_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lastFocused = list.GetFocusedItem();
            string id = list.GetFocusedStockId();
            list.HideList();
            singleGraph.ShowGraph(id);
        }

        private void stockFreshTimer_Tick(object sender, EventArgs e)
        {
            //Thread t = new Thread(new ThreadStart(DrawList));
            //t.Start();
        }

        private void OnClosed(object sender, EventArgs e)
        {
            DBHelper.UpdateRealtime(DataReader.GetRealTimeList());
        }
    }
}
