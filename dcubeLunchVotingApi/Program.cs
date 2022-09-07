using dcubeLunchVotingApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IVoteService, VoteService>();

builder.Services.AddDbContextFactory<LunchVotingContext>(options => 
options
    .EnableSensitiveDataLogging()
    .LogTo(Console.WriteLine, (eventId, logLevel) => logLevel > LogLevel.Information
                                   || eventId == CoreEventId.QueryCompilationStarting
                                   || eventId == CosmosEventId.ExecutedReadNext
                                   || eventId == CosmosEventId.ExecutedReadItem
                                   || eventId == CosmosEventId.ExecutedCreateItem
                                   || eventId == CosmosEventId.ExecutedReplaceItem
                                   || eventId == CosmosEventId.ExecutedDeleteItem
    ).UseCosmos(
    connectionString: "AccountEndpoint=...=",
    databaseName:"LunchVotingDb"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
