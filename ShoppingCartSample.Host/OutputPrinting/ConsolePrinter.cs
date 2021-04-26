using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Host.OutputPrinting
{
    public class ConsolePrinter : IConsolePrinter
    {
        public string Print(ShoppingCartSummaryModel model)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"User output BEGIN");
            sb.AppendLine($"Dear User, this is your shopping cart");
            foreach (ShoppingCartSummaryItemModel summaryItem in model.SummaryDict.Values)
            {
                sb.AppendLine(
                    $"{summaryItem.ProductName}; UP: {summaryItem.ProductUnitPrice}; Q: {summaryItem.Quantity}; TP: {summaryItem.TotalPrice}; TPD: {summaryItem.TotalPriceWithDiscount}; Discount applied: {summaryItem.DiscountApplied}");
            }

            sb.AppendLine($"Sum: {model.Sum}");

            sb.AppendLine($"User output END");

            return sb.ToString();
        }
    }
}
