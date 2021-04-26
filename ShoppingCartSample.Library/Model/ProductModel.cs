using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Library.Model
{
    /// <summary>
    /// Product
    /// </summary>
    /// <remarks>
    /// Key is ProductName. In real world, we would use either some natural "Code", or DB Id as a key.
    /// </remarks>    
    public class ProductModel
    {
        /// <summary>
        /// Creates Product.
        /// </summary>
        /// <remarks>
        /// Product can't exist without Name.
        /// Price is immutable from the point of view of Shopping cart.
        /// </remarks>
        public ProductModel(
            string name,
            decimal price)
        {
            this.Name= name;

            this.Price = price;
        }

        public string Name { get; }

        public decimal Price { get; }
    }
}
