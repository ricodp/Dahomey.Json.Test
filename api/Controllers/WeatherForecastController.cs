using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace api.Controllers
{
    public enum WeatherEnum
    {
        Sun = 0,
        Rain = 1,
        Snow
    }
    public abstract record Weather();
    public record WeatherForecast(string Value) : Weather;
    public record PostWeather(Weather Value);
    public record DictionaryForecast(Dictionary<string, string> Value) : Weather;
    public record DictionaryGuidForecast(Dictionary<string, string> Value) : Weather;
    public record TupleForecast((int, int) Value) : Weather;
    public record EnumForecast(WeatherEnum Value) : Weather;

    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        public WeatherForecastController()
        {
        }
        [HttpGet]
        public Weather Get()
        {
            return new WeatherForecast("Test"); // Returns: { "$type": "WeatherForecast", "value": "Test" }
    }
        [HttpPost]
        public Weather Post([FromBody] WeatherForecast weather)
        {
            //Post: { "$type": "WeatherForecast", "value": "test" }
            return weather; //Returns: { "$type": "WeatherForecast", "value": "test" }
        }
        [HttpPost("/Two")]
        public WeatherForecast Post2([FromBody] Weather weather)
        {
            //Post: { "$type": "WeatherForecast", "value": "test" }
            return (WeatherForecast)weather; //Returns: { "$type": "WeatherForecast", "value": "test" }
    }

        [HttpPost("/Three")]
        public Weather Post3([FromBody] PostWeather weather)
        {
            //Post: {"value": { "$type": "WeatherForecast", "value": "test" }}
            //Post: {"value": { "$type": "DictionaryForecast", "value": {"a": "a", "b": "b"} }}
            //Post: {"value": { "$type": "DictionaryGuidForecast", "value": {"3ae89e49-b257-4649-af63-34d906309552": "a", "00525cc4-80e3-4599-803a-071d683ecb46": "b"} }}
            //Post: {"value": { "$type": "TupleForecast", "value": [3, 3] }}
            //Post: {"value": { "$type": "EnumForecast", "value": 0 }}
            //Post: {"value": { "$type": "EnumForecast", "value": "Sun" }}
            return weather.Value;
        }

        [HttpPost("/Tuple")]
        public (int, int) Post4([FromBody] (int, int) tuple)
        {
            //Post: [3, 3]
            return tuple;
        }
    }
}