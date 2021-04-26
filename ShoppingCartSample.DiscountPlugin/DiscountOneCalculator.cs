using ShoppingCartSample.Host.BLL;
using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCartSample.DiscountPlugin
{

    public class DiscountOneCalculator : SingleDiscountCalculatorBase, IDiscountOneCalculator
    {
        protected override string RuleIdentifier
        {
            get
            {
                return "One";
            }
        }

        /// <summary>
        /// Buy two butters and get one bread at 50% off
        /// </summary>
        //TODO
        //Logging, a lot of it.
        protected override Dictionary<string, ItemToDiscount> ItemsToDiscountGet(
            Dictionary<string, ShoppingCartSummaryItemModel> copyDict)
        {
            Dictionary<string, ItemToDiscount> dictToReturn = new Dictionary<string, ItemToDiscount>();

            copyDict.TryGetValue(
                "Butter",
                out ShoppingCartSummaryItemModel butter);

            if (butter == null)
            {
                //no butter in cart
                return dictToReturn;
            }

            //TODO requirement is not clear. What should happen if we buy four butters?
            //assume that discount applies to one bread only, whatever the number of butters is.
            if (butter.Quantity < 2)
            {
                //nothing to do
                return dictToReturn;
            }

            copyDict.TryGetValue("Bread", out ShoppingCartSummaryItemModel bread);
            if (bread == null)
            {
                //no bread in cart
                return dictToReturn;
            }

            decimal amountToReduce = bread.ProductUnitPrice * .5M;

            decimal priceAfterDiscount = bread.TotalPriceWithDiscount - amountToReduce;

            dictToReturn.Add(
                bread.ProductName,
                new ItemToDiscount(bread, priceAfterDiscount));

            return dictToReturn;
        }
    }
}
