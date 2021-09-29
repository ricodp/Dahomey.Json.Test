using System;
using Xunit;
using Alba;
using System.Threading.Tasks;
using api.Controllers;

namespace api.tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var hostBuilder = Program.CreateHostBuilder(Array.Empty<string>());

            await using var host = new AlbaHost(hostBuilder);

            await host.Scenario(s =>
            {
                s.Get.Url("/WeatherForecast");
                s.ContentShouldBe(@"{""$type"":""WeatherForecast"",""value"":""Test""}");
            });
        }

        [Fact]
        public async Task Test2()
        {
            var hostBuilder = Program.CreateHostBuilder(Array.Empty<string>());

            await using var host = new AlbaHost(hostBuilder);

            var payload = new PostWeather(new WeatherForecast("test"));
            var json = @"{""Value"": { ""$type"": ""WeatherForecast"", ""Value"": ""test"" }}";
            await host.Scenario(s =>
            {
                s.Post
                .Json(payload)
                .ToUrl("/Three");
                s.ContentShouldBe(json);
            });
        }
    }
}
