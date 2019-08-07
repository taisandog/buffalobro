using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel
{
    public class CSVReader
    {
        CharEnumerator _charEnumer = null;
        public CSVReader(string str) 
        {
            if (str != null) 
            {
                _charEnumer = str.GetEnumerator();
                MoveNext();
            }
        }

        bool _isEnd = false;
        /// <summary>
        /// 是否已经读完
        /// </summary>
        public bool IsEnd
        {
            get { return _isEnd; }
        }
        private bool MoveNext() 
        {
            _isEnd = !_charEnumer.MoveNext();
            return !_isEnd;
        }

        /// <summary>
        /// 读取行
        /// </summary>
        /// <returns></returns>
        public string[] ReaderRow() 
        {
            if (_isEnd) 
            {
                return null;
            }
            List<string> datas = new List<string>();

            while (!_isEnd) 
            {
                switch (_charEnumer.Current) 
                {
                    case '\"':
                        datas.Add(ReadValue());
                        break;
                    case ',':
                        MoveNext();
                        continue;
                    case '\n':
                        MoveNext();
                        return datas.ToArray();
                    default:
                        datas.Add(ReadValue());
                        break;
                }
            }
            return datas.ToArray() ;
        }

        /// <summary>
        /// 读取值
        /// </summary>
        /// <returns></returns>
        private string ReadValue() 
        {
            bool hasQuote = false;
            if (_charEnumer.Current=='\"') 
            {
                hasQuote = true;
            }
            StringBuilder sbTmp = new StringBuilder();
            
            while (MoveNext())
            {
                if (_charEnumer.Current == '\"') 
                {
                    if (hasQuote && _charEnumer.MoveNext()) 
                    {
                        if (_charEnumer.Current == '\n' || _charEnumer.Current == ',' || _charEnumer.Current == ' ')
                        {
                            break;
                        }
                        
                    }
                }
                else if (_charEnumer.Current == ',' && !hasQuote)
                {
                    break;
                }
                else if (_charEnumer.Current == '\n' && !hasQuote)
                {
                    break;
                }
                sbTmp.Append(_charEnumer.Current);
            }

            return sbTmp.ToString();
        }

    }
}
