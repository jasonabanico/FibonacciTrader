using FibonacciTrader.Models;
using Newtonsoft.Json;

namespace FibonacciTrader.Services
{
    public class FileWriter
    {
        private readonly string _filePath;
        private List<string> _log = new List<string>();

        public FileWriter(string filePath)
        {
            _filePath = filePath;
        }

        public void WriteItemsToFile(List<ExchangeRateItem> items, string fileName)
        {
            var jsonData = JsonConvert.SerializeObject(items);
            File.WriteAllText($"{_filePath}\\{fileName}", jsonData);
        }

        public List<ExchangeRateItem> ReadItemsFromFile(string fileName)
        {
            if (!File.Exists($"{_filePath}\\{fileName}"))
            {
                return new List<ExchangeRateItem>();
            }

            var jsonString = File.ReadAllText($"{_filePath}\\{fileName}");
            return JsonConvert.DeserializeObject<List<ExchangeRateItem>>(jsonString);
        }

        public void AddToLog(string message) 
        { 
            _log.Add(message);
        }

        public void WriteToLog(string fileName)
        {
            using (StreamWriter writer = new StreamWriter($"{_filePath}\\{fileName}"))
            {
                foreach (var line in _log)
                {
                    writer.WriteLine(line);
                    Console.WriteLine(line);
                }
            }
        }
    }
}
