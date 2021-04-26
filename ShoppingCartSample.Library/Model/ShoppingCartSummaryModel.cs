using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Library.Model
{
    /// <summary>
    /// Display info of shopping cart.
    /// </summary>
    public class ShoppingCartSummaryModel
    {
        public decimal Sum { get; set; }

        public Dictionary<string, ShoppingCartSummaryItemModel> SummaryDict { get; set; }
    }
}
