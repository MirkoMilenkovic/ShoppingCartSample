using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Host.OutputPrinting
{
    public class HtmlPrinter : IHtmlPrinter
    {
        public string Print(ShoppingCartSummaryModel model)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"User output BEGIN");
            sb.AppendLine($"<ul>");
            foreach (ShoppingCartSummaryItemModel summaryItem in model.SummaryDict.Values)
            {
                sb.Append("<li>");
                sb.Append(
                    $"{summaryItem.ProductName}; UP: {summaryItem.ProductUnitPrice}; Q: {summaryItem.Quantity}; TP: {summaryItem.TotalPrice}; TPD: {summaryItem.TotalPriceWithDiscount}; Discount applied: {summaryItem.DiscountApplied}");
                sb.AppendLine("</li>");
            }

            sb.AppendLine($"Sum: {model.Sum}");
            sb.AppendLine($"<ul>");

            sb.AppendLine($"User output END");

            return sb.ToString();
        }
    }
}
