using Best.Practices.Core.Application.Services;
using Best.Practices.Core.Application.Services.Interfaces;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using System.Text.Json.Serialization;

namespace Best.Practices.Core.Presentation.AspNetCoreApi.Configurations
{
    public static class DefaultApiConfiguration
    {
        private static void MapDefaultApplicationServices(IServiceCollection service)
        {
            service.AddScoped<ITokenAuthentication, TokenAuthentication>();
        }

        public static WebApplication Configure(WebApplicationBuilder builder)
        {
            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            builder.Services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });
            MapDefaultApplicationServices(builder.Services);

            var section = builder.Configuration.GetSection("AppSettings");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowAnyOrigin();
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}