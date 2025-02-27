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
    /// �����������л�
    /// </summary>
    public class MemDataSerialize
    {
        /// <summary>
        /// Ĭ�ϱ���
        /// </summary>
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;
        /// <summary>
        /// ����ͷ��ʾ
        /// </summary>
        private static readonly byte[] HeadData = {77,68,65,84,65 };//MDATA
        /// <summary>
        /// ��������ͷ��ʾ
        /// </summary>
        private static readonly byte[] ListHeadData = { 76, 68, 65, 84, 65 };//LDATA

        #region д��
        /// <summary>
        /// ����д����ֽ�����
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
                    bw.Write(ds.Tables.Count);//д��������
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
        /// д�����ݱ���Ϣ
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="bw"></param>
        private static void WriteDataTable(DataTable dt, BinaryWriter bw) 
        {

            MemTypeManager.WriteString(bw, dt.TableName);
            
            //д������
            MemTypeManager.WriteInt(bw,dt.Columns.Count);
            List<MemTypeItem> lstItem = new List<MemTypeItem>(dt.Columns.Count);//�е���Ϣ
            MemTypeItem item = null;
            //д������Ϣ
            foreach (DataColumn col in dt.Columns) 
            {
                MemTypeManager.WriteString(bw,col.ColumnName);//����

                //������ID
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

            //����
            MemTypeManager.WriteInt(bw,dt.Rows.Count);

            //д������
            
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
        /// �����ݴ����м��س���
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

            int columnCount = (int)MemTypeManager.ReadInt(br);//����
            string name=null;
            int typeCode=0;
            List<MemTypeItem> lstItem = new List<MemTypeItem>(columnCount);//�е���Ϣ
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
        /// �ж�����ͷ�Ƿ��Ӧ
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


        #region ����
        /// <summary>
        /// �ж�����ͷ�Ƿ��Ӧ
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
        /// �����ݴ����м��س���
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
        /// ����д����ֽ�����
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
        /// д�����ݱ���Ϣ
        /// </summary>
        /// <param name="lst">ʵ�弯��</param>
        /// <param name="bw">д����</param>
        private static void WriteEntityList(IList lst, BinaryWriter bw)
        {
            if(lst.Count<=0)
            {
                return;
            }
            if(!(lst[0] is EntityBase))
            {
                throw new System.InvalidCastException("��ǰ���ϵ����ͱ�����EntityBase��������");
            }
            Type objType=lst[0].GetType();
            EntityInfoHandle eInfo=EntityInfoManager.GetEntityHandle(objType);
            if (eInfo == null) 
            {
                throw new System.TypeLoadException("ϵͳ�Ҳ�����:" + objType.FullName+"������Web.config�Ƿ��Ѿ������˼���");
            }

            //д������
            MemTypeManager.WriteInt(bw, eInfo.PropertyInfo.Count);
            List<MemPropertyInfo> lstItem = new List<MemPropertyInfo>(eInfo.PropertyInfo.Count);//�е���Ϣ
            
            
            //д������Ϣ
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

                MemTypeManager.WriteString(bw, pro.PropertyName);//����
                MemPropertyInfo info = new MemPropertyInfo();
                //������ID
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

            //����
            MemTypeManager.WriteInt(bw, lst.Count);

            //д������
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
        /// ����ʵ�弯����
        /// </summary>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        internal static IList CreateList(Type entityType) 
        {
            Type listType = typeof(List<>);
            listType = listType.MakeGenericType(entityType);
            IList lst = Activator.CreateInstance(listType) as IList;
            return lst;
        }

        /// <summary>
        /// ��ȡʵ�弯��
        /// </summary>
        /// <param name="br">��ȡ��</param>
        /// <param name="entityType">ʵ������</param>
        /// <returns></returns>
        private static IList ReadEntityList(BinaryReader br,Type entityType)
        {
            //Type listType = typeof(List<>);
            //listType = listType.MakeGenericType(entityType);
            IList lst = CreateList(entityType);

            int columnCount = (int)MemTypeManager.ReadInt(br);//����
            

            EntityInfoHandle eInfo = EntityInfoManager.GetEntityHandle(entityType);
            if (eInfo == null)
            {
                throw new System.TypeLoadException("ϵͳ�Ҳ�����:" + entityType.FullName + "������Web.config�Ƿ��Ѿ������˼���");
            }

            List<MemPropertyInfo> lstPro= new List<MemPropertyInfo>(eInfo.PropertyInfo.Count);//�е���Ϣ

            //��ȡ��ͷ
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

            //��ȡ����
            int rows = (int)MemTypeManager.ReadInt(br);
            for (int i = 0; i < rows; i++)
            {
                EntityBase objEntity = Activator.CreateInstance(entityType) as EntityBase;
                if (objEntity == null) 
                {
                    throw new System.InvalidCastException("��ǰ���ϵ����ͱ�����EntityBase��������");
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
 *�ļ��ṹ:
 * MDATA+���ݱ�����(int)+���ݱ�����(DataTable)
 *  DataTable�ṹ��
 *      ����+����Ϣ(����+�����ͱ�ʶ)
 *      ����+������
 *      �����ݣ�
 *          ��ͨ���ݣ��Ƿ��+����
 *          �������ݣ��Ƿ��+����+����
 */