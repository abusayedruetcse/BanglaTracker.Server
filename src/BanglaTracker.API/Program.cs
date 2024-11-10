using BanglaTracker.BLL.BackgroundServices;
using BanglaTracker.BLL.Interfaces;
using BanglaTracker.BLL.Services;
using BanglaTracker.Core.Interfaces;
using BanglaTracker.Infrastructure.Data;
using BanglaTracker.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext with In-Memory Database
builder.Services.AddDbContext<TrackerDbContext>(options =>
    options.UseInMemoryDatabase("TrackerInMemoryDb")); // Specify a name for your in-memory database

// Register the generic repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register the Services
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ITrainJourneyService, TrainJourneyService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITrainService, TrainService>();
builder.Services.AddScoped<IDataImportService, DataImportService>();

// Register the repository
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ITrainJourneyRepository, TrainJourneyRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register the background services
builder.Services.AddHostedService<TrainPositionUpdaterService>();   // Register as singleton

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
