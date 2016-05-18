/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace main
{
    public partial class Form_compiler : Form
    {
        public Form_compiler()
        {
            InitializeComponent();
            //List<HistoryStockData> data = null;
            //switch (Program.type)
            //{
                //case 1: data = Data.getHistoryDataFromFile(Program.codes, 100, ".\\data"); break;
                //case 3: data = Data.getHistoryData(Program.codes, 100); break;
            //}
            //compiler = new Compiler(data);
        }

        p//rivate Compiler compiler;

        private void textBox1_FocusEntered(object sender, EventArgs e)
        {
            if (textBox_s1.Text == "请在此输入输出量")
            {
                textBox_s1.ForeColor = Color.Black;
                textBox_s1.Text = "";
            }

        }

        private void textBox1_FocusLost(object sender, EventArgs e)
        {
            if (textBox_s1.Text == "")
            {
                textBox_s1.ForeColor = Color.LightGray;
                textBox_s1.Text = "请在此输入输出量";
            }
        }
        private void richTextBox_input_FocusEntered(object sender, EventArgs e)
        {
            if (richTextBox_input.Text == "请在此输入公式")
            {
                richTextBox_input.ForeColor = Color.Black;
                richTextBox_input.Text = "";
            }

        }

        private void richTextBox_input_FocusLost(object sender, EventArgs e)
        {
            if (richTextBox_input.Text == "")
            {
                richTextBox_input.ForeColor = Color.LightGray;
                richTextBox_input.Text = "请在此输入公式";
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定清空公式吗？此操作不可撤销", "警告", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                richTextBox_input.Text = "";
                textBox_s1.Text = "";
            }
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            String result = " ";
            try
            {
                result = compiler.invoke(richTextBox_input.Text, textBox_s1.Text);
                MessageBox.Show(textBox_s1.Text + " = " + result, "结果", MessageBoxButtons.OK);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "警告", MessageBoxButtons.OK);
            }
        }

        private void login_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Visible = false;
            e.Cancel = true;                   //禁止自己关闭，因为初始化太TMD的慢了
        }

    }
}*/
