using CsvHelper.Configuration.Attributes;

public class StockData
{
    public string? Name { get; set; }

    [TypeConverter(typeof(SafeDecimalConverter))]
    public decimal? Last { get; set; }

    [TypeConverter(typeof(SafeDecimalConverter))]
    public decimal? High { get; set; }

    [TypeConverter(typeof(SafeDecimalConverter))]
    public decimal? Low { get; set; }

    [Name("Chg.")]
    [TypeConverter(typeof(SafeDecimalConverter))]
    public decimal? Chg { get; set; }

    [Name("Chg. %")]
    [TypeConverter(typeof(SafeDecimalConverter))]
    public decimal? ChgPercentage { get; set; }

    [Name("Vol.")]
    public string? Volume { get; set; }

    public string? Time { get; set; }
    public string? Industry { get; set; }
    public StockData? StockWithHighestVolatility { get; set; }

}
