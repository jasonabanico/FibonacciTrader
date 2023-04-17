using FibonacciTrader.Models;
using System.Reflection.Emit;

namespace FibonacciTrader.Services
{
    public class FibonacciAnalyzer
    {
        public List<CycleStart> CycleStarts { get; set; } = new List<CycleStart>();
        public Dictionary<string, ExchangeRateItem> CycleTops { get; set; } = new Dictionary<string, ExchangeRateItem>();
        public Dictionary<string, ExchangeRateItem> CycleBottoms { get; set; } = new Dictionary<string, ExchangeRateItem>();
        public Dictionary<string, List<FibonacciMarker>> Retracements { get; set; } = new Dictionary<string, List<FibonacciMarker>>();
        public Dictionary<string, List<FibonacciMarker>> Extensions { get; set; } = new Dictionary<string, List<FibonacciMarker>>();
        public Dictionary<string, List<CrossingMarker>> RetracementCrossings { get; set; } = new Dictionary<string, List<CrossingMarker>>();
        public Dictionary<string, List<CrossingMarker>> ExtensionCrossings { get; set; } = new Dictionary<string, List<CrossingMarker>>();

        public void Analyze(string asset, List<ExchangeRateItem> items, string filePath)
        {
            MarkCycles(items);
            IdentifyCycleTops(items);
            IdentifyCycleBottoms(items);
            CalculateRetracements(asset);
            CalculateExtensions(asset);
            MarkCrossings(asset,items);
            GenerateReport(asset, new FileWriter(filePath));
        }

        public void MarkCycles(List<ExchangeRateItem> items)
        {
            var btcCreation = new DateTime(2009, 1, 3);
            int previousCycle = 0;

            foreach (var item in items)
            {
                var cycle = (item.TimePeriodStart - btcCreation).Days / 1400;
                if (cycle > previousCycle)
                {
                    CycleStarts.Add(new CycleStart(cycle, item.TimePeriodStart));
                    previousCycle = cycle;
                }

                item.Cycle = cycle;
            }
        }

        public void IdentifyCycleTops(List<ExchangeRateItem> items)
        {
            foreach (var item in items)
            {
                var cycle = item.Cycle;
                var key = $"{item.asset}-{cycle}";

                // only mark cycle top after the 1st half of the cycle has passed
                var cycleStart = CycleStarts.Where(cs => cs.Cycle == cycle).FirstOrDefault();
                if ((DateTime.Today - cycleStart.StartDate).TotalDays > 700)
                {
                    if (!CycleTops.ContainsKey(key))
                    {
                        CycleTops.Add(key, item);
                    }
                    else
                    {
                        var rate_close = item.RateClose;
                        if (rate_close > CycleTops[key].RateClose)
                        {
                            CycleTops[key] = item;
                        }
                    }
                }
            }
        }

        public void IdentifyCycleBottoms(List<ExchangeRateItem> items)
        {
            foreach (var item in items)
            {
                var cycle = item.Cycle;
                var key = $"{item.asset}-{cycle}";
                var cycleTopDate = CycleTops[key].TimePeriodStart;

                // marking bottoms only after top, as cycle starts on an uptrend
                if (item.TimePeriodStart > cycleTopDate)
                {
                    if (!CycleBottoms.ContainsKey(key))
                    {
                        CycleBottoms.Add(key, item);
                    }
                    else
                    {
                        if (item.RateClose < CycleBottoms[key].RateClose)
                        {
                            CycleBottoms[key] = item;
                        }
                    }
                }
            }
        }

        public static readonly List<decimal> RetracementLevels = new List<decimal>()
        {
            0.236m, 0.382m, 0.5m, 0.618m, 0.786m
        };

        public static readonly List<decimal> ExtensionLevels = new List<decimal>()
        {
            1.618m, 2.618m, 4.236m, 6.854m
        };

        public void CalculateRetracements(string asset)
        {
            var cycleTops = CycleTops.Where(ct => ct.Key.StartsWith(asset));
            foreach (var tops in cycleTops)
            {
                var values = new List<double>();
                var cycle = int.Parse(tops.Key.Replace($"{asset}-", ""));
                var previousCycle = cycle - 1;
                var previousCycleKey = $"{asset}-{previousCycle}";
                if (CycleBottoms.ContainsKey(previousCycleKey))
                {
                    var previousBottom = CycleBottoms[previousCycleKey].RateClose;
                    var currentTop = tops.Value.RateClose;
                    var retracements = new List<FibonacciMarker>();
                    foreach (var retracementLevel in RetracementLevels)
                    {
                        var value = currentTop - ((currentTop - previousBottom) * retracementLevel);
                        if (value > 0) retracements.Add(new FibonacciMarker(retracementLevel, value));
                    }

                    Retracements.Add(tops.Key, retracements);
                }
            }
        }

