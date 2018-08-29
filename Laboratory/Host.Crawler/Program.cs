using System;

namespace Host.Crawler
{
    /// <summary>
    /// Orleans test silo host
    /// </summary>
    public class Program
    {

        static OrleansHostWrapper hostWrapper;

        static void Main(string[] args)
        {
            // The Orleans silo environment is initialized in its own app domain in order to more
            // closely emulate the distributed situation, when the client and the server cannot
            // pass data via shared memory.
            AppDomain hostDomain = AppDomain.CreateDomain("OrleansHost", null, new AppDomainSetup
            {
                AppDomainInitializer = InitSilo,
                AppDomainInitializerArguments = args,
            });
            // TODO: once the previous call returns, the silo is up and running.
            //       This is the place your custom logic, for example calling client logic
            //       or initializing an HTTP front end for accepting incoming requests.
            Console.WriteLine("Orleans Silo is running.\nPress Enter to terminate...");
            Console.ReadLine();
            hostDomain.DoCallBack(ShutdownSilo);
            //

        }

        static void InitSilo(string[] args)
        {
            hostWrapper = new OrleansHostWrapper(args);
            //功能函数初始化
            //GrainImplement.Crawler.Helper.CrawlerHelper.Inilization("mongodb://root:!admin_1@139.129.7.130:27017");
            //GrainImplement.Crawler.Helper.CrawlerHelper.Run();
            if (!hostWrapper.Run())
            {
                Console.Error.WriteLine("Failed to initialize Orleans silo");
            }
        }

        static void ShutdownSilo()
        {
            if (hostWrapper != null)
            {
                hostWrapper.Dispose();
                GC.SuppressFinalize(hostWrapper);
            }
        }
 
    }
}
