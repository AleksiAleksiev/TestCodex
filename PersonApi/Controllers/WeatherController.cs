using Microsoft.AspNetCore.Mvc;
using PersonApi.Models;
using System.Text.Json;

namespace PersonApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly HttpClient _client;

        public WeatherController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecastModel>>> Get([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City is required.");
            }

            var url = $"https://wttr.in/{Uri.EscapeDataString(city)}?format=j1";
            using var stream = await _client.GetStreamAsync(url);
            using var doc = await JsonDocument.ParseAsync(stream);

            var days = new List<WeatherForecastModel>();
            var weatherDays = doc.RootElement.GetProperty("weather").EnumerateArray().Take(3);

            foreach (var day in weatherDays)
            {
                var desc = day.GetProperty("hourly")[0].GetProperty("weatherDesc")[0].GetProperty("value").GetString() ?? string.Empty;
                days.Add(new WeatherForecastModel
                {
                    Date = DateOnly.Parse(day.GetProperty("date").GetString() ?? string.Empty),
                    MaxTempC = int.Parse(day.GetProperty("maxtempC").GetString() ?? "0"),
                    MinTempC = int.Parse(day.GetProperty("mintempC").GetString() ?? "0"),
                    Description = desc.Trim()
                });
            }

            return Ok(days);
        }
    }
}
