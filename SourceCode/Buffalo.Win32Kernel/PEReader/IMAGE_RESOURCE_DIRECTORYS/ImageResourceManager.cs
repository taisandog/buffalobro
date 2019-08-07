using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.Win32Kernel.PEReader.IMAGE_RESOURCE_DIRECTORYS
{
    public class ImageResourceManager
    {
        ImageResourceInfo info;



        public ImageResourceManager(PeHeaderReader pe, long baseOffest) 
        {
            info = new ImageResourceInfo(pe, baseOffest,0);
            info.LoadInfo();
            FillItems();
        }



        List<ResourceItem> resourceItems = new List<ResourceItem>();

        public List<ResourceItem> ResourceItems
        {
            get { return resourceItems; }
            //set { resourceItems = value; }
        }

        /// <summary>
        /// 筛选出资源信息
        /// </summary>
        private void FillItems()
        {
            resourceItems = new List<ResourceItem>();
            foreach (ResourceDirectoryEntryInfo ninfo in info.LstResourceInfo)
            {
                ResourceItem item = new ResourceItem();

                //第一层是类型
                item.Type = (ResourceType)ninfo.ResourceDirectorEntry.Name;

                ImageResourceInfo dInfo = ninfo.NextDirectory;
                if (dInfo == null)
                {
                    continue;
                }

                foreach (ResourceDirectoryEntryInfo entryInfo in dInfo.LstResourceInfo)
                {
                    ResourceEntry entry = new ResourceEntry(item);
                    //第二层是名称
                    entry.Name = entryInfo.ResourceDirString.NameString;

                    ImageResourceInfo dInfo1 = entryInfo.NextDirectory;
                    if (dInfo1 == null)
                    {
                        dInfo1 = dInfo;
                    }
                    if (dInfo1.LstResourceInfo.Count > 0)
                    {
                        //第三层是资源指向的信息
                        entry.ResourceInfo = dInfo1.LstResourceInfo[0].ValueData;
                    }
                    item.Entrys.Add(entry);
                }
                resourceItems.Add(item);
            }
        }
        
    }
}
