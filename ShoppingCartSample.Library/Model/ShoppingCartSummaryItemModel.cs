using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Library.Model
{
    /// <summary>
    /// Shopping cart item with calculated price, based in discount.
    /// </summary>
    /// <remarks>
    /// It will also be used for printing, to save on some coding.
    /// </remarks>
    public class ShoppingCartSummaryItemModel
    {
        public ShoppingCartSummaryItemModel()
        {

        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public ShoppingCartSummaryItemModel(
            ShoppingCartSummaryItemModel original)
        {
            this.ProductUnitPrice = original.ProductUnitPrice;

            this.ProductName = original.ProductName;

            this.Quantity = original.Quantity;

            this.TotalPrice = original.TotalPrice;

            this.TotalPriceWithDiscount = original.TotalPriceWithDiscount;

            this.DiscountApplied = original.DiscountApplied;
        }

        public string ProductName { get; set; }

        public decimal ProductUnitPrice { get; set; }

        public decimal Quantity { get; set; }

        /// <summary>
        /// ItemPrice * Quantity
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// TotalPrice after applying discount.
        /// </summary>
        public decimal TotalPriceWithDiscount { get; set; }

        public string DiscountApplied { get; set; }
    }
}
