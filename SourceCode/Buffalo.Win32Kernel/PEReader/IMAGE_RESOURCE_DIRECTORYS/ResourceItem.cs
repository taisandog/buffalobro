using System;
using System.Collections.Generic;
using System.Text;


namespace Buffalo.Win32Kernel.PEReader.IMAGE_RESOURCE_DIRECTORYS
{
    public class ResourceItem
    {
        ResourceType type;

        /// <summary>
        /// ��Դ����
        /// </summary>
        public ResourceType Type
        {
            get { return type; }
            set { type = value; }
        }

        private List<ResourceEntry> entrys = new List<ResourceEntry>();

        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public List<ResourceEntry> Entrys
        {
            get { return entrys; }
        }


    }
}
