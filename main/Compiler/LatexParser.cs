using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Compiler
{
    public class LatexParser
    {
        public enum LatexType
        {
            Id,
            Number,
            Comma,
            Semicolon,
            Assign,
            Output,
            LeftBrake,
            RightBrake,
            Operator,
            BoolOperator,
        }

        public struct LatexUnit
        {
            public string id;
            public LatexType type;
            public bool isHandled;
        }

        //单例
        private static LatexParser instance = new LatexParser();
        public static LatexParser Instance { get { return instance; } }

        //词法分析作用的文本框
        private RichTextBox textBox;
        public RichTextBox TextBox { set { textBox = value; } }

        //文本框内容转换为流
        private MemoryStream memoryStream;

        //词法单元
        private LatexUnit unit;
        public LatexUnit Unit { get { return unit; } }

        //下一个处理的字符
        private char next;

        //行号
        private int lineNum;
        public int LineNum { get { return lineNum; } }

        //列号
        private int columnNum;
        public int ColumnNum { get { return columnNum; } }

        //是否处理完成
        private bool end;
        public bool End { get { return end; } }

        //读入下一个词法单元
        public void ReadNext()
        {
            //判断是否读完
            if (next == 65535) { end = true; return; }

            //重置词法单元处理状态
            unit.isHandled = false;

            ReadBlack();        //读取空字符
            ReadSymbol();       //读取符号
            ReadOperator();     //读取运算符
            ReadNumber();       //读取数字
            ReadId();           //读取id
        }

        private void ReadBlack()
        {
            bool quit = false;
            while (!quit)
            {
                switch (next)
                {
                    case ' ':
                    case '\t':
                        next = (char)memoryStream.ReadByte();
                        break;
                    case '\n':
                        ++lineNum;
                        columnNum = 0;
                        next = (char)memoryStream.ReadByte();

                        break;
                    default:
                        quit = true;
                        break;
                }
            }
        }

        private void ReadSymbol()
        {
            switch (next)
            {
                case ';':
                    unit.id = ";";
                    unit.type = LatexType.Semicolon;
                    unit.isHandled = true;
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    break;
                case ',':
                    unit.id = ",";
                    unit.type = LatexType.Comma;
                    unit.isHandled = true;
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    break;
                default:
                    break;
            }
        }

        private void ReadOperator() {
            if (unit.isHandled) return;
            switch (next)
            {
                case ':':
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    if (next == '=')
                    {
                        unit.id = ":=";
                        unit.type = LatexType.Assign;
                        next = (char)memoryStream.ReadByte();
                        ++columnNum;
                    }
                    else
                    {
                        unit.id = ":";
                        unit.type = LatexType.Output;
                    }
                    unit.isHandled = true;
                    break;
                case '+':
                case '-':
                case '*':
                case '/':
                    unit.id = "" + next;
                    unit.type = LatexType.Operator;
                    unit.isHandled = true;
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    break;
                case '<':
                case '>':
                case '=':
                    unit.id = "" + next;
                    unit.type = LatexType.BoolOperator;
                    unit.isHandled = true;
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    break;
                case '(':
                    unit.id = "(";
                    unit.type = LatexType.LeftBrake;
                    unit.isHandled = true;
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    break;
                case ')':
                    unit.id = ")";
                    unit.type = LatexType.RightBrake;
                    unit.isHandled = true;
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    break;
                default:
                    break;
            }
        }

        private void ReadNumber() {
            if (unit.isHandled) return;

            bool isDouble = false;
            int num = 0;
            int point = 1;

            try{
                num = num * 10 + Int16.Parse(next.ToString());
                if (isDouble) point *= 10;
            }
            catch(FormatException e){
                if (next == '.') isDouble = true;
                else return;
            }

            unit.isHandled = true;
            unit.type = LatexType.Number;

            while (true)
            {
                next = (char)memoryStream.ReadByte();
                ++columnNum;
                try
                {
                    num = num * 10 + Int16.Parse(next.ToString());
                    if (isDouble) point *= 10;
                }
                catch (FormatException e)
                {
                    if (next == '.')
                    {
                        if (!isDouble) { isDouble = true; }
                        else { }//出错
                    }
                    else break;
                }
            }

            if (point > 0) unit.id = (num * 1.0 / point).ToString();
            else unit.id = num.ToString();
        }
        private void ReadId() {
            if (unit.isHandled) return;

            if (Char.IsLetter(next))
            {
                unit.isHandled = true;
                unit.id = "" + next;
                unit.type = LatexType.Id;
                while(true){
                    next = (char)memoryStream.ReadByte();
                    ++columnNum;
                    if (Char.IsDigit(next) || Char.IsLetter(next))
                    {
                        unit.id += next;
                    }
                    else return;
                }
            }

            else { } //出错
        }
        
        //重置词法处理对象
        public void Reset()
        {
            lineNum = 0;
            columnNum = 0;
            end = false;
            memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(textBox.Text), false);
            next = (char)memoryStream.ReadByte();
        }
    }
}