        public void CalculateExtensions(string asset)
        {
            var cycleTops = CycleTops.Where(ct => ct.Key.StartsWith(asset));
            foreach (var tops in cycleTops)
            {
                var currentTop = tops.Value.RateClose;
                var currentBottom = CycleBottoms[tops.Key].RateClose;
                var retracement = currentTop - currentBottom;

                var extensions = new List<FibonacciMarker>();
                foreach (var extensionLevel in ExtensionLevels)
                {
                    var value = currentBottom + (extensionLevel * retracement);
                    if (value > 0) extensions.Add(new FibonacciMarker(extensionLevel, value));
                }

                Extensions.Add(tops.Key, extensions);
            }
        }

        public void MarkCrossings(string asset, List<ExchangeRateItem> items)
        {
            foreach (var item in items)
            {
                var cycle = item.Cycle;
                var cycleKey = $"{asset}-{cycle}";
                if (cycle > 1)
                {
                    var previousCycle = cycle - 1;
                    var previousCycleKey = $"{asset}-{previousCycle}";
                    if (Extensions.ContainsKey(previousCycleKey))
                    {
                        var extensions = Extensions[previousCycleKey];
                        CrossingMarker extensionCrossingMarker = null;
                        for (int i = 0; i < extensions.Count; i++)
                        {
                            if (!ExtensionCrossings.Any(rc => rc.Key == cycleKey &&
                                    rc.Value.Any(cm => cm.FibonacciMarker.Level == extensions[i].Level))
                                && item.RateClose > extensions[i].Value)
                            {
                                if (!ExtensionCrossings.ContainsKey(cycleKey))
                                    ExtensionCrossings[cycleKey] = new List<CrossingMarker>();
                                extensionCrossingMarker = new CrossingMarker(item, extensions[i]);
                            }
                        }
                        if (extensionCrossingMarker != null) ExtensionCrossings[cycleKey].Add(extensionCrossingMarker);

                        var retracements = Retracements[cycleKey];
                        CrossingMarker retracementCrossingMarker = null;
                        for (int i = 0; i < retracements.Count; i++)
                        {
                            if (!RetracementCrossings.Any(rc => rc.Key == cycleKey && 
                                    rc.Value.Any(cm => cm.FibonacciMarker.Level == retracements[i].Level))
                                && item.TimePeriodStart > CycleTops[cycleKey].TimePeriodStart
                                && item.RateClose < retracements[i].Value)
                            {
                                if (!RetracementCrossings.ContainsKey(cycleKey))
                                    RetracementCrossings[cycleKey] = new List<CrossingMarker>();
                                retracementCrossingMarker = new CrossingMarker(item, retracements[i]);
                            }
                        }
                        if (retracementCrossingMarker != null)
                            RetracementCrossings[cycleKey].Add(retracementCrossingMarker);
                    }
                }
            }
        }

