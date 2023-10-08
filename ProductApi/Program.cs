using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProductApi.AutoMapper;
using ProductApi.Configuration;
using ProductApi.Services.Interfaces;
using ProductApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//connection database
var connectionString = builder.Configuration.GetConnectionString("MongoDB");
var client = new MongoClient(connectionString);
var database = client.GetDatabase("Store");

MongoDBConfiguration.ConfigureProductCollectionValidation(database);
MongoDBConfiguration.ConfigureCategoryCollectionValidation(database);

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddSingleton<IMongoClient>(client);
builder.Services.AddSingleton<IMongoDatabase>(database);

//services

//mapper
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

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
