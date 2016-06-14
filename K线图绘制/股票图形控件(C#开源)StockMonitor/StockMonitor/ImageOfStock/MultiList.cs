using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageOfStock
{
    public partial class MultiList : UserControl
    {
        private SimpleTimeLineGraph[] stl = new SimpleTimeLineGraph[9];

        public MultiList()
        {
            InitializeComponent();
        }

        public void Draw(){
            for (int i = 0; i < tableLayoutPanel.RowCount; i++)
            {
                for (int j = 0; j < tableLayoutPanel.ColumnCount; j++)
                {
                    stl[j + i * 3] = tableLayoutPanel.GetControlFromPosition(j, i) as SimpleTimeLineGraph;
                    stl[j + i * 3].Draw();
                    stl[j + i * 3].MouseWheel += new MouseEventHandler(MultiMouseWheel);
                }
            }
        }

        public void BindAllData(List<String> stocksId, int pos)
        {
            for (int i = 0; i < 9; ++i)
            {
                stl[i].BindData(stocksId[i + pos]);
            }
        }

        private void MultiMouseWheel(object sender, MouseEventArgs e)
        {
            OnMouseWheel(e);
        }
    }
}
