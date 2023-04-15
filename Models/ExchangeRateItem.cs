using Newtonsoft.Json;

namespace FibonacciTrader.Models
{
    public class ExchangeRateItem
    {
        public string asset { get; set; }

        [JsonProperty("time_period_start")]
        public DateTime TimePeriodStart { get; set; }

        [JsonProperty("time_period_end")]
        public DateTime TimePeriodEnd { get; set; }

        [JsonProperty("time_open")]
        public DateTime TimeOpen { get; set; }

        [JsonProperty("time_close")]
        public DateTime TimeClose { get; set; }

        [JsonProperty("rate_open")]
        public decimal RateOpen { get; set; }

        [JsonProperty("rate_high")]
        public decimal RateHigh { get; set; }

        [JsonProperty("rate_low")]
        public decimal RateLow { get; set; }

        [JsonProperty("rate_close")]
        public decimal RateClose { get; set; }

        public int Cycle { get; set; }
        public Dictionary<string, decimal> Indicators { get; set; } = new Dictionary<string, decimal>();
    }
}
