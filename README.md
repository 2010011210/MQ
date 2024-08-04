# MQ
## 安装ErLang

## 安装RabbitMq
1. 找到下载的rabbitMq的文件夹，\RabbitMQ\rabbitmq_server-3.8.3\sbin
2. cmd命令行窗口使用管理员打开
3. cd C:\Users\wangcong\Desktop\RabbitMQ\rabbitmq_server-3.8.3\sbin  转到相应的文件夹
4. 

## 消息推送到队列
1. 需要声明一个队列Queue，然后声明一个Exchange，然后把队列Queue绑定到Exchange上。
~~~
ConnectionFactory factory = new ConnectionFactory();
factory.HostName = "localhost";  // RabbitMQ本地运行
factory.UserName = "guest";  // 登录名，
factory.Password = "guest";  // 密码
using (IConnection connection = factory.CreateConnection())
{
    using (IModel channel = connection.CreateModel())  //创建信道
    {
        // 1.声明一个队列
        channel.QueueDeclare(queue: "OnlyProductor", durable: true, exclusive: false, autoDelete: false, arguments: null);

        // 2.声明一个交换机
        channel.ExchangeDeclare(exchange: "OnlyProductorExchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

        // 3.队列Queue和交换机绑定

        channel.QueueBind(queue: "OnlyProductor", exchange: "OnlyProductorExchange", routingKey: string.Empty, arguments: null);

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("生产者已经准备就绪");
        int i = 0;
        while (i < 10000)
        {
            string message = $"Run消息{i}";
            byte[] body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "OnlyProductorExchange",  // 如果exchange是空，系统采用默认的exchange，推送到队列名称是routingKey这个字段赋值的名称的队列。
                                 routingKey: string.Empty,
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine($"Run消息：{message}已经发送");
            i++;
            Thread.Sleep(500);
        }
    }
}
~~~

如果不声明Exchange，Queue没有绑定Exchange。系统采用默认的Exchange。队列推送到routingKey赋值的队列上,如下面这段代码，如果有一个名字是helloWord的Queue，消息将推送到队列helloWord里面。
~~~
channel.BasicPublish(exchange: "",  
                    routingKey: "helloWord",
                    basicProperties: null,
                    body: body);
~~~

## Exchange的几种类型
1. Direct  
队列Queue绑定到Exchange的时候，会传一个routingKey的值。   
消息推送到Exchange，直接根据routingKey进行匹配，推送到相应的Queue
2. Fanout  
消息推送到Exchange不需要routingKey，会推送到所有绑定到当前Exchange的队列Queue里面。   
Queue绑定到Exchange的时候，也不需要传routingKey。ExchangeType是Fanout
3. Topic  
队列Queue绑定到Exchange的时候，ExchangeType是Topic, routingKey是一个模糊匹配的值。#是可以匹配多个单词，*是匹配一个单词。 

4. Header
和Direct有点像，不过不是根据routingKey，是根据header里面的键值对，性能比较差，完全匹配或者匹配任意一个（根据x-match是any或者all来判断）。
~~~
发送消息，绑定queue和Exchange的时候
channel.exchange_declare(exchange='my-headers-exchange', exchange_type='headers')
channel.basic_publish(
    exchange='my-headers-exchange',
    routing_key='',
    body='Hello World!',
    properties=pika.BasicProperties(headers={'h1': 'Header1'}))

接受消息
channel.queue_bind(
    queue='HealthQ',
    exchange='my-headers-exchange',
    routing_key='',
    arguments={'x-match': 'any', 'h1': 'Header1', 'h2': 'Header2'})
~~~

## 消息的过期时间
1. 在声明队列的时候，给x-message-ttl赋值，下面是60秒过期,  
文档：https://www.rabbitmq.com/docs/ttl
~~~
var args = new Dictionary<string, object>();
args.Add("x-message-ttl", 60000);
model.QueueDeclare("myqueue", false, false, false, args);
~~~


## 生产消息确认
1. Tx事务模式    
通过开启事务的方式，提交事务，保证中间环节没有问题。如果有问题，就需要抛出异常，就是让我们知道有问题
~~~
channel.TxSelect(); //开启事务
channel.TxCommit();  // 提交事务. 多个消息一起提交
channel.TxRollback();  //回滚事务
~~~
2. Confirm模式  
生产端消息确认模式。 broker收到消息后做一个回执通知：告诉生产端，我这已经收到消息
~~~
channel.ConfirmSelect();
channel.WaitForConfirms(); // 如果一个或多个消息都发送成功
channel.WaitForConfirmsOrDie(); // 或者用这个，发送成功正常执行，发送失败，抛出异常

~~~


## 持久化，服务停止，没有持久化的，数据会丢失
1. 路由，交换机都要持久化
2. 推送的时候，消息对象的basicProperties的Persistent设置为true
~~~
string message = $"Run消息{i}";
byte[] body = Encoding.UTF8.GetBytes(message);
IBasicProperties basicProperties = channel.CreateBasicProperties();
basicProperties.Persistent = true;
channel.BasicPublish(exchange: exchangeName,  
                     routingKey: string.Empty,
                     basicProperties: basicProperties,
                     body: body);
~~~


## 消费确认  
1. 手动确认，消费成功再从队列删除  
~~~
EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
int count = 0;
consumer.Received += (model, ea) =>
{
    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
    if (count < 10)
    {
        // 手动确认
        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
        Console.WriteLine($"{message}:消费成功");
        count++;
    }
    else
    {
        //channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);  //这里有死循环风险，重新回写后，会触发这个方法，一直回写。不要回写
        channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: false);  //直接删除，写到错误日志中
        Console.WriteLine($"{message}:消费失败");
        count++;
    }
};

