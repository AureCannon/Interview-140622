using Ct.Interview.Web.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAsxListedCompaniesService, AsxListedCompaniesService>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json",
        "Listed Companies API v1");
});

app.Run();

app.UseHttpsRedirection();

app.UseAuthorization();

app.Run();