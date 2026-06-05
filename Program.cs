using System.Reflection;
using GlobalSolution.Data;
using GlobalSolution.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<LocalizacaoService>();
builder.Services.AddScoped<PropriedadeService>();
builder.Services.AddScoped<PlantacaoService>();
builder.Services.AddScoped<SensorService>();
builder.Services.AddScoped<LeituraService>();
builder.Services.AddScoped<AlertaService>();

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseOracle(
        builder.Configuration.GetConnectionString("OracleConnection")
    );
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    options.IncludeXmlComments(
        Path.Combine(AppContext.BaseDirectory, xmlFilename)
    );

    options.EnableAnnotations();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();