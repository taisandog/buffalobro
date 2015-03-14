using System;
using System.Collections.Generic;
using System.Text;


namespace Buffalo.WebKernel.WebCommons.HTMLFilters
{
    public delegate void DoTicketValueFinished(TickValue value);
    public class HTMLFilter
    {
        //private Stack<string> stkInnerTick;//带内含文本的标签的栈
        public static event DoTicketValueFinished OnSetTicketValueFinished;
        private static Dictionary<string, bool> dicTick = new Dictionary<string, bool>();//记录允许的标签
        //StringBuilder sbRet = new StringBuilder();
        private Stack<char> stkChr = new Stack<char>();//记录尖括号的栈
        private Stack<char> stkChrEnd = new Stack<char>();//记录尖括号结束的栈
        private CharEnumerator enums;
        private Stack<string> stkTicks = new Stack<string>();//记录解释标签的栈
        private Stack<string> stkTicksEnd = new Stack<string>();//记录解释标签的栈
        private HTMLFilter(string sDetail)
        {
            enums = sDetail.GetEnumerator();
        }

        /// <summary>
        /// 通知外部标签属性信息加载完毕
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
        /// 添加UBB标签解释器
        /// </summary>
        /// <param name="tickName">标签名</param>
        /// <param name="info">标签信息</param>
        /// <param name="hasInnerText)">是否含有文本</param>
        public static void AddDecoder(string tickName, DoDecode info, bool hasInnerText)
        {
            Decoder.AddDecoder(tickName, info);
            dicTick.Add(tickName, hasInnerText);
        }

        

        /// <summary>
        /// 解释UBB字符串
        /// </summary>
        /// <returns></returns>
        public string ReadHtml()
        {
            StringBuilder tmpNumber = new StringBuilder();

            while (enums.MoveNext())
            {
                char chr = enums.Current;
                if (chr == '<') //如果是左括号就开始读取标签内容
                {

                    stkChr.Push('>');

                    string attStr = ReadHtml();//向下读取
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
                                if (hasInner) //如果有正文就继续读文本
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
                                stkTicksEnd.Push(info.TickName);//添加结束标记
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
                else if (chr == '>' && stkChr.Count > 0)//标签结束，跳出循环
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
