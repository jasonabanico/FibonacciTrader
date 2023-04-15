using FibonacciTrader.Services;

namespace FibonacciTrader
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var apiClient = new CoinApiClient();
            var fibonacciAnalyzer = new FibonacciAnalyzer();
            string filePath = "c:\\Output";
            var fileWriter = new FileWriter(filePath);

            string[] assets = { "BTC", "ETH", "ADA", "XRP", "BNB" };

            foreach (var asset in assets)
            {
                var itemsFilename = $"{asset}-items.txt";
                var items = fileWriter.ReadItemsFromFile(itemsFilename);

                var startDate = items.Count > 0 ? items[items.Count - 1].TimePeriodStart : new DateTime(2015, 1, 1);
                var endDate = DateTime.Today;
                if ((endDate - startDate).TotalDays > 1)
                    items = await apiClient.GetPrices(asset, startDate, endDate, items);
                fileWriter.WriteItemsToFile(items, itemsFilename);

                var technicalAnalyzer = new TechnicalAnalyzer(items);
                technicalAnalyzer.AnnotateTechnicalAnalysis();
                fibonacciAnalyzer.Analyze(asset, items, filePath);
            }
        }
    }
}