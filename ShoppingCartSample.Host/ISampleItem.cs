using ShoppingCartSample.Host.OutputPrinting;

namespace ShoppingCartSample.Host
{
    public interface ISampleItem
    {
        int SampleId { get;  }

        void Configure(
            int sampleId,
            PrintModeEnum printMode);

        void Run();
    }
}