using ShoppingCartSample.Library.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Host.OutputPrinting
{
    /// <summary>
    /// Turns Summary into a string to be printed.
    /// </summary>
    /// <remarks>
    /// Implementations will depend on the kind of UI.
    /// In real wolrd, this would return "ViewModel"
    /// </remarks>
    public interface IOutputPrinter
    {
        string Print(
            ShoppingCartSummaryModel model);
    }
}
