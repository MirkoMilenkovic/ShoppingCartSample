using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCartSample.Host.OutputPrinting
{
    public class OutputPrinterFactory : IOutputPrinterFactory
    {
        public OutputPrinterFactory(
            IHtmlPrinter htmlPrinter, 
            IConsolePrinter consolePrinter)
        {
            HtmlPrinter = htmlPrinter;
            ConsolePrinter = consolePrinter;
        }

        private IHtmlPrinter HtmlPrinter { get; }

        private IConsolePrinter ConsolePrinter { get; }

        public IOutputPrinter PrinterGet(PrintModeEnum printMode)
        {
            switch(printMode)
            {
                case PrintModeEnum.Console:
                    return this.ConsolePrinter;
                case PrintModeEnum.Html:
                    return this.HtmlPrinter;
                default:
                    throw new ArgumentOutOfRangeException($"{nameof(printMode)}: {printMode} is not supported");
            }
        }
    }
}
