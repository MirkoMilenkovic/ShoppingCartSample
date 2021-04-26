using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoppingCartSample.Host.BLL;
using ShoppingCartSample.Host.DAL;
using ShoppingCartSample.Host.Database;
using ShoppingCartSample.Host.OutputPrinting;
using ShoppingCartSample.Library;
using ShoppingCartSample.Library.BLL;
using ShoppingCartSample.Library.DAL;
using System;
using System.Threading.Tasks;

namespace ShoppingCartSample.Host
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Before startup");

            IHostBuilder hostBuilder = CreateHostBuilder();

            Console.WriteLine("Host builder created");

            IHost host = hostBuilder.Build();

            Console.WriteLine("Host built. To close, press Ctrl+C");

            await host.RunAsync();

            Console.WriteLine("Press any key, like in the old days :) ...");

            Console.ReadLine();


        }

        public static IHostBuilder CreateHostBuilder()
        {
            IHostBuilder hostBuilder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();

            hostBuilder.ConfigureServices(ConfigureServices);

            return hostBuilder;
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            //note the namespaces of injected services
            //this will tell you what comes from Host, and what from Library. 

            //IInMemoryDB should be singleton. We have only one DB.
            //DB will manage concurrency
            services.AddSingleton(typeof(IInMemoryDB), typeof(InMemoryDB));

            //implementation of ShoppingCart's IProductRepository specific to this app.
            //there si no state, so DAL classes can be singletons. 
            services.AddSingleton(typeof(IProductRepository), typeof(ProductRepository));

            //implementation of ShoppingCart's IShoppingCartRepository specific to this app.
            services.AddSingleton(typeof(IShoppingCartRepository), typeof(ShoppingCartRepository));


            //inject ISingleDiscountCalculator instances using Scrutor.
            //Thanks to Scrutor, Host does not need to know about all implementations.
            //Make sure that Host references DiscountPlugin project, so Scrutor can find them. 
            services.Scan(scan =>
                scan.FromApplicationDependencies()
                .AddClasses(classes => classes.AssignableTo<ISingleDiscountCalculator>())
                .AsSelfWithInterfaces()
                .WithSingletonLifetime());

            //IDiscountRepository
            services.AddSingleton(typeof(IDiscountPluginRepository), typeof(DiscountPluginRepository));

            //use default implementation of TotalDiscountCalculator from Library.
            services.AddSingleton(typeof(ITotalDiscountCalculator), typeof(TotalDiscountCalculator));

            //shopping cart is stateful, so we have to initialize every time
            //we'll use scoped so we can control lifetime explicitly, and simulate web request.
            services.AddScoped(typeof(IShoppingCart), typeof(ShoppingCart));

            //printing
            //all services are singletons
            services.AddSingleton(typeof(IHtmlPrinter), typeof(HtmlPrinter));
            services.AddSingleton(typeof(IConsolePrinter), typeof(ConsolePrinter));
            services.AddSingleton(typeof(IOutputPrinterFactory), typeof(OutputPrinterFactory));

            //test item is stateful. 
            services.AddTransient(typeof(ISampleItem), typeof(SampleItem));

            //add TestRunner as HostedService
            services.AddHostedService<SampleRunner>();
        }
    }
}
