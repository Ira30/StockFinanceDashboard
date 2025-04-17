using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

public class SafeDecimalConverter : DecimalConverter
{
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text) || text == "-")
            return null;

        // Remove % and commas
        var cleanedText = text.Replace(",", "").Replace("%", "").Trim();

        if (decimal.TryParse(cleanedText, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            return value;

        return base.ConvertFromString(cleanedText, row, memberMapData);
    }
}
