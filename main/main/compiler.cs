/*using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using Data;
namespace main
{
    public class Compiler
    {
        //static void Main(string[] args)
        //{
        //    List<HistoryStockData> data = Data.getHistoryData("sh600158", 100);
        //    Compiler cp = new Compiler(data);
        //    Console.WriteLine("初始化完成");
        //    Console.WriteLine("请输入公式内容：");
        //    string input = Console.ReadLine();
        //    Console.WriteLine("请输入输出量：");
        //    string s1 = Console.ReadLine();
        //    cp.invoke(input, s1);
        //    Console.ReadLine();

        //}
        private List<string> high = new List<string>();
       private List<string> low = new List<string>();
       private List<string> close = new List<string>();
       private List<string> open = new List<string>();

       public Compiler(List<DataDefine.StockHistory> data)
        {//把股票数据初始化
            for (int i = 0; i < 100; i++)
            {
               
             open.Add(data[i].open.ToString());
             close.Add(data[i].close.ToString());
              high.Add(data[i].high.ToString());
              low.Add(data[i].low.ToString());
            }
        }
        public string invoke(string input,string s1)
        {
            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

            // 2.ICodeComplier
            ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

            // 3.CompilerParameters
            CompilerParameters objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.Add("System.dll");
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;

            // 4.CompilerResults
            CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, code(input,s1));
            if (cr.Errors.HasErrors)
            {
                String error = "编译错误，错误原因是：" + '\n';
                foreach (CompilerError err in cr.Errors)
                {
                    error += err.ErrorText + '\n';
                    
                    Console.WriteLine(err.ErrorText);
                }
                throw new Exception(error);
            }
            else
            {
                // 通过反射，调用HelloWorld的实例
                Assembly objAssembly = cr.CompiledAssembly;
                object objHelloWorld = objAssembly.CreateInstance("DynamicCodeGenerate.HelloWorld");
                MethodInfo objMI = objHelloWorld.GetType().GetMethod("OutPut");
 
               string result=objMI.Invoke(objHelloWorld, null).ToString();
               //Console.WriteLine("结果：");
               //Console.WriteLine(result);
               return result;
            }
        }
        public string code(string s1,string s)
        {StringBuilder sb = new StringBuilder();   //初始化
        for (int i = 0; i < high.Count; i++)
        {
            sb.Append("high.Add(" + high[i] + ");");
            sb.Append(Environment.NewLine);
        }
        for (int i = 0; i < high.Count; i++)
        {
            sb.Append("low.Add(" + low[i] + ");");
            sb.Append(Environment.NewLine);
        }
        for (int i = 0; i < high.Count; i++)
        {
            sb.Append("close.Add(" + close[i] + ");");
            sb.Append(Environment.NewLine);
        }
        for (int i = 0; i < high.Count; i++)
        {
            sb.Append("open.Add(" + open[i] + ");");
            sb.Append(Environment.NewLine);
        }
            string code = @"using System;
using System.Collections.Generic;
       namespace DynamicCodeGenerate
    { 
      public class HelloWorld
  {  List<double> high=new List<double>();
     List<double> low=new List<double>();
     List<double> close=new List<double>();
     List<double> open=new List<double>();
       
      public int Short=12 ,Long=26,Mid=9;
      public int max(int a,int b)
    {
        return a>b?a:b;
    }
   public int min(int a,int b)
    {
        return a>b?b:a;
    }
 public double ema(List<double> data, int n)
      {
          double x=data[0];
          double y=x;
          for(int i=1;i<n;i++){
              y = (2 * data[i] + n * y) / (n + 2);
  }
  return y;
      }
    public double macd(List<double> data){
        double dif=ema(data,Short);
       double dea=ema(data,Long);
       return (dif-dea)*2;
  }
    public double llv(List<double> data, int n)
    {
        double min =data[0];

        for (int i = 1; i < n; i++)
        {
            if (data[i] < min)
            {
                min = data[i];
            }
        }
        return min;
    }
    public double hhv(List<double> data, int n)
    {
        double max = data[0];

        for (int i = 1; i < n; i++)
        {
            if (data[i] < max)
            {
                max = data[i];
            }
        }
        return max;
    }
    public double sma(List<double> data, int n,double m)
    {
        double x = data[0];
        double y = x;
        for (int i = 1; i < n; i++)
        {
            y = (m *data[i] + (n-m) * y) / n;
        }
        return y;
    }
    public double dma(List<double> data,  double a)
    {
        double x = data[0];
        double y = x;
        for (int i = 1; i < data.Count; i++)
        {
            y = (a * data[i] + (1 - a) * y) ;
        }
        return y;
    }
      public double OutPut()
    {" + sb.ToString()+s1+@"
      return " + s + @";
    }
  } 
}   ";
            return code;
        }
        }
    }
public class Formula                    //用到的公式
{
   
    public double ema(List<string> data, int n)
      {
          double x=Convert.ToDouble(data[0]);
          double y=x;
          for(int i=1;i<n;i++){
              y = (2 * Convert.ToDouble(data[i]) + n * y) / (n + 2);
  }
  return y;
      }
    public double macd(List<string> data){
        double dif=ema(data,18);
       double dea=ema(data,26);
       return (dif-dea)*2;
  }
    public double llv(List<string> data, int n)
    {
        double min = Convert.ToDouble(data[0]);

        for (int i = 1; i < n; i++)
        {
            if (Convert.ToDouble(data[i]) < min)
            {
                min = Convert.ToDouble(data[i]);
            }
        }
        return min;
    }
    public double hhv(List<string> data, int n)
    {
        double max = Convert.ToDouble(data[0]);

        for (int i = 1; i < n; i++)
        {
            if (Convert.ToDouble(data[i]) < max)
            {
                max = Convert.ToDouble(data[i]);
            }
        }
        return max;
    }
    public double sma(List<string> data, int n,double m)
    {
        double x = Convert.ToDouble(data[0]);
        double y = x;
        for (int i = 1; i < n; i++)
        {
            y = (m * Convert.ToDouble(data[i]) + (n-m) * y) / n;
        }
        return y;
    }
    public double dma(List<string> data,  double a)
    {
        double x = Convert.ToDouble(data[0]);
        double y = x;
        for (int i = 1; i < data.Count; i++)
        {
            y = (a * Convert.ToDouble(data[i]) + (1 - a) * y) ;
        }
        return y;
    }
}*/



