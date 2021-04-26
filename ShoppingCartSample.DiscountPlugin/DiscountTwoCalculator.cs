using ShoppingCartSample.Host.BLL;
using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.DiscountPlugin
{
    public class DiscountTwoCalculator : SingleDiscountCalculatorBase, IDiscountTwoCalculator
    {
        protected override string RuleIdentifier
        {
            get
            {
                return "Two";
            }
        }

        /// <summary>
        /// Buy three milks and get fourth one for free
        /// </summary>
        /// <param name="copyDict"></param>
        //TODO
        //Logging, a lot of it.
        protected override Dictionary<string, ItemToDiscount> ItemsToDiscountGet(
            Dictionary<string, ShoppingCartSummaryItemModel> copyDict)
        {
            Dictionary<string, ItemToDiscount> dictToReturn = new Dictionary<string, ItemToDiscount>();

            copyDict.TryGetValue(
                "Milk",
                out ShoppingCartSummaryItemModel milk);

            if (milk == null)
            {
                //no milk in cart
                return dictToReturn;
            }

            int milkQuantity = default;

            try
            {
                milkQuantity = Convert.ToInt32(milk.Quantity);
            }
            catch
            {
                throw new Exception($"How did you manage to buy part of one milk?");
            }

            if (milkQuantity < 4)
            {
                //not enough milk
                return dictToReturn;
            }

            int modulusOf4 = milkQuantity % 4;

            int numberOfPacksOf4 = milkQuantity / 4;

            decimal totalPriceOfPacksOf4 = numberOfPacksOf4 * (milk.ProductUnitPrice * 3);

            decimal totalPrice = totalPriceOfPacksOf4 + modulusOf4 * milk.ProductUnitPrice;

            dictToReturn.Add(
                milk.ProductName,
                new ItemToDiscount(milk, totalPrice));

            return dictToReturn;
        }
    }
}
