using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Library.Model
{
    /// <summary>
    /// State of ShoppingCart.
    /// </summary>
    /// <remarks>
    /// Note that there is no Sum, just Items. This is because there is a requirement to log entire cart. Therefore, Sum is a method.
    /// </remarks>
    public class ShoppingCartModel
    {
        /// <summary>
        /// New, empty, shopping cart.
        /// </summary>
        public ShoppingCartModel()
        {

        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public ShoppingCartModel(
            ShoppingCartModel original)
        {
            this.Id = original.Id;

            foreach(var originalItem in original.Items)
            {
                this.Items.Add(
                    originalItem.Key, 
                    new ShoppingCartItemModel(originalItem.Value));
            }
        }

        /// <summary>
        /// Surrogate key. Assigned on Save.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Key is Product.Name.
        /// Immutable. Initialize as empty, then add items using AddAndSave.
        /// </summary>
        public Dictionary<string, ShoppingCartItemModel> Items { get; } = new Dictionary<string, ShoppingCartItemModel>();
    }
}