channel.BasicConsume(queue: "ErrorMessageQueue",
    autoAck: false,   // 这个要设置为false、手动确认
    consumer: consumer);
~~~

## 死信队列 
1. 假如消息被channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: false)，或者消息超时了，不要放回原来的队列，消息消费不到了。要放到死信队列中。然后从死信队列中消费  
文档：https://www.rabbitmq.com/docs/dlx
~~~
 string normalQueue = "normalQueue";
 string normalQueueExchange = "normalQueueExchange";
 string deadLetterQueue = "DeadLetterQueue";
 string deadLetterExchange = "DeadLetterExchange";
 string DEAD_LETTER_EXCHANE = "x-dead-letter-exchange";
 string DEAD_LETTER_ROUTING_KEY = "x-dead-letter-routing-key";

 // 声明死信队列和交换机
 channel.QueueDeclare(queue: deadLetterQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
channel.ExchangeDeclare(exchange: deadLetterExchange, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);
// 绑定死信队列和死信交换机
channel.QueueBind(queue: deadLetterQueue, exchange: deadLetterExchange, routingKey: "dead_letter_test", arguments: null);

// 1.声明一个队列， 队列的arguments参数需要传x-dead-letter-exchange和x-dead-letter-routing-key
var arg = new Dictionary<string, object>();
arg[DEAD_LETTER_EXCHANE] = deadLetterExchange;
arg[DEAD_LETTER_ROUTING_KEY] = "dead_letter_test";
channel.QueueDeclare(queue: normalQueue, durable: true, exclusive: false, autoDelete: false, arguments: arg);

// 2.声明一个交换机
channel.ExchangeDeclare(exchange: normalQueueExchange, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

// 3.队列Queue和交换机绑定

channel.QueueBind(queue: normalQueue, exchange: normalQueueExchange, routingKey: "dead_letter_test", arguments: null);

while (i < 100)
{
    string message = $"Run消息{i}";
    byte[] body = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(exchange: normalQueueExchange,  // 如果exchange是空，系统采用默认的exchange，推送到队列名称是routingKey这个字段赋值的名称的队列。
                         routingKey: "dead_letter_test",
                         basicProperties: null,
                         body: body);
    Console.WriteLine($"Run消息：{message}已经发送");
    i++;
    Thread.Sleep(500);
}


~~~

## 疑问 
1. 消费端如何限流，如何一次获取多条数据 
~~~
basicQos(int prefetchSize, int prefetchCount, boolean global);
prefetchSize：消息内容体允许的最大大小（以字节为单位），如果设置为0，则表示对消息内容体的大小没有限制   
prefetchCount：服务器将推送给消费者的最大未确认消息数。这是实现限流的关键参数  
global：此设置是否应用于整个通道（channel）上的所有消费者，而不是仅仅当前消费者。如果设置为false，则仅对当前消费者生效；如果设置为true，则对当前通道上的所有消费者生效

BasicQos必须在basicConsume之前调用，否则不会生效。且basicConsume需要设置为手动确认。

~~~
2. 消费方式，有被动消费和主动拉取之分，有何区别？
~~~
// 主动消费
channel.BasicConsume(queue: "OnlyProductor",
    autoAck: true,
    consumer: consumer);

// 被动消费
var response = channel.BasicGet(queue: "OnlyProductor", autoAck: true);  //主动拉取  

~~~

