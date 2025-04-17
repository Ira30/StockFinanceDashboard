using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using CsvHelper;  // You need to install CsvHelper package to parse the CSV

public class StockController : Controller
{
    private List<StockData> _stockData;

    // Constructor that reads the CSV and loads stock data
    public StockController()
    {
        _stockData = LoadStockData();
    }

    // Method to load data from the CSV
    private List<StockData> LoadStockData()
    {
        var records = new List<StockData>();
        var csvFilePath = "Trending Stock.csv"; // Update this path to where your CSV is saved

        using (var reader = new StreamReader(csvFilePath))
        using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
        {
            records = csv.GetRecords<StockData>().ToList();
        }

        return records;
    }

    // Index method that returns data to the view
    public IActionResult Index()
    {
        return View(_stockData);  // Sends the data to the view
    }
}
