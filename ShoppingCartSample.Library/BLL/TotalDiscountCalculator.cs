using ShoppingCartSample.Library.DAL;
using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Library.BLL
{
    /// <summary>
    /// Default Implementation of ITotalDiscountCalculator.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class TotalDiscountCalculator : ITotalDiscountCalculator
    {
        public TotalDiscountCalculator(
            IProductRepository productRepository)
        {
            ProductRepository = productRepository;
        }

        private IProductRepository ProductRepository { get; }

        public Dictionary<string, ShoppingCartSummaryItemModel> Calculate(
            IReadOnlyDictionary<string, ShoppingCartItemModel> shoppingCartItems,
            IEnumerable<ISingleDiscountCalculator> discountCalculatorList)
        {
            Dictionary<string, ShoppingCartSummaryItemModel> summaryDict = new Dictionary<string, ShoppingCartSummaryItemModel>();
            //create initial list of ShoppingCartSummaryItemModel
            foreach (ShoppingCartItemModel shoppingCartItem in shoppingCartItems.Values)
            {
                ShoppingCartSummaryItemModel summaryItem = this.SummaryItemCreate(
                    shoppingCartItem);

                summaryDict.Add(
                    summaryItem.ProductName,
                    summaryItem);
            }

            //apply discount rules, as ordered by Host
            foreach(ISingleDiscountCalculator discountCalc in discountCalculatorList)
            {
                if(!this.DoesDiscountApply(discountCalc))
                {
                    //do not apply discount,
                    continue;
                }

                summaryDict = discountCalc.Calculate(
                    summaryDict);
            }

            return summaryDict;
        }

        /// <summary>
        /// Factory method.
        /// </summary>
        private ShoppingCartSummaryItemModel SummaryItemCreate(
            ShoppingCartItemModel shoppingCartItem)
        {
            ProductModel product = this.ProductRepository.GetByName(
                shoppingCartItem.ProductName);

            ShoppingCartSummaryItemModel summaryItem = new ShoppingCartSummaryItemModel()
            {
                ProductUnitPrice = product.Price,
                ProductName = product.Name,
                Quantity = shoppingCartItem.Quantity,
                TotalPrice = shoppingCartItem.Quantity * product.Price
            };

            //at first, price with discount is same as price.
            summaryItem.TotalPriceWithDiscount = summaryItem.TotalPrice;
            return summaryItem;
        }

        /// <summary>
        /// Returns true if rule applies.
        /// </summary>
        /// <remarks>
        /// At present, we do not know what logic might apply to discounts. It could depend on user history, date, shopping cart value etc.
        /// This is stub implementation.
        /// It would be nice to apply open-closed principle and make method virtual, but we do not have a clue about it's parameters. 
        /// Do not waste any more time on this now.
        /// </remarks>
        private bool DoesDiscountApply(
            ISingleDiscountCalculator calculator)
        {
            return true;
        }
    }
}
