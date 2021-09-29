using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Dahomey.Json;
using Dahomey.Json.Serialization.Conventions;
using Dahomey.Json.Serialization.Converters.DictionaryKeys;
using System.Text.Json;
using System.Text.Json.Serialization;
using api.Controllers;
using TupleAsJsonArray;
using Dahomey.Json.Serialization.Converters.Mappings;

namespace api
{
    public class Startup
    {
        public static void ConfigureJsonSerializerOptions(JsonSerializerOptions options)
        {
            options.Converters.Add(new TupleConverterFactory());
            options.Converters.Add(new JsonStringEnumConverter());
            options.SetupExtensions();
            options.GetDictionaryKeyConverterRegistry().RegisterDictionaryKeyConverter(new Utf8DictionaryKeyConverter<Guid>());
            var registry = options.GetDiscriminatorConventionRegistry();
            registry.RegisterConvention(new DefaultDiscriminatorConvention<string>(options));
            registry.DiscriminatorPolicy = DiscriminatorPolicy.Always;
            var objectMap = options.GetObjectMappingRegistry();
            Action<ObjectMapping<T>> SetDiscriminator<T>() => objectMapping => objectMapping.AutoMap().SetDiscriminator(typeof(T).Name);
            objectMap.Register(SetDiscriminator<WeatherForecast>());
            objectMap.Register(SetDiscriminator<DictionaryForecast>());
            objectMap.Register(SetDiscriminator<DictionaryGuidForecast>());
            objectMap.Register(SetDiscriminator<DictionaryGuidObjectForecast>());
            objectMap.Register(SetDiscriminator<TupleForecast>());
            objectMap.Register(SetDiscriminator<EnumForecast>());
            objectMap.Register(SetDiscriminator<DateForecast>());
        }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
            .AddJsonOptions(options => ConfigureJsonSerializerOptions(options.JsonSerializerOptions));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
            });
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1"));
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}