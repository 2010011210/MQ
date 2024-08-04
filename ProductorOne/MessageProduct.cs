using ProductorOne.Model;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProductorOne
{
    public class MessageProduct
    {
        #region MyRegion
        public static void Run()
        {
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

        }

        public static void Run2()
        {
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

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("生产者已经准备就绪");
                    int i = 0;
                    while (i < 10000)
                    {
                        string message = $"Run2消息{i}";
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "OnlyProductorExchange",
                                             routingKey: string.Empty,
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine($"Run2消息：{message}已经发送");
                        i++;
                        Thread.Sleep(200);
                    }
                }
            }





        }

        public static void Run3()
        {
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

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("生产者已经准备就绪");
                    int i = 0;
                    while (i < 10000)
                    {
                        string message = $"Run3消息{i}";
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "OnlyProductorExchange",
                                             routingKey: string.Empty,
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine($"Run3消息：{message}已经发送");
                        i++;
                        Thread.Sleep(500);
                    }
                }
            }

        }
        #endregion

        #region Direct  根据routingKey弯曲匹配，才推送到队列里。

        public static void DirectExchange() 
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";  // RabbitMQ本地运行
            factory.UserName = "guest";  // 登录名，
            factory.Password = "guest";  // 密码
            
            List<string> logTypes = new List<string>() { "Info", "Warn", "Debug", "Error"};
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())  //创建信道
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    // 1.声明一个队列
                    channel.QueueDeclare(queue: "AllMessageQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    channel.QueueDeclare(queue: "ErrorMessageQueue", durable: true, exclusive: false, autoDelete: false, arguments: null);

                    // 2.声明一个交换机
                    channel.ExchangeDeclare(exchange: "DirectExchange", type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                    // 3.队列Queue和交换机绑定
                    foreach (var type in logTypes) 
                    {
                        channel.QueueBind(queue: "AllMessageQueue", exchange: "DirectExchange", routingKey: type, arguments: null);
                    }

                    channel.QueueBind(queue: "ErrorMessageQueue", exchange: "DirectExchange", routingKey: "Error", arguments: null);
                    var logMsgs = GetLogMessage();
                    
                    foreach (var msg in logMsgs) 
                    {
                        string message = msg.MsgContent;
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "DirectExchange",
                                             routingKey: msg.MsgType,
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine($"Run消息：{message}已经发送");
                    }
                }
            }
        }


        public static List<LogMsg> GetLogMessage() 
        {
            List<LogMsg> logMsgs = new List<LogMsg>();
            for (int i = 0;  i < 500; i++) 
            {
                var s = i % 4;
                switch (s) 
                {
                    case 0: logMsgs.Add(new LogMsg() { MsgType = "Info", MsgContent=$"Info的第{i}条日志" }); break;
                    case 1: logMsgs.Add(new LogMsg() { MsgType = "Warn", MsgContent = $"Warn的第{i}条日志" }); break;
                    case 2: logMsgs.Add(new LogMsg() { MsgType = "Debug", MsgContent = $"Debug的第{i}条日志" }); break;
                    case 3: logMsgs.Add(new LogMsg() { MsgType = "Error", MsgContent = $"Error的第{i}条日志" }); break;
                    default: break;
                }
            }
            return logMsgs;
        }

        #endregion

        #region FanoutExchange  广播，推送到预exchange绑定的所有队列（发布订阅模式），不判断routingKey，routingKey可以为空
        public static void FanoutExchange()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";  // RabbitMQ本地运行
            factory.UserName = "guest";  // 登录名，
            factory.Password = "guest";  // 密码

            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())  //创建信道
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    string exchangeName = "FanoutExchange"; // Exchange的名字
                    string fanoutQueueOneName = "FanoutQueueOneName";
                    string fanoutQueueTwoName = "FanoutQueueTwoName";

                    // 1.声明一个队列
                    channel.QueueDeclare(queue: fanoutQueueOneName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    channel.QueueDeclare(queue: fanoutQueueTwoName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    // 2.声明一个交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);

                    // 3.队列Queue和交换机绑定
                    //channel.QueueBind(queue: fanoutQueueOneName, exchange: exchangeName, routingKey: string.Empty, arguments: null);  // 队列Queue和Exchange绑定的时候，不需要routingKey

                    //channel.QueueBind(queue: fanoutQueueTwoName, exchange: exchangeName, routingKey: string.Empty, arguments: null);  // 队列Queue和Exchange绑定的时候，不需要routingKey
                    var logMsgs = GetLogMessage();

                    foreach (var msg in logMsgs)
                    {
                        string message = msg.MsgContent;
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "exchangeName",
                                             routingKey: string.Empty,
                                             basicProperties: null,
                                             body: body);
                        Console.WriteLine($"Run消息：{message}已经发送");
                        Thread.Sleep(400);
                    }
                }
            }
        }

        #endregion

        #region Topic  相当于Direct，但是是routingKey的模糊匹配

        public static void TopicExchange()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";  // RabbitMQ本地运行
            factory.UserName = "guest";  // 登录名，
            factory.Password = "guest";  // 密码

            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())  //创建信道
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    string exchangeName = "topicExchange"; // Exchange的名字
                    string chinaQueue = "ChinaQuque";
                    string newsQueue = "newsQueue";
                    string AmericanNewsQueue = "AmericanNews";

                    // 1.声明一个队列
                    channel.QueueDeclare(queue: chinaQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                    channel.QueueDeclare(queue: newsQueue, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    // 2.声明一个交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic, durable: true, autoDelete: false, arguments: null);

                    // 3.队列Queue和交换机绑定
                    channel.QueueBind(queue: chinaQueue, exchange: exchangeName, routingKey: "China.#", arguments: null);  

                    channel.QueueBind(queue: newsQueue, exchange: exchangeName, routingKey: "#.news", arguments: null);

                    List<string> messages = new List<string>() { "Chinese news", "American news", "" };


                    string message = "Chinese news";
                    byte[] body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: exchangeName,
                                         routingKey: "China.news", // 会同时命中China.#和#.news，所以会推送到两个队列
                                         basicProperties: null,
                                         body: body);
                    Console.WriteLine($"Run消息：{message}已经发送");

                    string message2 = "American news";
                    byte[] body2 = Encoding.UTF8.GetBytes(message2);
                    channel.BasicPublish(exchange: exchangeName,
                                         routingKey: "American.news",
                                         basicProperties: null,
                                         body: body2);
                    Thread.Sleep(400);
                }
            }
        }

        #endregion

        #region 事务 TxSelect(), TxCommit(), TxRollback();

        public static void TxRun()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";  // RabbitMQ本地运行
            factory.UserName = "guest";  // 登录名，
            factory.Password = "guest";  // 密码
            string exchangeName = "MessageTxExchange";
            string queueName = "MessageTxQueue";
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())  //创建信道
                {
                    // 1.声明一个队列
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    // 2.声明一个交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                    // 3.队列Queue和交换机绑定

                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: string.Empty, arguments: null);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("生产者已经准备就绪");

                    channel.TxSelect(); //开启事务
                    for (int i = 0; i < 5; i++) 
                    {
                        string message = $"Run消息{i}";
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        IBasicProperties basicProperties = channel.CreateBasicProperties();
                        basicProperties.Persistent = true;  // 持久化，就算MQ宕机，数据也会保留，不会丢失。
                        channel.BasicPublish(exchange: exchangeName,  
                                             routingKey: string.Empty,
                                             basicProperties: basicProperties,
                                             body: body);
                        Console.WriteLine($"Run消息：{message}已经发送");
                        Thread.Sleep(500);
                    }

                    try
                    {
                        channel.TxCommit();  // 提交事务. 5个消息一起提交
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine($"错误信息：{ex.Message}");
                        channel.TxRollback();  //回滚事务
                    }

                }
            }

        }

        #endregion

        #region 消息确认模式
        public static void ConfirmSelect() 
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";  // RabbitMQ本地运行
            factory.UserName = "guest";  // 登录名，
            factory.Password = "guest";  // 密码
            string exchangeName = "MessageTxExchange";
            string queueName = "MessageTxQueue";
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())  //创建信道
                {
                    // 1.声明一个队列
                    channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    // 2.声明一个交换机
                    channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false, arguments: null);

                    // 3.队列Queue和交换机绑定

                    channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: string.Empty, arguments: null);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("生产者已经准备就绪");
                    try
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            channel.ConfirmSelect();

                            string message = $"Run消息{i}";
                            byte[] body = Encoding.UTF8.GetBytes(message);
                            IBasicProperties basicProperties = channel.CreateBasicProperties();
                            basicProperties.Persistent = true;  // 持久化，就算MQ宕机，数据也会保留，不会丢失。
                            channel.BasicPublish(exchange: exchangeName,
                                                 routingKey: string.Empty,
                                                 basicProperties: basicProperties,
                                                 body: body);

                            if (channel.WaitForConfirms())  // 如果一个或多个消息都发送成功
                            {
                                Console.WriteLine($"{message}发送成功");
                            }
                            else 
                            {
                                Console.WriteLine("发送失败，可以重试");
                            }

                            channel.WaitForConfirmsOrDie(); // 发送成功正常执行，发送失败，抛出异常
                            Thread.Sleep(500);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"错误信息：{ex.Message}");
                    }

                    channel.Close();
                }
                connection.Close();
            }
        }


        #endregion

    }
}
