using System;
using System.Data;
using System.Windows.Forms;
using Dao;

namespace ImageOfStock
{
    public partial class StockSearch : UserControl
    {
        private int listMaxSize = 10;
        public delegate void ToStock(string stockId);

        private ToStock toStockDel;
        public ToStock ToStockDel
        {
            set
            {
                toStockDel = value;
            }
        }

        public StockSearch()
        {
            InitializeComponent();
            input.TextChanged += new EventHandler(InputTextChanged);
            CreateListHeader();
            listView1.FullRowSelect = true;
            listView1.KeyPress += new KeyPressEventHandler(ListKeyPressed);
            listView1.MouseDoubleClick += new MouseEventHandler(ListDoubleClick);
        }

        private void CreateListHeader()
        {
            ColumnHeader ch0 = new ColumnHeader();
            ch0.Width = 100;
            ch0.TextAlign = HorizontalAlignment.Center;
            listView1.Columns.Add(ch0);

            ColumnHeader ch1 = new ColumnHeader();
            ch1.Width = 100;
            ch1.TextAlign = HorizontalAlignment.Center;
            listView1.Columns.Add(ch1);
        }

        public void SearchText(char c)
        {
            input.Text += c;
        }
        public void TextBack()
        {
            input.Text = input.Text.Substring(0, input.Text.Length - 1);
        }

        private void InputTextChanged(object sender, EventArgs e)
        {
            if(input.Text.Length == 0)
            {
                this.Hide();
            }
            else
            {
                LoadPosibleStocks();
            }
        }

        private void LoadPosibleStocks()
        {
            var result = from a in GlobalData.StocksCode.AsEnumerable()
                         where (a.Field<object>("stockId").ToString().Contains(input.Text)) ||
                         (a.Field<object>("abbr").ToString().Contains(input.Text))
                         select a;
            listView1.Items.Clear();
            listView1.BeginUpdate();
            int i = 0;
            foreach(DataRow r in result)
            {
                if (i >= listMaxSize) break;
                AddRow(r);
                ++i;
            }
            listView1.EndUpdate();
            if(listView1.Items.Count > 0)
            {
                listView1.Focus();
                listView1.Items[0].Selected = true;
            }
        }

        private void AddRow(DataRow r)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Name = r["stockId"].ToString();
            String code = r["stockId"].ToString();
            lvi.Text = code.Length == 6?code : code.Substring(1);
            lvi.SubItems.Add(r["name"].ToString());
            listView1.Items.Add(lvi);
        }

        private void ListKeyPressed(object sender, KeyPressEventArgs e)
        {
            if (
               (e.KeyChar >= 'a' && e.KeyChar <= 'z') ||
               (e.KeyChar >= 'A' && e.KeyChar <= 'Z') ||
               (e.KeyChar >= '0' && e.KeyChar <= '9'))
            {
                SearchText(e.KeyChar);
            }
            else if(e.KeyChar == '\b')
            {
                TextBack();
            }
            else if(e.KeyChar == '\r')
            {
                toStockDel(listView1.SelectedItems[0].Name);
                input.Text = "";
                Hide();
            }
        }

        private void ListDoubleClick(object sender, MouseEventArgs e)
        {
            toStockDel(listView1.SelectedItems[0].Name);
            input.Text = "";
            Hide();
        }
    }
}
