using ShoppingCartSample.Library.BLL;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Host.BLL
{
    public interface IDiscountPluginRepository
    {
        IEnumerable<ISingleDiscountCalculator> GetAll();
    }
}
