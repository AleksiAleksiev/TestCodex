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

        [HttpPost]
        public async Task<ActionResult<IEnumerable<WeatherForecastModel>>> Post([FromBody] WeatherRequestModel request)
        {
            if (string.IsNullOrWhiteSpace(request.City))
            {
                return BadRequest("City is required.");
            }

            if (request.Days <= 0)
            {
                return BadRequest("Days must be greater than zero.");
            }

            var url = $"https://wttr.in/{Uri.EscapeDataString(request.City)}?format=j1&num_of_days={request.Days}";
            using var stream = await _client.GetStreamAsync(url);
            using var doc = await JsonDocument.ParseAsync(stream);

            var days = new List<WeatherForecastModel>();
            var weatherDays = doc.RootElement.GetProperty("weather").EnumerateArray().Take(request.Days);

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
