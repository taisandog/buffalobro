using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    public delegate double DelExpressFunctionHandle(params double[] nums);
    public class ExpressFunctions 
    {
        private static Dictionary<string, DelExpressFunctionHandle> dicHandle = InitHandle();

        private static Dictionary<string, DelExpressFunctionHandle> InitHandle() 
        {
            Dictionary<string, DelExpressFunctionHandle> dic = new Dictionary<string, DelExpressFunctionHandle>();
            dic["sqrt"] = DoSqrt;
            dic["pow"] = DoPow;
            return dic;
        }

        /// <summary>
        /// 根据函数名获取函数
        /// </summary>
        /// <param name="fName"></param>
        /// <returns></returns>
        public static DelExpressFunctionHandle GetHandle(string fName) 
        {
            DelExpressFunctionHandle ret=null;
            dicHandle.TryGetValue(fName, out ret);
            return ret;
        }

        /// <summary>
        /// 开根号
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static double DoSqrt(params double[] nums) 
        {
            return Math.Sqrt(nums[0]);
        }

        /// <summary>
        /// 次幂
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static double DoPow(params double[] nums)
        {
            return Math.Pow(nums[0], nums[1]);
        }

    }

    public class ExpressionsEval
    {
        private CharEnumerator strEnum;
        public ExpressionsEval()
        {
            
        }
        private Stack<char> stkChr = new Stack<char>();//记录括号的栈
        private static string[] symbolPower ={"*/","+-" };//符号的优先级
        private Dictionary<string, double> dicParams = new Dictionary<string, double>();

        /// <summary>
        /// 变量集合
        /// </summary>
        public Dictionary<string, double> Params 
        {
            get 
            {
                return dicParams;
            }
        }
        /// <summary>
        /// 计算带变量的表达式
        /// </summary>
        /// <param name="expressions">表达式</param>
        /// <returns></returns>
        public double Eval(string expressions) 
        {
            strEnum = expressions.GetEnumerator();
            return Eval();
        }

        /// <summary>
        /// 计算表达式的值
        /// </summary>
        /// <returns></returns>
        private double Eval() 
        {
            LinkedList<double> stkNumber = new LinkedList<double>();//数字
            LinkedList<char> stkSymbol = new LinkedList<char>();//运算符
            StringBuilder tmpNumber=new StringBuilder();
            while (strEnum.MoveNext())
            {
                char cur = strEnum.Current;

                if (cur == '(') //如果是左括号就开始算表达式
                {
                    stkChr.Push(')');
                    double num = 0;
                    if (tmpNumber.Length > 0)
                    {
                        DelExpressFunctionHandle handle = ExpressFunctions.GetHandle(tmpNumber.ToString().ToLower());
                        if (handle == null)
                        {
                            throw new Exception("找不到函数" + tmpNumber.ToString());
                        }
                        List<double> lstParams = new List<double>();
                        while (stkChr.Count > 0 && stkChr.Peek() == ')')
                        {
                            lstParams.Add(Eval());
                        }
                        num = handle(lstParams.ToArray());
                        tmpNumber.Remove(0, tmpNumber.Length);
                    }
                    else
                    {
                        num = Eval();
                    }
                    stkNumber.AddLast(num);
                    
                }
                else if (cur == ',') 
                {
                    if (stkChr.Count < 0)
                    {
                        throw new Exception("表达式错误");
                    }
                    break;
                }
                else if (cur == ')')//表达式结束，跳出循环
                {
                    if (stkChr.Count < 0)
                    {
                        throw new Exception("表达式错误");
                    }
                    stkChr.Pop();
                    break;
                }
                else
                {
                    //if (IsNumber(cur))//如果是数组就加到临时数字字符串
                    //{
                    //    tmpNumber.Append(cur);
                    //    continue;
                    //}
                    //else
                    if (IsOperators(cur)) //如果是符号
                    {
                        stkSymbol.AddLast(cur);//加到符号列表
                        if (tmpNumber.Length > 0)
                        {

                            stkNumber.AddLast(GetNumber(tmpNumber));//加到数字列表
                            tmpNumber.Remove(0, tmpNumber.Length);//清空临时数字字符串
                        }
                    }
                    else
                    {

                        tmpNumber.Append(cur);
                        continue;
                    }
                }
            }
            if (tmpNumber.Length > 0)
            {
                stkNumber.AddLast(GetNumber(tmpNumber));//加到数字列表
            }
            return Calculation(stkNumber, stkSymbol);
        }

        /// <summary>
        /// 从字符串缓冲中获取值
        /// </summary>
        /// <param name="tmpNumber">字符串缓冲</param>
        /// <returns></returns>
        private double GetNumber(StringBuilder tmpNumber) 
        {
            string strTmp = tmpNumber.ToString();
            double curNumber = 0;
            if (!double.TryParse(strTmp, out curNumber))//如果strTmp为非数字就为变量则给curNumber赋变量的值
            {
                if (!dicParams.TryGetValue(strTmp, out curNumber))
                {
                    throw new Exception("找不到变量:" + strTmp);
                }
            }
            return curNumber;
        }

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="stkNumber">数字栈</param>
        /// <param name="stkSymbol">符号栈</param>
        /// <returns></returns>
        private double Calculation(LinkedList<double> stkNumber, LinkedList<char> stkSymbol) 
        {
            
            foreach(string strCurPower in symbolPower)
            {
                LinkedList<double> tmpNumber = new LinkedList<double>();//本次计算后的剩余数字
                LinkedList<char> tmpSymbol = new LinkedList<char>();//本次计算后的剩余运算符
                while (stkSymbol.Count > 0)
                {
                    char curChr = stkSymbol.First.Value;
                    stkSymbol.RemoveFirst();
                    if (strCurPower.IndexOf(curChr) >= 0)
                    {
                        //计算时候给数字出栈
                        double number1 = stkNumber.First.Value;
                        stkNumber.RemoveFirst();
                        double number2 = stkNumber.First.Value;
                        stkNumber.RemoveFirst();

                        stkNumber.AddFirst(Result(number1, number2, curChr));//把结果插回第一个数
                    }
                    else 
                    {
                        tmpNumber.AddLast(stkNumber.First.Value);
                        stkNumber.RemoveFirst();
                        tmpSymbol.AddLast(curChr);
                    }
                }
                if (stkNumber.Count > 0) 
                {
                    tmpNumber.AddLast(stkNumber.First.Value);
                    stkNumber.RemoveFirst();
                }
                stkNumber = tmpNumber;
                stkSymbol = tmpSymbol;
            }

            return stkNumber.First.Value;
        }

        /// <summary>
        /// 判断是否数字
        /// </summary>
        /// <param name="chr">字符</param>
        /// <returns></returns>
        private bool IsNumber(char chr)
        {
            if (char.IsDigit(chr) || chr == '.')//如果是数字就继续读
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断是否运算符
        /// </summary>
        /// <param name="chr">字符</param>
        /// <returns></returns>
        private bool IsOperators(char chr) 
        {
            foreach (string curSymbol in symbolPower)
            {
                if (curSymbol.IndexOf(chr) >= 0) 
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否数字
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns></returns>
        private bool IsNumber(string str)
        {
            for (int i = 0; i < str.Length; i++) 
            {
                if (!((str[i] >= '0' && str[i] <= '9') || str[i] == '.')) 
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 计算表达式的结果
        /// </summary>
        /// <param name="number1">数字1</param>
        /// <param name="number2">数字2</param>
        /// <param name="symbol">运算符</param>
        /// <returns></returns>
        private double Result(double number1, double number2, char symbol) 
        {
            if (symbol == '+') 
            {
                return number1 + number2;
            }
            if (symbol == '-')
            {
                return number1 - number2;
            }
            if (symbol == '*')
            {
                return number1 * number2;
            }
            if (symbol == '/')
            {
                return number1 / number2;
            }
            return double.NaN;
        }


    }
}


/*--------------------例子------------------
ExpressionsEval exp = new ExpressionsEval();

double val = exp.Eval("((48.9+0.4+0.4)+54*2+38.5+(5.2+0.4)*1.414*2)*3.15+22*(7.5-6.3)*2+51.9*1.35+(62.2-1.2-5.2-1.2-0.3)*1.35*2+(51.9-5.2-1.2-0.3-5.2-1.2-0.3)*1.35+7.5*12*2");
double val2 = ((48.9 + 0.4 + 0.4) + 54 * 2 + 38.5 + (5.2 + 0.4) * 1.414 * 2) * 3.15 + 22 * (7.5 - 6.3) * 2 + 51.9 * 1.35 + (62.2 - 1.2 - 5.2 - 1.2 - 0.3) * 1.35 * 2 + (51.9 - 5.2 - 1.2 - 0.3 - 5.2 - 1.2 - 0.3) * 1.35 + 7.5 * 12 * 2;
Response.Write(val+"<br/>");
Response.Write(val2 + "<br/>");

exp.Params["x"] = 16;

double val3 = exp.Eval("4+sqrt(x)");
Response.Write(val3 + "<br/>");

exp.Params["x"] = 4;
double val4 = exp.Eval("4+pow(x+5,3)");
Response.Write(val4 + "<br/>");
----------------------------------------*/