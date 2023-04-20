using Microsoft.EntityFrameworkCore;
using MyApp.Models;
using MyApp.Controllers;
using MyApp.Hubs;
using System.Linq;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
//var connectionString = ";

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var app = builder.Build();

//app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.MapControllers();

app.MapGet("/api/predict", (string content) =>
{

    SentimentModel.ModelInput input = new SentimentModel.ModelInput { Content = content };

    var sortedScoresWithLabel = SentimentModel.PredictAllLabels(input);
    //var sentiment = sortedScoresWithLabel.First().Key;

    return new
    {
        //sentiment,
        scores = sortedScoresWithLabel
        //contents
    };
});

//app.MapGet("/", () => "Hello World!");

app.MapHub<ProjectsHub>("/r/projectsHub");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
