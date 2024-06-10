using Cocona;
using cosmodb;
using cosmodb.Commands;
using cosmodb.Repository;
using Microsoft.Extensions.DependencyInjection;

var builder = CoconaApp.CreateBuilder();
builder.Services.AddSingleton(typeof(ICosmosDbRepository<>), typeof(CosmosDbRepository<>));
builder.Services.AddSingleton<IDatabaseRepository, DatabaseRepository>();
var app = builder.Build();

app.AddSubCommand("database", commands => { commands.AddCommands<CosmosDatabaseCommands>(); }).WithDescription("Cosmos DB Database commands");
app.AddSubCommand("cosmos", commands => { commands.AddCommands<CosmosCrudCommands>(); }).WithDescription("CRUD commands for data in azure cosmos db");

app.Run();