using Best.Practices.Core.Presentaton.AspNetCoreApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

var app = DefaultApiConfiguration.Configure(builder);

app.Run();
