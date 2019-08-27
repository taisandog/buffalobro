using System;
using System.Collections.Generic;
using System.Text;

using Buffalo.DB.CommBase;
using Buffalo.Kernel.Defaults;
using Buffalo.DB.PropertyAttributes;
using System.Data;
using Buffalo.DB.CommBase.BusinessBases;
namespace TestApp
{
    /// <summary>
    /// 用户
    /// </summary>
    public partial class TEUser:TEBase
    {
        /// <summary>
        ///  用户名
        /// </summary>
        protected string _name;
        /// <summary>
        ///  用户名
        /// </summary>
        public virtual string Name
        {
            get{ return _name; }
            set{ _name=value; }
        }
        /// <summary>
        /// 币数
        /// </summary>
        protected long _coins;

        /// <summary>
        /// 总充值
        /// </summary>
        protected long _buyCoins;
        /// <summary>
        /// 币数
        /// </summary>
        public virtual long Coins
        {
            get{ return _coins; }
            set{ _coins=value; }
        }
        /// <summary>
        /// 总充值
        /// </summary>
        public virtual long BuyCoins
        {
            get{ return _buyCoins; }
            set{ _buyCoins=value; }
        }
    }
}
