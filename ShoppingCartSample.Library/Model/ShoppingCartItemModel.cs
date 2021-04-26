using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Library.Model
{
    /// <summary>
    /// Item in shopping cart
    /// </summary>
    public class ShoppingCartItemModel
    {

        /// <summary>
        /// Creates ShoppingCartItem. 
        /// </summary>
        /// <remarks>
        /// Item can't exist without Product.
        /// </remarks>
        public ShoppingCartItemModel(
            string productName)
        {
            ProductName = productName;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public ShoppingCartItemModel(
            ShoppingCartItemModel original)
        {
            this.ProductName = original.ProductName;

            this.Quantity = original.Quantity;
        }

        /// <summary>
        /// ProductName. Immutable. 
        /// </summary>
        public string ProductName { get; }

        /// <summary>
        /// Quantity. Mutable.
        /// </summary>
        public decimal Quantity { get; set; }
    }
}
