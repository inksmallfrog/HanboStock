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
    public partial class kLinePanel : UserControl
    {
        public kLinePanel()
        {
            InitializeComponent();
        }

        public void Draw()
        {
            kLineGraph.CandleGraph();
        }

        public void BindData(DataTable histories, DataRow current)
        {
            kLineGraph.BindData(histories);
            kLineGraph.RefreshGraph();
            kLineGraph.Enabled = true;
        }
    }
}
