using ShoppingCartSample.Library.BLL;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace ShoppingCartSample.Host.BLL
{
    /// <summary>
    /// Returns all injected classes that implement ISingleDiscountCalculator
    /// </summary>
    public class DiscountPluginRepository : IDiscountPluginRepository
    {
        private IServiceProvider ServiceProvider { get; }

        public DiscountPluginRepository(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }


        public IEnumerable<ISingleDiscountCalculator> GetAll()
        {
            IEnumerable<ISingleDiscountCalculator> discountCalculators =  this.ServiceProvider.GetServices<ISingleDiscountCalculator>();

            return discountCalculators;
        }
    }
}
