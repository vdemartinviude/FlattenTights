using api.ConfigOptions;
using api.FlattenService;
using api.Mapster;
using api.Services;
using DotNetEnv;
using DotNetEnv.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddDotNetEnv(".env",LoadOptions.TraversePath());
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<AccessMongoTights>();
builder.Services.AddSingleton<FlatService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();


//app.UseAuthorization();

app.MapControllers();

app.Run();
