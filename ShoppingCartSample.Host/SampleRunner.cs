using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShoppingCartSample.Host.OutputPrinting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCartSample.Host
{
    public class SampleRunner : BackgroundService
    {
        private const int NUMBER_OF_USERS = 1;

        private const PrintModeEnum PRINT_MODE = PrintModeEnum.Html;

        public SampleRunner(
            ILogger<SampleRunner> logger,
            IServiceProvider services)
        {
            this.Logger = logger;
            Services = services;
        }

        private ILogger<SampleRunner> Logger { get; }

        /// <summary>
        /// Service locator.
        /// </summary>
        /// <remarks>
        /// If someone complains that this is anti-pattern, discuss.
        /// </remarks>
        private IServiceProvider Services { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //wait for a second for host to start.
            await Task.Delay(1000);

            this.Logger.LogInformation("TestRunner started");

            List<Task> testItemTaskList = new List<Task>();

            //run TestItem on multiple threads, so we can verify that we are not mixing data
            
            for (int i = 0; i < NUMBER_OF_USERS; i++)
            {
                try
                {
                    //we are resolving transient service into singleton
                    //it is OK here, because testItem is local.
                    ISampleItem testItem = this.Services.GetRequiredService<ISampleItem>();

                    testItem.Configure(
                        sampleId: i,
                        printMode: PRINT_MODE);

                    Task testItemTask = Task.Run(testItem.Run);

                    testItemTaskList.Add(testItemTask);
                }
                catch (Exception ex)
                {
                    this.Logger.LogError(ex.Message);
                    break;
                }
            }

            await Task.WhenAll(testItemTaskList);

            this.Logger.LogInformation("TestRunner completed");
        }
    }
}
