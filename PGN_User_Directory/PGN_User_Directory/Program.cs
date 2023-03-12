using System.Net;
using System.Reflection;
using Microsoft.Extensions.Configuration;

const string _configurationPath = "/Configuration";
var configurationDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + _configurationPath;

//IConfiguration configuration = new ConfigurationBuilder()
//                            .AddJsonFile("appsettings.json")
//                            .Build();

var builder = WebApplication.CreateBuilder(args);

//builder.
//    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Configuration.AddJsonFile(configurationDirectory + "/appsettings.json", optional: true, reloadOnChange: false);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Listen(IPAddress.Any, builder.Configuration.GetValue<int>("Port"), listenOptions =>
    {
        //var tmp = builder.Configuration.GetValue<int>("myPort");
    });
});
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

