using System;
using System.Text.Json;
using api;
using api.Controllers;

namespace console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine((byte)'V'); //86
            System.Console.WriteLine((byte)'v'); //118
            var options = new JsonSerializerOptions();
            Startup.ConfigureJsonSerializerOptions(options);
            var json = @"{""Value"": { ""$type"": ""WeatherForecast"", ""Value"": ""test"" }}";
            var x = JsonSerializer.Deserialize<PostWeather>(json, options);
            System.Console.WriteLine(x);
        }
    }
}
