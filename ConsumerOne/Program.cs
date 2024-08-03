namespace ConsumerOne
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //Task.Run(() => { Consumer.Run(); });
            //Task.Run(() => { Consumer.Run2(); });
            //Task.Run(() => { Consumer.Run3(); }); FanoutConsumerTwo
            Task.Run(() => { Consumer.FanoutConsumerOne(); }); 
            Task.Run(() => { Consumer.FanoutConsumerTwo(); });

            #region FanoutExchange


            #endregion

            Console.ReadLine();
        }
    }
}
