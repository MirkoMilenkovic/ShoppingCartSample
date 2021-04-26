using ShoppingCartSample.Host.Database;
using ShoppingCartSample.Library.DAL;
using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Host.DAL
{
    public class ProductRepository : IProductRepository
    {
        public ProductRepository(IInMemoryDB inMemoryDB)
        {
            InMemoryDB = inMemoryDB;
        }

        private IInMemoryDB InMemoryDB { get; }

        public Dictionary<string, ProductModel> GetAll()
        {
            return this.InMemoryDB.ProductGetAll();
        }

        public ProductModel GetByName(string name)
        {
            this.GetAll().TryGetValue(
                name,
                out ProductModel product);

            return product;
        }

    }
}
