using FibonacciTrader.Models;
using Newtonsoft.Json;

namespace FibonacciTrader.Services
{
    public class CoinApiClient
    {
        private readonly string _apiKey;
        private readonly string _apiUrl;
        public CoinApiClient() 
        {
            _apiKey = "";
            _apiUrl = "https://rest.coinapi.io/v1/exchangerate/[ASSET]/USD/history";
        }

        public async Task<List<ExchangeRateItem>> GetPrices(string asset, DateTime startDate, DateTime endDate, List<ExchangeRateItem> storedItems)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-CoinAPI-Key", _apiKey);
            var apiUrl = _apiUrl.Replace("[ASSET]", asset);
            var limit = (endDate - startDate).TotalDays;

            var url = $"{apiUrl}?period_id=1DAY&time_start={startDate.ToString("yyyy-MM-ddTHH:mm:ss")}&time_end={endDate.ToString("yyyy-MM-ddTHH:mm:ss")}&limit={limit}";
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var output = JsonConvert.DeserializeObject<List<ExchangeRateItem>>(responseContent);
                output.ForEach(eri => eri.asset = asset);
                output = MergeItemLists(storedItems, output);
                return output;
            }

            throw new Exception($"Failed to retrieve Ethereum prices: {response.StatusCode}");
        }

        private static List<ExchangeRateItem> MergeItemLists(List<ExchangeRateItem> oldList, List<ExchangeRateItem> newList)
        {
            Dictionary<DateTime, ExchangeRateItem> combinedList = new Dictionary<DateTime, ExchangeRateItem>();
            foreach (var item in oldList)
            {
                combinedList.Add(item.TimePeriodStart, item);
            }

            foreach (var item in newList)
            {
                if (item.RateClose > item.RateHigh)
                    item.RateClose = item.RateHigh;
                if (!combinedList.ContainsKey(item.TimePeriodStart))
                    combinedList.Add(item.TimePeriodStart, item);
            }

            return combinedList.Values.OrderBy(i => i.TimePeriodStart).ToList();
        }
    }
}
