using System.Text;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Buffalo.DB.EntityInfos;
using System;
using Buffalo.DB.CommBase;

namespace MemcacheClient
{
    /// <summary>
    /// 缓存数据序列化
    /// </summary>
    public class MemDataSerialize
    {
        /// <summary>
        /// 默认编码
        /// </summary>
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;
        /// <summary>
        /// 数据头表示
        /// </summary>
        private static readonly byte[] HeadData = {77,68,65,84,65 };//MDATA
        /// <summary>
        /// 集合数据头表示
        /// </summary>
        private static readonly byte[] ListHeadData = { 76, 68, 65, 84, 65 };//LDATA

        #region 写入
        /// <summary>
        /// 数据写入成字节数组
        /// </summary>
        /// <returns></returns>
        public static byte[] DataSetToBytes(DataSet ds)
        {
            byte[] ret = null;
            using (MemoryStream ms = new MemoryStream(5000))
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(HeadData);
                    bw.Write(ds.Tables.Count);//写入表的数量
                    foreach (DataTable dt in ds.Tables) 
                    {
                        WriteDataTable(dt, bw);
                    }
                }
                ret = ms.ToArray();
            }
            
            return ret;
        }

        /// <summary>
        /// 写入数据表信息
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="bw"></param>
        private static void WriteDataTable(DataTable dt, BinaryWriter bw) 
        {

            MemTypeManager.WriteString(bw, dt.TableName);
            
            //写入列数
            MemTypeManager.WriteInt(bw,dt.Columns.Count);
            List<MemTypeItem> lstItem = new List<MemTypeItem>(dt.Columns.Count);//列的信息
            MemTypeItem item = null;
            //写入列信息
            foreach (DataColumn col in dt.Columns) 
            {
                MemTypeManager.WriteString(bw,col.ColumnName);//列名

                //列类型ID
                item = MemTypeManager.GetTypeInfo(col.DataType);
                if (item != null)
                {
                    MemTypeManager.WriteInt(bw, item.TypeID);
                    lstItem.Add(item);
                }
                else 
                {
                    lstItem.Add(item);
                }
            }

            //行数
            MemTypeManager.WriteInt(bw,dt.Rows.Count);

            //写入数据
            
            foreach(DataRow row in dt.Rows)
            {
                for (int i = 0; i < lstItem.Count; i++) 
                {

                    if (row.IsNull(i)) 
                    {
                        lstItem[i].WriterHandle(bw, null);
                        continue;
                    }
                    object value = row[i];
                    if (lstItem[i] == null) 
                    {
                        continue;
                    }
                        lstItem[i].WriterHandle(bw, value);
                    
                    
                }
            }
        }
        #endregion

         /// <summary>
        /// 把数据从流中加载出来
        /// </summary>
        /// <returns></returns>
        public static DataSet LoadDataSet(Stream stm)
        {
            if (!IsHead(stm))
            {
                return null;
            }
            DataSet ds = new DataSet();
            BinaryReader br = new BinaryReader(stm);

            int tableCount = br.ReadInt32();
            for (int i = 0; i < tableCount; i++)
            {
                ds.Tables.Add(ReadDataTable(br));
            }

            return ds;
        }

        private static DataTable ReadDataTable(BinaryReader br) 
        {
            DataTable dt = new DataTable();
            dt.TableName=MemTypeManager.ReadString(br) as string;

            int columnCount = (int)MemTypeManager.ReadInt(br);//列数
            string name=null;
            int typeCode=0;
            List<MemTypeItem> lstItem = new List<MemTypeItem>(columnCount);//列的信息
            for (int i = 0; i < columnCount; i++) 
            {
                name = MemTypeManager.ReadString(br) as string;
                typeCode = (int)MemTypeManager.ReadInt(br);
                MemTypeItem item=MemTypeManager.GetTypeByID(typeCode);
                if (item != null)
                {
                    dt.Columns.Add(name, item.ItemType);
                    lstItem.Add(item);
                }
            }
            int rows = (int)MemTypeManager.ReadInt(br);
            dt.BeginLoadData();
            for (int i = 0; i < rows; i++) 
            {
                DataRow dr = dt.NewRow();
                for (int k=0;k<lstItem.Count;k++)
                {
                    MemTypeItem colItem = lstItem[k];
                    object value = colItem.ReadHandle(br);
                    if (value != null) 
                    {
                        dr[k] = value;
                    }
                }
                dt.Rows.Add(dr);
            }
            dt.EndLoadData();
            return dt;
        }

        /// <summary>
        /// 判断数据头是否对应
        /// </summary>
        /// <param name="stm"></param>
        /// <returns></returns>
        private static bool IsHead(Stream stm) 
        {
            byte[] head = new byte[HeadData.Length];
            stm.Read(head, 0, head.Length);
            for (int i = 0; i < head.Length; i++) 
            {
                if (head[i] != HeadData[i]) 
                {
                    return false;
                }
            }
            return true;
        }


        #region 对象
        /// <summary>
        /// 判断数据头是否对应
        /// </summary>
        /// <param name="stm"></param>
        /// <returns></returns>
        private static bool IsListHead(Stream stm)
        {
            byte[] head = new byte[ListHeadData.Length];
            stm.Read(head, 0, head.Length);
            for (int i = 0; i < head.Length; i++)
            {
                if (head[i] != ListHeadData[i])
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 把数据从流中加载出来
        /// </summary>
        /// <returns></returns>
        public static IList LoadList(Stream stm,Type entityType)
        {
            if (!IsListHead(stm))
            {
                return null;
            }
            BinaryReader br = new BinaryReader(stm);
            IList lst = ReadEntityList(br, entityType);

            return lst;
        }

        /// <summary>
        /// 数据写入成字节数组
        /// </summary>
        /// <returns></returns>
        public static byte[] ListToBytes(IList lst)
        {
            byte[] ret = null;
            using (MemoryStream ms = new MemoryStream(5000))
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(ListHeadData);

                    WriteEntityList(lst, bw);
                }
                ret = ms.ToArray();
            }

            return ret;
        }

        private static Type _nonSerType = typeof(NonSerializedAttribute);
        /// <summary>
        /// 写入数据表信息
        /// </summary>
        /// <param name="lst">实体集合</param>
        /// <param name="bw">写入器</param>
        private static void WriteEntityList(IList lst, BinaryWriter bw)
        {
            if(lst.Count<=0)
            {
                return;
            }
            if(!(lst[0] is EntityBase))
            {
                throw new System.InvalidCastException("当前集合的类型必须是EntityBase的派生类");
            }
            Type objType=lst[0].GetType();
            EntityInfoHandle eInfo=EntityInfoManager.GetEntityHandle(objType);
            if (eInfo == null) 
            {
                throw new System.TypeLoadException("系统找不到类:" + objType.FullName+"，请检查Web.config是否已经配置了加载");
            }

            //写入列数
            MemTypeManager.WriteInt(bw, eInfo.PropertyInfo.Count);
            List<MemPropertyInfo> lstItem = new List<MemPropertyInfo>(eInfo.PropertyInfo.Count);//列的信息
            
            
            //写入列信息
            foreach (EntityPropertyInfo pro in eInfo.PropertyInfo)
            {
                if (!pro.HasGetHandle) 
                {
                    continue;
                }
                object[] atts=pro.BelongPropertyInfo.GetCustomAttributes(_nonSerType,true);
                if (atts.Length>0) 
                {
                    if (atts[0] is NonSerializedAttribute) 
                    {
                        continue;
                    }
                }

                MemTypeManager.WriteString(bw, pro.PropertyName);//列名
                MemPropertyInfo info = new MemPropertyInfo();
                //列类型ID
                MemTypeItem item = MemTypeManager.GetTypeInfo(pro.FieldType);
                if (item != null)
                {
                    MemTypeManager.WriteInt(bw, item.TypeID);
                    //lstItem.Add(item);
                }
                info.MemItem = item;
                info.PropertyInfo = pro;
                lstItem.Add(info);

            }

            //行数
            MemTypeManager.WriteInt(bw, lst.Count);

            //写入数据
            foreach (object obj in lst)
            {
                foreach (MemPropertyInfo pro in lstItem)
                {
                    object value = pro.PropertyInfo.GetValue(obj);
                    if (value==null)
                    {
                        pro.MemItem.WriterHandle(bw, null);
                        continue;
                    }

                    if (pro.MemItem == null)
                    {
                        continue;
                    }
                    pro.MemItem.WriterHandle(bw, value);
                }
            }
        }

        /// <summary>
        /// 创建实体集合类
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        internal static IList CreateList(Type entityType) 
        {
            Type listType = typeof(List<>);
            listType = listType.MakeGenericType(entityType);
            IList lst = Activator.CreateInstance(listType) as IList;
            return lst;
        }

        /// <summary>
        /// 读取实体集合
        /// </summary>
        /// <param name="br">读取器</param>
        /// <param name="entityType">实体类型</param>
        /// <returns></returns>
        private static IList ReadEntityList(BinaryReader br,Type entityType)
        {
            //Type listType = typeof(List<>);
            //listType = listType.MakeGenericType(entityType);
            IList lst = CreateList(entityType);

            int columnCount = (int)MemTypeManager.ReadInt(br);//列数
            

            EntityInfoHandle eInfo = EntityInfoManager.GetEntityHandle(entityType);
            if (eInfo == null)
            {
                throw new System.TypeLoadException("系统找不到类:" + entityType.FullName + "，请检查Web.config是否已经配置了加载");
            }

            List<MemPropertyInfo> lstPro= new List<MemPropertyInfo>(eInfo.PropertyInfo.Count);//列的信息

            //读取列头
            for (int i = 0; i < columnCount; i++)
            {
                string name = MemTypeManager.ReadString(br) as string;
                int typeCode = (int)MemTypeManager.ReadInt(br);
                MemTypeItem item = MemTypeManager.GetTypeByID(typeCode);
                if (item != null)
                {
                    EntityPropertyInfo pro = eInfo.PropertyInfo[name];
                    MemPropertyInfo info = new MemPropertyInfo();
                    info.MemItem = item;
                    info.PropertyInfo = pro;
                    lstPro.Add(info);
                }
            }

            //读取数据
            int rows = (int)MemTypeManager.ReadInt(br);
            for (int i = 0; i < rows; i++)
            {
                EntityBase objEntity = Activator.CreateInstance(entityType) as EntityBase;
                if (objEntity == null) 
                {
                    throw new System.InvalidCastException("当前集合的类型必须是EntityBase的派生类");
                }

                for (int k = 0; k < lstPro.Count; k++)
                {
                    MemTypeItem colItem = lstPro[k].MemItem;
                    EntityPropertyInfo pro = lstPro[k].PropertyInfo;
                    try
                    {
                        object value = colItem.ReadHandle(br);
                        if (value != null)
                        {
                            if (!pro.HasSetHandle)
                            {
                                continue;
                            }
                            if (colItem.ItemType.IsEnum) 
                            {

                                value = Convert.ChangeType(value, Enum.GetUnderlyingType(colItem.ItemType));
                            }

                            pro.SetHandle(objEntity, value);
                        }
                    }
                    catch { }
                }
                lst.Add(objEntity);
            }

            return lst;
        }
        #endregion

    }
}
/*
 *文件结构:
 * MDATA+数据表数量(int)+数据表数据(DataTable)
 *  DataTable结构：
 *      列数+列信息(列名+列类型标识)
 *      行数+行数据
 *      行数据：
 *          普通数据：是否空+数据
 *          数组数据：是否空+长度+数据
 */