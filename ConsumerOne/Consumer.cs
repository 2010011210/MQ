using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerOne
{
    public class Consumer
    {
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    // 1.声明一个队列
                    try
                    {
                        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                        int count = 0;
                        consumer.Received += (model, ea) => 
                        {
                            // 秒杀业务，但不能是多线程的。
                            if (count > 10)
                            {
                                Console.WriteLine("秒杀结束，消息丢弃");
                                // 秒杀结束，消息丢弃
                            }
                            else
                            {
                                var msg = Encoding.UTF8.GetString(ea.Body.ToArray());
                                Console.WriteLine($"{msg}秒杀成功");
                                count++;
                            }
                            //var body = ea.Body;
                            //var message = Encoding.UTF8.GetString(body.ToArray());
                            //Console.WriteLine($"消费者01接口消息{message},处理业务");
                        };


                        channel.BasicConsume(queue: "OnlyProductor",
                            autoAck: true,
                            consumer: consumer);

                        Console.ReadLine();
                    }
                    catch (Exception ex) 
                    {
                        Console.WriteLine(ex.Message);
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    // 1.声明一个队列
                    try
                    {
                        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body.ToArray());
                            Console.WriteLine($"消费者02接口消息{message},处理业务");
                        };
                        channel.BasicConsume(queue: "OnlyProductor",
                            autoAck: true,
                            consumer: consumer);

                        Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
                    Console.ForegroundColor = ConsoleColor.Green;
                    // 1.声明一个队列
                    try
                    {
                        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body.ToArray());
                            Console.WriteLine($"消费者03接口消息{message},处理业务");
                        };

                        channel.BasicConsume(queue: "OnlyProductor",
                            autoAck: true,
                            consumer: consumer);

                        Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        #region MyRegion
        public static void FanoutConsumerOne()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";  // RabbitMQ本地运行
            factory.UserName = "guest";  // 登录名，
            factory.Password = "guest";  // 密码
            string fanoutQueueOneName = "FanoutQueueOneName";
            string fanoutQueueTwoName = "FanoutQueueTwoName";
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())  //创建信道
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    // 1.声明一个队列
                    try
                    {
                        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body.ToArray());
                            Console.WriteLine($"Fanout消费者01接口消息{message},处理业务");
                        };

                        channel.BasicConsume(queue: fanoutQueueOneName,
                            autoAck: true,
                            consumer: consumer);

                        Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
        public static void FanoutConsumerTwo()
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.HostName = "localhost";  // RabbitMQ本地运行
            factory.UserName = "guest";  // 登录名，
            string fanoutQueueTwoName = "FanoutQueueTwoName";
            using (IConnection connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())  //创建信道
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    // 1.声明一个队列
                    try
                    {
                        EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body.ToArray());
                            Console.WriteLine($"Fanout消费者02接口消息{message},处理业务");
                        };

                        channel.BasicConsume(queue: fanoutQueueTwoName,
                            autoAck: true,
                            consumer: consumer);

                        Console.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }


        #endregion


    }
}
