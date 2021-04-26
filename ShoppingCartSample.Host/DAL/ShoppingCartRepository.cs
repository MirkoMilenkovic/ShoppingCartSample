using ShoppingCartSample.Host.Database;
using ShoppingCartSample.Library.DAL;
using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Host.DAL
{
    public class ShoppingCartRepository: IShoppingCartRepository
    {
        public ShoppingCartRepository(IInMemoryDB inMemoryDB)
        {
            InMemoryDB = inMemoryDB;
        }

        private IInMemoryDB InMemoryDB { get; }

        public ShoppingCartModel Load(int shoppingCartId)
        {
            ShoppingCartModel sc = this.InMemoryDB.ShoppingCartGet(
                shoppingCartId);

            return sc;
        }

        public void Save(ShoppingCartModel shoppingCart)
        {
            shoppingCart.Id = this.InMemoryDB.ShoppingCartSave(
                shoppingCart);
        }
    }
}
