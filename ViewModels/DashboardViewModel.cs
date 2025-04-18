public class DashboardViewModel
{
    public List<StockData> Stocks { get; set; } = new();
    public List<string> Industries { get; set; } = new();
    public string? SelectedIndustry { get; set; }
    public StockData? StockWithHighestVolatility { get; set; } 
    public string? SelectedStockName { get; set; } 
   // public StockData? TopPerformingStock { get; set; }
    public StockData? WorstPerformingStock { get; set; }
    public List<IndustryStock> TopPerformingStocks { get; set; }
}

public class IndustryStock
{
    public string? Industry { get; set; }
    public StockData TopPerformingStock { get; set; }
}
