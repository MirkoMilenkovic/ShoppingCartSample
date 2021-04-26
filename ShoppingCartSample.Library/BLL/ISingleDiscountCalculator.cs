using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Library.BLL
{
    /// <summary>
    /// Applies one discount rule
    /// </summary>
    public interface ISingleDiscountCalculator
    {
        Dictionary<string, ShoppingCartSummaryItemModel> Calculate(
            IReadOnlyDictionary<string, ShoppingCartSummaryItemModel> originalDict);
    }
}
