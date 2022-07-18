using Infrastructure.Client;
using Infrastructure.Workers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<AsxCompanyClient>();

builder.Services.AddControllers();

builder.Services.AddHostedService<AsxCompanyUpdater>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
