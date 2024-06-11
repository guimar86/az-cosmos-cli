using Cocona;
using cosmodb.Commands;
using cosmodb.Repository;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

var builder = CoconaApp.CreateBuilder();
// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

// Add Serilog to the Cocona builder
builder.Host.UseSerilog();
builder.Services.AddSingleton(typeof(ICosmosDbRepository<>), typeof(CosmosDbRepository<>));
builder.Services.AddSingleton<IDatabaseRepository, DatabaseRepository>();
builder.Services.AddSingleton<CosmosCommandBase>();
var app = builder.Build();

app.AddSubCommand("database", commands => { commands.AddCommands<CosmosDatabaseCommands>(); }).WithDescription("Cosmos DB Database commands");
app.AddSubCommand("cosmos", commands => { commands.AddCommands<CosmosCrudCommands>(); }).WithDescription("CRUD commands for data in azure cosmos db");

app.Run();