using System;
using System.Data;
using Buffalo.DB.CommBase;
using Buffalo.DB.EntityInfos;
using System.Collections.Generic;
using Buffalo.DB.QueryConditions;
using Buffalo.DB.DbCommon;
using Buffalo.DB.PropertyAttributes;
using System.Data.Common;
using Buffalo.DB.BQLCommon.BQLKeyWordCommon;
using Buffalo.DB.BQLCommon.BQLConditions;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using System.Threading.Tasks;

namespace Buffalo.DB.DataBaseAdapter.IDbAdapters
{
    public interface IDBAdapter
    {
        
        /// <summary>
        /// 全文搜索时候排序字段是否显示表达式
        /// </summary>
        bool IsShowExpression
        {
            get;
        }
        /// <summary>
        /// 获取数据库的自增长字段的信息
        /// </summary>
        /// <returns></returns>
        string DBIdentity(string tableName, string paramName);
        /// <summary>
        /// 自增长是否一个数据类型
        /// </summary>
        /// <returns></returns>
        bool IdentityIsType { get;}
        /// <summary>
        /// 把DBType类型转成对应的SQLType
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="length">长度</param>
        /// <param name="canNull">是否空</param>
        /// <returns></returns>
        string DBTypeToSQL(DbType dbType,long length,bool canNull) ;

        /// <summary>
        /// 把DBType转成本数据库的实际类型
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        int ToRealDbType(DbType dbType, long length);
        /// <summary>
        /// 是否记录自增长字段作手动处理
        /// </summary>
        bool IsSaveIdentityParam
        {
            get;
        }

        /// <summary>
        /// 重建参数集合(Access需要)
        /// </summary>
        /// <param name="lstPrm"></param>
        /// <returns></returns>
        ParamList RebuildParamList(ref string sql,ParamList lstPrm);
        /// <summary>
        /// 获取清空表的SQL
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string GetTruncateTable(string tableName);
        /// <summary>
        /// 获取数据库当前时间
        /// </summary>
        /// <returns></returns>
        string GetNowDate(DbType dbType);
        /// <summary>
        /// 获取数据库当前格林威治时间
        /// </summary>
        /// <returns></returns>
        string GetUTCDate(DbType dbType);
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        string GetTimeStamp(DbType dbType);
        /// <summary>
        /// 返回全文检索的查询语句
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        string FreeTextLike(string paranName, string value);
        /// <summary>
        /// 返回全文检索的查询语句
        /// </summary>
        /// <param name="paranName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        string ContainsLike(string paranName, string value);
        /// <summary>
        /// 获取SQL适配器
        /// </summary>
        /// <returns></returns>
        IDbDataAdapter GetAdapter();
        /// <summary>
        /// 获取SQL命令类
        /// </summary>
        /// <returns></returns>
        DbCommand GetCommand();



        /// <summary>
        /// 获取SQL连接
        /// </summary>
        /// <returns></returns>
        DbConnection GetConnection(DBInfo db);
        /// <summary>
        /// 把数据类型转换成当前数据库支持的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        DbType ToCurrentDbType(DbType type);
        /// <summary>
        /// 获取top的查询字符串
        /// </summary>
        /// <param name="sql">查询字符串</param>
        /// <param name="top">top值</param>
        /// <returns></returns>
        string GetTopSelectSql(SelectCondition sql, int top);
        /// <summary>
        /// 获取参数类
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <param name="type">参数数据库类型</param>
        /// <param name="paramValue">参数值</param>
        /// <param name="paramDir">参数进出类型</param>
        /// <returns></returns>
        IDataParameter GetDataParameter(string paramName, System.Data.DbType type, object paramValue, System.Data.ParameterDirection paramDir);



        /// <summary>
        /// 获取自动增长的SQL
        /// </summary>
        /// <param name="info">实体信息</param>
        /// <returns></returns>
        string GetIdentitySQL(EntityPropertyInfo info);
        /// <summary>
        /// 获取自动增长值的SQL
        /// </summary>
        /// <returns></returns>
        string GetIdentityValueSQL(EntityPropertyInfo info);
        /// <summary>
        /// 格式化字段名
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        string FormatParam(string paramName);

        /// <summary>
        /// 格式化表格名
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        string FormatTableName(string tableName);
        /// <summary>
        /// 格式化变量名
        /// </summary>
        /// <param name="pname"></param>
        /// <returns></returns>
        string FormatValueName(string pname);
        /// <summary>
        /// 格式化变量的键名
        /// </summary>
        /// <param name="pname"></param>
        /// <returns></returns>
        string FormatParamKeyName(string pname);
        /// <summary>
        /// 获取字符串拼接SQl语句
        /// </summary>
        /// <param name="str">字符串集合</param>
        /// <returns></returns>
         string ConcatString(params string[] strs);
       
