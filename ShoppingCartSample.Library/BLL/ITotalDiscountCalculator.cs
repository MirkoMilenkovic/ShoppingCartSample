using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Library.BLL
{
    /// <summary>
    /// Applies all discount rules.
    /// </summary>
    public interface ITotalDiscountCalculator
    {
        /// <summary>
        /// Appliy discounts.
        /// </summary>
        /// <remarks>
        /// Note that params are read-only.
        /// </remarks>
        Dictionary<string, ShoppingCartSummaryItemModel> Calculate(
            IReadOnlyDictionary<string, ShoppingCartItemModel> shoppingCartItems,
            IEnumerable<ISingleDiscountCalculator> singleDiscountCalculatorList);
    }
}
