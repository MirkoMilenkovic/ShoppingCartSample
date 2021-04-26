using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Host.Database
{
    /// <summary>
    /// This is latest and greates in-memory, object database.
    /// </summary>
    /// <remarks>
    /// Use as Singleton!!! It is a database, we have only one.
    /// In real world, this would be Sql Server.    
    /// </remarks>
    public interface IInMemoryDB
    {
        /// <summary>
        /// Key is Product.Name.
        /// </summary>
        Dictionary<string, ProductModel> ProductGetAll();

        /// <summary>
        /// Returns Id assigned by DB.
        /// </summary>
        int ShoppingCartSave(
            ShoppingCartModel shoppingCart);

        ShoppingCartModel ShoppingCartGet(
            int shooopingCartId);
    }
}
