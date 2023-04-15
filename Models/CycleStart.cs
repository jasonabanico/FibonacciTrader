namespace FibonacciTrader.Models
{
    public class CycleStart
    {
        public int Cycle { get; set; }
        public DateTime StartDate { get; set; }

        public CycleStart(int cycle, DateTime startDate)
        {
            Cycle = cycle;
            StartDate = startDate;
        }
    }
}
