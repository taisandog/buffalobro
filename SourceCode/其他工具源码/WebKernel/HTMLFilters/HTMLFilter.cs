using System;
using System.Collections.Generic;
using System.Text;


namespace Buffalo.WebKernel.WebCommons.HTMLFilters
{
    public delegate void DoTicketValueFinished(TickValue value);
    public class HTMLFilter
    {
        //private Stack<string> stkInnerTick;//���ں��ı��ı�ǩ��ջ
        public static event DoTicketValueFinished OnSetTicketValueFinished;
        private static Dictionary<string, bool> dicTick = new Dictionary<string, bool>();//��¼����ı�ǩ
        //StringBuilder sbRet = new StringBuilder();
        private Stack<char> stkChr = new Stack<char>();//��¼�����ŵ�ջ
        private Stack<char> stkChrEnd = new Stack<char>();//��¼�����Ž�����ջ
        private CharEnumerator enums;
        private Stack<string> stkTicks = new Stack<string>();//��¼���ͱ�ǩ��ջ
        private Stack<string> stkTicksEnd = new Stack<string>();//��¼���ͱ�ǩ��ջ
        private HTMLFilter(string sDetail)
        {
            enums = sDetail.GetEnumerator();
        }

        /// <summary>
        /// ֪ͨ�ⲿ��ǩ������Ϣ�������
        /// </summary>
        /// <param name="value"></param>
        internal static void CallTacketValueFinish(TickValue value) 
        {
            if (OnSetTicketValueFinished != null)
            {
                OnSetTicketValueFinished(value);
            }
        }

        public static string Decode(string sDetail)
        {
            HTMLFilter decoder = new HTMLFilter(sDetail);
            return decoder.ReadHtml();
        }
        /// <summary>
        /// ���UBB��ǩ������
        /// </summary>
        /// <param name="tickName">��ǩ��</param>
        /// <param name="info">��ǩ��Ϣ</param>
        /// <param name="hasInnerText)">�Ƿ����ı�</param>
        public static void AddDecoder(string tickName, DoDecode info, bool hasInnerText)
        {
            Decoder.AddDecoder(tickName, info);
            dicTick.Add(tickName, hasInnerText);
        }

        

        /// <summary>
        /// ����UBB�ַ���
        /// </summary>
        /// <returns></returns>
        public string ReadHtml()
        {
            StringBuilder tmpNumber = new StringBuilder();

            while (enums.MoveNext())
            {
                char chr = enums.Current;
                if (chr == '<') //����������žͿ�ʼ��ȡ��ǩ����
                {

                    stkChr.Push('>');

                    string attStr = ReadHtml();//���¶�ȡ
                    if (stkTicks.Count > 0 && stkTicksEnd.Count > 0 && stkTicks.Peek() == stkTicksEnd.Peek())
                    {
                        tmpNumber.Append("<" + attStr);

                        stkChr.Pop();
                        break;
                    }
                    else if (stkChr.Count > 0 && stkChrEnd.Count > 0)
                    {
                        stkChr.Pop();
                        stkChrEnd.Pop();
                        HtmlTickInfo info = new HtmlTickInfo(attStr);
                        bool hasInner = false;

                        if (dicTick.TryGetValue(info.TickName, out hasInner))
                        {
                            if (!info.IsEnd)
                            {
                                if (hasInner) //��������ľͼ������ı�
                                {
                                    stkTicks.Push(info.TickName);
                                    string strInner = ReadHtml();
                                    if (stkTicks.Count > 0 && stkTicksEnd.Count > 0)
                                    {
                                        stkTicks.Pop();
                                        stkTicksEnd.Pop();
                                        info.InnerText = strInner;
                                        DoDecode decode = Decoder.GetDecoder(info.TickName);

                                        if (decode != null)
                                        {
                                            tmpNumber.Append(decode(info));
                                        }
                                    }
                                    else
                                    {
                                        string str = "<" + info.TickName;
                                        foreach (TickValue tvalue in info.TickAttributes)
                                        {
                                            str += " " + tvalue.AttributeName + "=\"" + tvalue.AttributeValue+"\"";
                                        }
                                        str += ">" + strInner;
                                        tmpNumber.Append(str);
                                    }
                                }
                                else 
                                {
                                    DoDecode decode = Decoder.GetDecoder(info.TickName);
                                    tmpNumber.Append(decode(info));
                                }
                            }
                            else if (info.IsEnd && stkTicks.Count > 0 && stkTicks.Peek() == info.TickName)
                            {
                                stkTicksEnd.Push(info.TickName);//��ӽ������
                                break;
                            }
                            else
                            {
                                tmpNumber.Append("</" + attStr + ">");
                                //break;
                            }
                        }
                        else
                        {
                            //string str = "<" ;
                            //if (info.IsEnd) 
                            //{
                            //    str += "/";
                            //}
                            //str += info.TickName;
                            //foreach (TickValue tvalue in info.TickAttributes)
                            //{
                            //    str += " " + tvalue.AttributeName + "=\"" + tvalue.AttributeValue + "\"";
                            //}
                            //str += ">";
                            tmpNumber.Append("<"+attStr+">");
                        }
                    }
                    else
                    {
                        tmpNumber.Append("<" + attStr);

                        stkChr.Pop();
                    }
                }
                else if (chr == '>' && stkChr.Count > 0)//��ǩ����������ѭ��
                {
                    stkChrEnd.Push('>');
                    break;
                }
                else
                {
                    tmpNumber.Append(chr);
                }
            }
            return tmpNumber.ToString();
        }

        

        

    }
}
