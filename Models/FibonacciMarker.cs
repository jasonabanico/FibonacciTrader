namespace FibonacciTrader.Models
{
    public class FibonacciMarker
    {
        public decimal Level { get; set; }
        public decimal Value { get; set; }

        public FibonacciMarker(decimal level, decimal value)
        {
            Level = level;
            Value = value;
        }
    }
}
