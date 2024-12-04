#region

using System.Globalization;
using System.Reflection;
using Azure;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TasQ.Projetos.Api.Setup;
using TasQ.Projetos.Data;

#endregion

var culturaPadrao = new CultureInfo("pt-BR");

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegistrarServicos();
builder.Services.AddDbContext<ProjetoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseLazyLoadingProxies()
        .UseSnakeCaseNamingConvention());
builder.Services.AddHttpContextAccessor();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TasQ Projetos API", Version = "v0.0.1.beta" });
    c.OperationFilter<SwaggerHeaderAttribute>();
});



builder.Services.AddMediatR(typeof(Program).GetTypeInfo().Assembly);
builder.Services.AddHealthChecks();


var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}
app.UseSwagger();
app.UseSwaggerUI();

var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culturaPadrao),
    SupportedCultures = new List<CultureInfo> { culturaPadrao },
    SupportedUICultures = new List<CultureInfo> { culturaPadrao }
};

app.UseRequestLocalization(localizationOptions);
app.UseHealthChecks("/health");
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();