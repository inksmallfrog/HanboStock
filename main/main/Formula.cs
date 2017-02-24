using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageOfStock;
using Dao;

namespace StockMonitor
{
    public partial class Formula : Form
    {
        private DataTable formulaTable;

        private SingleGraph graph;
        public SingleGraph Graph
        {
            set { graph = value; }
        }

        public TextBox FormulaBox
        {
            get { return formulaBox; }
        }
            
        public Formula()
        {
            InitializeComponent();
            listBox.SelectedIndexChanged += SelectFormula;

            FormClosing += Close;
        }

        private void SelectFormula(object sender, EventArgs e)
        {
            DataRow row = formulaTable.Select("name = '" + listBox.SelectedItem.ToString() + "'")[0];
            nameBox.Text = row["name"].ToString();
            formulaBox.Text = row["formula"].ToString();
        }

        public void Init()
        {
            formulaTable = DBHelper.GetFormula();
            PaintList();
        }

        private void Close(Object sender, FormClosingEventArgs e) {
            this.Hide();
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            graph.Generate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DBHelper.SaveFormula(nameBox.Text, formulaBox.Text);
            DataRow row = formulaTable.NewRow();
            row["name"] = nameBox.Text;
            row["formula"] = formulaBox.Text;
            formulaTable.Rows.Add(row);
            PaintList();
        }

        private void PaintList()
        {
            listBox.Items.Clear();
            foreach (DataRow row in formulaTable.Rows){
                listBox.Items.Add(row["name"]);
            }
        }
    }
}
