using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Post.Command.Api.Commands;
using Post.Command.Domain.Aggregates;
using Post.Command.Infrastructure.Config;
using Post.Command.Infrastructure.Dispatchers;
using Post.Command.Infrastructure.Handlers;
using Post.Command.Infrastructure.Producers;
using Post.Command.Infrastructure.Repositories;
using Post.Command.Infrastructure.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoDBConfig>(builder.Configuration.GetSection(nameof(MongoDBConfig)));
builder.Services.Configure<ApachePulsarConfig>(builder.Configuration.GetSection(nameof(ApachePulsarConfig)));

builder.Services.AddScoped<IMongoDBConfiguration, MongoDBConfiguration>();
builder.Services.AddScoped<IApachePulsarConfiguration, ApachePulsarConfiguration>();

builder.Services.AddScoped<IEventStoreRepository, EventStoreRespository>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

//register command handler methods
var CommandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispatcher = new CommandDispatcher();

dispatcher.RegisterHandler<NewPostCommand>(CommandHandler.HandleAsync);
dispatcher.RegisterHandler<EditMessageCommand>(CommandHandler.HandleAsync);
dispatcher.RegisterHandler<LikePostCommand>(CommandHandler.HandleAsync);
dispatcher.RegisterHandler<AddCommentCommand>(CommandHandler.HandleAsync);
dispatcher.RegisterHandler<EditCommentCommand>(CommandHandler.HandleAsync);
dispatcher.RegisterHandler<RemoveCommentCommand>(CommandHandler.HandleAsync);
dispatcher.RegisterHandler<DeletePostCommand>(CommandHandler.HandleAsync);

builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
