# Buffalo.MQ

## 可靠消费

`Commit()`/`CommitAsync()` 仍然可用，并分别等价于 `Ack()`/`AckAsync()`。消费回调还可以显式请求重试或进入死信：

```csharp
listener.OnMQReceivedAsync += async (_, message) =>
{
    try
    {
        await HandleAsync(message.Body);
        await message.AckAsync();
    }
    catch (ValidationException exception)
    {
        await message.DeadLetterAsync(exception.Message);
    }
    catch (Exception exception)
    {
        await message.RetryAsync(exception.Message, TimeSpan.FromSeconds(5));
    }
};
```

回调抛出异常或者在手动 ACK 模式下返回但没有结算消息时，Listener 会自动请求重试。达到最大投递次数后自动写入死信。

公共连接字符串参数：

| 参数 | 默认值 | 说明 |
| --- | --- | --- |
| `ackMode` | `manual` | `manual` 或 `onSuccess` |
| `retryEnabled` | `1` | 是否启用失败重试 |
| `retryOnException` | `1` | 回调异常时是否自动重试 |
| `maxRetry` | `4` | 第一次投递之外的最大重试次数 |
| `retryDelay` | `1000` | 重试延迟，毫秒 |
| `ackTimeout` | `30000` | Redis Stream Pending 可重新领取的最小空闲时间 |
| `pendingScanInterval` | `5000` | Redis Stream Pending 扫描间隔 |
| `deadLetterEnabled` | `1` | 是否启用死信 |
| `deadLetterSuffix` | `.DLQ` | 死信 Topic、Stream 或队列后缀 |

```text
ackMode=manual;retryEnabled=1;maxRetry=4;retryDelay=3000;
ackTimeout=30000;deadLetterEnabled=1;deadLetterSuffix=.DLQ
```

可以使用独立 Listener 显式监听死信：

```csharp
MQListener deadLetterListener = MQUnit.GetMQListener("orders");
deadLetterListener.OnMQDeadLetterReceivedAsync += async (_, message) =>
{
    Console.WriteLine($"{message.OriginalTopic}: {message.DeadLetterReason}");
    await message.AckAsync();
};
await deadLetterListener.StartDeadLetterListenAsync(new[] { "orders.created" });
```

后端行为：

- Redis Stream 使用 `XAUTOCLAIM` 周期性领取超时 Pending，要求 Redis 6.2 或更高版本。List/Subscriber 模式通过重新入队或重新发布实现应用异常重试。
- RabbitMQ 为每个源队列和 routing key 建立固定 TTL 重试队列，并为每个源队列建立 `.DLQ` 队列。
- Kafka 关闭自动 offset 提交；失败时 Seek 当前 offset，达到上限后写入 `.DLQ` Topic，成功写入后才提交源 offset。只有配置 `startOffset` 时才覆盖消费组位置。
- MQTT 默认关闭 MQTTnet 自动 ACK。建议使用 QoS 1/2、固定 `clientId`、`CleanSession=0` 和有效的会话过期时间。

死信重放应由业务显式执行：先确认重新发布成功，再 ACK 死信。所有消费者仍需根据 `MessageId` 或业务键实现幂等。
