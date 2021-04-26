using Microsoft.Extensions.Logging;
using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ShoppingCartSample.Host.Database
{
    public class InMemoryDB : IInMemoryDB
    {


        public InMemoryDB(
            ILogger<InMemoryDB> logger)
        {
            this.Logger = logger;
        }

        private ILogger<InMemoryDB> Logger { get; }

        /// <summary>
        /// Used to sync initialization.
        /// </summary>
        private object initLock = new object();

        /// <summary>
        /// List of products. Will be initialized lazily on first call.
        /// </summary>
        private Dictionary<string, ProductModel> ProductDict { get; set; } = null;

        /// <summary>
        /// Initialize to empty. Items will be added by business logic.
        /// </summary>
        private Dictionary<int, ShoppingCartModel> ShoppingCartDict { get; set; } = new Dictionary<int, ShoppingCartModel>();

        /// <summary>
        /// Id of shopping cart. Simulates Sql Server IDENTITY value.
        /// </summary>
        private int lastShoppingCartId;

        private void Init()
        {
            this.ProductDict = new Dictionary<string, ProductModel>();

            ProductModel butter = new ProductModel(
                "Butter",
                .8M);
            this.ProductDict.Add(
                butter.Name,
                butter);

            ProductModel milk = new ProductModel(
                "Milk",
                1.15M);
            this.ProductDict.Add(
                milk.Name,
                milk);

            ProductModel bread = new ProductModel(
                "Bread",
                1.0M);
            this.ProductDict.Add(
                bread.Name,
                bread);
        }

        public Dictionary<string, ProductModel> ProductGetAll()
        {
            //init under lock
            lock(this.initLock)
            {
                if(this.ProductDict == null)
                {
                    this.Init();
                }
            }

            //init complete.

            return this.ProductDict;
        }

        /// <summary>
        /// Gets shopping cart from DB. Throws if not found.
        /// </summary>
        public ShoppingCartModel ShoppingCartGet(
            int shoppingCartId)
        {
            this.ShoppingCartDict.TryGetValue(
                shoppingCartId,
                out var shoppingCart);

            if(shoppingCart == null)
            {
                //oooops!!!
                throw new Exception($"{nameof(shoppingCartId)}: {shoppingCartId} not found in DB.");
            }

            return shoppingCart;
        }

        /// <summary>
        /// Saves a copy to DB. For new items, assigns id int thread-safe manner.
        /// </summary>
        public int ShoppingCartSave(
            ShoppingCartModel shoppingCart)
        {
            //if shoppingCart is new, this will be it's id.
            int newId = default(int);

            //create copy, to break reference with shoppingCart.
            //this simulates "out-of-process" saving.
            ShoppingCartModel copy = new ShoppingCartModel(
                shoppingCart);

            if (shoppingCart.Id == null)
            {
                //new shopping cart

                //assign Id. 
                //this is simulating Sql Server IDENTITY or SEQUENCE
                this.Logger.LogInformation(
                    $"Assigning new Id on Thread: {Thread.CurrentThread.ManagedThreadId}");

                //simulate delay, so other thread can jump over this one.
                Random rnd = new Random(DateTime.Now.Millisecond);
                int delayInMs = rnd.Next(
                    0,
                    1000);
                Thread.Sleep(delayInMs);
                
                newId = Interlocked.Increment(
                    ref this.lastShoppingCartId);

                this.Logger.LogInformation(
                    $"Assigned new Id: {newId} on Thread: {Thread.CurrentThread.ManagedThreadId}.");

                copy.Id = newId;

                //INSERT row to DB.
                this.ShoppingCartDict.Add(
                    copy.Id.Value,
                    copy);
            }
            else
            {
                //existing shopping cart

                //UPDATE row in DB.
                this.ShoppingCartDict[copy.Id.Value] = copy;
            }

            return copy.Id.Value;
        }

        public ProductModel GetByName(
            string name)
        {
            this.ProductDict.TryGetValue(
                name,
                out var product);

            if (product == null)
            {
                //oooops!!!
                throw new Exception($"{nameof(name)}: {name} not found in DB.");
            }

            return product;
        }
    }
}
