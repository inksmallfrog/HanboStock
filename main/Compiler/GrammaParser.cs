using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class GrammaParser
    {
        LatexParser latex = LatexParser.Instance;

        public bool IsGramma()
        {
            latex.ReadNext(); //不应该写在这里！！

            if (!IsStatement()) { return false; }
            if (latex.Unit.type != LatexParser.LatexType.Semicolon) { return false; }
            else { latex.ReadNext(); }
            if (!latex.End) { return IsGramma(); }
            
            return true;
        }

        public bool IsStatement()
        {
            if (latex.Unit.type != LatexParser.LatexType.Id) { return false; }
            else { latex.ReadNext(); }
            return IsAfterStatement();
        }

        public bool IsAfterStatement()
        {
            if (latex.Unit.type == LatexParser.LatexType.Assign)
            {
                latex.ReadNext();
                if (!IsValue()) { return false; }
                return IsOptionalState();
            }
            else if (latex.Unit.type == LatexParser.LatexType.Output)
            {
                latex.ReadNext();
                return IsValue();
            }
            else return false;
        }

        public bool IsOptionalState()
        {
            if (latex.Unit.type == LatexParser.LatexType.Comma)
            {
                latex.ReadNext();
                return latex.Unit.type == LatexParser.LatexType.Id;
            }
            return true;
        }

        public bool IsValue()
        {
            if (!IsTerm()) return false;
            return IsAfterTerm();
        }

        public bool IsTerm()
        {
            if(latex.Unit.type == LatexParser.LatexType.Id){
                latex.ReadNext();
                return true;
            }
            return IsFunc();
        }

        public bool IsAfterTerm()
        {
            if (latex.Unit.type != LatexParser.LatexType.Operator) return false;
            else { latex.ReadNext(); }
            return IsTerm();
        }

        public bool IsFunc()
        {
            if (latex.Unit.type != LatexParser.LatexType.Id) return false;
            else { latex.ReadNext(); }
            if (latex.Unit.type != LatexParser.LatexType.LeftBrake) return false;
            else { latex.ReadNext(); }
            if (!IsParameters()) return false;
            if (latex.Unit.type != LatexParser.LatexType.RightBrake) return false;
            else { latex.ReadNext(); }
            return true;
        }

        public bool IsParameters()
        {
            if (!IsValue()) return false;
            if (!IsAfterParameters()) return false;
            return true;
        }

        public bool IsAfterParameters()
        {
            if (latex.Unit.type != LatexParser.LatexType.Comma) return true;
            else { 
                latex.ReadNext();
                return IsParameters();
            }
        }

        public bool IsBoolParameter()
        {
            if (latex.Unit.type != LatexParser.LatexType.Id) return false;
            else { latex.ReadNext(); }
            if (latex.Unit.type != LatexParser.LatexType.BoolOperator) return false;
            else { latex.ReadNext(); }
            if (latex.Unit.type != LatexParser.LatexType.Id) return false;
            else { latex.ReadNext(); }
            return IsAfterBoolParameter();
        }

        public bool IsAfterBoolParameter()
        {
            if (latex.Unit.type != LatexParser.LatexType.BoolOperator) return true;
            else { latex.ReadNext(); }
            if (latex.Unit.type != LatexParser.LatexType.Id) return false;
            else { latex.ReadNext(); }
            return IsAfterBoolParameter();
        }
    }
}
