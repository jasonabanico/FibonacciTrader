using FibonacciTrader.Models;
using TANet.Core;

namespace FibonacciTrader.Services
{
    public class TechnicalAnalyzer
    {
        private readonly List<ExchangeRateItem> _items;

        public TechnicalAnalyzer(List<ExchangeRateItem> items)
        {
            _items = items;
        }

        public void Analayze()
        {
            Sma(200);
            Rsi(14);
            Macd(26, 12, 9);
        }

        public void Sma(int period)
        {
            var prices = new List<decimal>();
            _items.ForEach(i => prices.Add(i.RateClose));
            var results = Indicators.Sma(prices.ToArray(), period).Ma;
            var key = $"Sma{period}";
            UpdateIndicators(key, results, _items);
        }

        public void Rsi(int period)
        {
            var prices = new List<decimal>();
            _items.ForEach(i => prices.Add(i.RateClose));
            var results = Indicators.Rsi(prices.ToArray(), period).Rsi;
            var key = $"Rsi{period}";
            UpdateIndicators(key, results, _items);
        }

        public void Macd(int fastPeriod, int slowPeriod, int signalPeriod)
        {
            var prices = new List<decimal>();
            _items.ForEach(i => prices.Add(i.RateClose));
            var results = Indicators.Macd(prices.ToArray(), fastPeriod, slowPeriod, signalPeriod).Macd;
            var key = $"Macd{fastPeriod}-{slowPeriod}-{signalPeriod}";
            UpdateIndicators(key, results.ToArray(), _items);
        }

        public static void UpdateIndicators(string key, decimal[] results, List<ExchangeRateItem> items)
        {
            var starting = items.Count - results.Length;
            for (int i = 0; i < results.Count(); i++)
            {
                var index = starting + i;
                var indicators = items[index].Indicators;
                if (indicators == null) indicators = new Dictionary<string, decimal>();

                if (!indicators.ContainsKey(key)) indicators.Add(key, results[i]);
                else indicators[key] = results[i];

                items[index].Indicators = indicators;
            }
        }
    }
}
