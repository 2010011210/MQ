namespace ProductorOne
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //Task.Run(() => { MessageProduct.Run(); });
            //Task.Run(() => { MessageProduct.Run2(); });
            //Task.Run(() => { MessageProduct.Run3(); }); DirectExchange
            //Task.Run(() => { MessageProduct.DirectExchange(); }); 
            //Task.Run(() => { MessageProduct.FanoutExchange(); }); 
            //Task.Run(() => { MessageProduct.TopicExchange(); }); //
            //Task.Run(() => { MessageProduct.TxRun(); }); //
            // Task.Run(() => { MessageProduct.ConfirmSelect(); }); 
            Task.Run(() => { MessageProduct.DeadLetter(); }); // 死信队列
            Console.ReadLine();
        }
    }
}
