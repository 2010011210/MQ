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
            Task.Run(() => { MessageProduct.TopicExchange(); }); //TopicExchange
            Console.ReadLine();
        }
    }
}
