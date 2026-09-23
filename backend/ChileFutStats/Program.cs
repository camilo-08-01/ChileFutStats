using ChileFutStats.Data;
using ChileFutStats.Repositories;
using ChileFutStats.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<OracleConnectionFactory>();
builder.Services.AddScoped<EquipoRepository>();
builder.Services.AddScoped<SofascoreService>();
builder.Services.AddScoped<PartidoRepository>();
builder.Services.AddScoped<TemporadaRepository>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<TokenService>();

var jwtKey = builder.Configuration["Jwt:Key"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
