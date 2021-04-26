using ShoppingCartSample.Library.BLL;
using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Host.BLL
{
    public abstract class SingleDiscountCalculatorBase : ISingleDiscountCalculator
    {
        /// <summary>
        /// Return value of DiscountItems method
        /// </summary>
        protected class ItemToDiscount
        {
            public ItemToDiscount(
                ShoppingCartSummaryItemModel item,
                decimal totalPriceAfterDiscount)
            {
                Item = item;
                TotalPriceAfterDiscount = totalPriceAfterDiscount;
            }

            public ShoppingCartSummaryItemModel Item { get; }

            public decimal TotalPriceAfterDiscount { get; }
        }


        protected abstract string RuleIdentifier { get; }

        /// <summary>
        /// Calculate discounts and return copy of original dict.
        /// </summary>
        public Dictionary<string, ShoppingCartSummaryItemModel> Calculate(
            IReadOnlyDictionary<string, ShoppingCartSummaryItemModel> originalDict)
        {
            Dictionary<string, ShoppingCartSummaryItemModel> copyDict = this.MakeACopy(
                originalDict);

            Dictionary<string, ItemToDiscount> itemsToDiscount = this.ItemsToDiscountGet(
                copyDict);

            //for each itemToDiscount, apply new price and mark as discounted
            foreach(KeyValuePair<string, ItemToDiscount> itemToDiscount in itemsToDiscount)
            {
                ShoppingCartSummaryItemModel item = copyDict[itemToDiscount.Key];
                if (!this.CanApply(item, itemToDiscount.Value.TotalPriceAfterDiscount))
                {
                    //validation failed
                    continue;
                }

                item.DiscountApplied = this.RuleIdentifier;
                item.TotalPriceWithDiscount = itemToDiscount.Value.TotalPriceAfterDiscount;
            }

            return copyDict;
        }

        protected Dictionary<string, ShoppingCartSummaryItemModel> MakeACopy(
            IReadOnlyDictionary<string,
                ShoppingCartSummaryItemModel> originalDict)
        {
            Dictionary<string, ShoppingCartSummaryItemModel> copyDict = new Dictionary<string, ShoppingCartSummaryItemModel>();

            foreach (ShoppingCartSummaryItemModel original in originalDict.Values)
            {
                ShoppingCartSummaryItemModel copy = new ShoppingCartSummaryItemModel(original);

                copyDict.Add(
                    copy.ProductName,
                    copy);
            }

            return copyDict;
        }


        protected abstract Dictionary<string, ItemToDiscount> ItemsToDiscountGet(
            Dictionary<string, ShoppingCartSummaryItemModel> copyDict);

        /// <summary>
        /// False if price would go below zero.
        /// </summary>
        /// <remarks>
        /// Feel free to override to add additional rules.
        /// If it gets complex, replace with responsibility chain.
        /// </remarks>
        protected virtual bool CanApply(
            ShoppingCartSummaryItemModel item,
            decimal priceAfterDiscount)
        {
            if (priceAfterDiscount < 0)
            {
                return false;
            }

            return true;
        }

    }
}
