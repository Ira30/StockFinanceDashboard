using System.Globalization;
using CsvHelper;
using FinanceDashboard.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc;


public class HomeController : Controller
{
    private readonly IWebHostEnvironment _env;

    public HomeController(IWebHostEnvironment env)
    {
        _env = env;
    }
public IActionResult Index(string industry, string stockName)
{
    // Get all stocks filtered by industry
    var stocks = GetStocksData(industry, stockName);

    var industries = stocks
        .Where(s => !string.IsNullOrEmpty(s.Industry))
        .Select(s => s.Industry!)
        .Distinct()
        .ToList();

    // Find the stock with the highest volatility (ChgPercentage)
    var mostVolatileStock = stocks
        .OrderByDescending(s => Math.Abs(s.ChgPercentage ?? 0))
        .FirstOrDefault();

    var topPerformingStocks = stocks
        .GroupBy(s => s.Industry)
        .Select(group => new IndustryStock
        {
            Industry = group.Key,
            TopPerformingStock = group.OrderByDescending(s => s.ChgPercentage ?? 0).FirstOrDefault()
        })
        .ToList();

    // Find the stock with the lowest price increase (Chg)
    var worstPerformingStock = stocks
        .OrderBy(s => s.Chg)
        .FirstOrDefault();

    var model = new DashboardViewModel
    {
        Stocks = stocks,
        Industries = industries,
        SelectedIndustry = industry,
        StockWithHighestVolatility = mostVolatileStock,
        WorstPerformingStock = worstPerformingStock,
        TopPerformingStocks = topPerformingStocks,
        SelectedStockName = stockName
    };

    return View(model);

}


    private List<StockData> GetStocksData(string industry, string stockName)
    {
        var stocks = new List<StockData>();
        var filePath = Path.Combine(_env.ContentRootPath, "Data", "Trending Stocks.csv");

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.TypeConverterCache.AddConverter<decimal?>(new SafeDecimalConverter());

        stocks = csv.GetRecords<StockData>().ToList();

        // Filter by industry if provided
        if (!string.IsNullOrEmpty(industry))
        {
            stocks = stocks.Where(s => s.Industry == industry).ToList();
        }

        // Filter by stockName if provided
        if (!string.IsNullOrEmpty(stockName))
        {
            stocks = stocks.Where(s => s.Name.Contains(stockName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        return stocks;
    }
}
