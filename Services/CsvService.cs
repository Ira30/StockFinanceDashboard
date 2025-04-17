using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using FinanceDashboard.Models;


namespace FinanceDashboard.Services
{
    public class CsvService
    {
        public List<StockData> GetStockData()
        {
            var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Trending Stocks.csv");

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                var records = csv.GetRecords<StockData>().ToList();
                return records;
            }
        }
    }
}
