namespace ConsumerOne
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //Task.Run(() => { Consumer.Run(); });
            //Task.Run(() => { Consumer.Run2(); });
            //Task.Run(() => { Consumer.Run3(); }); 
            //Task.Run(() => { Consumer.FanoutConsumerOne(); }); // 广播模式
            //Task.Run(() => { Consumer.FanoutConsumerTwo(); }); //
            Task.Run(() => { Consumer.ConfirmConsumer(); }); //
            Task.Run(() => { Consumer.DeadLetterConsumer(); }); //死信队列

            #region FanoutExchange


            #endregion

            Console.ReadLine();
        }
    }
}
