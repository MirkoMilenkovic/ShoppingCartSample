using Microsoft.Extensions.Logging;
using ShoppingCartSample.Library.BLL;
using ShoppingCartSample.Library.DAL;
using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCartSample.Library
{
    /// <summary>
    /// Implementation of shopping cart.
    /// If we would go "by the book", this class should be sealed. All the logic that could be chnmaged is injected.
    /// Also, this should be internal, but 
    /// </summary>
    public class ShoppingCart : IShoppingCart
    {

        public ShoppingCart(
            IShoppingCartRepository shoppingCartRepository,
            ITotalDiscountCalculator totalDiscountCalculator, 
            ILogger<ShoppingCart> logger)
        {
            ShoppingCartRepository = shoppingCartRepository;
            TotalDiscountCalculator = totalDiscountCalculator;
            Logger = logger;
        }

        private ILogger<ShoppingCart> Logger { get; }

        private IShoppingCartRepository ShoppingCartRepository { get; }

        private ITotalDiscountCalculator TotalDiscountCalculator { get; }

        /// <summary>
        /// Items, Id etc...
        /// </summary>
        private ShoppingCartModel State { get; set; }

        /// <summary>
        /// Leave it as null, initially. It has to be configured by Host.
        /// </summary>
        private IEnumerable<ISingleDiscountCalculator> SingleDiscountCalculatorList { get; set; }

        public int? Id
        {
            get
            {
                return this.State?.Id;
            }
        }

        public void ConfigureDiscounts(
            IEnumerable<ISingleDiscountCalculator> singleDiscountCalculatorList)
        {
            this.SingleDiscountCalculatorList = singleDiscountCalculatorList;
        }


        /// <summary>
        /// Get from DB and save to State.
        /// </summary>
        public void Load(int shoppingCartId)
        {
            this.State = this.ShoppingCartRepository.Load(
                shoppingCartId);
        }

        public void AddAndSave(
            ProductModel product,
            decimal quantity)
        {
            ShoppingCartItemModel newItem = new ShoppingCartItemModel(
                product.Name);

            if (this.State == null)
            {
                //shopping cart is new
                this.State = new ShoppingCartModel();
            }

            this.State.Items.TryGetValue(
                product.Name,
                out ShoppingCartItemModel existingItem);

            if (existingItem == null)
            {
                //we are adding item for the first time

                newItem.Quantity = quantity;

                this.State.Items.Add(
                    newItem.ProductName,
                    newItem);
            }
            else
            {
                //update quantity of existing item
                //do not update existingItem, because DB call might fail. Then, we will have dirty memory.

                //add new quantity to existingItem.Quantity 
                newItem.Quantity = existingItem.Quantity + quantity;

                this.State.Items[newItem.ProductName] = newItem;
            }

            //save to DB
            this.ShoppingCartRepository.Save(
                this.State);
        }

        public void ClearAndSave()
        {
            this.State.Items.Clear();

            //save to DB
            this.ShoppingCartRepository.Save(
                this.State);
        }


        public ShoppingCartSummaryModel Sum()
        {
            //validate that discounts are configured
            if (this.SingleDiscountCalculatorList == null)
            {
                throw new InvalidOperationException($"Call {nameof(ConfigureDiscounts)} before this");
            }

            ShoppingCartSummaryModel summary = new ShoppingCartSummaryModel();

            //apply discounts
            summary.SummaryDict = this.TotalDiscountCalculator.Calculate(
                this.State.Items,
                this.SingleDiscountCalculatorList);

            //calculate Sum from TotalPriceWithDiscount
            summary.Sum = summary.SummaryDict.Values.Sum(
                x => x.TotalPriceWithDiscount);

            //print, per requirment

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"SC log: {this.State.Id} BEGIN");
            foreach (ShoppingCartSummaryItemModel summaryItem in summary.SummaryDict.Values)
            {
                sb.AppendLine(
                    $"{summaryItem.ProductName}; UP: {summaryItem.ProductUnitPrice}; Q: {summaryItem.Quantity}; TP: {summaryItem.TotalPrice}; TPD: {summaryItem.TotalPriceWithDiscount}; Discount applied: {summaryItem.DiscountApplied}" );
            }

            sb.AppendLine($"Sum: {summary.Sum}");

            sb.AppendLine($"SC log: {this.State.Id} END");

            this.Logger.LogInformation(
                sb.ToString());
            //END print

            return summary;
        }
    }
}
