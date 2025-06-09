namespace PersonApi.Models
{
    public class WeatherForecastModel
    {
        public DateOnly Date { get; set; }
        public int MaxTempC { get; set; }
        public int MinTempC { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