        /// <summary>
        /// 生成分页SQL语句
        /// </summary>
        /// <param name="list">参数列表</param>
        /// <param name="oper">连接对象</param>
        /// <param name="objCondition">条件对象</param>
        /// <param name="objPage">分页记录类</param>
        /// <param name="cacheTables">需要缓存的表名</param>
        /// <returns></returns>
        string CreatePageSql(ParamList list, DataBaseOperate oper, SelectCondition objCondition,
            PageContent objPage, bool useCache);
        /// <summary>
        /// 生成分页SQL语句
        /// </summary>
        /// <param name="list">参数列表</param>
        /// <param name="oper">连接对象</param>
        /// <param name="objCondition">条件对象</param>
        /// <param name="objPage">分页记录类</param>
        /// <param name="cacheTables">需要缓存的表名</param>
        /// <returns></returns>
        Task<string> CreatePageSqlAsync(ParamList list, DataBaseOperate oper, SelectCondition objCondition,
            PageContent objPage, bool useCache);
        /// <summary>
        ///  查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="lstParam">参数集合</param>
        /// <param name="sql">sql语句</param>
        /// <param name="objPage">分页实体</param>
        /// <param name="oper">数据库链接</param>
        /// <returns></returns>
        DbDataReader Query(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper);
        /// <summary>
        ///  查询
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="lstParam">参数集合</param>
        /// <param name="sql">sql语句</param>
        /// <param name="objPage">分页实体</param>
        /// <param name="oper">数据库链接</param>
        /// <returns></returns>
        Task<DbDataReader> QueryAsync(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper);
        /// <summary>
        /// 查询并且返回DataSet(游标分页)
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="lstParam">参数集合</param>
        /// <param name="objPage">分页对象</param>
        /// <param name="oper">数据库对象</param>
        /// <param name="curType">映射的实体类型(如果用回数据库的原列名，则此为null)</param>
        /// <returns></returns>
        DataTable QueryDataTable(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType);
        
        /// <summary>
        /// 查询并且返回DataSet(游标分页)
        /// </summary>
        /// <param name="sql">要查询的SQL语句</param>
        /// <param name="lstParam">参数集合</param>
        /// <param name="objPage">分页对象</param>
        /// <param name="oper">数据库对象</param>
        /// <param name="curType">映射的实体类型(如果用回数据库的原列名，则此为null)</param>
        /// <returns></returns>
        Task<DataTable> QueryDataTableAsync(string sql, ParamList lstParam, PageContent objPage, DataBaseOperate oper, Type curType);
        /// <summary>
        /// 把变量转变成SQL语句中的时间表达式
        /// </summary>
        /// <returns></returns>
        string GetDateTimeString(object value);

        /// <summary>
        /// 插入时候的主键字段名
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        string GetIdentityParamName(EntityPropertyInfo info);
        /// <summary>
        /// 插入时候的主键字段值
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        string GetIdentityParamValue(EntityInfoHandle entityInfo, EntityPropertyInfo info);
        /// <summary>
        /// 根据Reader的内容把数值赋进实体
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="index">当前Reader的索引</param>
        /// <param name="arg">目标对象</param>
        /// <param name="info">目标属性的句柄</param>
        void SetObjectValueFromReader(IDataReader reader, int index, object arg, EntityPropertyInfo info,bool needChangeType);
        /// <summary>
        /// 根据Reader的内容把数值赋进实体
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="index">当前Reader的索引</param>
        /// <param name="arg">目标对象</param>
        /// <param name="info">目标属性的句柄</param>
        Task SetObjectValueFromReaderAsync(DbDataReader reader, int index, object arg, EntityPropertyInfo info, bool needChangeType);


        /// <summary>
        /// 获取序列名
        /// </summary>
        /// <param name="info">属性</param>
        /// <returns></returns>
        string GetSequenceName(EntityPropertyInfo info);
        /// <summary>
        ///  获取默认序列名
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="paramName">字段名</param>
        /// <returns></returns>
        string GetDefaultSequenceName(string tableName,string paramName);
        /// <summary>
        /// 初始化序列名
        /// </summary>
        /// <param name="seqName"></param>
        string GetSequenceInit(string seqName,EntityParam prm, DataBaseOperate oper);

        ///// <summary>
        ///// 获取变量列表
        ///// </summary>
        //ParamList BQLSelectParamList
        //{
        //    get;
        //}
        /// <summary>
        /// 连接被关闭时候触发
        /// </summary>
        /// <param name="conn">连接</param>
        /// <param name="db">数据库信息</param>
        /// <returns></returns>
        bool OnConnectionClosed(DbConnection conn, DBInfo db);

        /// <summary>
        /// 获取创建注释的SQL
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="pInfo">字段（如果为空则设置表注释）</param>
        /// <returns></returns>
        string GetAddDescriptionSQL(KeyWordTableParamItem table, EntityParam pInfo, DBInfo info);
        /// <summary>
        /// 获取在字段添加SQL
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="pInfo">字段（如果为空则设置表注释）</param>
        /// <returns></returns>
        string GetColumnDescriptionSQL(EntityParam pInfo, DBInfo info);

        /// <summary>
        /// 创建表语句的结束
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        string CreateTableSQLEnd(DBInfo info) ;
        /// <summary>
        /// Default的关键字是否在NotNull前边
        /// </summary>
        /// <returns></returns>
        bool KeyWordDEFAULTFront();
        /// <summary>
        /// 进行like不去分大小写
        /// </summary>
        /// <param name="source"></param>
        /// <param name="lstParam"></param>
        /// <returns></returns>
        string DoLike(string source, string param, BQLLikeType type, BQLCaseType caseType, DBInfo info);
        

        /// <summary>
        /// 进行排序
        /// </summary>
        /// <param name="param">字段</param>
        /// <param name="sortType">排序</param>
        /// <param name="isCase">是否区分大小写</param>
        /// <param name="info">数据库信息</param>
        /// <returns></returns>
        string DoOrderBy(string param, SortType sortType, BQLCaseType caseType, DBInfo info);
        /// <summary>
        /// 在form位置的加锁方式(MSSQL)
        /// </summary>
        /// <param name="keyworkInfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        string ShowFromLockUpdate(BQLLockType lockType, DBInfo info);
        /// <summary>
        /// 结尾位置的加锁方式
        /// </summary>
        /// <param name="keyworkInfo"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        string LockUpdate(BQLLockType lockType, DBInfo info);
    }
}
