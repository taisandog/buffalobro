using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.WebKernel.WebCommons.HTMLFilters
{
    /// <summary>
    /// 标签信息类
    /// </summary>
    public class HtmlTickInfo
    {
        private string tickName=null;
        private TickValueCollection tickAttributes;
        private string innerText=null;
        private bool isEnd;

        /// <summary>
        /// 根据html代码获取标签信息
        /// </summary>
        /// <param name="html">html代码</param>
        public HtmlTickInfo(string html) 
        {
            LoadTickInfo(html);
        }

        /// <summary>
        /// 读取标签信息
        /// </summary>
        /// <param name="strTick"></param>
        /// <returns></returns>
        private void LoadTickInfo(string strTick)
        {
            strTick = strTick.Trim();
            Dictionary<string, TickValue> tickValues = new Dictionary<string, TickValue>();
            tickAttributes = new TickValueCollection(tickValues);
            if (strTick != null && strTick != "")
            {
                if (strTick[0] == '/')
                {
                    IsEnd = true;
                    TickName = strTick.Substring(1, strTick.Length - 1).ToLower();
                }
                else
                {
                    IsEnd = false;
                    char point = '\0';//字符串符号的栈
                    StringBuilder sbTmp = new StringBuilder();
                    
                    string tname=null;
                    string tvalue = null;
                    TickValue tick = null;
                    foreach (char chr in strTick)
                    {
                        if (chr == ' ' && point == '\0')
                        {
                            if (tickName == null)
                            {
                                tickName = sbTmp.ToString().ToLower();
                            }
                            else
                            {
                                if (tname == null)
                                {
                                    tname = sbTmp.ToString().ToLower();
                                }
                                else if (tvalue == null)
                                {
                                    tvalue = sbTmp.ToString();
                                }

                                if (!tickValues.ContainsKey(tname))
                                {

                                    tick = new TickValue(tname, tvalue);
                                    HTMLFilter.CallTacketValueFinish(tick);
                                    if (tick.AttributeName != null)
                                    {
                                        tickValues.Add(tick.AttributeName, tick);
                                    }
                                }
                            }
                            sbTmp.Remove(0, sbTmp.Length);
                            tname = null;
                            tvalue = null;
                        }
                        else if (chr == '=' && point=='\0') 
                        {
                            tname = sbTmp.ToString();
                            sbTmp.Remove(0,sbTmp.Length);
                        }
                        else if (chr == '\"' || chr == '\'') 
                        {
                            if (point == '\0') //记录字符串标记
                            {
                                point=chr;
                            }
                            else if (point == chr)//结束字符串标记
                            {
                                point = '\0';
                            }
                            else 
                            {
                                sbTmp.Append(chr);
                            }
                        }
                        else
                        {
                            sbTmp.Append(chr);
                        }
                    }
                    if (tickName == null)
                    {
                        tickName = sbTmp.ToString().ToLower();
                    }
                    else
                    {
                        if (tname == null)
                        {
                            tname = sbTmp.ToString().ToLower();
                        }
                        else if (tvalue == null)
                        {
                            tvalue = sbTmp.ToString();
                        }

                        if (!tickValues.ContainsKey(tname))
                        {

                            tick = new TickValue(tname, tvalue);
                            HTMLFilter.CallTacketValueFinish(tick);
                            if (tick.AttributeName != null)
                            {
                                tickValues.Add(tick.AttributeName, tick);
                            }
                        }
                    }
                }

            }
            else
            {
                TickName = "";
            }
            
        }

        /// <summary>
        /// 标签名
        /// </summary>
        public string TickName 
        {
            get 
            {
                if (tickName == null) 
                {
                    return "";
                }
                return tickName;
            }
            set 
            {
                tickName = value;
            }
        }

        /// <summary>
        /// 标签值
        /// </summary>
        public TickValueCollection TickAttributes
        {
            get
            {
                if (tickAttributes == null)
                {
                    return null;
                }
                return tickAttributes;
            }
            set
            {
                tickAttributes = value;
            }
        }

        /// <summary>
        /// 标签包含文本
        /// </summary>
        public string InnerText
        {
            get
            {
                if (innerText == null)
                {
                    return "";
                }
                return innerText;
            }
            set
            {
                innerText = value;
            }
        }

        /// <summary>
        /// 是否结束标签
        /// </summary>
        public bool IsEnd
        {
            get
            {
                return isEnd;
            }
            set
            {
                isEnd = value;
            }
        }
    }
}
