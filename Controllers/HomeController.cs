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

    public IActionResult Index(string industry)
    {
        var stocks = GetStocksData(industry);
        var industries = stocks
            .Where(s => !string.IsNullOrEmpty(s.Industry))
            .Select(s => s.Industry!)
            .Distinct()
            .ToList();

        var model = new DashboardViewModel
        {
            Stocks = stocks,
            Industries = industries,
            SelectedIndustry = industry
        };

        return View(model);
    }

    private List<StockData> GetStocksData(string industry)
    {
        var stocks = new List<StockData>();
        var filePath = Path.Combine(_env.ContentRootPath, "Data", "Trending Stocks.csv");


        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.TypeConverterCache.AddConverter<decimal?>(new SafeDecimalConverter());

        stocks = csv.GetRecords<StockData>().ToList();

        if (!string.IsNullOrEmpty(industry))
        {
            stocks = stocks.Where(s => s.Industry == industry).ToList();
        }

        return stocks;
    }
}
