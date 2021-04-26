using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Library.DAL
{
    /// <summary>
    /// Shopping cart repository.
    /// </summary>
    /// <remarks>
    /// Implementation is host-dependant, so it will be injected by host.
    /// </remarks>
    public interface IShoppingCartRepository
    {
        ShoppingCartModel Load(
            int shoppingCartId);

        void Save(
            ShoppingCartModel shoppingCart);
    }
}
