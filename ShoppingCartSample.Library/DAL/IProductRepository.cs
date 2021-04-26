using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Library.DAL
{
    /// <summary>
    /// Product repository.
    /// </summary>
    /// <remarks>
    /// Implementation is host-dependant, so it will be injected by host.
    /// </remarks>
    public interface IProductRepository
    {
        /// <summary>
        /// Returns all products from storage.  Key is Product.Name
        /// </summary>
        /// <remarks>
        /// Returns Dictionary to emphasise that Name is unique.
        /// 
        /// </remarks>
        Dictionary<string, ProductModel> GetAll();

        /// <summary>
        /// Returns a product.
        /// </summary>
        ProductModel GetByName(
            string name);
    }
}
