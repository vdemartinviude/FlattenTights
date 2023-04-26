using api.ConfigOptions;
using api.FlattenService;
using api.Mapster;
using api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TightsStorageDatabaseSettings>(builder.Configuration.GetSection("TightsStoreDatabase"));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<AccessMongoTights>();
builder.Services.AddSingleton<FlatService>();

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
