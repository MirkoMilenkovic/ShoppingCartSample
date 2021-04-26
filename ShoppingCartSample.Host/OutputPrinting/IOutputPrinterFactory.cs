using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Host.OutputPrinting
{
    public interface IOutputPrinterFactory
    {
        IOutputPrinter PrinterGet(
            PrintModeEnum printMode);
    }
}
