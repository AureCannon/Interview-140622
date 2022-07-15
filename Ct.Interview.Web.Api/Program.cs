using Ct.Interview.Web.Api.Data;
using Ct.Interview.Web.Api.Models;
using Ct.Interview.Web.Api.Services;
using Ct.Interview.Web.Api.Policies;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<AsxSettings>(builder.Configuration.GetSection("AsxSettings"));
var asxSettingsSection = builder.Configuration.GetSection("AsxSettings").Get<AsxSettings>();
builder.Services.AddSingleton<AsxSettings>(asxSettingsSection);
builder.Services.AddTransient<IAsxListedCompaniesService, AsxListedCompaniesService>();
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient("HttpClient")
    .AddPolicyHandler(ClientPolicy.GetRetryPolicy());

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
