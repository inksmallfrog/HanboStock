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
        Button current;

        public kLinePanel()
        {
            InitializeComponent();
            current = macd;
            current.ForeColor = Color.White;
        }

        public void Draw()
        {
            kLineGraph.CandleGraph();
        }

        public void Generate()
        {
            kLineGraph.Generate();
        }

        public void BindFormulaBox(TextBox box)
        {
            kLineGraph.Gramma.Latex.TextBox = box;
        }

        public void BindData(DataTable histories, DataRow current)
        {
            kLineGraph.BindData(histories);
            kLineGraph.RefreshGraph();
            kLineGraph.Enabled = true;
        }

        private void macd_Click(object sender, EventArgs e)
        {
            if (current == macd) return;
            kLineGraph.ChoseIndicator(KLineGraph.IndicatorType.MACD);
            current.ForeColor = Color.Red;
            current = macd;
            current.ForeColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (current == button1) return;
            kLineGraph.ChoseIndicator(KLineGraph.IndicatorType.StochasticOscillator);
            current.ForeColor = Color.Red;
            current = button1;
            current.ForeColor = Color.White;
        }
    }
}
