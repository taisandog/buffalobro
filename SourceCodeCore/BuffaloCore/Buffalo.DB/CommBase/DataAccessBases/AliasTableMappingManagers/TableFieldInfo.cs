using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.DB.CommBase.DataAccessBases.AliasTableMappingManagers
{
    /// <summary>
    /// 数据表信息
    /// </summary>
    public class TableFieldInfo
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public Type FieldType { get; set; }
    }
}
