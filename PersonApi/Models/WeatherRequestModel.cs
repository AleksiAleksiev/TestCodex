namespace PersonApi.Models
{
    public class WeatherRequestModel
    {
        public string City { get; set; } = string.Empty;
        public int Days { get; set; }
    }
}