using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DB.PropertyAttributes
{
    /// <summary>
    /// ��ͼ������
    /// </summary>
    public class ViewRelationTables : System.Attribute
    {
        private Type[] entityTypes;

        public Type[] EntityTypes
        {
            get { return entityTypes; }
        }

        public string[] EntityNames 
        {
            get 
            {
                if (entityTypes.Length <= 0) 
                {
                    return null;
                }
                string[] names = new string[entityTypes.Length];
                for (int i = 0; i < entityTypes.Length; i++) 
                {
                    names[i] = entityTypes[i].FullName;
                }
                return names;
            }
        }

            /// <summary>
            /// ��ͼ������
            /// </summary>
            /// <param name="entityType"></param>
            public ViewRelationTables(params Type[] entityTypes) 
        {
            this.entityTypes = entityTypes;
        }


    }
}
