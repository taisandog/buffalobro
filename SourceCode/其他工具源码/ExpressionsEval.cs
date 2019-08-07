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
        /// ���ݺ�������ȡ����
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
        /// ������
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static double DoSqrt(params double[] nums) 
        {
            return Math.Sqrt(nums[0]);
        }

        /// <summary>
        /// ����
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
        private Stack<char> stkChr = new Stack<char>();//��¼���ŵ�ջ
        private static string[] symbolPower ={"*/","+-" };//���ŵ����ȼ�
        private Dictionary<string, double> dicParams = new Dictionary<string, double>();

        /// <summary>
        /// ��������
        /// </summary>
        public Dictionary<string, double> Params 
        {
            get 
            {
                return dicParams;
            }
        }
        /// <summary>
        /// ����������ı��ʽ
        /// </summary>
        /// <param name="expressions">���ʽ</param>
        /// <returns></returns>
        public double Eval(string expressions) 
        {
            strEnum = expressions.GetEnumerator();
            return Eval();
        }

        /// <summary>
        /// ������ʽ��ֵ
        /// </summary>
        /// <returns></returns>
        private double Eval() 
        {
            LinkedList<double> stkNumber = new LinkedList<double>();//����
            LinkedList<char> stkSymbol = new LinkedList<char>();//�����
            StringBuilder tmpNumber=new StringBuilder();
            while (strEnum.MoveNext())
            {
                char cur = strEnum.Current;

                if (cur == '(') //����������žͿ�ʼ����ʽ
                {
                    stkChr.Push(')');
                    double num = 0;
                    if (tmpNumber.Length > 0)
                    {
                        DelExpressFunctionHandle handle = ExpressFunctions.GetHandle(tmpNumber.ToString().ToLower());
                        if (handle == null)
                        {
                            throw new Exception("�Ҳ�������" + tmpNumber.ToString());
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
                        throw new Exception("���ʽ����");
                    }
                    break;
                }
                else if (cur == ')')//���ʽ����������ѭ��
                {
                    if (stkChr.Count < 0)
                    {
                        throw new Exception("���ʽ����");
                    }
                    stkChr.Pop();
                    break;
                }
                else
                {
                    //if (IsNumber(cur))//���������ͼӵ���ʱ�����ַ���
                    //{
                    //    tmpNumber.Append(cur);
                    //    continue;
                    //}
                    //else
                    if (IsOperators(cur)) //����Ƿ���
                    {
                        stkSymbol.AddLast(cur);//�ӵ������б�
                        if (tmpNumber.Length > 0)
                        {

                            stkNumber.AddLast(GetNumber(tmpNumber));//�ӵ������б�
                            tmpNumber.Remove(0, tmpNumber.Length);//�����ʱ�����ַ���
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
                stkNumber.AddLast(GetNumber(tmpNumber));//�ӵ������б�
            }
            return Calculation(stkNumber, stkSymbol);
        }

        /// <summary>
        /// ���ַ��������л�ȡֵ
        /// </summary>
        /// <param name="tmpNumber">�ַ�������</param>
        /// <returns></returns>
        private double GetNumber(StringBuilder tmpNumber) 
        {
            string strTmp = tmpNumber.ToString();
            double curNumber = 0;
            if (!double.TryParse(strTmp, out curNumber))//���strTmpΪ�����־�Ϊ�������curNumber��������ֵ
            {
                if (!dicParams.TryGetValue(strTmp, out curNumber))
                {
                    throw new Exception("�Ҳ�������:" + strTmp);
                }
            }
            return curNumber;
        }

        /// <summary>
        /// ������ʽ
        /// </summary>
        /// <param name="stkNumber">����ջ</param>
        /// <param name="stkSymbol">����ջ</param>
        /// <returns></returns>
        private double Calculation(LinkedList<double> stkNumber, LinkedList<char> stkSymbol) 
        {
            
            foreach(string strCurPower in symbolPower)
            {
                LinkedList<double> tmpNumber = new LinkedList<double>();//���μ�����ʣ������
                LinkedList<char> tmpSymbol = new LinkedList<char>();//���μ�����ʣ�������
                while (stkSymbol.Count > 0)
                {
                    char curChr = stkSymbol.First.Value;
                    stkSymbol.RemoveFirst();
                    if (strCurPower.IndexOf(curChr) >= 0)
                    {
                        //����ʱ������ֳ�ջ
                        double number1 = stkNumber.First.Value;
                        stkNumber.RemoveFirst();
                        double number2 = stkNumber.First.Value;
                        stkNumber.RemoveFirst();

                        stkNumber.AddFirst(Result(number1, number2, curChr));//�ѽ����ص�һ����
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
        /// �ж��Ƿ�����
        /// </summary>
        /// <param name="chr">�ַ�</param>
        /// <returns></returns>
        private bool IsNumber(char chr)
        {
            if (char.IsDigit(chr) || chr == '.')//��������־ͼ�����
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// �ж��Ƿ������
        /// </summary>
        /// <param name="chr">�ַ�</param>
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
        /// �ж��Ƿ�����
        /// </summary>
        /// <param name="str">�ַ���</param>
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
        /// ������ʽ�Ľ��
        /// </summary>
        /// <param name="number1">����1</param>
        /// <param name="number2">����2</param>
        /// <param name="symbol">�����</param>
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


/*--------------------����------------------
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