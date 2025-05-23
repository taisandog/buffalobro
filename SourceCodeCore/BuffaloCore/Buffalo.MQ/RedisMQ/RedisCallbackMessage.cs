using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.MQ.RedisMQ
{
    public class RedisCallbackMessage : MQCallBackMessage
    {
        /// <summary>
        /// 关联DB
        /// </summary>
        private IDatabase _db;
        /// <summary>
        /// 消费组
        /// </summary>
        private string _consumerGroup;
        /// <summary>
        /// 消息Id
        /// </summary>
        private RedisValue _messId;
        /// <summary>
        /// 标记
        /// </summary>
        private CommandFlags _commandFlags;

        public RedisCallbackMessage(string topic,  byte[] body) :
            base(topic, body)
        {
            
        }

        public RedisCallbackMessage(string topic, byte[] body, IDatabase db, string consumerGroup, RedisValue messId, CommandFlags commandFlags) :
           base(topic, body)
        {
            _db = db;
            _messId = messId;
            _commandFlags = commandFlags;
            _consumerGroup = consumerGroup;

        }
        public override void Commit()
        {
            if (_db == null) 
            {
                return;
            }
            _db.StreamAcknowledge(_topic, _consumerGroup, _messId,_commandFlags);
        }

        public override void Dispose()
        {
            _db = null;
            _messId = RedisValue.Null;
            _commandFlags = CommandFlags.None;
            _consumerGroup = null;
            base.Dispose();
        }

        public override async Task CommitAsync()
        {
            if (_db == null)
            {
                return;
            }
            await _db.StreamAcknowledgeAsync(_topic, _consumerGroup, _messId, _commandFlags);
        }

        ~RedisCallbackMessage()
        {
            Dispose();
        }
    }
}