        public void GenerateReport(string asset, FileWriter writer)
        {
            var decimalFormat = "###,###,##0.00";
            writer.AddToLog("\n----------");
            writer.AddToLog($"{asset} REPORT");
            writer.AddToLog("----------");

            writer.AddToLog("\nNote: Cycle 1 is 2015 because that's the earliest record in CoinApi.");

            writer.AddToLog("\nCycle Tops:");
            var cycleTops = CycleTops.Where(ct => ct.Key.StartsWith(asset));
            foreach (var top in cycleTops)
            {
                if (top.Value.RateClose > 0)
                {
                    var cycle = top.Key.Replace($"{asset}-", "");
                    writer.AddToLog($"  For cycle {CycleString(cycle)}, top happened on {top.Value.TimePeriodStart.ToShortDateString()} " +
                        $"at ${top.Value.RateClose.ToString(decimalFormat)}. " +
                        $"[RSI: {top.Value.Indicators["Rsi14"].ToString(decimalFormat)}] " +
                        $"[MACD: {top.Value.Indicators["Macd26-12-9-Macd"].ToString(decimalFormat)} " +
                        $"/ Signal: {top.Value.Indicators["Macd26-12-9-Signal"].ToString(decimalFormat)}]");
                }
            }

            writer.AddToLog("\nCycle Bottoms:");
            var cycleBottoms = CycleBottoms.Where(ct => ct.Key.StartsWith(asset));
            foreach (var bottom in cycleBottoms)
            {
                if (bottom.Value.RateClose > 0)
                {
                    var cycle = bottom.Key.Replace($"{asset}-", "");
                    writer.AddToLog($"  For cycle {CycleString(cycle)}, bottom happened on {bottom.Value.TimePeriodStart.ToShortDateString()} " +
                        $"at ${bottom.Value.RateClose.ToString(decimalFormat)}. " +
                        $"[RSI: {bottom.Value.Indicators["Rsi14"].ToString(decimalFormat)}] " +
                        $"[MACD: {bottom.Value.Indicators["Macd26-12-9-Macd"].ToString(decimalFormat)} " +
                        $"/ Signal: {bottom.Value.Indicators["Macd26-12-9-Signal"].ToString(decimalFormat)}]");
                }
            }

            writer.AddToLog("\nCycle Retracement Crossings:");
            var retracementCrossings = RetracementCrossings.Where(r => r.Key.StartsWith(asset));
            foreach (var retracementCrossingCycles in retracementCrossings)
            {
                var cycle = retracementCrossingCycles.Key.Replace($"{asset}-", "");
                writer.AddToLog($"- For cycle {CycleString(cycle)}:");
                foreach (var retracementCrossing in retracementCrossingCycles.Value)
                    writer.AddToLog($"  Retracement level {retracementCrossing.FibonacciMarker.Level} of " +
                        $"${retracementCrossing.FibonacciMarker.Value.ToString(decimalFormat)} crossed on " +
                        $"{retracementCrossing.ExchangeRateItem.TimePeriodStart.ToShortDateString()} at " +
                        $"${retracementCrossing.ExchangeRateItem.RateClose.ToString(decimalFormat)}. " +
                        $"[RSI: {retracementCrossing.ExchangeRateItem.Indicators["Rsi14"].ToString(decimalFormat)}] " +
                        $"[MACD: {retracementCrossing.ExchangeRateItem.Indicators["Macd26-12-9-Macd"].ToString(decimalFormat)} " +
                        $"/ Signal: {retracementCrossing.ExchangeRateItem.Indicators["Macd26-12-9-Signal"].ToString(decimalFormat)}]");
            }

            writer.AddToLog("\nCycle Extension Crossings:");
            var extensionCrossings = ExtensionCrossings.Where(r => r.Key.StartsWith(asset));
            string lastCycle = "";
            foreach (var extensionCrossingCycles in extensionCrossings)
            {
                var cycle = extensionCrossingCycles.Key.Replace($"{asset}-", "");
                lastCycle = extensionCrossingCycles.Key;
                writer.AddToLog($"- For cycle {CycleString(cycle)}:");
                foreach (var extensionCrossing in extensionCrossingCycles.Value)
                    writer.AddToLog($"  Extension level {extensionCrossing.FibonacciMarker.Level} of " +
                        $"${extensionCrossing.FibonacciMarker.Value.ToString(decimalFormat)} crossed on " +
                        $"{extensionCrossing.ExchangeRateItem.TimePeriodStart.ToShortDateString()} at " +
                        $"${extensionCrossing.ExchangeRateItem.RateClose.ToString(decimalFormat)}. " +
                        $"[RSI: {extensionCrossing.ExchangeRateItem.Indicators["Rsi14"].ToString(decimalFormat)}] " +
                        $"[MACD: {extensionCrossing.ExchangeRateItem.Indicators["Macd26-12-9-Macd"].ToString(decimalFormat)} " +
                        $"/ Signal: {extensionCrossing.ExchangeRateItem.Indicators["Macd26-12-9-Signal"].ToString(decimalFormat)}]");
            }

            writer.AddToLog("\nNext Cycle Extensions:");
            foreach (var extension in Extensions[lastCycle])
            {
                writer.AddToLog($"  Extension level {extension.Level} is ${extension.Value.ToString(decimalFormat)}.");
                var futureRetracements = CalculateFutureRetracements(CycleBottoms[lastCycle].RateClose, extension);
                foreach (var retracement in futureRetracements)
                    writer.AddToLog($"    Retracement level {retracement.Level} is ${retracement.Value.ToString(decimalFormat)}.");
            }

            writer.WriteToLog($"{asset}.txt");
        }

        private string CycleString(string cycle)
        {
            var year = CycleStarts.Where(cs => cs.Cycle == int.Parse(cycle)).FirstOrDefault().StartDate.Year;
            return $"{cycle} ({year})";
        }

        private List<FibonacciMarker> CalculateFutureRetracements(
            decimal previousBottom, FibonacciMarker extension)
        {
            var extensionSwing = extension.Value - previousBottom;
            var retracements = new List<FibonacciMarker>();
            foreach (var retracementLevel in RetracementLevels)
            {
                var value = extensionSwing - ((extension.Value - previousBottom) * retracementLevel);
                if (value > 0) retracements.Add(new FibonacciMarker(retracementLevel, value));
            }
            return retracements;
        }
    }
}
