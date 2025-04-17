public class DashboardViewModel
{
    public List<StockData> Stocks { get; set; } = new();
    public List<string> Industries { get; set; } = new();
    public string? SelectedIndustry { get; set; }
}
