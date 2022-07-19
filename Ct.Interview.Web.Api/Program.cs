using Core;
using Ct.Interview.Web.Api.Extensions;
using Infrastructure;
using Infrastructure.Client;
using Infrastructure.Workers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var appsettings = builder.Configuration.GetSection(nameof(AsxSettings))
    .Get<AsxSettings>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
// Add services to the container.
builder.Services.AddHttpClient<AsxCompanyClient>();
builder.Services.AddDbContext<ApplicationDbContext>(
    opt => opt.UseSqlite(appsettings.ConnectionStrings));
builder.Services.AddSingleton(appsettings);
builder.Services.ConfigureDependencies();

builder.Services.AddControllers();

builder.Services.AddHostedService<AsxCompanyUpdater>();

builder.Services.AddRouting(opts => opts.LowercaseUrls = true);
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
