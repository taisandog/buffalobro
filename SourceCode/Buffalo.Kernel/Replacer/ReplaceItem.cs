using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Kernel.Replacer
{
    public class ReplaceItem
    {
        private string oldString;
        private string newString;

        /// <summary>
        /// ¾É×Ö·û´®
        /// </summary>
        public string OldString 
        {
            get 
            {
                return oldString;
            }
            set 
            {
                oldString = value;
            }
        }

        /// <summary>
        /// ÐÂ×Ö·û´®×Ö·û´®
        /// </summary>
        public string NewString
        {
            get
            {
                return newString;
            }
            set
            {
                newString = value;
            }
        }
    }
}
