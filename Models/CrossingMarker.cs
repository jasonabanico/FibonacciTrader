namespace FibonacciTrader.Models
{
    public class CrossingMarker
    {
        public ExchangeRateItem ExchangeRateItem { get; set; }
        public FibonacciMarker FibonacciMarker { get; set; }

        public CrossingMarker(ExchangeRateItem exchangeRateItem, FibonacciMarker fibonacciMarker)
        {
            ExchangeRateItem = exchangeRateItem;
            FibonacciMarker = fibonacciMarker;
        }
    }
}
