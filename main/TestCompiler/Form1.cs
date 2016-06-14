using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Compiler;

namespace TestCompiler
{
    public partial class Form1 : Form
    {
        LatexParser latexParser;
        public Form1()
        {
            InitializeComponent();
            latexParser = LatexParser.Instance;
            latexParser.TextBox = formulaBox;
        }

        private void compileButton_Click(object sender, EventArgs e)
        {
            ResultShow();
        }

        private void ResultShow()
        {
            latexParser.Reset();
            latexParser.ReadNext();
            string type;
            while (!latexParser.End)
            {
                resultBox.Text += latexParser.Unit.id + " (";
                switch (latexParser.Unit.type)
                {
                    case LatexParser.LatexType.Id:
                        type = "ID";
                        break;
                    case LatexParser.LatexType.Comma:
                        type = "Comma";
                        break;
                    case LatexParser.LatexType.BoolOperator:
                        type = "BoolOperator";
                        break;
                    case LatexParser.LatexType.Assign:
                        type = "Assign";
                        break;
                    case LatexParser.LatexType.LeftBrake:
                        type = "LeftBrake";
                        break;
                    case LatexParser.LatexType.Number:
                        type = "Number";
                        break;
                    case LatexParser.LatexType.Operator:
                        type = "Operator";
                        break;
                    case LatexParser.LatexType.Output:
                        type = "OutPut";
                        break;
                    case LatexParser.LatexType.RightBrake:
                        type = "RightBrake";
                        break;
                    case LatexParser.LatexType.Semicolon:
                        type = "Simicolon";
                        break;
                    default:
                        type = "Unknown!";
                        break;
                }
                resultBox.Text += type + ")\n";
                latexParser.ReadNext();
            }
        }
    }
}
