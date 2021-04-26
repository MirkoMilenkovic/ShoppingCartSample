using ShoppingCartSample.Library.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.DiscountPlugin
{
    /// <summary>
    /// Buy two butters and get one bread at 50% off
    /// </summary>
    public interface IDiscountOneCalculator : ISingleDiscountCalculator
    {

    }
}
