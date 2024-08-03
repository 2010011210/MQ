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
和Direct有点像，不过不是根据routingKey，是根据header里面的值



## 生产消息确认
1. Tx事务模式    
通过开启事务的方式，提交事务，保证中间环节没有问题。如果有问题，就需要抛出异常，就是让我们知道有问题

2. Confirm模式  
生产端消息确认模式。 broker收到消息后做一个回执通知：告诉生产端，我这已经收到消息

## 持久化，服务停止，没有持久化的，数据会丢失
1. 路由，交换机都要持久化
2. Persistent设置为true

## 消费确认


