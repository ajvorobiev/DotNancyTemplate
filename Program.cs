namespace DotNancyTemplate
{
    using System;
    using System.Linq;
    using System.Threading;
    using Nancy;
    using Nancy.Hosting.Self;
    
    /// <summary>
    /// The main injection point of the application.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new NancyHost(new Uri("http://localhost:8888")))
            {
                host.Start();

                //Under mono if you deamonize a process a Console.ReadLine with cause an EOF
                //so we need to block another way
                if (args.Any(s => s.Equals("-d", StringComparison.CurrentCultureIgnoreCase)))
                {
                    while (true) Thread.Sleep(10000000);
                }
                else
                {
                    Console.ReadKey();
                }

                host.Stop();  // stop hosting
            }
        }
    }
}
