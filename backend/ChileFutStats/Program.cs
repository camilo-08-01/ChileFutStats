using ChileFutStats.Data;
using ChileFutStats.Repositories;
using ChileFutStats.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<OracleConnectionFactory>();
builder.Services.AddScoped<EquipoRepository>();
builder.Services.AddScoped<SofascoreService>();
builder.Services.AddScoped<PartidoRepository>();

builder.Services.AddHttpClient("Sofascore", client =>
{
    client.BaseAddress = new Uri("https://sofascore.p.rapidapi.com/");
    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "sofascore.p.rapidapi.com");
    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", builder.Configuration["Sofascore:ApiKey"]);
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
