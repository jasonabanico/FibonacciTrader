namespace FibonacciTrader.Models
{
    public class CrossingMarker
    {
        public ExchangeRateItem ExchangeRateItem { get; set; }
        public FibonacciMarker FibonacciMarker { get; set; }
        public int Days { get; set; }

        public CrossingMarker(ExchangeRateItem exchangeRateItem, FibonacciMarker fibonacciMarker, int days)
        {
            ExchangeRateItem = exchangeRateItem;
            FibonacciMarker = fibonacciMarker;
            Days = days;
        }
    }
}
