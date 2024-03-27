using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ
{
    //public class MQBatchAction:IDisposable
    //{
    //    /// <summary>
    //    /// 自释放事务类
    //    /// </summary>
    //    /// <param name="oper"></param>
    //    /// <param name="runnow"></param>
    //    internal MQBatchAction(MQConnection oper)
    //    {
    //        _oper = oper;
    //    }


    //    private MQConnection _oper;

    //    /// <summary>
    //    /// 是否当前运行
    //    /// </summary>
    //    public bool Runnow
    //    {
    //        get { return _oper != null; }
    //    }



    //    #region IDisposable 成员

    //    /// <summary>
    //    /// 释放事务
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        EndBatch();
    //        GC.SuppressFinalize(this);
    //    }

    //    /// <summary>
    //    /// 结束批量操作
    //    /// </summary>
    //    private void EndBatch()
    //    {
    //        if (_oper != null)
    //        {
    //            _oper._isBatch = false;
    //            _oper.AutoClose();
    //        }
    //    }
    //    #endregion
    //}
}
