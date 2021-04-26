using ShoppingCartSample.Library.BLL;
using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Library
{
    /// <summary>
    /// An instance of shopping cart.
    /// </summary>
    /// <remarks>
    /// Stateful!!!
    /// Thread-unsafe!!!
    /// </remarks>
    public interface IShoppingCart
    {
        /// <summary>
        /// Id of ShoppingCart.State
        /// </summary>
        int? Id { get; }

        /// <summary>
        /// Configures discounts to apply.
        /// </summary>
        /// <remarks>
        /// Call before calling Sum.
        /// Note:
        /// This could have been implemented by injecting IDiscountRepository, but we have opted to inject "strategy" explicitly. 
        /// This is key business logic (as opposed to dumb DAL classes), so it should be clear where the logic is coming from.
        /// </remarks>
        void ConfigureDiscounts(
            IEnumerable<ISingleDiscountCalculator> singleDiscountCalculatorList);

        /// <summary>
        /// Load from DB.
        /// </summary>
        void Load(
            int shoppingCartId);

        /// <summary>
        /// Adds Product and saves to DB.
        /// </summary>
        void AddAndSave(
            ProductModel product,
            decimal quantity);

        /// <summary>
        /// Removes all items.
        /// </summary>
        void ClearAndSave();

        /// <summary>
        /// Calculates discount and returns sum and display info.
        /// </summary>
        /// <remarks>
        /// There is a requirement for a cart to log it's state on sum. We are fulfilling that by returing list of strings to display.
        /// </remarks>
        ShoppingCartSummaryModel Sum();

    }
}
