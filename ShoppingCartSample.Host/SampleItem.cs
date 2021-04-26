using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShoppingCartSample.Host.BLL;
using ShoppingCartSample.Host.OutputPrinting;
using ShoppingCartSample.Library;
using ShoppingCartSample.Library.DAL;
using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ShoppingCartSample.Host
{
    /// <summary>
    /// This class will simulate one user clicking his shopping cart.
    /// </summary>
    public class SampleItem : ISampleItem
    {

        public SampleItem(
            ILogger<SampleItem> logger,
            IServiceProvider services,
            IProductRepository productRepository,
            IDiscountPluginRepository discountRepository,
            IOutputPrinterFactory outputPrinterFactory)
        {
            this.Logger = logger;
            Services = services;
            ProductRepository = productRepository;
            DiscountRepository = discountRepository;
            OutputPrinterFactory = outputPrinterFactory;
        }

        private ILogger<SampleItem> Logger { get; }


        /// <summary>
        /// Service locator used to get new instance of shopping cart.
        /// </summary>
        private IServiceProvider Services { get; }

        private IProductRepository ProductRepository { get; }

        private IDiscountPluginRepository DiscountRepository { get; }

        private IOutputPrinterFactory OutputPrinterFactory { get; }

        public int SampleId { get; private set; }

        private PrintModeEnum PrintMode { get; set; }

        public void Configure(
            int sampleId,
            PrintModeEnum printMode)
        {
            this.SampleId = sampleId;

            this.PrintMode = printMode;
        }

        public void Run()
        {
            Logger.LogInformation($"TestItem {this.SampleId} BEGIN");

            //get product catalog
            Dictionary<string, ProductModel> products = this.ProductRepository.GetAll();

            //create shortcuts to products, we'll need them throughout the test
            ProductModel bread = products["Bread"];
            ProductModel milk = products["Milk"];
            ProductModel butter = products["Butter"];

            int shoppingCartId = default(int);

            //user session 1
            //create scoped shopping cart, add few items.
            using (IServiceScope scope = this.Services.CreateScope())
            {
                IShoppingCart shoppingCart = scope.ServiceProvider.GetRequiredService<IShoppingCart>();
                //configure discounts
                shoppingCart.ConfigureDiscounts(
                    this.DiscountRepository.GetAll());

                shoppingCart.AddAndSave(
                    bread,
                    1);

                //we have id after first save.
                shoppingCartId = shoppingCart.Id.Value;

                Logger.LogInformation($"TestItem {this.SampleId} works with {nameof(shoppingCart)}: {shoppingCartId}");

                //print
                this.SumAndPrint(
                   shoppingCart);

                //add milk
                shoppingCart.AddAndSave(
                    milk,
                    1);

                this.SumAndPrint(
                    shoppingCart);

                //add butter
                shoppingCart.AddAndSave(
                    butter,
                    1);

                this.SumAndPrint(
                    shoppingCart);
            }
            //END user session 1
            //shopping cart goes Dodo, but we have date in DB.

            //user session 2
            //start new scope, to get new instance of shopping cart
            using (IServiceScope scope = this.Services.CreateScope())
            {
                IShoppingCart shoppingCart = scope.ServiceProvider.GetRequiredService<IShoppingCart>();
                shoppingCart.Load(
                    shoppingCartId);

                //configure discounts, SumAndPrint will not work without that
                shoppingCart.ConfigureDiscounts(
                    this.DiscountRepository.GetAll());

                //make a printout
                //state should be like at the end of previous user session.

                this.SumAndPrint(
                    shoppingCart);

                //clear
                shoppingCart.ClearAndSave();

                //make a printout
                //state should be like at the end of previous user session.
                this.SumAndPrint(
                    shoppingCart);

                shoppingCart.AddAndSave(
                    butter,
                    2);

                shoppingCart.AddAndSave(
                    bread,
                    1);

                //test update
                shoppingCart.AddAndSave(
                    bread,
                    1);

                this.SumAndPrint(
                    shoppingCart);
            }
            //END user session 2

            //user session 3
            using (IServiceScope scope = this.Services.CreateScope())
            {
                IShoppingCart shoppingCart = scope.ServiceProvider.GetRequiredService<IShoppingCart>();
                shoppingCart.Load(
                    shoppingCartId);

                //clear
                shoppingCart.ClearAndSave();

                shoppingCart.AddAndSave(
                    milk,
                    4);

                //test that SumAndPrint will fail without ConfigureDiscounts
                try
                {
                    this.SumAndPrint(
                        shoppingCart);
                }
                catch (InvalidOperationException ex)
                {
                    this.Logger.LogError(ex.Message);
                }

                //now, configure discounts
                shoppingCart.ConfigureDiscounts(
                    this.DiscountRepository.GetAll());

                this.SumAndPrint(
                    shoppingCart);
            }
            //END user session 3

            //user session 4
            using (IServiceScope scope = this.Services.CreateScope())
            {
                IShoppingCart shoppingCart = scope.ServiceProvider.GetRequiredService<IShoppingCart>();
                shoppingCart.Load(
                    shoppingCartId);

                //clear
                shoppingCart.ClearAndSave();

                shoppingCart.AddAndSave(
                    butter,
                    2);

                shoppingCart.AddAndSave(
                    bread,
                    1);

                shoppingCart.AddAndSave(
                    milk,
                    8);

                //configure discounts
                shoppingCart.ConfigureDiscounts(
                    this.DiscountRepository.GetAll());

                this.SumAndPrint(
                    shoppingCart);
            }
            //END user session 4

            Logger.LogInformation($"TestItem {this.SampleId} END");
        }

        private ShoppingCartSummaryModel SumAndPrint(
            IShoppingCart shoppingCart)
        {
            //Sleep a bit, to allow thread switch
            Random rnd = new Random(
                shoppingCart.Id.Value);
            var delay = rnd.Next(100);

            Thread.Sleep(delay);

            ShoppingCartSummaryModel summary = shoppingCart.Sum();

            this.SummaryPrint(
                shoppingCart.Id.Value,
                summary);

            return summary;
        }

        private void SummaryPrint(
            int id,
            ShoppingCartSummaryModel summary)
        {
            IOutputPrinter outputPrinter = this.OutputPrinterFactory.PrinterGet(
                this.PrintMode);

            string output = outputPrinter.Print(
                summary);

            this.Logger.LogInformation(
                output);
        }
    }
}
