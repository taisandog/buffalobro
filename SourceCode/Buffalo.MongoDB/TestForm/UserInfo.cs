using Buffalo.Kernel;
using Buffalo.MongoDB;
using Buffalo.MongoDB.ProxyBase;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForm
{
    [MGCollection("YMRDB", "UserInfo")]
    [BsonDiscriminator("UserInfo1")]
    public class UserInfo : MongoEntityBase
    {
        public readonly static MongoDBOperate<UserInfo> Context = MongoDBManager.CreateOperate<UserInfo>();


        private string _name;
        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string Name { get => _name; set => _name = value; }

        public virtual int UserId { get; set; }
        private long _createTime;
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual long CreateTime { get => _createTime;
            set
            {
                _createTime = value ;
                OnPropertyUpdated("CreateTime");
            }
        }
        [MongoAutoIncrement]
        public virtual long SeqID { get; set; }
        [MongoAutoIncrement("YMRDB.UserInfo.LiqId.IndexID")]
        public virtual long IndexID { get; set; }
        public virtual ObjectId _id { get; set; }

        [BsonIgnore]
        public DateTime CreateDate
        {
            get
            {
                return CommonMethods.ConvertIntDateTime(_createTime);
            }
            set
            {
                CreateTime = (long)CommonMethods.ConvertDateTimeInt(value);
            }
        }
    }
}
