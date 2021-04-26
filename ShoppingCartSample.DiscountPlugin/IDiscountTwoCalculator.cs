using ShoppingCartSample.Library.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.DiscountPlugin
{
    /// <summary>
    /// Buy three milks and get fourth one for free
    /// </summary>
    public interface IDiscountTwoCalculator : ISingleDiscountCalculator
    {
    }
}
