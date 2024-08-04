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

                        // var response = channel.BasicGet(queue: "OnlyProductor", autoAck: true);  //主动拉取

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

        #region FanoutConsume
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

        #region Confirm 消费的时候，手动确认
        /// <summary>
        /// 
        /// </summary>
        public static void ConfirmConsumer()
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

                        channel.BasicQos(0, 3, false);
                        channel.BasicConsume(queue: "MessageTxQueue",
                            autoAck: false,   // 这个要设置为false、手动确认
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

        #region 死信队列消费

        public static void DeadLetterConsumer()
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

                        channel.BasicQos(0, 3, false);
                        channel.BasicConsume(queue: "normalQueue",
                            autoAck: false,   // 这个要设置为false、手动确认
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
