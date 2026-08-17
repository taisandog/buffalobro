# Buffalo.MQ

## 可靠消费

`Commit()`/`CommitAsync()` 仍然可用，并分别等价于 `Ack()`/`AckAsync()`。消费回调还可以显式请求重试或进入死信：

> Redis 只有 Stream 模式支持本节的确认、自动重试和死信机制。Polling、Subscriber、BlockQueue 为简单收发模式，异常由业务处理。

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

支持可靠消费的模式中，回调抛出异常或者在手动 ACK 模式下返回但没有结算消息时，Listener 会自动请求重试。达到最大投递次数后自动写入死信。

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

可以使用独立 Listener 显式监听死信；Redis 仅 Stream 模式支持此接口：

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

- Redis Stream 使用 `XAUTOCLAIM` 周期性领取超时 Pending，要求 Redis 6.2 或更高版本；`XACK` 只移除 Pending，不会物理删除 Stream 记录。
- Redis Polling、Subscriber、Subscriber+List 和 BlockQueue 都是简单收发模式：成功回调自动完成，回调异常只触发 `OnMQException`，不会自动重试或写死信。`AckAsync`/`CommitAsync` 是兼容性的无操作完成标记，`RetryAsync`、`DeadLetterAsync` 和死信监听会抛出 `NotSupportedException`。非 Stream 连接字符串中的可靠消费参数会被忽略。
- RabbitMQ 为每个源队列和 routing key 建立固定 TTL 重试队列，并为每个源队列建立 `.DLQ` 队列。
- Kafka 关闭自动 offset 提交；失败时 Seek 当前 offset，达到上限后写入 `.DLQ` Topic，成功写入后才提交源 offset。只有配置 `startOffset` 时才覆盖消费组位置。
- MQTT 默认关闭 MQTTnet 自动 ACK。建议使用 QoS 1/2、固定 `clientId`、`CleanSession=0` 和有效的会话过期时间。

死信重放应由业务显式执行：先确认重新发布成功，再 ACK 死信。所有消费者仍需根据 `MessageId` 或业务键实现幂等。

## 消息保留与清理

`AckAsync()` 只表示当前消费者处理完成；物理删除和保留策略通过 `IMQRetentionManager` 独立管理。可先检查 `RetentionCapabilities`，不支持的策略会抛出 `NotSupportedException`。

```csharp
MQConnection connection = MQUnit.GetMQConnection("orders");
IMQRetentionManager retention = connection;

await retention.ApplyRetentionPolicyAsync("orders.created", new MQRetentionPolicy
{
    CleanupMode = MQCleanupMode.MaxAge,
    MaxAge = TimeSpan.FromDays(7),
    MaxBytes = 10L * 1024 * 1024 * 1024
});
```

- RabbitMQ、MQTT：普通消息 ACK 后由 Broker 移除，能力包含 `AckRemovesMessage`，不提供额外的逐消息删除。
- Kafka：支持通过管理接口设置 Topic 的 `retention.ms` 和 `retention.bytes`，不支持按消费 ACK 删除。
- Redis Stream：支持 `none`、`maxLength`、`maxAge` 和 `deleteOnAck`。周期修剪前会确认所有消费组均无 Pending 且 Lag 为 0。
- Redis 非 Stream：不支持保留策略，`RetentionCapabilities` 返回 `None`。

Redis Stream 连接字符串参数：

| 参数 | 默认值 | 说明 |
| --- | --- | --- |
| `cleanupMode` | `none` | `none`、`maxLength`、`maxAge` 或 `deleteOnAck` |
| `cleanupInterval` | `1800000` | 自动清理检查间隔，毫秒；`xTrimInterval` 仍作为兼容别名 |
| `topicMaxLength` | `0` | `maxLength` 模式的最大记录数 |
| `messageRetention` | `0` | `maxAge` 模式的最大消息年龄，毫秒 |

`deleteOnAck` 会用一个 Redis 事务执行 `XACK + XDEL`，仅允许 Stream 中存在当前一个消费组；检测到多个消费组会拒绝启动或 ACK。未配置 `cleanupMode` 时默认不清理；旧连接字符串如果显式配置了大于 0 的 `topicMaxLength`，仍兼容为 `maxLength` 模式。
